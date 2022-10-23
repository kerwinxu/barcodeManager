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
    public class JsonSerialize : ISerialize
    {
        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value,jsonSerializerSettings);
            //throw new NotImplementedException();
        }

        public string SerializeObject(object obj)
        {
             
            return JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
            //throw new NotImplementedException();
        }

        /// <summary>
        /// 有这个是确保反序列化到正确的类型。
        /// </summary>
        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings()
        {
            TypeNameHandling = TypeNameHandling.All, // Json.NET会在序列化后的json文本中附加一个属性说明json到底是从什么类序列化过来的

        };
    }
}
