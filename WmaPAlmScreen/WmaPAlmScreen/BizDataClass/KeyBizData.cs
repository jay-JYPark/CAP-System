using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NCASBIZ.NCasType;
using NCasAppCommon.Type;

namespace WmaPAlmScreen
{
    public class KeyBizData : NCasObject
    {
        private NCasKeyData keyData = null;
        private bool isLocal = true;

        /// <summary>
        /// 버튼키 공유를 위한 프로퍼티
        /// </summary>
        public NCasKeyData KeyData
        {
            get { return this.keyData; }
            set { this.keyData = value; }
        }

        /// <summary>
        /// 로컬 버튼을 눌렀는지 듀얼 버튼이 눌렸는지 구분하는 프로퍼티
        /// </summary>
        public bool IsLocal
        {
            get { return this.isLocal; }
            set { this.isLocal = value; }
        }

        public override void CloneFrom(NCasObject obj)
        {
            KeyBizData newObject = obj as KeyBizData;
            this.keyData = newObject.keyData;
            this.isLocal = newObject.isLocal;
        }

        public override NCasObject CloneTo()
        {
            KeyBizData newObject = new KeyBizData();
            newObject.keyData = this.keyData;
            newObject.isLocal = this.isLocal;
            return newObject;
        }
    }
}