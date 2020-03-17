using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Adeng.Framework.Net;
using CAPLib;

namespace StdWarningInstallation.DataClass
{
    public class EventArgsDefinitions
    {
        public string test;
    }

     /// <summary>
     /// 통합경보게이트웨이 연결 정보 아규먼트
     /// </summary>
    public class IAGWConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// 인증 여부
        /// </summary>
        private bool isAuthenticated = false;
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set { isAuthenticated = value; }
        }

        /// <summary>
        /// 통합경보게이트웨이에 연결 여부
        /// </summary>
        private bool isConnected = false;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set { this.isConnected = value; }
        }

        private int errorCode = 0;
        public int ErrorCode
        {
            get { return errorCode; }
            set { errorCode = value; }
        }

        public bool DeepCopyFrom(IAGWConnectionEventArgs src)
        {
            System.Diagnostics.Debug.Assert(src != null);

            if (src == null)
            {
                return false;
            }

            this.isAuthenticated = src.isAuthenticated;
            this.isConnected = src.isConnected;
            this.errorCode = src.errorCode;

            return true;
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionState">
        /// true - 연결
        /// false - 종료
        /// </param>
        public IAGWConnectionEventArgs()
        {
        }
        public IAGWConnectionEventArgs(bool authState, bool connectionState)
        {
            this.isAuthenticated = authState;
            this.isConnected = connectionState;
        }
        public IAGWConnectionEventArgs(bool authState, bool connectionState, int error)
        {
            this.isAuthenticated = authState;
            this.isConnected = connectionState;
            this.errorCode = error;
        }
    }
    public class ConnectionStateEventArgs : EventArgs
    {
        /// <summary>
        /// 통합경보게이트웨이에 연결 여부
        /// </summary>
        private bool isConnected = false;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set { this.isConnected = value; }
        }
        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="connectionState">
        /// true - 연결
        /// false - 종료
        /// </param>
        public ConnectionStateEventArgs()
        {
        }
        public ConnectionStateEventArgs(bool connectionState)
        {
            this.isConnected = connectionState;
        }
    }

    /// <summary>
    /// 발령에 대한 응답 이벤트 아규먼트 클래스
    /// </summary>
    public class OrderResultEventArgs : EventArgs
    {
        //private IEASPrtCmd2 cmd2 = null;

        //public IEASPrtCmd2 CMD2
        //{
        //    get { return this.cmd2; }
        //    set { this.cmd2 = value; }
        //}

        ///// <summary>
        ///// 생성자
        ///// </summary>
        ///// <param name="_cmd2"></param>
        //public OrderResultEventArgs(IEASPrtCmd2 _cmd2)
        //{
        //    this.cmd2 = _cmd2;
        //}
    }

    /// <summary>
    /// 접속 인증에 대한 결과 이벤트 아규먼트
    /// </summary>
    public class AuthEventArgs : EventArgs
    {
        private bool isAuthenticated;
        public bool IsAuthenticated
        {
            get { return isAuthenticated; }
            set { isAuthenticated = value; }
        }

        public AuthEventArgs(bool isAuthenticated)
        {
            this.isAuthenticated = isAuthenticated;
        }
        //private IEASPrtCmd4 cmd4 = null;

        //public IEASPrtCmd4 CMD4
        //{
        //    get { return this.cmd4; }
        //    set { this.cmd4 = value; }
        //}

        ///// <summary>
        ///// 생성자
        ///// </summary>
        ///// <param name="_cmd4"></param>
        //public AuthEventArgs(IEASPrtCmd4 _cmd4)
        //{
        //    this.cmd4 = _cmd4;
        //}
    }
    /// <summary>
    /// CAP 데이터 수신 이벤트 아규먼트
    /// </summary>
    public class CapEventArgs : EventArgs
    {
        private ReceivedCAPInfo data;
        public ReceivedCAPInfo Data
        {
            get { return data; }
            set { data = value; }
        }

        public CapEventArgs(ReceivedCAPInfo input)
        {
            this.data = input;
        }
    }
    /// <summary>
    /// </summary>
    public class DataSyncEvtArgs : EventArgs
    {
        private SynchronizationReqData data;
        public SynchronizationReqData Data
        {
            get { return data; }
            set { data = value; }
        }

        public DataSyncEvtArgs(SynchronizationReqData input)
        {
            this.data = input;
        }
    }

    /// <summary>
    /// 소켓 연결/해제 시도에 대한 결과 이벤트 아규먼트 클래스
    /// </summary>
    public class ConnectEvtArgs : EventArgs
    {
        private AdengClientSocket socket;

        public AdengClientSocket Socket
        {
            get { return this.socket; }
            set { this.socket = value; }
        }

        public ConnectEvtArgs(AdengClientSocket _socket)
        {
            this.socket = _socket;
        }
    }

    /// <summary>
    /// 데이터 수신 이벤트 아규먼트 클래스
    /// </summary>
    public class ReceiveEvtArgs : EventArgs
    {
        private AdengClientSocket socket;
        private byte[] buff = null;

        public AdengClientSocket Socket
        {
            get { return this.socket; }
            set { this.socket = value; }
        }

        public byte[] Buff
        {
            get { return this.buff; }
            set { this.buff = value; }
        }

        public ReceiveEvtArgs(AdengClientSocket _socket, byte[] _buff)
        {
            this.socket = _socket;
            this.buff = _buff;
        }
    }

}
