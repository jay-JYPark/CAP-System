using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Diagnostics;

using Adeng.Framework.Net;
using StdWarningInstallation.DataClass;


namespace StdWarningInstallation
{
    public class SessionManager
    {
        private AdengClientSocket socket = null;

        private string targetIP = string.Empty;
        private int targetPort = 0;

        public event EventHandler<ConnectEvtArgs> NotifyConnected;
        public event EventHandler<ConnectEvtArgs> NotifyDisconnected;
        public event EventHandler<ReceiveEvtArgs> NotifyDataReceived;


        /// <summary>
        /// 생성자
        /// 싱글톤 생성 메소드를 사용하여 인스턴스 생성할 것.
        /// </summary>
        public SessionManager()
        {
            FileLogManager.GetInstance().WriteLog("[SessionManager] 시작");
        }

        /// <summary>
        /// 파괴자
        /// 정상적이지 못한 종료의 경우, 이벤트 핸들러 제거
        /// </summary>
        ~SessionManager()
        {
            FileLogManager.GetInstance().WriteLog("[SessionManager] 종료");

            if (this.socket != null)
            {
                this.socket.connectEvtHandler -= new ConnectEvtHandler(socket_OnOpen);
                this.socket.closeEvtHandler -= new CloseEvtHandler(socket_OnClose);
                this.socket.recvEvtHandler -= new RecvEvtHandler(socket_OnReceive);

                if (this.socket.Connected)
                {
                    this.socket.Close();
                }
                this.socket = null;
            }
        }


        /// <summary>
        /// 접속할 대상지의 아이피와 포트 번호를 저장한다.
        /// </summary>
        /// <param name="newIP">접속 대상지 아이피</param>
        /// <param name="newPort">접속 대상지 포트</param>
        public void SetConnectionInfo(string newIP, string newPort)
        {
            FileLogManager.GetInstance().WriteLog("[SessionManager] SetConnectionInfo( start >> newIP=[" + newIP + "], newPort=[" + newPort + "] )");

            if (this.targetIP == newIP && this.targetPort.ToString() == newPort)
            {
                // 동일 정보인 경우, 아무 것도 하지 않음.
                FileLogManager.GetInstance().WriteLog("[SessionManager] SetConnectionInfo( end - 동일 정보 갱신은 무시 )");

                return;
            }

            this.targetIP = newIP;
            int portNumber = 0;
            if (int.TryParse(newPort, out portNumber))
            {
                this.targetPort = portNumber;
            }
            else
            {
                this.targetPort = 0;
            }

            // 변경된 정보로 재접속
            Disconnect();
            Connect();

            FileLogManager.GetInstance().WriteLog("[SessionManager] SetConnectionInfo( end )");
        }

        /// <summary>
        /// 대상지(서버)와 연결
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( start )");

            bool result = true;

            try
            {
                if (socket != null && !socket.Connected)
                {
                    FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( 통신 끊긴 상태=> 이벤트를 해제, 소켓 닫음 )");

                    // 끊긴 상태
                    this.socket.connectEvtHandler -= new ConnectEvtHandler(socket_OnOpen);
                    this.socket.closeEvtHandler -= new CloseEvtHandler(socket_OnClose);
                    this.socket.recvEvtHandler -= new RecvEvtHandler(socket_OnReceive);
                    this.socket.Close();
                    this.socket = null;

                    return false;
                }

                if (this.socket == null)
                {
                    FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( 소켓이 존재하지 않음 => 소켓 생성 )");

                    this.socket = new AdengClientSocket();
                    this.socket.connectEvtHandler += new ConnectEvtHandler(socket_OnOpen);
                    this.socket.closeEvtHandler += new CloseEvtHandler(socket_OnClose);
                    this.socket.recvEvtHandler += new RecvEvtHandler(socket_OnReceive);
                }

                if (!this.socket.Connected)
                {
                    this.socket.Connect(this.targetIP, this.targetPort);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] Connect( " + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( " + ex.ToString() + " )");

                if (socket != null)
                {
                    if (this.socket.Connected)
                    {
                        Disconnect();
                    }
                }

                result = false;
                throw ex;
            }

            FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( end )");

            return result;
        }

