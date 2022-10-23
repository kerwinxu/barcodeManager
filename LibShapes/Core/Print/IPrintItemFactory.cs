using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    /// <summary>
    /// 这个是取得PrintItem
    /// </summary>
    interface IPrintItemFactory
    {
        /// <summary>
        /// 根据名字取得PrintItem
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        PrintItem GetPrintItem(string name);

    }
}
