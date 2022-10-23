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
            return JsonConvert.DeserializeObject<T>(value);
            //throw new NotImplementedException();
        }

        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
            //throw new NotImplementedException();
        }
    }
}