        /// <summary>
        /// 대상지(서버)와의 연결 해제
        /// </summary>
        /// <returns></returns>
        public bool Disconnect()
        {
            FileLogManager.GetInstance().WriteLog("[SessionManager] Disconnect( start )");

            if (this.socket == null)
            {
                FileLogManager.GetInstance().WriteLog("[SessionManager] Disconnect( socket is null )");

                return true;
            }

            bool result = true;
            try
            {
                this.socket.connectEvtHandler -= new ConnectEvtHandler(socket_OnOpen);
                this.socket.closeEvtHandler -= new CloseEvtHandler(socket_OnClose);
                this.socket.recvEvtHandler -= new RecvEvtHandler(socket_OnReceive);

                if (this.socket.Connected)
                {
                    this.socket.Close();
                }
                this.socket = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] Disconnect(Exception:" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[SessionManager] Disconnect( Exception=[" + ex.ToString() + " )");

                result = false;
                this.socket = null;
                throw ex;
            }

            FileLogManager.GetInstance().WriteLog("[SessionManager] Connect( end )");

            return result;
        }

        /// <summary>
        /// 데이터 전송.
        /// 이 함수는 처리 속도를 고려하여, Connect() 처리가 선행되는 것을 전제한다.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>데이터 전송 결과</returns>
        public bool SendData(byte[] data)
        {
            bool result = true;

            try
            {
                if (this.socket == null)
                {
                    System.Console.WriteLine("[SessionManager] SendData(socket is null)");
                    FileLogManager.GetInstance().WriteLog("[SessionManager] SendData( socket is null )");

                    return false;
                }
                else if (!this.socket.Connected)
                {
                    System.Console.WriteLine("[SessionManager] SendData(socket is not Connected!!)");
                    FileLogManager.GetInstance().WriteLog("[SessionManager] SendData( socket is not connected!! )");

                    this.socket.connectEvtHandler -= new ConnectEvtHandler(socket_OnOpen);
                    this.socket.closeEvtHandler -= new CloseEvtHandler(socket_OnClose);
                    this.socket.recvEvtHandler -= new RecvEvtHandler(socket_OnReceive);

                    this.socket.Close();
                    this.socket = null;

                    System.Console.WriteLine("[SessionManager] SendData(socket is disconnected)");
                    return false;
                }
                else
                {
                    // 
                }

                this.socket.Send(data, data.Length);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] SendData(Exception:" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[SessionManager] SendData( " + ex.ToString() + " )");

                result = false;
            }

            return result;
        }


        #region 소켓 통지 이벤트 핸들러
        /// <summary>
        /// 소켓 연결 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void socket_OnOpen(object sender, AdengConnectEvtArgs e)
        {
            System.Console.WriteLine("[SessionManager] socket_OnOpen(Connected=" + e.Connected.ToString() + ")");
            FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnOpen( e.Connected=[" + e.Connected.ToString() + "] )");

            try
            {
                if (e.Connected)
                {
                    if (this.NotifyConnected != null)
                    {
                        this.NotifyConnected(this, new ConnectEvtArgs(e.ClientSocket));
                    }

                }
                else
                {
                    if (this.NotifyDisconnected != null)
                    {
                        this.NotifyDisconnected(this, new ConnectEvtArgs(e.ClientSocket));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] socket_OnOpen (Exception Occured!! \n " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnOpen( " + ex.ToString() + " )");
            }
        }
        /// <summary>
        /// 소켓 연결 해제 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void socket_OnClose(object sender, AdengCloseEvtArgs e)
        {
            System.Console.WriteLine("[SessionManager] socket_OnClose( Connected=" + e.ClientSocket.Connected.ToString() + " )");
            FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnClose( e.Conected=[" + e.ClientSocket.Connected.ToString() + "] )");

            try
            {
                if (this.NotifyDisconnected != null)
                {
                    this.NotifyDisconnected(this, new ConnectEvtArgs(e.ClientSocket));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] socket_OnClose (Exception Occured!! \n " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnClose( Exception=[" + ex.ToString() + "] )");
            }
        }
        /// <summary>
        /// 데이터 수신 이벤트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void socket_OnReceive(object sender, AdengRecvEvtArgs e)
        {
            System.Console.WriteLine("[SessionManager] socket_OnReceive( start )");
            //FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnReceive( 데이터 수신 )");

            try
            {
                byte[] tmpByte = new byte[e.Len];
                Buffer.BlockCopy(e.Buff, 0, tmpByte, 0, e.Len);

                if (this.NotifyDataReceived != null)
                {
                    this.NotifyDataReceived(this, new ReceiveEvtArgs(e.ClientSocket, tmpByte));
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[SessionManager] socket_OnReceive (Exception Occured!! \n " + ex.ToString());
                FileLogManager.GetInstance().WriteLog("[SessionManager] socket_OnReceive( e.Conected=[" + ex.ToString() + "] )");
            }
        }
        #endregion
    }
}