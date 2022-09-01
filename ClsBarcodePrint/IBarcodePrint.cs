using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xuhengxiao.MyDataStructure;

namespace Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint
{
    interface IBarcodePrint
    {
        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="PrinterName"></param>
        /// <param name="arr2Data"></param>
        /// <param name="printCount"></param>
        /// <param name="isFull"></param>
        void print(VestShapes.Shapes shapes,  List<List<clsKeyValue>> arr2Data, List<int>printCount, string PrinterName, bool isFull);

        /// <summary>
        /// 将这个保存起来，然后取得key
        /// </summary>
        /// <param name="printItem"></param>
        /// <returns></returns>
        string SavePrintItem(PrintItem printItem);

        /// <summary>
        /// 根据key取得保存的PrintItem打印项目
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        PrintItem GetPrintItem(string key);

        void DeletePrintItem(string key);

    }
}
