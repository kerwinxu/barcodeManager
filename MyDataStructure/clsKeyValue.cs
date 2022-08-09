using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个类封装一个关键词和他的值,用这个来表示是因为Dictionary不支持序列化
    /// </summary>
    [Serializable]
    public class clsKeyValue: ISerializable
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

        /// <summary>
        /// 序列化函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("key", this.Key);
            info.AddValue("value", this.Value);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 构造函数，可以反序列化
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public clsKeyValue(SerializationInfo info, StreamingContext context)
        {
            this.Key = (string)info.GetValue("key", typeof(string));
            this.Value = (string)info.GetValue("value", typeof(string));

        }

    }
}
