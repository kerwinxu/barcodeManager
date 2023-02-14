using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Serialize
{
    /// <summary>
    /// json的序列化
    /// </summary>
    public class JsonSerialize : AbstractSerialize
    {
        public override  T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value,jsonSerializerSettings);
            //throw new NotImplementedException();
        }

        public override  string SerializeObject(object obj)
        {
             
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 有这个是确保反序列化到正确的类型。
        /// </summary>
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.Auto, // 自动的，All的话会有问题。
            DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat,
            DateFormatString = "yyyy-MM-dd HH:mm:ss",                         //空值处理
            //NullValueHandling = NullValueHandling.Ignore,                     //高级用法九中的`Bool`类型转换设置
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,          // 循环引用的的解决方式，如下如下两种设置。
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,  // 
            Formatting = Formatting.Indented, // 缩进的
        };
    }
}
