using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace BarcodeTerminator
{
    public partial class FrmQueuePrint : Form
    {
        //这个窗体不停的用定时器来搜索打印队列
        //而对这个打印队列，唯一的操作就是删除，当然还有常规的确定
        private ArrayList   queuePrintList = new ArrayList();



        public FrmQueuePrint()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //首先判断是否有选择的，如果有选择的就保存
            //读取打印队列，判断是否有更改，如果有更更对对话框
            //看看那个已选择的是否在打印队列中，如果是，就将这个设置为新的列表框中的选择的
            string strSelectPrint = lstQueuePrint.Text;

            if (ClsBarcodePrint.arrlistPrint.Equals(queuePrintList))
                return;//如果相同了，那么就直接返回就可以了。

            //queuePrintList =(ArrayList) ClsBarcodePrint.arrlistPrint.Clone();
            
            ////首先清除所有项目，再重新添加
            //lstQueuePrint.Items.Clear();
            //foreach (queuePrintItem myqueuePrintItem in queuePrintList)
            //{
            //    lstQueuePrint.Items.Add(myqueuePrintItem.ToString());
            //}            

            if (lstQueuePrint.Items.Contains(strSelectPrint))
                lstQueuePrint.Text = strSelectPrint;

            
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //从打印队列中删除一项。
            //首先判断是否选择了一项
            if (lstQueuePrint.SelectedIndex >= 0)
            {
                /**
                 * 下边这个有些绕口，
                 * lstQueuePrint.SelectedIndex 为选择项目的索引
                 * queuePrintList[lstQueuePrint.SelectedIndex]，就是项目的值了
                 * Remove 是删除匹配项
                 * */
                //ClsBarcodePrint.arrlistPrint.Remove((queuePrintItem)(queuePrintList[lstQueuePrint.SelectedIndex]));
            }

        }


    }
}
