using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace VestShapes
{
    /// <summary>
    /// 转换类，主要是为了对文字进行转换的
    /// </summary>
    /// 
    [Serializable]
    public  class ClsTransforms
    {
        private bool  _isSuppression;
        /// <summary>
        /// 抑制
        /// </summary>
        [XmlElement]
        public bool  isSuppression
        {
            get { return _isSuppression; }
            set { _isSuppression = value; }
        }

        private bool _isCharactorFilter;
        /// <summary>
        /// 字符筛选器
        /// </summary>
        [XmlElement]
        public bool isCharActorFilter
        {
            get { return _isCharactorFilter; }
            set { _isCharactorFilter = value; }
        }


        private bool _isTrunction;
        /// <summary>
        /// 截断
        /// </summary>
        [XmlElement]
        public bool isTrunction
        {
            get { return _isTrunction; }
            set { _isTrunction = value; }
        }

        private bool _isNumberOfCharacters;
        /// <summary>
        /// 字符数
        /// </summary>
        [XmlElement]
        public bool isNumberOfCharacters
        {
            get { return _isNumberOfCharacters; }
            set { _isNumberOfCharacters = value; }
        }

        private bool _isSearchAndReplace;
        /// <summary>
        /// 搜索并替换
        /// </summary>
        [XmlElement]
        public bool isSearchAndReplace
        {
            get { return _isSearchAndReplace; }
            set { _isSearchAndReplace = value; }
        }

        private bool _isScript;
        /// <summary>
        /// 脚本
        /// </summary>
        [XmlElement]
        public bool isScript
        {
            get { return _isScript; }
            set { _isScript = value; }
        }

        private bool _isSerialization;
        /// <summary>
        /// 序列号，流水号
        /// </summary>
        [XmlElement]
        public bool isSerialization
        {
            get { return _isSerialization; }
            set { _isSerialization = value; }
        }
        
        

        
    }
}
