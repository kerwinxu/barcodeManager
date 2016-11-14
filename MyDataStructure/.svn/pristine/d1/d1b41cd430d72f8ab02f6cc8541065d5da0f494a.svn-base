using System;
using System.Collections.Generic;
using System.Text;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个类封装一个关键词和他的值
    /// </summary>
    [Serializable]
    public class clsKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }

        //默认的构造函数， xml序列化用
        public clsKeyValue()
        {

        }

        public clsKeyValue(string pKey, string pValue)
        {
            this.Key = pKey;
            this.Value = pValue;
        }
        public override string ToString()
        {
            return this.Key;
        }
    }
}
