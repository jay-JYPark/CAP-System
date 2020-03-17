using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    public enum DataSyncCommand
    {
        ChkEntireHashKey = 0x01,
        ChkSingleHashKey = 0x02,
        UpdateProfileData = 0x03,
        ChkDataValidation = 0x10,
        ResultOfChkEntireHashKey = 0x11,
        ResultOfChkSingleHashKey = 0x12,
        ResultOfUpdateProfileData = 0x13,
    }

    class DataSyncInfo
    {
    }

    public class SynchronizationReqData
    {
        #region PROPERTIES
        private DataSyncCommand command;
        public DataSyncCommand Command
        {
            get { return command; }
            set { command = value; }
        }
        /// <summary>
        /// 데이터 동기화 이벤트 아이디.
        /// 요청 이벤트 당 아이디 발행.
        /// 분할 전송의 경우, 동일한 아이디 소유.
        /// </summary>
        private uint eventID;
        public uint EventID
        {
            get { return eventID; }
            set { eventID = value; }
        }
        /// <summary>
        /// 전체 패킷 수.
        /// 단일 갱신의 경우에는 0.
        /// </summary>
        private uint totalPacketCnt = 0;
        public uint TotalPacketCnt
        {
            get { return totalPacketCnt; }
            set { totalPacketCnt = value; }
        }
        /// <summary>
        /// 현재 패킷 번호.
        /// 단일 갱신의 경우에는 0.
        /// </summary>
        private uint currentPacketNo = 0;
        public uint CurrentPacketNo
        {
            get { return currentPacketNo; }
            set { currentPacketNo = value; }
        }
        private ProfileUpdateMode mode;
        public ProfileUpdateMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        /// <summary>
        /// [단일] 프로필 아이디
        /// </summary>
        private string profileID = string.Empty;
        public string ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }
        /// <summary>
        /// [단일] profileID에 해당하는 해쉬 값
        /// </summary>
        private byte[] profileHashKey = null;
        public byte[] ProfileHashKey
        {
            get { return profileHashKey; }
            set { profileHashKey = value; }
        }
        /// <summary>
        /// [단일] 프로필 정보. 
        /// 개별 갱신이 아닌 경우에도, 프로필 단위로 분할되어 데이터가 전송되므로 구조는 동일.
        /// </summary>
        private SASProfile systemProfile;
        public SASProfile SystemProfile
        {
            get { return systemProfile; }
            set { systemProfile = value; }
        }
        private byte[] crc16;
        public byte[] Crc16
        {
            get { return crc16; }
            set { crc16 = value; }
        }
        #endregion
    }
    public class SASProfileHash
    {
        private string profileID = string.Empty;
        public string ProfileID
        {
            get { return profileID; }
            set { profileID = value; }
        }
        private byte[] hashCode = null;
        public byte[] HashKey
        {
            get { return hashCode; }
            set { hashCode = value; }
        }

        public SASProfileHash()
        {
        }
        public SASProfileHash(string identifier, byte[] hash)
        {
            // 메모리 생성 필요?
            this.profileID = identifier;
            this.hashCode = hash;
        }
    }
    /// <summary>
    /// 전체/개별 해쉬키체크 요청 시의 데이터.
    /// 패킷 분할 전송의 경우, 이 클래스를 사용하여 데이터 누적.
    /// </summary>
    public class ProfileHashCheckReqData
    {
        private uint reqEventID;
        public uint ReqEventID
        {
            get { return reqEventID; }
        }
        private uint totalCnt;
        public uint TotalCnt
        {
            get { return totalCnt; }
        }
        private List<SASProfileHash> lstProfileHash;
        public List<SASProfileHash> LstProfileHash
        {
            get { return lstProfileHash; }
        }

        public bool Insert(int index, SASProfileHash obj)
        {
            if (index < 0 || index >= totalCnt)
            {
                return false;
            }

            this.lstProfileHash[index].ProfileID = obj.ProfileID;
            this.lstProfileHash[index].HashKey = obj.HashKey;

            return true;
        }
        public bool IsAllSet()
        {
            for (int index = 0; index < this.totalCnt; index++)
            {
                SASProfileHash hash = this.lstProfileHash[index];
                if (null == hash.HashKey || hash.HashKey.Count() <= 0)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(hash.ProfileID))
                {
                    return false;
                }
            }
            return true;
        }
        public void ClearAll()
        {
            this.reqEventID = uint.MinValue;
            this.totalCnt = uint.MinValue;
            this.lstProfileHash.Clear();
        }

        public ProfileHashCheckReqData(uint requestID, uint totalCount)
        {
            if (totalCount <= 0)
            {
                FileLogManager.GetInstance().WriteLog("[DataSyncInfo] ProfileHashCheckReqData ( Invalid Total Count. )");
                throw new Exception("Invalid Total Count.");
            }

            this.reqEventID = requestID;
            this.totalCnt = totalCount;

            this.lstProfileHash = new List<SASProfileHash>((int)totalCount);
            for (int index = 0; index < totalCount; index++)
            {
                SASProfileHash hash = new SASProfileHash();
                this.lstProfileHash.Add(hash);
            }
        }
    }
    /// <summary>
    /// 프로필 갱신 요청 시의 데이터.
    /// 패킷 분할 전송의 경우, 이 클래스를 사용하여 데이터 누적.
    /// </summary>
    public class ProfileUpdateReqData
    {
        private uint reqEventID;
        public uint ReqEventID
        {
            get { return reqEventID; }
        }
        private uint totalCnt;
        public uint TotalCnt
        {
            get { return totalCnt; }
        }
        private ProfileUpdateMode mode;
        public ProfileUpdateMode Mode
        {
            get { return mode; }
            set { mode = value; }
        }
        private List<SASProfile> lstSASProfile;
        public List<SASProfile> LstSASProfile
        {
            get { return lstSASProfile; }
        }

        public bool Insert(int index, SASProfile obj)
        {
            System.Diagnostics.Debug.Assert(obj != null);

            if (index < 0 || index >= totalCnt)
            {
                return false;
            }

            this.lstSASProfile[index].DeepCopyFrom(obj);

            return true;
        }
        public bool IsAllSet()
        {
            for (int index = 0; index < this.totalCnt; index++)
            {
                SASProfile system = this.lstSASProfile[index];
                if (string.IsNullOrEmpty(system.ID))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(system.Name))
                {
                    return false;
                }
            }
            return true;
        }
        public void ClearAll()
        {
            System.Diagnostics.Debug.Assert(this != null);

            this.reqEventID = uint.MinValue;
            this.totalCnt = uint.MinValue;
            this.mode = ProfileUpdateMode.Modify;
            this.lstSASProfile.Clear();
        }
        public void DeepCopyFrom(ProfileUpdateReqData src)
        {
            System.Diagnostics.Debug.Assert(this != null);
            System.Diagnostics.Debug.Assert(src != null);

            this.reqEventID = src.reqEventID;
            this.totalCnt = src.totalCnt;
            this.mode = src.mode;
            this.lstSASProfile.Clear();
            foreach (SASProfile profile in src.lstSASProfile)
            {
                SASProfile copyProfile = new SASProfile();
                copyProfile.DeepCopyFrom(profile);
                this.lstSASProfile.Add(copyProfile);
            }
        }

        public ProfileUpdateReqData(uint requestID, uint totalCount, ProfileUpdateMode mode)
        {
            if (totalCount <= 0)
            {
                FileLogManager.GetInstance().WriteLog("[DataSyncInfo] ProfileUpdateReqData ( Invalid Total Count. )");

                throw new Exception("Invalid Total Count.");
            }

            this.reqEventID = requestID;
            this.totalCnt = totalCount;
            this.mode = mode;

            this.lstSASProfile = new List<SASProfile>((int)totalCount);
            for (int index = 0; index < totalCount; index++)
            {
                SASProfile system = new SASProfile();
                this.lstSASProfile.Add(system);
            }
        }
    }

    /// <summary>
    /// 프로필 갱신 요청 이벤트 아규먼트 클래스
    /// </summary>
    public class SASProfileUpdateEventArgs : EventArgs
    {
        private ProfileUpdateReqData updateData;
        public ProfileUpdateReqData UpdateData
        {
            get { return updateData; }
            set { updateData = value; }
        }

        public SASProfileUpdateEventArgs(ProfileUpdateReqData data)
        {
            this.updateData = new ProfileUpdateReqData(data.ReqEventID, data.TotalCnt, data.Mode);
            this.updateData.DeepCopyFrom(data);
        }
    }
}
