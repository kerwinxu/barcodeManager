using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Xuhengxiao.MyDataStructure;

namespace BarcodeTerminator
{
    public partial class UserControlBarcode : UserControl
    {
        public UserControlBarcode()
        {
            InitializeComponent();

            //设置默认的条形码类型为EAN 13
            comboBoxEncoding.SelectedItem = "EAN13";

            //加载变量名。从arrlistSellectRow得到列名。条形码数据也是个变量

            for (int i = 0; i < frmMain.arrlistSellectRow.Count; i++)
            {
                string str = ((clsKeyValue)(frmMain.arrlistSellectRow[i])).Key;
                comboBoxVaribaleName.Items.Add(str);
            }

        }

        private void btnFrmDispose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        //如下是设置属性

        //坐标x
        public string strX
        {
            get
            {
                return txtX.Text;
            }
            set
            {
                txtX.Text = value;
            }
        }

        //坐标y
        public string strY
        {
            get
            {
                return txtY.Text;
            }
            set
            {
                txtY.Text = value;
            }
        }
        //宽度
        public string strWidth
        {
            get
            {
                return txtWidth.Text;
            }
            set
            {
                txtWidth.Text = value;
            }
        }
        //高度
        public string strHeight
        {
            get
            {
                return txtHeight.Text;
            }
            set
            {
                txtHeight.Text = value;
            }
        }
        //条形码类型
        public string strEncoding
        {
            get 
            {
                return comboBoxEncoding.SelectedItem.ToString();
            }
            set
            {
                comboBoxEncoding.SelectedItem = value;
            }
        }


        //变量名
        public string strVariableName
        {
            get
            {
                return comboBoxVaribaleName.Text;
            }
            set
            {
                comboBoxVaribaleName.Text = value;
            }
        }

        //条形码数据
        public string strData
        {
            get
            {
                return txtBarcodeData.Text;
            }
            set
            {
                txtBarcodeData.Text = value;
            }
        }
        public bool isIncludeLabel
        {
            get
            {
                return chkIncludeLabel.Checked;
            }
            set
            {
                chkIncludeLabel.Checked = value;
            }
        }

        private void comboBoxVaribaleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //在这个更改时就更改相应的变量 
            foreach (clsKeyValue keyvalue in frmMain.arrlistSellectRow)
            {
                if (keyvalue.Key == comboBoxVaribaleName.Text)
                {
                    txtBarcodeData.Text = keyvalue.Value;
                    return;
                }
            }
        }


    }
}
