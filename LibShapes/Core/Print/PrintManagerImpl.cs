using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Print
{
    /// <summary>
    /// 打印管理者的实现
    /// </summary>
    public class PrintManagerImpl : AbstractPrintItemFactory, IPrintManager
    {
        /// <summary>
        /// 打印的时候需要的id，区分不同的打印的。
        /// </summary>
        private int _id = 0;

        public void addPrintItem(PrintItem printItem)
        {
            // 这里收到一个打印的部分，然后需要拆分，
            // 思路是，首先区分是否充满，充满的话，就是按照整数去打印，而不充满的，是要填充的。
            if (printItem.Valss.Count != printItem.PrintCounts.Count) throw new SizeNotEqual();
            // 判断是否是充满打印，由具体的函数去负责处理。
            if (printItem.isFullPrint)
            {
                addPrintItemFullPrint(printItem);
            }
            else
            {
                addPrintItemNotFullPrint(printItem);

            }

            //throw new NotImplementedException();
        }

        /// <summary>
        /// 不是充满的打印。
        /// </summary>
        /// <param name="printItem"></param>
        protected void addPrintItemNotFullPrint(PrintItem printItem)
        {
            // 如下的提取出来是方便减少代码的。
            int rows = printItem.Shapes.Paper.Rows;
            int cols = printItem.Shapes.Paper.Cols;
            // 一个临时的打印变量。
            PrintItem printItem_tmp = new PrintItem();
            printItem_tmp.PrinterName = printItem.PrinterName;
            printItem_tmp.Shapes = printItem.Shapes;
            // 循环条件是还有要打印的。
            while (printItem.Valss.Count > 0)
            {
                // 判断前面的是否为空,为空表示下边的这个可以一次性打印几张。
                if (printItem_tmp.Valss.Count == 0)
                {
                    // 判断是否判断是否大于一张
                    if (printItem.PrintCounts.First() >= rows* cols)
                    {
                        
                        // 填充指定数量的变量
                        for (int i = 0; i < rows*cols; i++)
                        {
                            printItem_tmp.Valss.Add(printItem.Valss.First());
                        }
                        int num = printItem.PrintCounts.First() / (rows * cols);// 取整
                        printItem.PrintCounts[0] -= num * rows * cols;          // 减去指定的数量
                        sendToPrinter(printItem_tmp, num);  // 发送给打印机
                        printItem_tmp = new PrintItem();    // 重新 
                        printItem_tmp.PrinterName = printItem.PrinterName;
                        printItem_tmp.Shapes = printItem.Shapes;
                    }
                    else
                    {
                        // 这里表示不够一页啊
                        for (int i = 0; i < printItem.PrintCounts.First(); i++)
                        {
                            printItem_tmp.Valss.Add(printItem.Valss.First());
                        }
                        // 添加上去
                        printItem.PrintCounts[0] = 0;

                    }

                }
                else
                {
                    // 添加指定的多个。这里表示这次打印的数量是1张。
                    // 这里判断是否够一张
                    if (printItem.PrintCounts.First() + printItem_tmp.Valss.Count >= rows * cols)
                    {
                        // 看看还却多少。
                        int i_max = rows * cols - printItem_tmp.Valss.Count;
                        for (int i = 0; i < i_max; i++)
                        {
                            printItem.Valss.Add(printItem.Valss.First());
                        }
                        // 减去指定的多少。
                        printItem.PrintCounts[0] -= i_max;          // 减去指定的数量
                        sendToPrinter(printItem_tmp, 1);  // 发送给打印机
                        printItem_tmp = new PrintItem();    // 重新 
                        printItem_tmp.PrinterName = printItem.PrinterName;
                        printItem_tmp.Shapes = printItem.Shapes;
                    }
                    else
                    {
                        for (int i = 0; i < printItem.PrintCounts.First(); i++)
                        {
                            printItem_tmp.Valss.Add(printItem.Valss.First());
                        }
                        printItem.PrintCounts[0] = 0;// 不够减，看看下一个把。
                    }
                    
                }

                // 这里删除空白的
                while (printItem.PrintCounts.Count > 0 && printItem.PrintCounts.First() == 0)
                {
                    // 删除第一个。
                    printItem.Valss.RemoveAt(0);
                    printItem.PrintCounts.RemoveAt(0);
                }

            }

            // 如果还有数据，就打印吧
            if (printItem_tmp.Valss.Count != 0)
            {
                sendToPrinter(printItem_tmp, 1);  // 发送给打印机
            }

        }


        /// <summary>
        /// 充满的打印。
        /// </summary>
        /// <param name="printItem"></param>
        protected void addPrintItemFullPrint(PrintItem printItem)
        {
            // 如下的提取出来是方便减少代码的。
            int rows = printItem.Shapes.Paper.Rows;
            int cols = printItem.Shapes.Paper.Cols;
            // 每一个都是充满打印。
            for (int i = 0; i < printItem.Valss.Count; i++)
            {
                PrintItem printItem_tmp = new PrintItem();         //
                printItem_tmp.PrinterName = printItem.PrinterName; // 打印机
                for (int j = 0; j < rows * cols; j++)
                {
                    printItem_tmp.Valss.Add(printItem.Valss[i]);   // 添加多次就是啦。
                }
                // 这里计算打印的数量
                int num = (int)((printItem.PrintCounts[i] + 0.5) / (rows + cols));
                // 发送给打印机
                sendToPrinter(printItem_tmp, num);
            }
        }


        /// <summary>
        /// 发送给打印机
        /// </summary>
        /// <param name="printItem"></param>
        /// <param name="printnum"></param>
        protected void sendToPrinter(PrintItem printItem, int printnum)
        {
            // 这里真正的发给打印机
            PrintDocument printDocument = new PrintDocument();
            printDocument.PrinterSettings.PrinterName = printItem.PrinterName; // 设置打印机
            printDocument.PrinterSettings.Copies = (short)printnum;            // 设置打印数量
            // 然后设置打印的id
            string id = registerPrintItem(printItem);
            printDocument.DocumentName = id;            // 这个id跟printItem是存在一一对应关系的
            printDocument.PrintPage += PrintDocument_PrintPage;  // 打印事件处理
            printDocument.Print();                               // 打印。

        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            // 这里真正的打印，首先取得打印的数据
            PrintDocument printDocument = sender as PrintDocument;
            // 这里取得打印的数据了
            PrintItem printItem = GetPrintItem(printDocument.DocumentName);
            // 先提取出数据
            int rows = printItem.Shapes.Paper.Rows;
            int cols = printItem.Shapes.Paper.Cols;
            float top = printItem.Shapes.Paper.Top;
            float left = printItem.Shapes.Paper.Left;
            float modelWidth = printItem.Shapes.Paper.ModelWidth;
            float modelHeight = printItem.Shapes.Paper.ModelHeight;
            float hor = printItem.Shapes.Paper.HorizontalIntervalDistance;// 模板的水平间隔
            float ver = printItem.Shapes.Paper.VerticalIntervalDistance;  // 模板的垂直间隔
            var valsss = printItem.Valss; // 变量集合。
            var shapes = printItem.Shapes;  // 形状
            // 循环打印
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    // 更改变量
                    // 这里要判断一下是否超过索引
                    if (i * rows + j >= printItem.Valss.Count) break;
                    shapes.Vars = valsss[i * rows + j];
                    // 然后计算偏移
                    Matrix matrix = new Matrix();
                    matrix.Translate(
                        left + j * (modelWidth + hor),
                        top + i * (modelHeight + ver)
                        );
                    // 绘图
                    shapes.Draw(e.Graphics, matrix, false);

                }
            }

            //throw new NotImplementedException();
        }

        public override string getNoRepeatId()
        {
            return (++_id).ToString();// 假设打印的部分很小，足够了把。
            //throw new NotImplementedException();
        }
    }
}
