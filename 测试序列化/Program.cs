using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
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
           
            // 说明颜色可以直接转成json

            // 我这里看一下矩阵是否支持序列化
            Matrix matrix = new Matrix();
            matrix.Translate(10, 20);
            matrix.Scale(2, 3);
            string json2 = JsonConvert.SerializeObject(matrix);
            Console.WriteLine(json2);

            Console.ReadKey();

        }

        class ColorHolder
        {
            public System.Drawing.Color Value { get; set; }
        }

    }
}
