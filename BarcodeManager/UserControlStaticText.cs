using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Text;

namespace BarcodeTerminator
{
    public partial class UserControlStaticText : UserControl
    {
        public UserControlStaticText()
        {
            InitializeComponent();

            loadFonts();

        }

        private void loadFonts()
        {
            //获取系统已经安装的字体 
            InstalledFontCollection MyFontCollection = new InstalledFontCollection();
            FontFamily[] MyFontFamilies = MyFontCollection.Families;
            int Count = MyFontFamilies.Length;
            for (int i = 0; i < Count; i++)
            {
                string FontName = MyFontFamilies[i].Name;
                comboBoxFontName.Items.Add(FontName);
            } 

            //并设置Arial为默认字体

            comboBoxFontName.Text = "Arial";

            MyFontCollection.Dispose();//释放


        }

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

        //字体名称
        public string strFontName
        {
            get
            {
                return comboBoxFontName.Text;
            }
            set
            {
                comboBoxFontName.Text = value;
            }
        }
        //字体大小
        public string strFontSize
        {
            get
            {
                return comboBoxFontSize.Text;
            }
            set
            {
                comboBoxFontSize.Text = value;
            }
        }
        //是否粗体
        public bool isBold
        {
            get
            {
                return chkBold.Checked;
            }
            set
            {
                chkBold.Checked = value;
            }
        }
        //是否倾斜
        public bool isItalic
        {
            get
            {
                return chkItalic.Checked;
            }
            set
            {
                chkItalic.Checked = value;
            }
        }
        //是否下划线
        public bool isUnderLine
        {
            get
            {
                return chkUnderline.Checked;
            }
            set
            {
                chkUnderline.Checked = value;
            }
        }

        //静态文字
        public string strText
        {
            get
            {
                return txtStaticText.Text;
            }
            set
            {
                txtStaticText.Text = value;
            }
        }



        private void btnFrmDispose_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void chkBold_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBold.Checked)
            {
                chkBold.Font = new Font(chkBold.Font, FontStyle.Bold);
            }
            else
            {
                chkBold.Font = new Font(chkBold.Font, FontStyle.Regular);
            }
        }

        private void chkItalic_CheckedChanged(object sender, EventArgs e)
        {
            if (chkItalic.Checked)
            {
                chkItalic.Font = new Font(chkItalic.Font, FontStyle.Italic);
            }
            else
            {
                chkItalic.Font = new Font(chkItalic.Font, FontStyle.Regular);
            }
           
        }

        private void chkUnderline_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUnderline.Checked)
            {
                chkUnderline.Font = new Font(chkUnderline.Font, FontStyle.Underline);
            }
            else
            {
                chkUnderline.Font = new Font(chkUnderline.Font, FontStyle.Regular);
            }
        }



    }
}
