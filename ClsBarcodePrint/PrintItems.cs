using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xuhengxiao.MyDataStructure;

namespace Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint
{
    [Serializable]
    /// <summary>
    /// 打印项目，里边有需要打印的详细信息
    /// </summary>
    public class PrintItem
    {
        /// <summary>
        /// 图形，这个应该深度拷贝，不能用原先的，可能会触发修改。
        /// </summary>
        public VestShapes.Shapes Shapes { get; set; }

        /// <summary>
        /// 表格的名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据内容
        /// </summary>
        public List<List<clsKeyValue>> Arr2ListRow { get; set; }

        /// <summary>
        /// 打印数量
        /// </summary>
        public int PrintCount { get; set; }

    }
}
