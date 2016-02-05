using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Xuhengxiao.ImportData
{
    public partial class FrmChooseExcelSheet : Form
    {
        public FrmChooseExcelSheet()
        {
            InitializeComponent();
        }

        public static string strSheetName;

        public FrmChooseExcelSheet(string[] strExcelSheets)
        {

            InitializeComponent();


            foreach (string strExcelSheetName in strExcelSheets)
            {
                comboBoxExcelSheet.Items.Add(strExcelSheetName);
            }

            comboBoxExcelSheet.Text = comboBoxExcelSheet.Items[0].ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            strSheetName = comboBoxExcelSheet.Text;
            this.Dispose();
        }

    }
}
