using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    /// <summary>
    /// 打印管理者
    /// </summary>
    interface IPrintManager
    {
        /// <summary>
        /// 添加一个新的打印
        /// </summary>
        /// <param name="printItem"></param>
        void addPrintItem(PrintItem printItem);
    }
}
