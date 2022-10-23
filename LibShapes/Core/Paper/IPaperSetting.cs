using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Paper
{
    /// <summary>
    /// 取得纸张设置的接口
    /// </summary>
    interface IPaperSetting
    {
        /// <summary>
        /// 取得纸张的信息
        /// </summary>
        /// <returns></returns>
        Paper GetPaper();
    }
}
