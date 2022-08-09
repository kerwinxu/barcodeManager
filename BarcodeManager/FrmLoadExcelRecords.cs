using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Xuhengxiao.DataBase;

namespace BarcodeTerminator
{
    public partial class FrmLoadExcelRecords : Form
    {
        public static string strTableName;

        public FrmLoadExcelRecords()
        {
            InitializeComponent();

            //将main表填充到窗体中。
            ClsDataBase myClsDataBase=new ClsDataBase ();
            //dataGridView1.DataSource = myClsDataBase.getMainTable();

            strTableName = "";//初始化而已
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //首先判断是否选择了某一行，如果选择了，就取得相应的表名并关闭就可以了,如果没有取得就弹出对框框说请选择，最后设置返回值属性。
            if (dataGridView1.SelectedCells.Count > 0)
            {
                strTableName = dataGridView1.CurrentRow.Cells["表名"].Value.ToString();//获取用户选择的表名
                this.DialogResult = DialogResult.OK;//对话框返回值
                this.Dispose();
            }
            else
            {
                //如果没有选择就弹出对话框说要选择
                MessageBox.Show("请选择要打开的以前导入的表");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //设置返回值并销毁窗口
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //这个跟单击确定唯一的区别是，如果没有选择，不用弹出让用户选择的的消息。
            //首先判断是否选择了某一行，如果选择了，就取得相应的表名并关闭就可以了,如果没有取得就弹出对框框说请选择，最后设置返回值属性。
            if (dataGridView1.SelectedCells.Count > 0)
            {
                strTableName = dataGridView1.CurrentRow.Cells["表名"].Value.ToString();//获取用户选择的表名
                this.DialogResult = DialogResult.OK;//对话框返回值
                this.Dispose();
            }

        }
    }
}
