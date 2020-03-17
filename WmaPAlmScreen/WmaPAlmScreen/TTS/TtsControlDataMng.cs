using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    public class TtsControlDataMng
    {
        #region element
        private static char[] returnData = new char[7];
        #endregion

        /// <summary>
        /// TTS 시작 데이터를 반환한다.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTtsPlayData()
        {
            returnData[0] = 'A';
            returnData[1] = 'D';
            returnData[2] = 'E';
            returnData[3] = 'N';
            returnData[4] = 'G';
            returnData[5] = 'A';
            returnData[6] = '1';

            return Encoding.Default.GetBytes(returnData);
        }

        /// <summary>
        /// TTS 멈춤 데이터를 반환한다.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTtsStopData()
        {
            returnData[0] = 'A';
            returnData[1] = 'D';
            returnData[2] = 'E';
            returnData[3] = 'N';
            returnData[4] = 'G';
            returnData[5] = 'A';
            returnData[6] = '0';

            return Encoding.Default.GetBytes(returnData);
        }

        /// <summary>
        /// 음성 시작 데이터를 반환한다.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTeleStartData()
        {
            returnData[0] = 'A';
            returnData[1] = 'D';
            returnData[2] = 'E';
            returnData[3] = 'N';
            returnData[4] = 'G';
            returnData[5] = 'G';
            returnData[6] = '1';

            return Encoding.Default.GetBytes(returnData);
        }

        /// <summary>
        /// 음성 멈춤 데이터를 반환한다.
        /// </summary>
        /// <returns></returns>
        public static byte[] GetTeleStopData()
        {
            returnData[0] = 'A';
            returnData[1] = 'D';
            returnData[2] = 'E';
            returnData[3] = 'N';
            returnData[4] = 'G';
            returnData[5] = 'G';
            returnData[6] = '0';

            return Encoding.Default.GetBytes(returnData);
        }
    }
}