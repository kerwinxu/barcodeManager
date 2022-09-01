using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using VestShapes;
using Xuhengxiao.MyDataStructure;
using Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint;

namespace Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint
{
    /// <summary>
    /// 这个是这个类的实现
    /// </summary>
    public class BarcodePrintImpl : IBarcodePrint
    {
        // 这里存放数据。
        private static Dictionary<string, PrintItem> dictPrintItem = new Dictionary<string, PrintItem>();


        public string SavePrintItem(PrintItem printItem)
        {
            string _key = (dictPrintItem.Keys.Count + 1).ToString();
            dictPrintItem[_key] = printItem;
            return _key;
            //throw new NotImplementedException();
        }

        public PrintItem GetPrintItem(string key)
        {
            return dictPrintItem[key];
            //throw new NotImplementedException();
        }

        public void DeletePrintItem(string key)
        {
            dictPrintItem.Remove(key);
            //throw new NotImplementedException();
        }

        public void print(Shapes shapes, List<List<clsKeyValue>> arr2Data, List<int> printCount, string PrinterName, bool isFull=false)
        {
            var shapes2 = ClsXmlSerialization.DeepCopy<VestShapes.Shapes>(shapes);
            shapes2.Zoom = 1; // 重新设置为比例1

            if (arr2Data.Count != printCount.Count)
            {
                // 抛出异常
                return;
            }


            // 这个实现思路是，首先判断是否充满的
            if (isFull)
            {
                printFull(shapes2, arr2Data, printCount, PrinterName);
            }
            else
            {
                printNotFull(shapes2, arr2Data, printCount, PrinterName);
            }


            //throw new NotImplementedException();
        }


