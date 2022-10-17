using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Serialize
{
    /// <summary>
    /// 序列化接口
    /// </summary>
    public interface ISerialize
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string SerializeObject(Object obj);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        T DeserializeObject<T>(string value);
    }
}
