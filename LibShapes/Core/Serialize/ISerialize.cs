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

        // 这里表示从文件中序列化和反序列化

        /// <summary>
        /// 序列化到文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="file_path"></param>
        void SerializeObjectToFile(Object obj, string file_path);


        T DeserializeObjectFromFile<T>(string file_path);
    }
}
