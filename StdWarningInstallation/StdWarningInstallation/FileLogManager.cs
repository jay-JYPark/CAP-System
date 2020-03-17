using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

using Adeng.Framework.Log;

namespace StdWarningInstallation
{
    public class FileLogManager
    {
        static FileLogMng fileLogMng = new FileLogMng();

        private static Mutex mutex = new Mutex();
        private static string logDirectoryPath = @"Log\";
        private static FileLogManager instance = null;

        private bool isLogProcessingContinue = true;
        private Thread logDataProcessingThread = null;
        private ManualResetEvent manualEvtFileLog = null;
        private Queue<string> dataFIleLogQueue = new Queue<string>();

        /// <summary>
        /// 싱글톤 처리.
        /// 반드시 이 메소드를 호출하여 인스턴스를 사용해야 하며,
        /// WriteLog 호출로 인스턴스가 종료된다.
        /// </summary>
        /// <returns></returns>
        public static FileLogManager GetInstance()
        {
            mutex.WaitOne();
            if (instance == null)
            {
                instance = new FileLogManager();
            }
            return instance;
        }
        private FileLogManager()
        {
            StartLogThread();
        }
        ~FileLogManager()
        {
            EndLogThread();

            mutex.Close();
        }

        /// <summary>
        /// 로그 디렉토리 존재 체크 및 생성.
        /// </summary>
        /// <returns></returns>
        private void CheckLogFileDirectory()
        {
            if (!Directory.Exists(logDirectoryPath))
            {
                Directory.CreateDirectory(logDirectoryPath);
            }
        }

        /// <summary>
        /// 로그 데이터를 큐에 저장.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public bool WriteLog(string content)
        {
            try
            {
                if (this.dataFIleLogQueue != null)
                {
                    lock (this.dataFIleLogQueue)
                    {
                        this.dataFIleLogQueue.Enqueue(content);
                        if (this.manualEvtFileLog != null)
                        {
                            this.manualEvtFileLog.Set();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[FileLogManager] WriteLog( Exception=[" + ex.ToString() + "] )");
            }
            finally
            {
                mutex.ReleaseMutex();
            }

            return false;
        }

        /// <summary>
        /// 파일로그 데이터 감시 쓰레딩 시작.
        /// </summary>
        private void StartLogThread()
        {
            System.Console.WriteLine("[FileLogManager] StartLogThread()");

            if (this.manualEvtFileLog == null)
            {
                this.manualEvtFileLog = new ManualResetEvent(false);
            }

            this.isLogProcessingContinue = true;
            if (this.logDataProcessingThread == null)
            {
                this.logDataProcessingThread = new Thread(new ThreadStart(LogDataProcessing));
                this.logDataProcessingThread.IsBackground = true;
                this.logDataProcessingThread.Name = "LogDataProcessingThread";
                this.logDataProcessingThread.Start();
            }
        }
        /// <summary>
        /// 파일로그 데이터 감시 쓰레딩 종료.
        /// </summary>
        private void EndLogThread()
        {
            System.Console.WriteLine("[FileLogManager] EndLogThread()");

            if (this.logDataProcessingThread != null && this.logDataProcessingThread.IsAlive)
            {
                this.isLogProcessingContinue = false;
                bool terminated = this.logDataProcessingThread.Join(500);
                if (!terminated)
                {
                    this.logDataProcessingThread.Abort();
                }
            }
            this.logDataProcessingThread = null;
        }

        /// <summary>
        /// 파일로그 데이터 처리 쓰레드 함수.
        /// </summary>
        private void LogDataProcessing()
        {
            System.Console.WriteLine("[FileLogManager] LogDataProcessing ( 파일 로그 쓰레드 시작 )");

            try
            {
                while (this.isLogProcessingContinue)
                {
                    Thread.Sleep(500);

                    int count = 0;
                    lock (this.dataFIleLogQueue)
                    {
                        count = this.dataFIleLogQueue.Count;
                    }
                    if (count <= 0)
                    {
                        this.manualEvtFileLog.WaitOne();
                        this.manualEvtFileLog.Reset();

                        continue;
                    }

                    List<string> logs = new List<string>();
                    lock (this.dataFIleLogQueue)
                    {
                        for (int index = 0; index < this.dataFIleLogQueue.Count; index++)
                        {
                            string log = this.dataFIleLogQueue.Dequeue();
                            logs.Add(log);
                        }
                    }

                    // 로그 폴더 체크(작성)
                    CheckLogFileDirectory();

                    // 로그 쓰기
                    foreach (string logContent in logs)
                    {
                        string fileName = DateTime.Now.ToString("yyyyMMdd");
                        if (!fileLogMng.LogFileWrite(logDirectoryPath, fileName, logContent))
                        {
                            System.Console.WriteLine("[FileLogManager] LogDataProcessing( Write Error!! )");
                        }
                    }
                }
            }
            catch (ThreadAbortException exAbort)
            {
                System.Console.WriteLine("[FileLogManager] LogDataProcessing( ABORT 검출: " + exAbort.Message + ")");
                Thread.ResetAbort();    // 상위로 다시 익셉션을 Throw 하지 않게 하기 위해서 콜.
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[FileLogManager] LogDataProcessing( " + ex.ToString() + " )");
            }
            finally
            {
                // 쓰레드 종료 전에 해야할 일이 있다면 이 타이밍에 처리.
                System.Console.WriteLine("[FileLogManager] LogDataProcessing ( 파일 로그 쓰레드 종료 )");
            }
        }
    }
}
