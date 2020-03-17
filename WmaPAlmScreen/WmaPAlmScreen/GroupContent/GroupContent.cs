using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WmaPAlmScreen
{
    public class GroupContent
    {
        private List<GroupData> lstGroupData = new List<GroupData>();
        private string title = string.Empty;

        /// <summary>
        /// 그룹 정보 리스트 프로퍼티
        /// </summary>
        public List<GroupData> LstGroupData
        {
            get { return this.lstGroupData; }
            set { this.lstGroupData = value; }
        }

        /// <summary>
        /// 그룹명 리스트 프로퍼티
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// 단일 그룹 정보 추가
        /// </summary>
        /// <param name="groupData"></param>
        public void AddGroupData(GroupData groupData)
        {
        }

        /// <summary>
        /// 단일 그룹 정보 가져오기
        /// </summary>
        /// <param name="ipAddr"></param>
        /// <returns></returns>
        public GroupData GetGroupData(string ipAddr)
        {
            return new GroupData();
        }

        /// <summary>
        /// 단일 그룹 정보 삭제
        /// </summary>
        /// <param name="ipAddr"></param>
        public void RemoveGroupData(GroupData ipAddr)
        {
        }
    }
}