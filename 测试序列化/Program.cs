using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace 测试序列化
{
    class Program
    {
        static void Main(string[] args)
        {
            // 这里先看一下颜色是否可以序列化
            ColorHolder colorHolder = new ColorHolder() { Value = Color.FromArgb(10, 20, 30, 40) };
            string json = JsonConvert.SerializeObject(colorHolder);
            Console.WriteLine(json);
            ColorHolder colorHolder2 = JsonConvert.DeserializeObject<ColorHolder>(json);
            Console.WriteLine(colorHolder2.Value);
            Console.ReadKey();
            // 说明颜色可以直接转成json


        }

        class ColorHolder
        {
            public System.Drawing.Color Value { get; set; }
        }

    }
}
