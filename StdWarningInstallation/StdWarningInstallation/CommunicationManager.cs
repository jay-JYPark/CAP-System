using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

using IEASProtocol;
using CAPLib;
using StdWarningInstallation.DataClass;

namespace StdWarningInstallation
{
    class CommunicationManager
    {
        private static CommunicationManager thisObj = null;
        private static Mutex mutex = new Mutex();
        private ManualResetEvent manualEvtReceiveData = null;
        private ManualResetEvent manualEvtPolling = null;

        private SessionManager sessionManager = new SessionManager();
        private IAGWConnectionEventArgs currentIAGWConnectionState = new IAGWConnectionEventArgs();

        private Thread dataProcessingThread = null;                         // 수신 데이터 처리 쓰레드
        private Queue<byte[]> receivedPacketQueue = new Queue<byte[]>();    // 수신 데이터 저장 큐
        private byte[] remainderPacket = null;                              // 프레임 단위로 처리하고 남은 미처리 패킷
        private bool isReceivedDataProcessingContinue = false;

        private Thread pollingThread = null;    // 폴링 체크 쓰레드
        private bool isPollingContinue = false;        // 폴링 체크 쓰레드 제어 용 플래그

        public event EventHandler<IAGWConnectionEventArgs> NotifyIAGWConnectionState;
        public event EventHandler<CapEventArgs> NotifyCAPReceived;
        public event EventHandler<DataSyncEvtArgs> NotifyDataSyncRequested;


        /// <summary>
        /// 생성자
        /// </summary>
        /// <returns></returns>
        private CommunicationManager()
        {
            this.sessionManager.NotifyConnected += new EventHandler<ConnectEvtArgs>(this.sessionManager_OnNotifyConnected);
            this.sessionManager.NotifyDisconnected += new EventHandler<ConnectEvtArgs>(this.sessionManager_OnNotifyDisconnected);
            this.sessionManager.NotifyDataReceived += new EventHandler<ReceiveEvtArgs>(this.sessionManager_OnNotifyDataReceived);
        }

        /// <summary>
        /// 싱글톤 인스턴스 취득
        /// </summary>
        /// <returns>통신관리 클래스의 인스턴스</returns>
        public static CommunicationManager GetInstance()
        {
            mutex.WaitOne();

            if (thisObj == null)
            {
                thisObj = new CommunicationManager();
            }

            mutex.ReleaseMutex();
            return thisObj;
        }
        
        /// <summary>
        /// 파괴자
        /// 정상적이지 못한 종료의 경우, 이벤트 핸들러 제거
        /// </summary>
        ~CommunicationManager()
        {
            CommunicationManager.GetInstance().DisconnectWithGateway();
            EndDataProcessing();
            EndConnectionSupervisory();

            this.sessionManager.NotifyConnected -= new EventHandler<ConnectEvtArgs>(this.sessionManager_OnNotifyConnected);
            this.sessionManager.NotifyDisconnected -= new EventHandler<ConnectEvtArgs>(this.sessionManager_OnNotifyDisconnected);
            this.sessionManager.NotifyDataReceived -= new EventHandler<ReceiveEvtArgs>(this.sessionManager_OnNotifyDataReceived);
        }

        /// <summary>
        /// 접속할 대상지의 아이피와 포트 번호를 저장한다.
        /// </summary>
        /// <param name="ip">접속 대상지 아이피</param>
        /// <param name="port">접속 대상지 포트</param>
        /// <returns></returns>
        public void SetConnectionInfo(string ip, string port)
        {
            this.sessionManager.SetConnectionInfo(ip, port);
        }
        /// <param name="newInfo">통합경보게이트웨이 접속 정보</param>
        /// <returns></returns>
        public void SetConnectionInfo(ConfigIAGWData newInfo)
        {
            this.sessionManager.SetConnectionInfo(newInfo.ServerIP, newInfo.ServerPort);
        }

        /// <summary>
        /// 게이트웨이에 접속.
        /// 입력 파라미터가 없는 경우, 하위의 세션 매니저가 저장하고 있는 접속 정보로 연결한다.
        /// </summary>
        /// <returns>접속 결과</returns>
        public bool ConnectToGateway()
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이에 접속 요청");

