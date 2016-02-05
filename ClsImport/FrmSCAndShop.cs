using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Xuhengxiao.ImportData
{
    public partial class FrmSCAndShop : Form
    {
        public static string strSC;
        public static string strShop;

        public FrmSCAndShop()
        {
            InitializeComponent();
            
            //为了防止以前的数据
            strSC = "";
            strShop = "1";
        }
        private void frmSCAndShop_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            strSC = txtSC.Text;
            strShop = txtShop.Text;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

    }
}
