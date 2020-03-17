using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using NCASFND.NCasLogging;
using NCASFND.NCasNet;
using NCASBIZ.NCasBiz;
using NCASBIZ.NCasDefine;
using NCASBIZ.NCasProtocol;
using NCASBIZ.NCasPlcProtocol;
using NCASBIZ.NCasData;
using NCASBIZ.NCasUtility;
using NCasAppCommon.Define;
using NCasMsgCommon.Tts;
using NCasContentsModule;
using NCasContentsModule.TTS;
using NCASBIZ.NCasWmaLedProtocol;
using CAPLib;
using IEASProtocol;

namespace WmaPAlmScreen
{
    public class PAlmScreenBiz : NCasBizProcess
    {
        #region element
        private MainForm mainForm = null;
        private ProvInfo provInfo = null;
        private int TtsDelay = 5000; //TTS 발령일 때 사용하는 Delay
        private NCasUdpSocket udpSoc = new NCasUdpSocket();
        private readonly int SendDelay = 2000; //화생방 or 재난경계(MIC/TTS) 일 때 사용하는 Delay
        private readonly string LoopBackIP = "127.0.0.1";
        private readonly string ProvBroadIP = "10.0.255.255";
        private List<OrderBizData> lstOrderBizData = new List<OrderBizData>();
        #endregion

        #region 생성자
        /// <summary>
        /// 생성자
        /// </summary>
        public PAlmScreenBiz()
        {
        }

        /// <summary>
        /// 생성자
        /// </summary>
        /// <param name="mainForm">MainForm에서 넘겨주는 참조</param>
        public PAlmScreenBiz(MainForm mainForm)
            : this()
        {
            this.mainForm = mainForm;
            this.provInfo = this.mainForm.ProvInfo;
            this.TtsDelay = this.mainForm.TtsDelayTime;
        }
        #endregion

        #region UnInit
        /// <summary>
        /// PAlmScreenBiz UnInit 메소드
        /// </summary>
        public void UnInit()
        {
            this.udpSoc.Close();
        }
        #endregion

        #region private Method
        #region 시도 전체 발령
        /// <summary>
        /// 시도 전체 발령
        /// </summary>
        /// <param name="orderBizData"></param>
        private void OrderProvAll(OrderBizData orderBizData)
        {
            NCasProtocolBase baseProto = NCasProtocolFactory.ParseFrame(orderBizData.SendBuff);
            NCasProtocolTc1 tc1 = baseProto as NCasProtocolTc1;

            for (int i = 0; i < this.provInfo.LstTerms.Count; i++)
            {
                //tc1.AlarmNetIdOrIpByString = NCasUtilityMng.INCasCommUtility.AddIpAddr(tc1.AlarmNetIdOrIpByString, 0, 7, 0, 0);

                if (this.provInfo.LstTerms[i].TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                tc1.AlarmNetIdOrIpByString = this.provInfo.LstTerms[i].IpAddrToSring;
                break;
            }

            tc1.Sector = NCasDefineSectionCode.SectionTerm;
            byte[] sendBuff = NCasProtocolFactory.MakeUdpFrame(tc1);
            NCasProtocolBase protoBase = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcPublicAlarmOrder);
            NCasProtocolTc151 protoTc151 = protoBase as NCasProtocolTc151;

            if (orderBizData.SelectedDisasterBroadKind != OrderView19201080.DisasterBroadKind.StroredMessage)
            {
                string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
                List<string> reptAlarmServerIpAddr = new List<string>();

                for (int i = 0; i < this.provInfo.LstRepts.Count; i++)
                {
                    reptAlarmServerIpAddr.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(this.provInfo.LstRepts[i].NetIdToString, 0, 0, 1, 1));
                }

                udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, sendBuff);
                udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, sendBuff);

