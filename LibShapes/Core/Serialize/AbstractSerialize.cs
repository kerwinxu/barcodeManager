using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Serialize
{
    public abstract class AbstractSerialize : ISerialize
    {
        // 实现了如下的两个方法。

        public void SerializeObjectToFile(object obj, string file_path)
        {
            System.IO.File.WriteAllText(file_path, SerializeObject(obj));
        }

        public T DeserializeObjectFromFile<T>(string file_path)
        {
            return DeserializeObject<T>(System.IO.File.ReadAllText(file_path));
        }


        // 如下的等待具体的类去实现。

        public abstract T DeserializeObject<T>(string value);

        public abstract string SerializeObject(object obj);

    }
}
