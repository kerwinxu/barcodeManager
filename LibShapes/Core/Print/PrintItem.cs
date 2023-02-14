using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/**
 * 外部往里边传入这个变量，
 * 内部把这个拆分成一张纸，比如一张纸上有10个模板，那么就10个变量集合，不用打印数量,打印机了。
 * 
 * */

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    /// <summary>
    /// 打印项目
    /// </summary>
    public class PrintItem
    {
        /// <summary>
        /// 打印机名称
        /// </summary>
        public string PrinterName { get; set; }

        /// <summary>
        /// 图形
        /// </summary>
        public Shapes Shapes { get; set; }

        /// <summary>
        /// 变量的集合
        /// </summary>
        public List<Dictionary<string, string>> Vals { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public List<int> PrintCounts { get; set; }

        /// <summary>
        /// 是否充满打印，
        /// </summary>
        public bool isFullPrint { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public PrintItem()
        {
            // 初始化变量
            Vals = new List<Dictionary<string, string>>();
            PrintCounts = new List<int>();
        }

    }
}