                for (int i = 0; i < reptAlarmServerIpAddr.Count; i++)
                {
                    udpSoc.SendTo(reptAlarmServerIpAddr[i], (int)NCasPortID.PortIdIntTeleRAlarm, sendBuff);
                }
            }

            if (tc1.AlarmKind == NCasDefineOrderKind.AlarmBiochemist ||
                (tc1.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic) ||
                (tc1.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts))
            {
                Thread.Sleep(this.SendDelay);
            }

            byte[] tmpStoBuff = null;

            if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage)
            {
                protoTc151.AlarmKind = NCasDefineOrderKind.BroadMessage;
                protoTc151.AlarmNetIdOrIpByString = tc1.AlarmNetIdOrIpByString;
                protoTc151.AuthenFlag = tc1.AuthenFlag;
                protoTc151.CtrlKind = tc1.CtrlKind;
                protoTc151.Media = tc1.Media;
                protoTc151.Mode = tc1.Mode;
                protoTc151.Source = tc1.Source;
                protoTc151.Sector = tc1.Sector;
                protoTc151.RespReqFlag = tc1.RespReqFlag;
                protoTc151.OrderTimeByDateTime = tc1.OrderTimeByDateTime;
                protoTc151.MsgNum1 = GetStoredMsgHeaderNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                protoTc151.MsgNum2 = int.Parse(orderBizData.SelectedStoredMessage.MsgNum);
                protoTc151.MsgNum3 = GetStoredMsgTailNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                protoTc151.RepeatNum = (byte)orderBizData.StoredMessageRepeatCount;

                tmpStoBuff = NCasProtocolFactory.MakeUdpFrame(protoTc151);
            }

            this.mainForm.MmfMng.WriteOrder(tc1);

            if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage) //저장메시지 발령인 경우..
            {
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, tmpStoBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, tmpStoBuff);
            }
            else
            {
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, sendBuff);
                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, sendBuff);
            }

            if (tc1.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts)
            {
                byte[] tmpBuff = TtsControlDataMng.GetTtsPlayData();
                string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
                udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                Thread.Sleep(this.TtsDelay);

                string ttsData = "TTS" + NCasContentsMng.ttsOption.SpeechSpeed.ToString().PadLeft(3, '0') +
                    NCasContentsMng.ttsOption.RepeatCount.ToString().PadLeft(2, '0') +
                    NCasContentsMng.ttsOption.SentenceInterval.ToString().PadLeft(4, '0') +
                    NCasContentsMng.ttsOption.RestInterval.ToString().PadLeft(4, '0') +
                    orderBizData.SelectedTtsMessage.Text;

                udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsMessage, Encoding.Default.GetBytes(ttsData));
            }
            //else if (tc1.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic)
            //{
            //    byte[] tmpBuff = TtsControlDataMng.GetTtsPlayData();
            //    string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
            //    udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
            //    udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
            //    udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
            //}
            else if (tc1.AlarmKind != NCasDefineOrderKind.DisasterBroadcast)
            {
                udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
            }
        }
        #endregion

        #region 단말개별 발령
        /// <summary>
        /// 단말개별 발령
        /// </summary>
        private void OrderTerm()
        {
            NCasProtocolBase protoBase = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcPublicAlarmOrder);
            NCasProtocolTc151 protoTc151 = protoBase as NCasProtocolTc151;
            byte[] tmpBuff = TtsControlDataMng.GetTeleStartData();
            string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
            List<string> reptAlarmServerIpAddr = new List<string>();

            for (int i = 0; i < this.provInfo.LstRepts.Count; i++)
            {
                reptAlarmServerIpAddr.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(this.provInfo.LstRepts[i].NetIdToString, 0, 0, 1, 1));
            }

            if (this.lstOrderBizData[0].SelectedDisasterBroadKind != OrderView19201080.DisasterBroadKind.StroredMessage)
            {
                udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);

                foreach (OrderBizData orderBizData in this.lstOrderBizData)
                {
                    udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, orderBizData.SendBuff);
                    udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, orderBizData.SendBuff);

                    for (int i = 0; i < reptAlarmServerIpAddr.Count; i++)
                    {
                        udpSoc.SendTo(reptAlarmServerIpAddr[i], (int)NCasPortID.PortIdIntTeleRAlarm, orderBizData.SendBuff);
                    }
                }

                tmpBuff = TtsControlDataMng.GetTeleStopData();
                udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
            }

            if (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.AlarmBiochemist ||
                (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic) ||
                (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts))
            {
                Thread.Sleep(this.SendDelay);
            }

            //그룹정보 전송
            //if (this.lstOrderBizData[0].OrderTermGroupFlag)
            //{
            //    for (int i = 0; i < lstOrderBizData[0].GroupName.Count; i++)
            //    {
            //        NCasProtocolBase protoBase77 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcGroupOrder);
            //        NCasProtocolTc77 protoTc77 = protoBase77 as NCasProtocolTc77;
            //        protoTc77.AlarmKind = this.lstOrderBizData[0].AlmProtocol.AlarmKind;
            //        protoTc77.AlarmNetIdOrIpByString = this.lstOrderBizData[0].AlmProtocol.AlarmNetIdOrIpByString;
            //        protoTc77.CtrlKind = this.lstOrderBizData[0].AlmProtocol.CtrlKind;
            //        protoTc77.GroupName = lstOrderBizData[0].GroupName[i];
            //        protoTc77.Media = this.lstOrderBizData[0].AlmProtocol.Media;
            //        protoTc77.Mode = this.lstOrderBizData[0].AlmProtocol.Mode;
            //        protoTc77.OrderTimeByDateTime = this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime;
            //        protoTc77.Sector = this.lstOrderBizData[0].AlmProtocol.Sector;
            //        protoTc77.Source = this.lstOrderBizData[0].AlmProtocol.Source;
            //        byte[] tmpBuff77 = NCasProtocolFactory.MakeUdpFrame(protoTc77);

            //        string disasterMainIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 232);
            //        string main = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 9);
            //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmpBuff77);
            //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmpBuff77);
            //        this.udpSoc.SendTo(disasterMainIpAddr, 7003, tmpBuff77);
            //        this.udpSoc.SendTo(main, 7003, tmpBuff77);

            //        Console.WriteLine("##### 단말그룹 정보 전송 - " + lstOrderBizData[0].GroupName[i]);
            //    }

            //    if (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.AlarmCancel)
            //    {
            //        this.mainForm.SetGroupListClear();
            //    }
            //}

            foreach (OrderBizData orderBizData in this.lstOrderBizData)
            {
                byte[] storedBuff = null;

                if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage)
                {
                    protoTc151.AlarmKind = NCasDefineOrderKind.BroadMessage;
                    protoTc151.AlarmNetIdOrIpByString = orderBizData.AlmProtocol.AlarmNetIdOrIpByString;
                    protoTc151.AuthenFlag = orderBizData.AlmProtocol.AuthenFlag;
                    protoTc151.CtrlKind = orderBizData.AlmProtocol.CtrlKind;
                    protoTc151.Media = orderBizData.AlmProtocol.Media;
                    protoTc151.Mode = orderBizData.AlmProtocol.Mode;
                    protoTc151.Source = orderBizData.AlmProtocol.Source;
                    protoTc151.Sector = orderBizData.AlmProtocol.Sector;
                    protoTc151.RespReqFlag = orderBizData.AlmProtocol.RespReqFlag;
                    protoTc151.OrderTimeByDateTime = orderBizData.AlmProtocol.OrderTimeByDateTime;
                    protoTc151.MsgNum1 = GetStoredMsgHeaderNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                    protoTc151.MsgNum2 = int.Parse(orderBizData.SelectedStoredMessage.MsgNum);
                    protoTc151.MsgNum3 = GetStoredMsgTailNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                    protoTc151.RepeatNum = (byte)orderBizData.StoredMessageRepeatCount;

                    storedBuff = NCasProtocolFactory.MakeUdpFrame(protoTc151);
                }

                this.mainForm.MmfMng.WriteOrder(orderBizData.AlmProtocol);

                if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage) //저장메시지 발령인 경우..
                {
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, storedBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, storedBuff);
                }
                else
                {
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, orderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, orderBizData.SendBuff);
                }

                if (orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.First || orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.FirstEnd)
                {
                    if (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.AlarmBiochemist ||
                        (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic) ||
                        (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts))
                    {
                        Thread.Sleep(this.SendDelay);
                    }

                    if (orderBizData.AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts)
                    {
                        tmpBuff = TtsControlDataMng.GetTtsPlayData();
                        udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                        udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                        Thread.Sleep(this.TtsDelay);

                        string ttsData = "TTS" + NCasContentsMng.ttsOption.SpeechSpeed.ToString().PadLeft(3, '0') +
                            NCasContentsMng.ttsOption.RepeatCount.ToString().PadLeft(2, '0') +
                            NCasContentsMng.ttsOption.SentenceInterval.ToString().PadLeft(4, '0') +
                            NCasContentsMng.ttsOption.RestInterval.ToString().PadLeft(4, '0') +
                            orderBizData.SelectedTtsMessage.Text;

                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsMessage, Encoding.Default.GetBytes(ttsData));
                    }
                    //else if (orderBizData.AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic)
                    //{
                    //    tmpBuff = TtsControlDataMng.GetTtsPlayData();
                    //    udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                    //    udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                    //    udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                    //}
                    else if (orderBizData.AlmProtocol.AlarmKind != NCasDefineOrderKind.DisasterBroadcast)
                    {
                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                    }
                }
            }
        }
        #endregion

        #region 시군 발령
        /// <summary>
        /// 시군 발령
        /// </summary>
        private void OrderDist()
        {
            NCasProtocolBase baseProto = NCasProtocolFactory.ParseFrame(this.lstOrderBizData[0].SendBuff);
            NCasProtocolTc1 tc1 = baseProto as NCasProtocolTc1;

            for (int i = 0; i < this.provInfo.LstTerms.Count; i++)
            {
                if (this.provInfo.LstTerms[i].TermFlag != NCasDefineTermKind.TermMutil)
                    continue;

                tc1.AlarmNetIdOrIpByString = this.provInfo.LstTerms[i].IpAddrToSring;
                break;
            }

            tc1.Sector = NCasDefineSectionCode.SectionTerm;
            byte[] sendBuff = NCasProtocolFactory.MakeUdpFrame(tc1);
            NCasProtocolBase protoBase = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcPublicAlarmOrder);
            NCasProtocolTc151 protoTc151 = protoBase as NCasProtocolTc151;
            string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
            List<string> reptAlarmServerIpAddr = new List<string>();

            for (int i = 0; i < this.provInfo.LstRepts.Count; i++)
            {
                reptAlarmServerIpAddr.Add(NCasUtilityMng.INCasCommUtility.AddIpAddr(this.provInfo.LstRepts[i].NetIdToString, 0, 0, 1, 1));
            }

            if (this.lstOrderBizData[0].SelectedDisasterBroadKind != OrderView19201080.DisasterBroadKind.StroredMessage)
            {
                foreach (OrderBizData orderBizData in this.lstOrderBizData)
                {
                    udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, sendBuff);
                    udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, sendBuff);

                    for (int i = 0; i < reptAlarmServerIpAddr.Count; i++)
                    {
                        udpSoc.SendTo(reptAlarmServerIpAddr[i], (int)NCasPortID.PortIdIntTeleRAlarm, sendBuff);
                    }
                }
            }

            if (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.AlarmBiochemist ||
                (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic) ||
                (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && this.lstOrderBizData[0].SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts))
            {
                Thread.Sleep(this.SendDelay);
            }

            //그룹정보 전송
            //if (this.lstOrderBizData[0].OrderDistGroupFlag)
            //{
            //    for (int i = 0; i < lstOrderBizData[0].GroupName.Count; i++)
            //    {
            //        NCasProtocolBase protoBase77 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcGroupOrder);
            //        NCasProtocolTc77 protoTc77 = protoBase77 as NCasProtocolTc77;
            //        protoTc77.AlarmKind = this.lstOrderBizData[0].AlmProtocol.AlarmKind;
            //        protoTc77.AlarmNetIdOrIpByString = this.lstOrderBizData[0].AlmProtocol.AlarmNetIdOrIpByString;
            //        protoTc77.CtrlKind = this.lstOrderBizData[0].AlmProtocol.CtrlKind;
            //        protoTc77.GroupName = lstOrderBizData[0].GroupName[i];
            //        protoTc77.Media = this.lstOrderBizData[0].AlmProtocol.Media;
            //        protoTc77.Mode = this.lstOrderBizData[0].AlmProtocol.Mode;
            //        protoTc77.OrderTimeByDateTime = this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime;
            //        protoTc77.Sector = this.lstOrderBizData[0].AlmProtocol.Sector;
            //        protoTc77.Source = this.lstOrderBizData[0].AlmProtocol.Source;
            //        byte[] tmp77Buff = NCasProtocolFactory.MakeUdpFrame(protoTc77);

            //        string disasterMainIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 232);
            //        string main = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 9);
            //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmp77Buff);
            //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmp77Buff);
            //        this.udpSoc.SendTo(disasterMainIpAddr, 7003, tmp77Buff);
            //        this.udpSoc.SendTo(main, 7003, tmp77Buff);

            //        Console.WriteLine("##### 시군그룹 정보 전송 - " + lstOrderBizData[0].GroupName[i]);
            //    }

            //    if (this.lstOrderBizData[0].AlmProtocol.AlarmKind == NCasDefineOrderKind.AlarmCancel)
            //    {
            //        this.mainForm.SetGroupListClear();
            //    }
            //}

            foreach (OrderBizData orderBizData in this.lstOrderBizData)
            {
                DistInfo distInfo = this.mainForm.MmfMng.GetDistInfoByNetId(NCasUtilityMng.INCasCommUtility.SubtractIpAddr(orderBizData.AlmProtocol.AlarmNetIdOrIpByString, 0, 0, 0, 255));
                byte[] tmpStoredBuff = null;

                if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage)
                {
                    protoTc151.AlarmKind = NCasDefineOrderKind.BroadMessage;
                    protoTc151.AlarmNetIdOrIpByString = tc1.AlarmNetIdOrIpByString;
                    protoTc151.AuthenFlag = orderBizData.AlmProtocol.AuthenFlag;
                    protoTc151.CtrlKind = orderBizData.AlmProtocol.CtrlKind;
                    protoTc151.Media = orderBizData.AlmProtocol.Media;
                    protoTc151.Mode = orderBizData.AlmProtocol.Mode;
                    protoTc151.Source = orderBizData.AlmProtocol.Source;
                    protoTc151.Sector = tc1.Sector;
                    protoTc151.RespReqFlag = orderBizData.AlmProtocol.RespReqFlag;
                    protoTc151.OrderTimeByDateTime = orderBizData.AlmProtocol.OrderTimeByDateTime;
                    protoTc151.MsgNum1 = GetStoredMsgHeaderNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                    protoTc151.MsgNum2 = int.Parse(orderBizData.SelectedStoredMessage.MsgNum);
                    protoTc151.MsgNum3 = GetStoredMsgTailNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                    protoTc151.RepeatNum = (byte)orderBizData.StoredMessageRepeatCount;
                    tmpStoredBuff = NCasProtocolFactory.MakeUdpFrame(protoTc151);
                }

                this.mainForm.MmfMng.WriteOrder(orderBizData.AlmProtocol);

                if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage) //저장메시지 발령인 경우..
                {
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, tmpStoredBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, tmpStoredBuff);
                }
                else
                {
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCentSate, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, sendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, sendBuff);
                }

                //지진해일 시군
                //if (distInfo.IsDisasterDist)
                //{
                //    orderBizData.AlmProtocol.AlarmNetIdOrIpByString = NCasUtilityMng.INCasCommUtility.AddIpAddr(orderBizData.AlmProtocol.AlarmNetIdOrIpByString, 0, 2, 0, 0);
                //    byte[] tmpDistBuff = NCasProtocolFactory.MakeUdpFrame(orderBizData.AlmProtocol);
                //    byte[] tmpDistStoredBuff = null;

                //    if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage)
                //    {
                //        protoTc151.AlarmKind = NCasDefineOrderKind.BroadMessage;
                //        protoTc151.AlarmNetIdOrIpByString = orderBizData.AlmProtocol.AlarmNetIdOrIpByString;
                //        protoTc151.AuthenFlag = orderBizData.AlmProtocol.AuthenFlag;
                //        protoTc151.CtrlKind = orderBizData.AlmProtocol.CtrlKind;
                //        protoTc151.Media = orderBizData.AlmProtocol.Media;
                //        protoTc151.Mode = orderBizData.AlmProtocol.Mode;
                //        protoTc151.Source = orderBizData.AlmProtocol.Source;
                //        protoTc151.Sector = orderBizData.AlmProtocol.Sector;
                //        protoTc151.RespReqFlag = orderBizData.AlmProtocol.RespReqFlag;
                //        protoTc151.OrderTimeByDateTime = orderBizData.AlmProtocol.OrderTimeByDateTime;
                //        protoTc151.MsgNum1 = GetStoredMsgHeaderNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                //        protoTc151.MsgNum2 = int.Parse(orderBizData.SelectedStoredMessage.MsgNum);
                //        protoTc151.MsgNum3 = GetStoredMsgTailNumber(int.Parse(orderBizData.SelectedStoredMessage.MsgNum));
                //        protoTc151.RepeatNum = (byte)orderBizData.StoredMessageRepeatCount;
                //        tmpDistStoredBuff = NCasProtocolFactory.MakeUdpFrame(protoTc151);
                //    }

                //    this.mainForm.MmfMng.WriteOrder(orderBizData.AlmProtocol);

                //    if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.StroredMessage) //저장메시지 발령인 경우..
                //    {
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, tmpDistStoredBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, tmpDistStoredBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmpDistStoredBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmpDistStoredBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, tmpDistStoredBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, tmpDistStoredBuff);
                //    }
                //    else
                //    {
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, tmpDistBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, tmpDistBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, tmpDistBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, tmpDistBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaTermTerm, tmpDistBuff);
                //        this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaCentSate, tmpDistBuff);
                //    }
                //}

                if (orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.First || orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.FirstEnd)
                {
                    if (orderBizData.AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts)
                    {
                        byte[] tmpBuff = TtsControlDataMng.GetTtsPlayData();
                        udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                        udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                        Thread.Sleep(this.TtsDelay);

                        string ttsData = "TTS" + NCasContentsMng.ttsOption.SpeechSpeed.ToString().PadLeft(3, '0') +
                            NCasContentsMng.ttsOption.RepeatCount.ToString().PadLeft(2, '0') +
                            NCasContentsMng.ttsOption.SentenceInterval.ToString().PadLeft(4, '0') +
                            NCasContentsMng.ttsOption.RestInterval.ToString().PadLeft(4, '0') +
                            orderBizData.SelectedTtsMessage.Text;

                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsMessage, Encoding.Default.GetBytes(ttsData));
                    }
                    //else if (orderBizData.AlmProtocol.AlarmKind == NCasDefineOrderKind.DisasterBroadcast && orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic)
                    //{
                    //    byte[] tmpBuff = TtsControlDataMng.GetTtsPlayData();
                    //    udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                    //    udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                    //    udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                    //}
                    else if (orderBizData.AlmProtocol.AlarmKind != NCasDefineOrderKind.DisasterBroadcast)
                    {
                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 방송할 저장메시지에 해당되는 Header 메시지 번호를 반환한다.
        /// </summary>
        /// <param name="storedMsgNumber">방송할 저장메시지 번호</param>
        /// <returns>방송할 저장메시지에 해당되는 Header 메시지 번호</returns>
        private int GetStoredMsgHeaderNumber(int storedMsgNumber)
        {
            int resultNum = 0;

            if ((storedMsgNumber % 2 == 0) && (storedMsgNumber > 201 && storedMsgNumber < 219))
            {
                resultNum = 954;
            }
            else if (storedMsgNumber == 157 || storedMsgNumber == 158)
            {
                resultNum = 954;
            }
            else
            {
                resultNum = 951;
            }

            return resultNum;
        }

        /// <summary>
        /// 방송할 저장메시지에 해당되는 Tail 메시지 번호를 반환한다.
        /// </summary>
        /// <param name="storedMsgNumber">방송할 저장메시지 번호</param>
        /// <returns>방송할 저장메시지에 해당되는 Tail 메시지 번호</returns>
        private int GetStoredMsgTailNumber(int storedMsgNumber)
        {
            int resultNum = 0;

            if (storedMsgNumber > 155 && storedMsgNumber < 170)
            {
                resultNum = 509;
            }
            else
            {
                resultNum = 502;
            }

            return resultNum;
        }
        #endregion

        #region OnAsyncDataProcessing
        protected override void OnAsyncDataProcessing(NCASBIZ.NCasType.NCasObject param)
        {
            if (param is OrderBizData)
            {
                AddOutputData(param, false, true, false);
            }
            else if (param is NCasProtocolBase)
            {
                AddOutputData(param, false, true, false);
            }
            else if (param is LedBizData)
            {
                AddOutputData(param, false, true, false);
            }
            else if (param is AutoOrderBizData)
            {
                AddOutputData(param, false, true, false);
            }
        }
        #endregion

        #region OnAsyncDispProcessing
        protected override void OnAsyncDispProcessing(NCASBIZ.NCasType.NCasObject param)
        {
        }
        #endregion

        #region OnAsyncExternProcessing
        protected override void OnAsyncExternProcessing(NCASBIZ.NCasType.NCasObject param)
        {
            if (param is OrderBizData)
            {
                try
                {
                    OrderBizData orderBizData = param as OrderBizData;

                    if (orderBizData.IsLocal == false)
                        return;

                    if (orderBizData.SendBuff == null)
                    {
                        NCasLoggingMng.ILogging.WriteLog("PAlmScreenBiz", "발령이 정상적으로 처리되지 않았습니다.",
                            "TC " + orderBizData.AlmProtocol.TcCode.ToString() + " - " + NCasUtilityMng.INCasEtcUtility.Bytes2HexString(orderBizData.SendBuff));
                        return;
                    }

                    if (orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.First)
                    {
                        udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdTtsControl, Encoding.Default.GetBytes("RES"));
                        Thread.Sleep(500);
                    }

                    if (orderBizData.TtsOrderFlag) //마지막 발령이 TTS발령이면..
                    {
                        byte[] tmpBuff = TtsControlDataMng.GetTtsStopData();
                        string teleServerIpAddr = NCasUtilityMng.INCasCommUtility.AddIpAddr(NCasUtilityMng.INCasCommUtility.MakeNetIdByAnyIp(this.provInfo.NetIdToString), 0, 0, 1, 111);
                        udpSoc.SendTo(teleServerIpAddr, (int)NCasPortID.PortIdExtCallPgServer, tmpBuff);
                        udpSoc.SendTo(NCasUtilityMng.INCasEtcUtility.GetIPv4(), (int)NCasPortID.PortIdExtCallPgMan, tmpBuff);
                    }

                    if (orderBizData.AllDestinationFlag || orderBizData.OrderTermAllFlag) //시도전체 발령(무조건 1개 패킷 전송)
                    {
                        this.OrderProvAll(orderBizData);

                        //전광판으로 발령문구 전송
                        if (orderBizData.AlmProtocol.Mode == NCasDefineOrderMode.RealMode)
                        {
                            NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
                            proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
                            proto1.StartMonth = (byte)DateTime.Now.Month;
                            proto1.StartDay = (byte)DateTime.Now.Day;
                            proto1.StartHour = (byte)DateTime.Now.Hour;
                            proto1.StartMin = (byte)DateTime.Now.Minute;
                            proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
                            proto1.EndMonth = (byte)DateTime.Now.Month;
                            proto1.EndDay = (byte)DateTime.Now.Day;
                            proto1.EndHour = (byte)DateTime.Now.Hour;
                            proto1.EndMin = (byte)(DateTime.Now.Minute + 2);
                            proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
                            proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                            proto1.Msg = string.Format("!fH2{0}/{1}/{2} {3}:{4}    {5} [{6}] 발령", orderBizData.AlmProtocol.OrderTimeByDateTime.Year.ToString(),
                                orderBizData.AlmProtocol.OrderTimeByDateTime.Month.ToString().PadLeft(2, '0'),
                                orderBizData.AlmProtocol.OrderTimeByDateTime.Day.ToString().PadLeft(2, '0'), orderBizData.AlmProtocol.OrderTimeByDateTime.Hour.ToString().PadLeft(2, '0'),
                                orderBizData.AlmProtocol.OrderTimeByDateTime.Minute.ToString().PadLeft(2, '0'),
                                NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(orderBizData.AlmProtocol.Mode),
                                NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(orderBizData.AlmProtocol.AlarmKind));
                            proto1.PrintFinishWaitTimeInSec = 4;
                            proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
                            proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                            proto1.RoomNum = 1;
                            proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
                            byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

                            NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
                            proto80.LedData = new byte[ledBuff.Length];
                            proto80.TermIpByString = "10.24.8.129"; //임시

                            Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
                            byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);
                            this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, sendBuff);
                        }

                        //CAP 데이터 전송 시작..
                        try
                        {
                            CAP cap = new CAP();
                            string id = "시도경기서해안경보대";

                            #region Alert 속성들
                            {
                                // Identifier (필수)
                                cap.MessageID = id + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0')
                                    + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');

                                // Sender (필수)
                                cap.SenderID = id;

                                // Sent (필수)
                                cap.SentDateTime = DateTime.Now;

                                // Status (필수)
                                #region Status
                                switch (orderBizData.AlmProtocol.Mode)
                                {
                                    case NCasDefineOrderMode.RealMode:
                                        cap.MessageStatus = StatusType.Actual;
                                        break;

                                    case NCasDefineOrderMode.TestMode:
                                        cap.MessageStatus = StatusType.Test;
                                        break;

                                    default:
                                        cap.MessageStatus = StatusType.Test;
                                        break;
                                }
                                #endregion

                                cap.MessageType = MsgType.Alert;

                                // source (옵션)
                                cap.Source = id;
                                cap.Scope = ScopeType.Private;
                                cap.Addresses = "10.24.8.129"; //임시

                                // Code (옵션)
                                cap.HandlingCode.Add("대한민국정부1.1");
                            }
                            #endregion

                            InfoType info = new InfoType();
                            cap.Info.Add(info);

                            #region Info 속성들
                            {
                                // Language (옵션)
                                info.Language = "ko-KR";
                                info.Category.Add(CategoryType.Security);
                                string tmpEventValue = string.Empty;

                                // Event (필수)
                                switch (orderBizData.AlmProtocol.AlarmKind)
                                {
                                    case NCasDefineOrderKind.AlarmStandby: //예비
                                        info.Event = "예비";
                                        tmpEventValue = "RDY";
                                        break;

                                    case NCasDefineOrderKind.AlarmWatch: //경계
                                        info.Event = "경계";
                                        tmpEventValue = "AL2";
                                        break;

                                    case NCasDefineOrderKind.AlarmAttack: //공습
                                        info.Event = "공습";
                                        tmpEventValue = "AL1";
                                        break;

                                    case NCasDefineOrderKind.AlarmBiochemist: //화생방
                                        info.Event = "화생방";
                                        tmpEventValue = "CBR";
                                        break;

                                    case NCasDefineOrderKind.DisasterWatch: //재난위험
                                        info.Event = "재난위험";
                                        tmpEventValue = "AL3";
                                        break;

                                    case NCasDefineOrderKind.DisasterBroadcast: //재난경계
                                        info.Event = "재난경계";
                                        tmpEventValue = "DPA";
                                        break;

                                    case NCasDefineOrderKind.AlarmCancel: //해제
                                        info.Event = "해제";
                                        tmpEventValue = "CLR";
                                        break;
                                }

                                // Urgency (필수)
                                info.Urgency = UrgencyType.Unknown;

                                // Severity (필수)
                                info.Severity = SeverityType.Unknown;

                                // Certainty (필수)
                                info.Certainty = CertaintyType.Unknown;

                                // eventCode (옵션)
                                NameValueType nvt = new NameValueType();
                                nvt.Name = "KRDSTCode";
                                nvt.Value = tmpEventValue;
                                info.EventCode.Add(nvt);

                                // senderName
                                info.SenderName = id;
                                info.Headline = orderBizData.AlmProtocol.OrderTimeByDateTime.ToString() + " ["
                                    + ((orderBizData.AlmProtocol.Mode == NCasDefineOrderMode.RealMode) ? "실제" : "시험") + "] ["
                                    + info.Event + "] 발령";

                                NameValueType nvtParameter = new NameValueType();
                                nvtParameter.Name = "TTSText";
                                nvtParameter.Value = info.Headline;
                                info.Parameter.Add(nvtParameter);

                                nvtParameter = new NameValueType();
                                nvtParameter.Name = "CBSText";
                                nvtParameter.Value = info.Headline;
                                info.Parameter.Add(nvtParameter);

                                nvtParameter = new NameValueType();
                                nvtParameter.Name = "BoardText";
                                nvtParameter.Value = info.Headline;
                                info.Parameter.Add(nvtParameter);

                                nvtParameter = new NameValueType();
                                nvtParameter.Name = "DMBText";
                                nvtParameter.Value = info.Headline;
                                info.Parameter.Add(nvtParameter);
                            }
                            #endregion

                            ResourceType resource = new ResourceType();
                            info.Resource.Add(resource);

                            #region Resource 속성들
                            {
                                // resourceDesc (필수)
                                resource.ResourceDesc = "사용안함";

                                // mimeType (필수)
                                resource.MimeType = "사용안함";
                            }
                            #endregion

                            AreaType area = new AreaType();
                            info.Area.Add(area);

                            #region Area 속성들
                            {
                                // AreaDesc (필수)
                                area.AreaDesc = "대한민국";

                                // geocode (옵션)
                                NameValueType nvt = new NameValueType();
                                nvt.Name = "KRDSTGeocode";
                                nvt.Value = "10.24.8.129"; //임시
                                area.GeoCode.Add(nvt);
                            }
                            #endregion

                            string capMsg = cap.WriteToXML();
                            NCasLoggingMng.ILogging.WriteLog("CAP 데이터 처리", "CAP Message - " + capMsg);

                            IEASPrtCmd1 prt1 = IEASProtocolManager.CreateProtocol(1) as IEASPrtCmd1;
                            prt1.SenderType = IEASSenderType.None;
                            prt1.CAPMessage = capMsg;
                            byte[] sendCapBuff = IEASProtocolManager.MakeFrame(prt1);

                            if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic
                                || orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts)
                            {
                                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCapTerm, sendCapBuff);
                            }
                        }
                        catch (Exception ex)
                        {
                            NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Method ProvAll CAP - ", ex);
                        }
                    }
                    else //하나의 발령에 의해 여러개의 패킷을 전송해야 하는 경우..
                    {
                        if (orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.First || orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.None)
                        {
                            this.lstOrderBizData.Add(orderBizData);
                        }
                        else if (orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.End || orderBizData.IsEnd == OrderView19201080.OrderDataSendStatus.FirstEnd)
                        {
                            this.lstOrderBizData.Add(orderBizData);

                            if (orderBizData.OrderTermFlag || orderBizData.OrderTermGroupFlag || orderBizData.OrderDistTermFlag) //단말개별 발령
                            {
                                this.OrderTerm();
                            }
                            else if (orderBizData.OrderDistFlag || orderBizData.OrderDistGroupFlag || orderBizData.OrderDistTermAllFlag) //시군 발령
                            {
                                this.OrderDist();
                            }

                            //전광판으로 발령문구 전송
                            if (orderBizData.AlmProtocol.Mode == NCasDefineOrderMode.RealMode)
                            {
                                NCasWmaLedProtocolCmd1 proto1 = NCasWmaLedProtocolFactory.CreateWmaProtocol(NCasDefineWmaCommand.LedMsg) as NCasWmaLedProtocolCmd1;
                                proto1.StartYear = (byte)(DateTime.Now.Year - 2000);
                                proto1.StartMonth = (byte)DateTime.Now.Month;
                                proto1.StartDay = (byte)DateTime.Now.Day;
                                proto1.StartHour = (byte)DateTime.Now.Hour;
                                proto1.StartMin = (byte)DateTime.Now.Minute;
                                proto1.EndYear = (byte)(DateTime.Now.Year - 2000);
                                proto1.EndMonth = (byte)DateTime.Now.Month;
                                proto1.EndDay = (byte)DateTime.Now.Day;
                                proto1.EndHour = (byte)DateTime.Now.Hour;
                                proto1.EndMin = (byte)(DateTime.Now.Minute + 2);
                                proto1.FinishNum = NCASBIZ.NCasDefine.NCasDefineWmaFinishEffect.LeftToClear;
                                proto1.FinishSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                                proto1.Msg = string.Format("!fH2{0}/{1}/{2} {3}:{4}    {5} [{6}] 발령", this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime.Year.ToString(),
                                    this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime.Month.ToString().PadLeft(2, '0'),
                                    this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime.Day.ToString().PadLeft(2, '0'),
                                    this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime.Hour.ToString().PadLeft(2, '0'),
                                    this.lstOrderBizData[0].AlmProtocol.OrderTimeByDateTime.Minute.ToString().PadLeft(2, '0'),
                                    NCasUtilityMng.INCasCommUtility.NCasDefineOrderMode2String(this.lstOrderBizData[0].AlmProtocol.Mode),
                                    NCasUtilityMng.INCasCommUtility.NCasDefineOrderKind2String(this.lstOrderBizData[0].AlmProtocol.AlarmKind));
                                proto1.PrintFinishWaitTimeInSec = 4;
                                proto1.PrintNum = NCASBIZ.NCasDefine.NCasDefineWmaViewEffect.LeftScroll;
                                proto1.PrintSpeed = NCASBIZ.NCasDefine.NCasDefineWmaSpeedEffect.Fast4;
                                proto1.RoomNum = 1;
                                proto1.Siren = NCASBIZ.NCasDefine.NCasDefineWmaSirenUse.UnUsed;
                                byte[] ledBuff = NCasWmaLedProtocolFactory.MakeFrame(proto1);

                                NCasProtocolTc80 proto80 = NCasProtocolFactory.CreateCasProtocol(NCasDefineTcCode.TcWmaLed) as NCasProtocolTc80;
                                proto80.LedData = new byte[ledBuff.Length];
                                proto80.TermIpByString = "10.24.8.129"; //임시

                                Array.Copy(ledBuff, proto80.LedData, ledBuff.Length);
                                byte[] sendBuff = NCasProtocolFactory.MakeTcpFrame(proto80);
                                this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, sendBuff);
                            }

                            this.lstOrderBizData.Clear();

                            //CAP 데이터 전송 시작..
                            try
                            {
                                CAP cap = new CAP();
                                string id = "시도경기서해안경보대";

                                #region Alert 속성들
                                {
                                    // Identifier (필수)
                                    cap.MessageID = id + DateTime.Now.Month.ToString().PadLeft(2, '0') + DateTime.Now.Day.ToString().PadLeft(2, '0')
                                        + DateTime.Now.Hour.ToString().PadLeft(2, '0') + DateTime.Now.Minute.ToString().PadLeft(2, '0') + DateTime.Now.Second.ToString().PadLeft(2, '0');

                                    // Sender (필수)
                                    cap.SenderID = id;

                                    // Sent (필수)
                                    cap.SentDateTime = DateTime.Now;

                                    // Status (필수)
                                    #region Status
                                    switch (orderBizData.AlmProtocol.Mode)
                                    {
                                        case NCasDefineOrderMode.RealMode:
                                            cap.MessageStatus = StatusType.Actual;
                                            break;

                                        case NCasDefineOrderMode.TestMode:
                                            cap.MessageStatus = StatusType.Test;
                                            break;

                                        default:
                                            cap.MessageStatus = StatusType.Test;
                                            break;
                                    }
                                    #endregion

                                    cap.MessageType = MsgType.Alert;

                                    // source (옵션)
                                    cap.Source = id;
                                    cap.Scope = ScopeType.Private;
                                    cap.Addresses = "10.24.8.129"; //임시

                                    // Code (옵션)
                                    cap.HandlingCode.Add("대한민국정부1.1");
                                }
                                #endregion

                                InfoType info = new InfoType();
                                cap.Info.Add(info);

                                #region Info 속성들
                                {
                                    // Language (옵션)
                                    info.Language = "ko-KR";
                                    info.Category.Add(CategoryType.Security);
                                    string tmpEventValue = string.Empty;

                                    // Event (필수)
                                    switch (orderBizData.AlmProtocol.AlarmKind)
                                    {
                                        case NCasDefineOrderKind.AlarmStandby: //예비
                                            info.Event = "예비";
                                            tmpEventValue = "RDY";
                                            break;

                                        case NCasDefineOrderKind.AlarmWatch: //경계
                                            info.Event = "경계";
                                            tmpEventValue = "AL2";
                                            break;

                                        case NCasDefineOrderKind.AlarmAttack: //공습
                                            info.Event = "공습";
                                            tmpEventValue = "AL1";
                                            break;

                                        case NCasDefineOrderKind.AlarmBiochemist: //화생방
                                            info.Event = "화생방";
                                            tmpEventValue = "CBR";
                                            break;

                                        case NCasDefineOrderKind.DisasterWatch: //재난위험
                                            info.Event = "재난위험";
                                            tmpEventValue = "AL3";
                                            break;

                                        case NCasDefineOrderKind.DisasterBroadcast: //재난경계
                                            info.Event = "재난경계";
                                            tmpEventValue = "DPA";
                                            break;

                                        case NCasDefineOrderKind.AlarmCancel: //해제
                                            info.Event = "해제";
                                            tmpEventValue = "CLR";
                                            break;
                                    }

                                    // Urgency (필수)
                                    info.Urgency = UrgencyType.Unknown;

                                    // Severity (필수)
                                    info.Severity = SeverityType.Unknown;

                                    // Certainty (필수)
                                    info.Certainty = CertaintyType.Unknown;

                                    // eventCode (옵션)
                                    NameValueType nvt = new NameValueType();
                                    nvt.Name = "KRDSTCode";
                                    nvt.Value = tmpEventValue;
                                    info.EventCode.Add(nvt);

                                    // senderName
                                    info.SenderName = id;
                                    info.Headline = orderBizData.AlmProtocol.OrderTimeByDateTime.ToString() + " ["
                                        + ((orderBizData.AlmProtocol.Mode == NCasDefineOrderMode.RealMode) ? "실제" : "시험") + "] ["
                                        + info.Event + "] 발령";

                                    NameValueType nvtParameter = new NameValueType();
                                    nvtParameter.Name = "TTSText";
                                    nvtParameter.Value = info.Headline;
                                    info.Parameter.Add(nvtParameter);

                                    nvtParameter = new NameValueType();
                                    nvtParameter.Name = "CBSText";
                                    nvtParameter.Value = info.Headline;
                                    info.Parameter.Add(nvtParameter);

                                    nvtParameter = new NameValueType();
                                    nvtParameter.Name = "BoardText";
                                    nvtParameter.Value = info.Headline;
                                    info.Parameter.Add(nvtParameter);

                                    nvtParameter = new NameValueType();
                                    nvtParameter.Name = "DMBText";
                                    nvtParameter.Value = info.Headline;
                                    info.Parameter.Add(nvtParameter);
                                }
                                #endregion

                                ResourceType resource = new ResourceType();
                                info.Resource.Add(resource);

                                #region Resource 속성들
                                {
                                    // resourceDesc (필수)
                                    resource.ResourceDesc = "사용안함";

                                    // mimeType (필수)
                                    resource.MimeType = "사용안함";
                                }
                                #endregion

                                AreaType area = new AreaType();
                                info.Area.Add(area);

                                #region Area 속성들
                                {
                                    // AreaDesc (필수)
                                    area.AreaDesc = "대한민국";

                                    // geocode (옵션)
                                    NameValueType nvt = new NameValueType();
                                    nvt.Name = "KRDSTGeocode";
                                    nvt.Value = "10.24.8.129"; //임시
                                    area.GeoCode.Add(nvt);
                                }
                                #endregion

                                string capMsg = cap.WriteToXML();
                                NCasLoggingMng.ILogging.WriteLog("CAP 데이터 처리", "CAP Message - " + capMsg);

                                IEASPrtCmd1 prt1 = IEASProtocolManager.CreateProtocol(1) as IEASPrtCmd1;
                                prt1.SenderType = IEASSenderType.None;
                                prt1.CAPMessage = capMsg;
                                byte[] sendCapBuff = IEASProtocolManager.MakeFrame(prt1);

                                if (orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Mic
                                    || orderBizData.SelectedDisasterBroadKind == OrderView19201080.DisasterBroadKind.Tts)
                                {
                                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaCapTerm, sendCapBuff);
                                }
                            }
                            catch (Exception ex)
                            {
                                NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Not ProvAll Method CAP - ", ex);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Method - OrderBizData", ex);
                }
            }
            else if (param is NCasProtocolBase)
            {
                try
                {
                    NCasProtocolBase nCasPlcProtocolBase = param as NCasProtocolBase;
                    byte[] buff = NCasProtocolFactory.MakeUdpFrame(nCasPlcProtocolBase);

                    if (nCasPlcProtocolBase.TcCode == NCasDefineTcCode.None)
                    {
                        NCasLoggingMng.ILogging.WriteLog("PAlmScreenBiz", "NCasPlcProtocolFactory.TcCode is NCasDefineTcCode.None");
                        return;
                    }

                    if (buff == null)
                    {
                        NCasLoggingMng.ILogging.WriteLog("PAlmScreenBiz", "buff is null");
                        return;
                    }

                    //this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeNccDevAlmKey, buff);
                }
                catch (Exception ex)
                {
                    NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Method - NCasPlcProtocolBase", ex);
                }
            }
            else if (param is LedBizData)
            {
                try
                {
                    LedBizData ledBizData = param as LedBizData;

                    if (ledBizData.SendBuff == null)
                    {
                        NCasLoggingMng.ILogging.WriteLog("PAlmScreenBiz", "LedBizData.SendBuff is Null");
                        return;
                    }

                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaTermTerm, ledBizData.SendBuff);
                }
                catch (Exception ex)
                {
                    NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Method - LedBizData", ex);
                }
            }
            else if (param is AutoOrderBizData)
            {
                try
                {
                    AutoOrderBizData autoOrderBizData = param as AutoOrderBizData;

                    if (autoOrderBizData.SendBuff == null)
                    {
                        NCasLoggingMng.ILogging.WriteLog("PAlmScreenBiz", "AutoOrderBizData.SendBuff is Null");
                        return;
                    }

                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdIntAuthorityDAlarm, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPortID.PortIdExtCasMonitor, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvAlm, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipePdaProvPDMain, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaWeathTerm, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvAlm, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMain, autoOrderBizData.SendBuff);
                    this.udpSoc.SendTo(this.LoopBackIP, (int)NCasPipes.PipeWpaProvMultiMain, autoOrderBizData.SendBuff);

                    this.mainForm.MmfMng.WriteOrder(autoOrderBizData.AlmProtocol);
                }
                catch (Exception ex)
                {
                    NCasLoggingMng.ILoggingException.WriteException("PAlmScreenBiz", "PAlmScreenBiz.OnAsyncExternProcessing Method - AutoOrderBizData", ex);
                }
            }
        }
        #endregion
    }
}