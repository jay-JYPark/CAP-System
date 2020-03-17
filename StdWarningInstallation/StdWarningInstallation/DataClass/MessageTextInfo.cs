using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StdWarningInstallation.DataClass
{
    public class MessageTextInfo
    {
    }

    public class MsgText
    {
        private string identifier;
        public string ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private int mediaTypeID;
        public int MediaTypeID
        {
            get { return mediaTypeID; }
            set { mediaTypeID = value; }
        }
        private int languageKindID;
        public int LanguageKindID
        {
            get { return languageKindID; }
            set { languageKindID = value; }
        }
        private int cityTypeID;
        public int CityTypeID
        {
            get { return cityTypeID; }
            set { cityTypeID = value; }
        }
        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public void DeepCopyFrom(MsgText src)
        {
            this.identifier = src.identifier;
            this.mediaTypeID = src.mediaTypeID;
            this.languageKindID = src.languageKindID;
            this.cityTypeID = src.cityTypeID;
            this.text = src.text;
        }
    }

    public class DisasterMsgText
    {
        private Disaster disaster;
        public Disaster Disaster
        {
            get { return disaster; }
            set { disaster = value; }
        }
        private MsgText msgTxt;
        public MsgText MsgTxt
        {
            get { return msgTxt; }
            set { msgTxt = value; }
        }

        public void DeepCopyFrom(DisasterMsgText src)
        {
            this.disaster.DeepCopyFrom(src.disaster);
            this.msgTxt.DeepCopyFrom(src.msgTxt);
        }

        public void Initialize()
        {
            this.disaster = new Disaster();
            this.disaster.Initialize();
            this.msgTxt = new MsgText();
        }

        public DisasterMsgText()
        {
        }
    }
    public class MsgTextDisplayMediaType
    {
        private uint identifier = uint.MinValue;
        public uint ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private string typeCode = string.Empty;
        public string Code
        {
            get { return typeCode; }
            set { typeCode = value; }
        }
        private string typeName = string.Empty;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
        private uint letterCountLimit = 0;
        public uint LetterCountLimit
        {
            get { return letterCountLimit; }
            set { letterCountLimit = value; }
        }

        public void DeepCopyFrom(MsgTextDisplayMediaType src)
        {
            this.identifier = src.identifier;
            this.typeCode = src.typeCode;
            this.typeName = src.typeName;
            this.letterCountLimit = src.letterCountLimit;
        }
    }

    public class MsgTextDisplayLanguageKind
    {
        private int identifier;
        public int ID
        {
            get { return identifier; }
            set { identifier = value; }
        }

        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }
        private string languageName;
        public string LanguageName
        {
            get { return languageName; }
            set { languageName = value; }
        }
        private bool isDefault;
        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        public void DeepCopyFrom(MsgTextDisplayLanguageKind src)
        {
            this.identifier = src.identifier;
            this.languageCode = src.languageCode;
            this.languageName = src.languageName;
            this.isDefault = src.isDefault;
        }
        public void AllClear()
        {
            this.identifier = 0;
            this.languageCode = string.Empty;
            this.languageName = string.Empty;
            this.isDefault = false;
        }

        public override string ToString()
        {
            return this.languageName;
        }
    }

    public class MsgTextCityType
    {
        private int identifier = 0;
        public int ID
        {
            get { return identifier; }
            set { identifier = value; }
        }
        private string typeCode;
        public string TypeCode
        {
            get { return typeCode; }
            set { typeCode = value; }
        }
        private string typeName;
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public void DeepCopyFrom(MsgTextCityType src)
        {
            this.identifier = src.identifier;
            this.typeCode = src.typeCode;
            this.typeName = src.typeName;
        }
        public void Clear()
        {
            this.identifier = 0;
            this.typeCode = string.Empty;
            this.typeName = string.Empty;
        }
        public override string ToString()
        {
            return typeName;
        }
    }

    public class CAPParameterMsgInfo
    {
        private string languageCode;
        public string LanguageCode
        {
            get { return languageCode; }
            set { languageCode = value; }
        }
        private string valueName;
        public string ValueName
        {
            get { return valueName; }
            set { valueName = value; }
        }
        private string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
