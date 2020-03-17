using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace StdWarningInstallation
{
    public class EventLogManager
    {
        private static EventLogManager instance = null;
        private static Mutex mutex = new Mutex();
        private string sourceName = "표준발령대";   // 어플리케이션의 이름을 얻어오는 방법 없나?
        private string currentUserNme = "LocalUser";

        public string SourceName
        {
            get { return sourceName; }
            set { sourceName = value; }
        }

        /// <summary>
        /// 생성자
        /// 싱글톤 생성 메소드를 사용하여 인스턴스 생성할 것.
        /// </summary>
        public EventLogManager()
        {
        }

        /// <summary>
        /// 싱글톤 처리
        /// 반드시 이 메소드를 호출하여 인스턴스를 사용해야 한다.
        /// </summary>
        /// <returns></returns>
        public static EventLogManager GetInstance()
        {
            mutex.WaitOne();

            if (instance == null)
            {
                instance = new EventLogManager();
            }

            mutex.ReleaseMutex();
            return instance;
        }

        /// <summary>
        /// 로그 출력시 참조하는 유저명을 설정한다.
        /// </summary>
        /// <param name="userName"></param>
        public void SetUserName(string userName)
        {
            System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(userName));

            if (!string.IsNullOrEmpty(userName))
            {
                this.currentUserNme = userName;
            }
        }

        /// <summary>
        /// 로컬의 윈도우 이벤트로그를 기록을 읽어온다.
        /// </summary>
        /// <param name="sourceName">이벤트 뷰어의 소스명</param>
        /// <param name="_startTime">조회할 시간의 시작시간</param>
        /// <param name="_endTime">조회할 시간의 종료시간</param>
        /// <returns></returns>
        public List<Log> ReadLog(string _sourceName, DateTime _startTime, DateTime _endTime)
        {
            List<Log> logList = new List<Log>();
            Log log = null;
            EventLog eventLog = new EventLog();

            if (!EventLog.SourceExists(_sourceName))
            {
                EventLog.CreateEventSource(_sourceName, _sourceName);
            }

            eventLog.Log = _sourceName;

            foreach (EventLogEntry eventLogEntry in eventLog.Entries)
            {
                if ((eventLogEntry.TimeWritten) >= _startTime && (eventLogEntry.TimeWritten) <= _endTime)
                {
                    log = new Log();
                    log.Kind = eventLogEntry.EntryType.ToString();
                    log.Date = eventLogEntry.TimeWritten.Date.ToString().Substring(0, 10);
                    log.Time = eventLogEntry.TimeWritten.TimeOfDay.ToString();
                    log.Message = eventLogEntry.Message;
                    log.UserName = Encoding.Default.GetString(eventLogEntry.Data);
                    logList.Add(log);
                }
            }

            return logList;
        }
        /// <summary>
        /// 로컬의 윈도우 이벤트로그를 기록을 읽어온다.
        /// </summary>
        /// <param name="_startTime">조회할 시간의 시작시간</param>
        /// <param name="_endTime">조회할 시간의 종료시간</param>
        /// <returns></returns>
        public List<Log> ReadLog(DateTime _startTime, DateTime _endTime)
        {
            List<Log> logList = new List<Log>();
            Log log = null;
            EventLog eventLog = new EventLog();

            if (!EventLog.SourceExists(this.sourceName))
            {
                EventLog.CreateEventSource(this.sourceName, this.sourceName);
            }

            eventLog.Log = this.sourceName;

            foreach (EventLogEntry eventLogEntry in eventLog.Entries)
            {
                if ((eventLogEntry.TimeWritten) >= _startTime && (eventLogEntry.TimeWritten) <= _endTime)
                {
                    log = new Log();
                    log.Kind = eventLogEntry.EntryType.ToString();
                    log.Date = eventLogEntry.TimeWritten.Date.ToString().Substring(0, 10);
                    log.Time = eventLogEntry.TimeWritten.TimeOfDay.ToString();
                    log.Message = eventLogEntry.Message;
                    log.UserName = Encoding.Default.GetString(eventLogEntry.Data);
                    logList.Add(log);
                }
            }

            return logList;
        }

        /// <summary>
        /// 로컬에 윈도우 이벤트 로그를 기록한다.
        /// </summary>
        /// <param name="message">이벤트기록의 내용</param>
        /// <param name="userName">로그인된 사용자이름</param>
        /// <returns></returns>
        public bool WriteLog(string message, string userName)
        {
            try
            {
                if (!EventLog.SourceExists(this.sourceName))
                {
                    EventLog.CreateEventSource(this.sourceName, this.sourceName);
                }

                EventLog eventLog = new EventLog();
                eventLog.Log = this.sourceName;
                eventLog.Source = this.sourceName + "_이벤트";

                byte[] rawData = Encoding.Default.GetBytes(userName);
                eventLog.WriteEntry(message, EventLogEntryType.Information, 0, 0, rawData);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[EventLogManager] WriteLog (Exception: " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[EventLogManager] WriteLog( " + ex.ToString() + " )");

                return false;
            }
        }
        /// <summary>
        /// 로컬에 윈도우 이벤트 로그를 기록한다.
        /// </summary>
        /// <param name="message">이벤트기록의 내용</param>
        /// <returns></returns>
        public bool WriteLog(string message)
        {
            try
            {
                if (!EventLog.SourceExists(this.sourceName))
                {
                    EventLog.CreateEventSource(this.sourceName, this.sourceName);
                }

                EventLog eventLog = new EventLog();
                eventLog.Log = this.sourceName;
                eventLog.Source = this.sourceName + "_이벤트";

                byte[] rawData = Encoding.Default.GetBytes(currentUserNme);
                eventLog.WriteEntry(message, EventLogEntryType.Information, 0, 0, rawData);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[EventLogManager] WriteLog2 ( Exception: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[EventLogManager] WriteLog2( " + ex.ToString() + " )");

                return false;
            }
        }
        /// <summary>
        /// 로컬에 윈도우 이벤트 로그를 기록한다.
        /// </summary>
        /// <param name="type">이벤트뷰어의 종류</param>
        /// <param name="message">이벤트기록의 내용</param>
        /// <param name="userName">로그인된 사용자이름</param>
        /// <returns></returns>
        public bool WriteLog(EventLogEntryType type, string message, string userName)
        {
            try
            {
                if (!EventLog.SourceExists(this.sourceName))
                {
                    EventLog.CreateEventSource(this.sourceName, this.sourceName);
                }

                byte[] rawData = Encoding.Default.GetBytes(userName);

                EventLog eventLog = new EventLog();
                eventLog.Log = this.sourceName;
                eventLog.Source = this.sourceName + "_이벤트";
                eventLog.WriteEntry(message, type, 0, 0, rawData);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[EventLogManager] WriteLog3 ( Exception: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[EventLogManager] WriteLog3( " + ex.ToString() + " )");

                return false;
            }
        }
        /// <summary>
        /// 로컬에 윈도우 이벤트 로그를 기록한다.
        /// </summary>
        /// <param name="type">이벤트뷰어의 종류</param>
        /// <param name="message">이벤트기록의 내용</param>
        /// <returns></returns>
        public bool WriteLog(EventLogEntryType type, string message)
        {
            try
            {
                if (!EventLog.SourceExists(this.sourceName))
                {
                    EventLog.CreateEventSource(this.sourceName, this.sourceName);
                }

                byte[] rawData = Encoding.Default.GetBytes(currentUserNme);

                EventLog eventLog = new EventLog();
                eventLog.Log = this.sourceName;
                eventLog.Source = this.sourceName + "_이벤트";
                eventLog.WriteEntry(message, type, 0, 0, rawData);

                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[EventLogManager] WriteLog4 ( Exception: " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[EventLogManager] WriteLog4( " + ex.ToString() + " )");

                return false;
            }
        }
    }

    /// <summary>
    /// 로그 기록 및 로드에 사용할 클래스
    /// </summary>
    public class Log
    {
        private string kind = String.Empty;
        private string date = String.Empty;
        private string time = String.Empty;
        private string message = String.Empty;
        private string userName = String.Empty;

        public string Kind
        {
            get { return kind; }
            set { kind = value; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }
        public string Time
        {
            get { return time; }
            set { time = value; }
        }
        public string Message
        {
            get { return message; }
            set { message = value; }
        }
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        /// <summary>
        /// 생성자
        /// </summary>
        public Log()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="_kind"></param>
        /// <param name="_date"></param>
        /// <param name="_time"></param>
        /// <param name="message"></param>
        /// <param name="userName"></param>
        public Log(string _kind, string _date, string _time, string _message, string _userName)
        {
            this.kind = _kind;
            this.date = _date;
            this.time = _time;
            this.message = _message;
            this.userName = _userName;
        }
    }
}