            return this.sessionManager.Connect();
        }
        public bool ConnectToGateway(string targetIP, string targetPort)
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이에 접속 요청");

            this.sessionManager.SetConnectionInfo(targetIP, targetPort);
            return this.sessionManager.Connect();
        }
        /// <summary>
        /// 게이트웨이와의 접속 해제.
        /// </summary>
        /// <returns>해제 결과</returns>
        public bool DisconnectWithGateway()
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이에 해제 요청");

            bool result = false;
            try
            {
                result = this.sessionManager.Disconnect();

                if (this.NotifyIAGWConnectionState != null)
                {
                    IAGWConnectionEventArgs copy = new IAGWConnectionEventArgs();
                    lock (this.currentIAGWConnectionState)
                    {
                        this.currentIAGWConnectionState.IsAuthenticated = false;
                        this.currentIAGWConnectionState.IsConnected = false;

                        copy.DeepCopyFrom(this.currentIAGWConnectionState);
                    }
                    this.NotifyIAGWConnectionState(this, copy);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] DisconnectWithGateway (Exception:" + ex.ToString() + ")");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] DisconnectWithGateway( Exception=[" + ex.ToString() + "] )");
            }

            return result;
        }

        /// <summary>
        /// 게이트웨이 연결 상태 감시 쓰레딩 시작.
        /// </summary>
        public bool PrepareConnectionSupervisory()
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이 연결 상태 감시 쓰레드 준비");

            bool result = false;
            try
            {
                if (this.pollingThread == null)
                {
                    this.pollingThread = new Thread(this.Polling);
                    this.pollingThread.IsBackground = true;
                    this.pollingThread.Name = "PollingThread";
                    this.pollingThread.Start();
                }

                result = true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] PrepareConnectionSupervisory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] PrepareConnectionSupervisory( Exception=[" + ex.ToString() + "] )");

                EndConnectionSupervisory();
                return false;
            }

            return result;
        }
        /// <summary>
        /// 게이트웨이 연결 상태 감시 쓰레딩 시작.
        /// </summary>
        public int StartConnectionSupervisory()
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이 연결 상태 감시 시작");

            try
            {
                if (this.pollingThread == null)
                {
                    PrepareConnectionSupervisory();
                }

                this.isPollingContinue = true;

                if (this.manualEvtPolling == null)
                {
                    this.manualEvtPolling = new ManualResetEvent(false);
                }
                this.manualEvtPolling.Set();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] StartConnectionSupervisory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] StartConnectionSupervisory( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        /// <summary>
        /// 게이트웨이 연결 상태 감시 쓰레딩 종료.
        /// </summary>
        public int EndConnectionSupervisory()
        {
            System.Console.WriteLine("[CommunicationManager] 게이트웨이 연결 상태 감시 종료");

            try
            {
                this.isPollingContinue = false;
                if (this.manualEvtPolling != null)
                {
                    this.manualEvtPolling.Set();
                }
                if (this.pollingThread != null && this.pollingThread.IsAlive)
                {
                    bool terminated = this.pollingThread.Join(500);
                    if (!terminated)
                    {
                        this.pollingThread.Abort();
                    }
                }
                this.pollingThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] EndConnectionSupervisory( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] EndConnectionSupervisory( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }

        /// <summary>
        /// 데이터 수신 처리 쓰레딩 시작.
        /// </summary>
        public int StartDataProcessing()
        {
            try
            {
                this.isReceivedDataProcessingContinue = true;
                if (this.dataProcessingThread == null)
                {
                    this.dataProcessingThread = new Thread(new ThreadStart(ReceiveDataProcessing));
                    this.dataProcessingThread.IsBackground = true;
                    this.dataProcessingThread.Name = "DataProcessingThread";
                    this.dataProcessingThread.Start();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] StartDataProcessing( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] StartDataProcessing( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        /// <summary>
        /// 데이터 수신 처리 쓰레딩 종료.
        /// </summary>
        public int EndDataProcessing()
        {
            try
            {
                this.isReceivedDataProcessingContinue = false;
                if (this.manualEvtReceiveData != null)
                {
                    this.manualEvtReceiveData.Set();
                }
                if (this.dataProcessingThread != null && this.dataProcessingThread.IsAlive)
                {
                    bool isTerminated = this.dataProcessingThread.Join(500);
                    if (!isTerminated)
                    {
                        this.dataProcessingThread.Abort();
                    }
                }
                this.dataProcessingThread = null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] EndDataProcessing( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] EndDataProcessing( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }

        /// <summary>
        /// 인증 요청
        /// </summary>
        public  bool RequestAuth(string authCode)
        {
            bool result = false;
            try
            {
                IEASProtocolBase protoBase = IEASProtocolManager.CreateProtocolForKCAP(KCAPCmdValue.RequestAuth);
                protoBase.SenderType = IEASSenderType.SWI;
                IEASPrtCmd3 protoCmd3 = protoBase as IEASPrtCmd3;
                protoCmd3.AuthentiCode = authCode;
                byte[] authRequest = IEASProtocolManager.MakeFrameForKCAP(protoCmd3);

                result = this.sessionManager.SendData(authRequest);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] RequestAuth( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] RequestAuth( Exception=[" + ex.ToString() + "] )");

                return false;
            }

            return result;
        }
        /// <summary>
        /// 게이트웨이로 CAP 메시지 전송
        /// </summary>
        public int SendCAP(string xmlCAPData)
        {
            try
            {
                IEASProtocolBase protoBase = IEASProtocolManager.CreateProtocolForKCAP(KCAPCmdValue.Order);
                protoBase.SenderType = IEASSenderType.SWI;
                IEASPrtCmd1 protoCmd1 = protoBase as IEASPrtCmd1;
                protoCmd1.CAPMessage = xmlCAPData;
                byte[] frameData = IEASProtocolManager.MakeFrameForKCAP(protoCmd1);

                bool result = this.sessionManager.SendData(frameData);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] SendCAP( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] SendCAP( Exception=[" + ex.ToString() + "] )");

                return -99;
            }
            return 0;
        }
        /// <summary>
        /// 데이터 동기화 수신 데이터 CRC16 체크 결과 전송
        /// </summary>
        /// <param name="validationResult"></param>
        /// <returns></returns>
        public int SendResultOfCheckDataValidation(uint requestEventID, DataValidationCheckResult validationResult, List<uint> lstPacketNo = null)
        {
            int result = 0;

            try
            {
                SYNCProtocolBase protoBase = IEASProtocolManager.CreateProtocolForSYNC(SYNCCmdValue.RecvResult);
                SYNCPrtCmd10 cmd10 = protoBase as SYNCPrtCmd10;
                cmd10.Identifier = requestEventID;
                cmd10.Result = RecvResult.Success;

                if (lstPacketNo == null)
                {
                    cmd10.Num = 0;
                    byte[] frameData = IEASProtocolManager.MakeFrameForSYNC(cmd10);

                    bool sendResult = this.sessionManager.SendData(frameData);
                    if (!sendResult)
                    {
                        result--;
                    }
                }
                else
                {
                    foreach (uint packetNo in lstPacketNo)
                    {
                        cmd10.Num = packetNo;
                        byte[] frameData = IEASProtocolManager.MakeFrameForSYNC(cmd10);

                        bool sendResult = this.sessionManager.SendData(frameData);
                        if (!sendResult)
                        {
                            result--;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] SendResultOfCheckDataValidation( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] SendResultOfCheckDataValidation( Exception=[" + ex.ToString() + "] )");

                return -99;
            }
            return result;
        }
        /// <summary>
        /// 게이트웨이로 데이터동기화의 동기 처리 결과 메시지 전송.
        /// </summary>
        public bool SendResultOfSASProfileUpdate(uint requestEventID, int updateResult)
        {
            bool result = false;
            try
            {
                SyncResult syncResult = SyncResult.Success;
                if (updateResult != 0)
                {
                    syncResult = SyncResult.Fail;
                }

                SYNCProtocolBase protoBase = IEASProtocolManager.CreateProtocolForSYNC(SYNCCmdValue.SyncResult);
                SYNCPrtCmd13 cmd13 = protoBase as SYNCPrtCmd13;
                cmd13.Result = syncResult;
                cmd13.Identifier = requestEventID;
                byte[] frameData = IEASProtocolManager.MakeFrameForSYNC(cmd13);

                result = this.sessionManager.SendData(frameData);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] SendResultOfSASProfileUpdate( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] SendResultOfSASProfileUpdate( Exception=[" + ex.ToString() + "] )");

                return false;
            }
            return result;
        }
        /// <summary>
        /// 게이트웨이로 전체 해쉬키 체크 결과 전송
        /// </summary>
        public bool SendResultOfCheckEntireHashKey(uint requestEventID, bool isMatched)
        {
            bool result = false;
            try
            {
                ChkTotalHashResult checkResult = (isMatched ? ChkTotalHashResult.Success : ChkTotalHashResult.Fail);
                SYNCProtocolBase protoBase = IEASProtocolManager.CreateProtocolForSYNC(SYNCCmdValue.ChkTotalHashResult);
                SYNCPrtCmd11 cmd11 = protoBase as SYNCPrtCmd11;
                cmd11.Result = checkResult;
                cmd11.Identifier = requestEventID;
                byte[] frameData = IEASProtocolManager.MakeFrameForSYNC(cmd11);

                result = this.sessionManager.SendData(frameData);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] SendResultOfCheckEntireHashKey( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] SendResultOfCheckEntireHashKey( Exception=[" + ex.ToString() + "] )");

                return false;
            }
            return result;
        }
        /// <summary>
        /// 게이트웨이로 개별 해쉬키 체크 결과 전송.
        /// 전체 해쉬키가 불일치일 때만 개별해쉬키 체크가 수행되므로, 불일치 프로필 아이디가 널인 경우는 논리적으로는 있을 수 없다.
        /// </summary>
        public bool SendResultOfCheckSingleHashKey(uint requestEventID, List<string> lstProfileID)
        {
            System.Console.WriteLine("[CommunicationManager] SendResultOfCheckSingleHashKey (requestEventID=[" + requestEventID + "])");

            bool sendResult = true ;

            try
            {
                if (lstProfileID == null || lstProfileID.Count <= 0)
                {
                    System.Console.WriteLine("[ERROR] 모든 데이터가 동기화된 상태!!!");

                    // 빈 패킷이라도 보내야 하나?
                }
                else
                {
                    uint totalCount = (uint)(lstProfileID.Count());
                    uint currentNo = 1;
                    foreach (string profileID in lstProfileID)
                    {
                        SYNCProtocolBase protoBase = IEASProtocolManager.CreateProtocolForSYNC(SYNCCmdValue.ChkSingleHashResult);
                        SYNCPrtCmd12 cmd12 = protoBase as SYNCPrtCmd12;
                        cmd12.Identifier = requestEventID;
                        cmd12.TotalCount = totalCount;
                        cmd12.Num = currentNo++;
                        cmd12.ProfileID = profileID;

                        byte[] frameData = IEASProtocolManager.MakeFrameForSYNC(cmd12);

                        bool result = this.sessionManager.SendData(frameData);
                        if (!result)
                        {
                            System.Console.WriteLine("패킷 전송 실패(currentNo=" + currentNo + ")");
                            sendResult = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] SendResultOfCheckSingleHashKey( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] SendResultOfCheckSingleHashKey( Exception=[" + ex.ToString() + "] )");

                return false;
            }

            System.Console.WriteLine("[CommunicationManager] SendResultOfCheckSingleHashKey (sendResult=[" + sendResult.ToString() + "])");

            return sendResult;
        }

        /// <summary>
        /// 큐에 저장된 패킷 데이터를 프레임 단위로 파싱.
        /// </summary>
        private int ParsingQueuingData(byte[] queuingData)
        {
            try
            {
                if (queuingData == null)
                {
                    System.Console.WriteLine("[CommunicationManager] 파싱 실패 : 입력 파라미터가 널");
                    return -1;
                }

                AnalyzeResult frameResult = IEASProtocolManager.AnalyzeFrame(this.remainderPacket, queuingData);
                if (frameResult == null || frameResult.FrameInfo == null)
                {
                    System.Console.WriteLine("[CommunicationManager] 파싱 실패 : 입력 파라미터가 널");
                    return -2;
                }

                // 프레임을 분리하고 남은 불완전 데이터를 남은데이터 보관 버퍼(로컬)에 저장
                this.remainderPacket = null;
                if (frameResult.RemainderFrame != null && frameResult.RemainderFrame.Length > 0)
                {
                    this.remainderPacket = new byte[frameResult.RemainderFrame.Length];
                    Buffer.BlockCopy(frameResult.RemainderFrame, 0, this.remainderPacket, 0, frameResult.RemainderFrame.Length);
                }

                // 파싱 실패 체크: 프레임 단위로 분리할 만한 데이터 크기가 아닌 경우 등
                if (frameResult.FrameInfo.Count == 0)
                {
                    System.Console.WriteLine("[CommunicationManager] 프레임 분리 실패(길이 부족 등)");
                    return -3;
                }

                for (int index = 0; index < frameResult.FrameInfo.Count; index++)
                {
                    Frame currentFrame = frameResult.FrameInfo[index];
                    if (currentFrame.HeaderKind == HeaderKind.KCAP)
                    {
                        IEASProtocolBase baseData = IEASProtocolManager.ParseFrameForKCAP(currentFrame.Data);
                        DistributeKCAPCommandData(baseData.CmdValue, baseData);
                    }
                    else
                    {
                        SYNCProtocolBase baseData = IEASProtocolManager.ParseFrameForSYNC(currentFrame.Data);
                        DistributeSYNCCommandData(baseData.CmdValue, baseData);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] ParsingQueuingData( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] ParsingQueuingData( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        /// <summary>
        /// 큐에 저장된 패킷 데이터를 프레임 단위로 파싱.
        /// </summary>
        private int DistributeKCAPCommandData(KCAPCmdValue command, IEASProtocolBase baseData)
        {
            try
            {
                System.Console.WriteLine("[CommunicationManager] KCAP 프레임 데이터 분배 - command(" + command + ")");
                switch (command)
                {
                    case KCAPCmdValue.OrderResponse:
                        {
                            IEASPrtCmd2 protoCmd2 = baseData as IEASPrtCmd2;
                            if (this.NotifyCAPReceived != null)
                            {
                                CAP capMsg = new CAP(protoCmd2.CAPMessage);
                                SenderTypes senderType = ConvertToLocalSenderType(protoCmd2.SenderType);
                                ReceivedCAPInfo capInfo = new ReceivedCAPInfo(senderType, capMsg);
                                this.NotifyCAPReceived(this, new CapEventArgs(capInfo));
                            }
                        }
                        break;
                    case KCAPCmdValue.AuthResult:
                        {
                            IEASPrtCmd4 protoCmd4 = baseData as IEASPrtCmd4;
                            if (this.NotifyIAGWConnectionState != null)
                            {
                                if (protoCmd4.SenderType == IEASSenderType.IAGW)
                                {
                                    bool authResult = (protoCmd4.AuthentiResult == 0x01) ? true : false;
                                    IAGWConnectionEventArgs copy = new IAGWConnectionEventArgs();
                                    lock (this.currentIAGWConnectionState)
                                    {
                                        this.currentIAGWConnectionState.IsAuthenticated = authResult;
                                        copy.DeepCopyFrom(this.currentIAGWConnectionState);
                                    }
                                    this.NotifyIAGWConnectionState(this, copy);
                                }
                                else
                                {
                                    // 메시지 유효성 오류: 무시
                                }
                            }
                        }
                        break;
                    case KCAPCmdValue.Order:
                        {
                            IEASPrtCmd1 protoCmd1 = baseData as IEASPrtCmd1;
                            if (this.NotifyCAPReceived != null)
                            {
                                CAP capMsg = new CAP(protoCmd1.CAPMessage);
                                SenderTypes senderType = ConvertToLocalSenderType(protoCmd1.SenderType);
                                ReceivedCAPInfo capInfo = new ReceivedCAPInfo(senderType, capMsg);
                                this.NotifyCAPReceived(this, new CapEventArgs(capInfo));
                            }
                        }
                        break;
                    case KCAPCmdValue.Polling:
                    default:
                        {
                            // do nothing
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] DistributeKCAPCommandData( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] DistributeKCAPCommandData( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        private int DistributeSYNCCommandData(SYNCCmdValue command, SYNCProtocolBase baseData)
        {
            System.Console.WriteLine("[CommunicationManager] SYNC 프레임 데이터 분배 - command(" + command.ToString() + ")");

            try
            {
                switch (command)
                {
                    case SYNCCmdValue.ChkTotalHash:// 전체 해쉬키 체크 요청
                    case SYNCCmdValue.ChkSingleHash:
                    case SYNCCmdValue.UpdateProfile:
                        {
                            if (this.NotifyDataSyncRequested != null)
                            {
                                SynchronizationReqData syncReqData = ConvertToSynchronizationReqData(command, baseData);
                                this.NotifyDataSyncRequested(this, new DataSyncEvtArgs(syncReqData));
                            }
                        }
                        break;
                    default:
                        {
                            // do nothing
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] DistributeSYNCCommandData( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] DistributeSYNCCommandData( Exception=[" + ex.ToString() + "] )");

                return -99;
            }

            return 0;
        }
        private SynchronizationReqData ConvertToSynchronizationReqData(SYNCCmdValue command, SYNCProtocolBase baseData)
        {
            SynchronizationReqData reqData = null;

            switch (command)
            {
                case SYNCCmdValue.ChkTotalHash:// 전체 해쉬키 체크 요청
                    {
                        reqData = new SynchronizationReqData();
                        reqData.Command = DataSyncCommand.ChkEntireHashKey;

                        SYNCPrtCmd1 protoCmd1 = baseData as SYNCPrtCmd1;

                        reqData.EventID = protoCmd1.Identifier;
                        reqData.ProfileHashKey = protoCmd1.HashKey;
                        reqData.Crc16 = protoCmd1.Crc16;
                    }
                    break;
                case SYNCCmdValue.ChkSingleHash:
                    {
                        reqData = new SynchronizationReqData();
                        reqData.Command = DataSyncCommand.ChkSingleHashKey;

                        SYNCPrtCmd2 protoCmd2 = baseData as SYNCPrtCmd2;

                        reqData.EventID = protoCmd2.Identifier;
                        reqData.TotalPacketCnt = protoCmd2.TotalCount;
                        reqData.CurrentPacketNo = protoCmd2.Num;
                        reqData.ProfileID = protoCmd2.ProfileID;
                        reqData.ProfileHashKey = protoCmd2.HashKey;
                        reqData.Crc16 = protoCmd2.Crc16;
                    }
                    break;
                case SYNCCmdValue.UpdateProfile:
                    {
                        reqData = new SynchronizationReqData();
                        reqData.Command = DataSyncCommand.UpdateProfileData;

                        SYNCPrtCmd3 protoCmd3 = baseData as SYNCPrtCmd3;

                        reqData.EventID = protoCmd3.Identifier;
                        reqData.TotalPacketCnt = protoCmd3.TotalCount;
                        reqData.CurrentPacketNo = protoCmd3.Num;
                        switch (protoCmd3.Mode)
                        {
                            case SyncUpdateMode.Regist:
                                reqData.Mode = ProfileUpdateMode.Regist;
                                break;
                            case SyncUpdateMode.Modify:
                                reqData.Mode = ProfileUpdateMode.Modify;
                                break;
                            case SyncUpdateMode.Delete:
                                reqData.Mode = ProfileUpdateMode.Delete;
                                break;
                            default:
                                break;
                        }
                        reqData.ProfileID = protoCmd3.ID;

                        reqData.SystemProfile = new SASProfile();
                        reqData.SystemProfile.ID = protoCmd3.ID;
                        reqData.SystemProfile.Name = protoCmd3.SystemName;
                        reqData.SystemProfile.AuthCode = protoCmd3.AuthCode;
                        reqData.SystemProfile.KindCode = protoCmd3.SystemKindCode;
                        reqData.SystemProfile.IpType = IPType.IPv4;
                        if (protoCmd3.IPType == IPAddressType.IPv4)
                        {
                            reqData.SystemProfile.IpType = IPType.IPv4;
                        }
                        else if (protoCmd3.IPType == IPAddressType.IPv6)
                        {
                            reqData.SystemProfile.IpType = IPType.IPv6;
                        }
                        else
                        {
                            //
                        }
                        reqData.SystemProfile.IpAddress = protoCmd3.IPAddress;
                        reqData.SystemProfile.Latitude = protoCmd3.Latitude.ToString();
                        reqData.SystemProfile.Longitude = protoCmd3.Longitude.ToString();
                        reqData.SystemProfile.AssignedIAGWRegionCode = protoCmd3.AssignedIAGWRegionCode;
                        reqData.SystemProfile.Address = protoCmd3.Address;
                        reqData.SystemProfile.ManagerName = protoCmd3.ManagerName;
                        reqData.SystemProfile.ManagerDepartment = protoCmd3.ManagerDepartment;
                        reqData.SystemProfile.ManagerPhone = protoCmd3.ManagerPhone;
                    }
                    break;
                default:
                    {
                        // do nothing
                    }
                    break;
            }
            return reqData;
        }
        /// <summary>
        /// IEASProtocol.IEASSenderType 을 로컬 데이터 타입 SenderTypes 형으로 변환.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private SenderTypes ConvertToLocalSenderType(IEASSenderType type)
        {
            SenderTypes senderType = SenderTypes.IAGW;
            switch (type)
            {
                case IEASSenderType.SAS:
                    {
                        senderType = SenderTypes.SAS;
                    }
                    break;
                case IEASSenderType.SWI:
                    {
                        senderType = SenderTypes.SWI;
                    }
                    break;
                case IEASSenderType.IAGW:
                default:
                    {
                        senderType = SenderTypes.IAGW;
                    }
                    break;
            }

            return senderType;
        }

        #region 쓰레드 함수
        /// <summary>
        /// 수신 데이터 처리 (쓰레드 호출용 함수)
        /// </summary>
        private void ReceiveDataProcessing()
        {
            try
            {
                if (this.manualEvtReceiveData == null)
                {
                    this.manualEvtReceiveData = new ManualResetEvent(false);
                }

                while (this.isReceivedDataProcessingContinue)
                {
                    int count = 0;
                    lock (this.receivedPacketQueue)
                    {
                        count = this.receivedPacketQueue.Count;
                    }
                    if (count <= 0)
                    {
                        this.manualEvtReceiveData.WaitOne();
                        this.manualEvtReceiveData.Reset();

                        continue;
                    }

                    byte[] buffData = null;
                    lock (this.receivedPacketQueue)
                    {
                        buffData = this.receivedPacketQueue.Dequeue();
                    }

                    ParsingQueuingData(buffData);
                }
            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine("[CommunicationManager] ReceiveDataProcessing( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] ReceiveDataProcessing( Exception=[ ThreadAbortException ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] ReceiveDataProcessing (Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] ReceiveDataProcessing( Exception=[" + ex.ToString() + "] )");

                throw new Exception("[CommunicationManager] CAP/동기화 데이터 처리 중에 예외가 발생하였습니다.");
            }
            finally
            {
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] ReceiveDataProcessing( 종료 )");

                this.isReceivedDataProcessingContinue = false;
                if (this.manualEvtReceiveData != null)
                {
                    this.manualEvtReceiveData.Close();
                    this.manualEvtReceiveData = null;
                }
            }
        }

        /// <summary>
        /// 폴링 처리 (쓰레드 호출용 함수)
        /// </summary>
        public void Polling()
        {
            try
            {
                System.Console.WriteLine("[CommunicationManager] 폴링 감시 준비");

                if (this.manualEvtPolling == null)
                {
                    this.manualEvtPolling = new ManualResetEvent(false);
                }

                // 최초 5초는 폴링 없이 대기
                this.manualEvtPolling.WaitOne(5000);
                this.manualEvtPolling.Reset();

                IEASProtocolBase protoBase = IEASProtocolManager.CreateProtocolForKCAP(KCAPCmdValue.Polling);
                protoBase.SenderType = IEASSenderType.SWI;
                byte[] pollingData = IEASProtocolManager.MakeFrameForKCAP(protoBase);

                System.Console.WriteLine("[CommunicationManager] 폴링 감시 시작");
                while (this.isPollingContinue)
                {
                    System.Console.WriteLine("[CommunicationManager] 폴링 체크");
                    bool pollingState = this.sessionManager.SendData(pollingData);

                    if (this.NotifyIAGWConnectionState != null)
                    {
                        IAGWConnectionEventArgs copy = new IAGWConnectionEventArgs();
                        lock (this.currentIAGWConnectionState)
                        {
                            this.currentIAGWConnectionState.IsConnected = pollingState;
                            copy.DeepCopyFrom(this.currentIAGWConnectionState);
                        }
                        this.NotifyIAGWConnectionState(this, copy);
                    }

                    this.manualEvtPolling.WaitOne(5000);
                    this.manualEvtPolling.Reset();
                }
            }
            catch (ThreadAbortException ex)
            {
                System.Console.WriteLine("[CommunicationManager] Polling( Exception=[ ThreadAbortException ] )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] Polling( Exception=[ ThreadAbortException ] )");

                Thread.ResetAbort();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] Polling( Exception=[" + ex.ToString() + "] )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] Polling( Exception=[" + ex.ToString() + "] )");

                throw new Exception("[CommunicationManager] 통합경보게이트웨이 연결 감시 처리 중에 예외가 발생하였습니다.");
            }
            finally
            {
                System.Console.WriteLine("[CommunicationManager] 폴링 감시 종료");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] Polling( 종료 )");

                this.isPollingContinue = false;
                if (this.manualEvtPolling != null)
                {
                    this.manualEvtPolling.Close();
                    this.manualEvtPolling = null;
                }
            }
        }
        #endregion


        #region 세션 매니저 통지 이벤트 핸들러
        /// <summary>
        /// 연결 성공 통지
        /// </summary>
        public void sessionManager_OnNotifyConnected(object sender, ConnectEvtArgs e)
        {
            try
            {
                System.Console.WriteLine("[CommunicationManager] : OnNotifyConnected(" + e.Socket.Connected.ToString() + ")");

                if (this.NotifyIAGWConnectionState != null)
                {
                    IAGWConnectionEventArgs copy = new IAGWConnectionEventArgs();
                    lock (this.currentIAGWConnectionState)
                    {
                        this.currentIAGWConnectionState.IsConnected = e.Socket.Connected;
                        copy.DeepCopyFrom(this.currentIAGWConnectionState);
                    }
                    this.NotifyIAGWConnectionState(this, copy);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] sessionManager_OnNotifyConnected( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] sessionManager_OnNotifyConnected( Exception=[" + ex.ToString() + "] )");
            }
        }
        /// <summary>
        /// 연결 해제 성공 통지
        /// </summary>
        public void sessionManager_OnNotifyDisconnected(object sender, ConnectEvtArgs e)
        {
            try
            {
                System.Console.WriteLine("[CommunicationManager] : sessionManager_OnNotifyDisconnected(" + e.Socket.Connected.ToString() + ")");

                if (this.NotifyIAGWConnectionState != null)
                {
                    IAGWConnectionEventArgs copy = new IAGWConnectionEventArgs();
                    lock (this.currentIAGWConnectionState)
                    {
                        this.currentIAGWConnectionState.IsConnected = false;
                        copy.DeepCopyFrom(this.currentIAGWConnectionState);
                    }
                    this.NotifyIAGWConnectionState(this, copy);
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] sessionManager_OnNotifyDisconnected( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] sessionManager_OnNotifyDisconnected( Exception=[" + ex.ToString() + "] )");
            }
        }
        /// <summary>
        /// 데이터 수신 통지
        /// </summary>
        public void sessionManager_OnNotifyDataReceived(object sender, ReceiveEvtArgs e)
        {
            try
            {
                System.Console.WriteLine("[CommunicationManager] : sessionManager_OnNotifyDataReceived(dataLength=" + e.Buff.Length.ToString() + ")");
                //System.Diagnostics.Debug.Assert(this.receivedPacketQueue != null);
                //System.Diagnostics.Debug.Assert(this.manualEvtReceiveData != null);

                if (this.receivedPacketQueue != null)
                {
                    this.receivedPacketQueue.Enqueue(e.Buff);
                    if (this.manualEvtReceiveData != null)
                    {
                        this.manualEvtReceiveData.Set();
                    }
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("[CommunicationManager] sessionManager_OnNotifyDataReceived( " + ex.ToString() + " )");
                FileLogManager.GetInstance().WriteLog("[CommunicationManager] sessionManager_OnNotifyDataReceived( Exception=[" + ex.ToString() + "] )");
            }
        }
        #endregion
    }
}