        /// <summary>
        /// 充满打印
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="arr2Data"></param>
        /// <param name="printCount"></param>
        public void printFull(Shapes shapes, List<List<clsKeyValue>> arr2Data, List<int> printCount, string PrinterName)
        {
            // 充满打印指的是比如一张纸上可以打印2*2个图形，充满打印指的是全部打印一样的
            for (int i = 0; i < arr2Data.Count; i++)
            {
                int intNumOfLine = shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
                int intNumOfColumn = shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;

                // 这里打印是这样则
                PrintDocument printDocument = new PrintDocument();
                //  设置打印机
                if (!string.IsNullOrEmpty(PrinterName)) printDocument.PrinterSettings.PrinterName = PrinterName;
                // 是否纵向
                printDocument.DefaultPageSettings.Landscape = shapes.BarcodePageSettings.BarcodePaperLayout.LandScape;
                printDocument.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
                printDocument.OriginAtMargins = false;//从位于可打印区域的左上角打印
                printDocument.DefaultPageSettings.PaperSize = shapes.BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize;//设置纸张
                // 打印数量，向上取整。
                printDocument.PrinterSettings.Copies = (short)Math.Ceiling(printCount[i] * 0.1 / (intNumOfLine * intNumOfColumn));


                // 然后这里要设置要打印的数据
                PrintItem printItem = new PrintItem();
                printItem.Shapes = shapes;
                printItem.Arr2ListRow = new List<List<clsKeyValue>>();
                for (int j = 0; j < intNumOfLine * intNumOfColumn; j++)
                {
                    printItem.Arr2ListRow.Add(arr2Data[i]); // 添加多次。
                }
                printItem.PrintCount = printDocument.PrinterSettings.Copies;

                // 然后要添加到总的
                printDocument.DocumentName = SavePrintItem(printItem);

                // 事件处理，负责真正的打印。
                printDocument.PrintPage += PrintDocument_PrintPage;

                try
                {
                    printDocument.Print();
                    printDocument.Dispose();

                }
                catch (Exception)
                {

                    //throw;
                }

            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // 我这里看看是否能够取得对应的对象
            PrintDocument printDocument = sender as PrintDocument;
            // 
            PrintItem printItem = GetPrintItem(printDocument.DocumentName);
            // 这里根据最多多少个元素来画图
            int intNumOfLine = printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
            int intNumOfColumn = printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;
            int count = printItem.Arr2ListRow.Count;
            for (int i = 0; i < count; i++)
            {
                // 首先设置变量
                printItem.Shapes.arrlistKeyValue = printItem.Arr2ListRow[i];
                // 然后画图
                int row = i / intNumOfColumn;
                int column = i % intNumOfColumn;
                float fx = printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.Left + row * (printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.ModelWidth + printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.HorizontalInterval);
                float fy = printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.Top + column * (printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.ModelHeight + printItem.Shapes.BarcodePageSettings.BarcodePaperLayout.VerticalInterval);
                printItem.Shapes.DrawShapes(e.Graphics, fx, fy);

            }

            // 最后删除这一个项目
            DeletePrintItem(printDocument.DocumentName);

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 不充满打印
        /// </summary>
        /// <param name="shapes"></param>
        /// <param name="arr2Data"></param>
        /// <param name="printCount"></param>
        public void printNotFull(Shapes shapes, List<List<clsKeyValue>> arr2Data, List<int> printCount, string PrinterName)
        {
            // 不充满打印，可能意味着一张纸可以打印多个不同的
            int i = 0;
            int intNumOfLine = shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
            int intNumOfColumn = shapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;
            while (i < arr2Data.Count) // 这里还是要循环
            {
                // 这里打印是这样则
                PrintDocument printDocument = new PrintDocument();
                //  设置打印机
                if (!string.IsNullOrEmpty(PrinterName)) printDocument.PrinterSettings.PrinterName = PrinterName;
                // 是否纵向
                printDocument.DefaultPageSettings.Landscape = shapes.BarcodePageSettings.BarcodePaperLayout.LandScape;
                printDocument.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
                printDocument.OriginAtMargins = false;//从位于可打印区域的左上角打印
                printDocument.DefaultPageSettings.PaperSize = shapes.BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize;//设置纸张
                printDocument.PrinterSettings.Copies = 1; // 默认一张。
                int j = 0;
                // 然后这里要设置要打印的数据
                PrintItem printItem = new PrintItem();
                printItem.Shapes = shapes;
                printItem.Arr2ListRow = new List<List<clsKeyValue>>();
                printItem.PrintCount = printDocument.PrinterSettings.Copies;
                // 这里是通过添加多少个数据来实现的
                while (j < intNumOfLine * intNumOfColumn)
                {
                    // 这里要判断是否超出边界
                    if (i >= arr2Data.Count) break;

                    int count_1 = printCount[i]; // 这里代表这一个还有多少个
                    if (count_1 + j < intNumOfLine * intNumOfColumn) // 如果剩余的少于了。
                    {
                        for (int k = 0; k < count_1; k++)
                        {
                            printItem.Arr2ListRow.Add(arr2Data[i]);
                        }

                        j += count_1; // 添加进去。
                        i++; // 下一个
                    }
                    else
                    {
                        // 这里表示大于等于，表示至少是一张。
                        // 这里要分2种情况，一种是只有这一种，一种是还有其他的
                        if (j == 0)
                        {
                            // 添加这个。
                            for (int k = 0; k < intNumOfLine * intNumOfColumn; k++)
                            {
                                printItem.Arr2ListRow.Add(arr2Data[i]);
                            }
                            // 然后也是打印数量
                            printDocument.PrinterSettings.Copies =(short) (count_1 / intNumOfLine / intNumOfColumn);
                            printItem.PrintCount = printDocument.PrinterSettings.Copies;
                            
                        }
                        else
                        {
                            // 这里减去余数
                            for (int k = 0; k < intNumOfLine * intNumOfColumn - j; k++)
                            {
                                printItem.Arr2ListRow.Add(arr2Data[i]);
                            }

                        }

                        printCount[i] -= printDocument.PrinterSettings.Copies * intNumOfLine * intNumOfColumn;
                        //下一个还是继续这个变量?
                        if (printCount[i] == 0)
                        {
                            i++;
                        }
                    }
                }

                // 然后要添加到总的
                printDocument.DocumentName = SavePrintItem(printItem);
                // 事件处理，负责真正的打印。
                printDocument.PrintPage += PrintDocument_PrintPage;
                try
                {
                    printDocument.Print();
                    printDocument.Dispose();

                }
                catch (Exception)
                {

                    //throw;
                }

            }
        }

        
    }
}
