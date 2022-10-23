using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.Xml.Serialization;
using System.IO;
using Xuhengxiao.MyDataStructure;

namespace VestShapes
{
    public partial class FrmPageSettings : Form
    {
        public  ClsPageSettings BarcodePageSettings=new ClsPageSettings ();//这个作为静态变量来返回窗口的方法


        public FrmPageSettings()
        {
            InitializeComponent();
            myInit();

        }

        /// <summary>
        /// 我自己的初始化
        /// </summary>
        private void myInit()
        {
            tabControl1.Location = new Point(0, 0);
            //tabControl1.Width = this.Width;
            //tabControl1.Height = this.Height;

            loadPrintersAndPaperSize();

            LoadPageSetting();

            
            visiableFalseShapesAttrib();

            //更新事件
            BarcodePageSettings.BarcodePaperLayout.PaperLayoutChanged += new ClsPaperLayout.PaperLayoutChangedEventHandler(BarcodePaperLayout_PaperLayoutChanged);
            this.comboPrinters.SelectedValueChanged += new System.EventHandler(this.comboPrinters_SelectedValueChanged);
            // 
           
        }

        

        void BarcodePaperLayout_PaperLayoutChanged(object sender, PaperLayoutChangedEventArgs e)
        {
            LoadPageSetting();
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 加载参数到控件中
        /// </summary>
        private void LoadPageSetting()
        {
            
            if (BarcodePageSettings != null)
            {

                BarcodePageSettings.BarcodePaperLayout.Compute();

                //如下之纸张项目
                //因为这个是在初始化中，所以需要判断原先是否为空
                if (comboPaperSizes.Items.Count==0)
                {
                    ClsKeyOjbect myKeyValue = new ClsKeyOjbect(BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize.PaperName, BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize);
                    comboPaperSizes.Items.Add(myKeyValue);
                   
                }
                comboPaperSizes.Text = BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize.PaperName;//设置纸张内容
                
                //如下是布局项目
                //如下是布局
                UpDownNumberOfLine.Value = BarcodePageSettings.BarcodePaperLayout.NumberOfLine;// 设置行数
                UpDownNumberOfColumn.Value = BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;//设置列数

                //如下是边距
                txtMaginsTop.Text = BarcodePageSettings.BarcodePaperLayout.Top.ToString();
                txtMaginsLeft.Text = BarcodePageSettings.BarcodePaperLayout.Left.ToString();
                txtMaginsRight.Text = BarcodePageSettings.BarcodePaperLayout.Right.ToString();
                txtMaginsBottom.Text = BarcodePageSettings.BarcodePaperLayout.Bottom.ToString();

                //如下是模板大小
                txtModelWidth.Text = BarcodePageSettings.BarcodePaperLayout.ModelWidth.ToString();
                txtModelHeight.Text = BarcodePageSettings.BarcodePaperLayout.ModelHeight.ToString();
                chkCustomModelSize.Checked = BarcodePageSettings.BarcodePaperLayout.CustomModelSize;

                //如下是间距
                txtHorizontalInterval.Text = BarcodePageSettings.BarcodePaperLayout.HorizontalInterval.ToString();
                txtVerticalInterval.Text = BarcodePageSettings.BarcodePaperLayout.VerticalInterval.ToString();
                chkCustomInterval.Checked = BarcodePageSettings.BarcodePaperLayout.CustomDistance;

                //在预览的下边显示模板大小
                lblModelSize.Text = "模板大小：  " + BarcodePageSettings.BarcodePaperLayout.ModelWidth.ToString() + " X " + BarcodePageSettings.BarcodePaperLayout.ModelHeight.ToString() + " 毫米"; 
                //如下显示纸张大小
                lblPaperSize.Text = "纸张大小：  " + BarcodePageSettings.BarcodePaperLayout.PaperWidth.ToString() + " X " +BarcodePageSettings.BarcodePaperLayout.PaperHeight.ToString() + " 毫米";

                //设置是否横向
                if (BarcodePageSettings.BarcodePaperLayout.LandScape)
                {
                    radioButton2.Checked = true;
                    radioButton1.Checked = false;
                }
                else
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }

                
                //如下是调用预览 
                pictureBox1.Refresh();

            }

        }

        public FrmPageSettings(ClsPageSettings pageSettings)
        {
            InitializeComponent();


            //如下是用序列化深度拷贝一个对象
            #region
            try
            {
                if (pageSettings==null)
                {
                    BarcodePageSettings = new ClsPageSettings();
                }
                else
                {
                    //如下是用内存流来序列化
                    if (true)
                    {
                        using (MemoryStream memory3 = new MemoryStream())
                        {
                            XmlSerializer xs = new XmlSerializer(typeof(ClsPageSettings));

                            xs.Serialize(memory3, pageSettings);//序列化
                            memory3.Seek(0, SeekOrigin.Begin);//移动到开头

                            //反序列化
                            XmlSerializer xs2 = new XmlSerializer(typeof(ClsPageSettings));
                            BarcodePageSettings = xs2.Deserialize(memory3) as ClsPageSettings;//这样就深度拷贝了

                            memory3.Close();//销毁
                        }
                    
                }


                }


                #region  如下的会产生问题,所以注释掉了

                if (false)
                {
                    using (Stream stream = new FileStream(Application.StartupPath + "\\tempPageSettings", FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        //如下是序列化
                        XmlSerializer xmls = new XmlSerializer(typeof(ClsPageSettings));
                        xmls.Serialize(stream, pageSettings);

                        //stream.Seek(0, SeekOrigin.Begin);//回到开头。


                    }
                    //如下是反序列化
                    using (Stream stream = new FileStream(Application.StartupPath + "\\tempPageSettings", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        XmlSerializer xmls = new XmlSerializer(typeof(ClsPageSettings));
                        //如下是反序列化
                        BarcodePageSettings = xmls.Deserialize(stream) as ClsPageSettings;

                    }
                }
#endregion

                myInit();


            }
            catch (Exception exception)
            {
                //ClsErrorFile.WriteLine("不成功，原因是", exception);
                //MessageBox.Show("保存不成功，原因是" + exception.Message);

                if (exception.InnerException.Message != null)
                {
                    //ClsErrorFile.WriteLine("xml不成功" + exception.InnerException.Message);
                    //MessageBox.Show(exception.InnerException.Message);
                }
            }
            finally
            {

            }
            #endregion



        }


        /// <summary>
        /// 加载打印机和纸张尺寸
        /// </summary>
        private void loadPrintersAndPaperSize()
        {
            try
            {
                //将所有打印机名添加到
                foreach (object obj in PrinterSettings.InstalledPrinters)
                {

                    comboPrinters.Items.Add(obj.ToString());

                }

                //获得默认打印机
                /**
                PrintDocument printDoc = new PrintDocument();
                string strPrinterName = printDoc.DefaultPageSettings.PrinterSettings.PrinterName;
                loadPaperSize(strPrinterName);
                comboPrinters.Text = strPrinterName;
                **/

            }
            catch (Exception ex)
            {
                MessageBox.Show("加载纸张尺寸不成功，原因是"+ex.Message);
                
                //throw;
            }

        }

        /// <summary>
        /// 加载尺寸
        /// </summary>
        /// <param name="strPrinterName">打印机名称</param>
        private void loadPaperSize(string strPrinterName)
        {
            try
            {
                
                PrintDocument printDoc = new PrintDocument();

                printDoc.DefaultPageSettings.PrinterSettings.PrinterName = strPrinterName;

                //保存原先的
                ClsKeyOjbect objSelectItem =new ClsKeyOjbect("",null);;
                if (comboPaperSizes.SelectedItem!=null)
                {
                    objSelectItem = (ClsKeyOjbect)comboPaperSizes.SelectedItem;
                }

                comboPaperSizes.Items.Clear();

                foreach (PaperSize item in printDoc.PrinterSettings.PaperSizes)
                {
                    ClsKeyOjbect myKeyValue = new ClsKeyOjbect(item.PaperName, item);
                    comboPaperSizes.Items.Add(myKeyValue);

                    //判断是否是原先的项目,原先的为空就不用验证了,
                    if ((objSelectItem.strKey!="") && (objSelectItem.strKey == item.PaperName))
                    {
                        comboPaperSizes.SelectedItem = myKeyValue;
                    }
                }

                //如果有相同的纸张定义就选择原先的
                if (comboPaperSizes.SelectedItem==null)
                {
                    //如果没有，就显示第一项
                    comboPaperSizes.SelectedIndex = 0;

                }

            }
            catch (System.Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            

        }

        private void chkCustomModelSize_CheckedChanged(object sender, EventArgs e)
        {
            txtModelHeight.Enabled = chkCustomModelSize.Checked;
            txtModelWidth.Enabled = chkCustomModelSize.Checked;

            BarcodePageSettings.BarcodePaperLayout.CustomModelSize = chkCustomModelSize.Checked;
        }

        private void radioButtonRect_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonRect.Checked)
            {
                visiableFalseShapesAttrib();

                BarcodePageSettings.ModelShapes = "方形";

                LoadPageSetting();
            }

        }

        /// <summary>
        /// 将形状界面的多项不通用部分隐藏
        /// </summary>
        private void visiableFalseShapesAttrib()
        {
            lblRoundRadius.Visible = false;
            txtRoundRadius.Visible = false;
            chkEllipseHole.Visible = false;
            lblEllipseHoleRadius.Visible = false;
            txtEllipseHoleRadius.Visible = false;
            txtEllipseHoleRadius.Enabled = false;//默认这个是不启用的


        }

        private void radioButtonRoundRect_CheckedChanged(object sender, EventArgs e)
        {
            //如果被选择则显示那么多
            if (radioButtonRoundRect.Checked)
            {
                visiableFalseShapesAttrib();
                lblRoundRadius.Visible = true;
                txtRoundRadius.Visible = true;

                BarcodePageSettings.ModelShapes = "圆角矩形";

                LoadPageSetting();

            }
        }

        private void radioButtonEllipse_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEllipse.Checked)
            {
                visiableFalseShapesAttrib();
                chkEllipseHole.Visible = true;
                lblEllipseHoleRadius.Visible = true;
                txtEllipseHoleRadius.Visible = true;
                txtEllipseHoleRadius.Enabled = false;//默认这个是不启用的

                BarcodePageSettings.ModelShapes = "椭圆形";

                LoadPageSetting();

            }
        }

        private void chkEllipseHole_CheckedChanged(object sender, EventArgs e)
        {
            txtEllipseHoleRadius.Enabled = chkEllipseHole.Checked;
        }

        private void UpDownNumberOfColumn_ValueChanged(object sender, EventArgs e)
        {
            BarcodePageSettings.BarcodePaperLayout.NumberOfColumn = Convert.ToInt32(UpDownNumberOfColumn.Value);

            //如果大于1，才会选择间距
            if (UpDownNumberOfColumn.Value == 1)
            {
                txtHorizontalInterval.Enabled = false;

            }

        }

        private void UpDownNumberOfLine_ValueChanged(object sender, EventArgs e)
        {
            BarcodePageSettings.BarcodePaperLayout.NumberOfLine = Convert.ToInt32(UpDownNumberOfLine.Value);

            //只有大于1，才会有间距
            if (UpDownNumberOfLine.Value == 1)
            {
                txtVerticalInterval.Enabled = false ;

            }

        }

        private void comboPaperSizes_SelectedValueChanged(object sender, EventArgs e)
        {
            PaperSize paperSize = (PaperSize)(((ClsKeyOjbect)comboPaperSizes.SelectedItem).objValue);

            BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize = paperSize;//设置到

            //LoadPageSetting();

            //显示纸张大小
            //lblPaperSize.Text = "纸张大小：  " + Math.Round(paperSize.Width*0.254,0) + " X " + Math.Round(paperSize.Height*0.254,0) + " 毫米"; 

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }

        private void FrmPageSettings_Load(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void txtMaginsTop_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.Top = Convert.ToSingle(txtMaginsTop.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到上边距，原因是"+ex.Message);
                //ClsErrorFile.WriteLine("读取不到上边距",ex);
                //throw;
            }
            
        }

        private void txtMaginsLeft_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.Left = Convert.ToSingle(txtMaginsLeft.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到左边距，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到左边距", ex);
                //throw;
            }
        }

        private void txtMaginsBottom_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.Bottom = Convert.ToSingle(txtMaginsBottom.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到下边距，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到下边距", ex);
                //throw;
            }
        }

        private void txtMaginsRight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.Right = Convert.ToSingle(txtMaginsRight.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到右边距，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到右边距", ex);
                //throw;
            }
        }

        private void txtModelWidth_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.ModelWidth = Convert.ToSingle(txtModelWidth.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到模板宽度，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到模板宽度", ex);
                //throw;
            }
        }

        private void txtModelHeight_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.ModelHeight = Convert.ToSingle(txtModelHeight.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到模板高度，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到模板高度", ex);
                //throw;
            }
        }

        private void txtHorizontalInterval_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.HorizontalInterval = Convert.ToSingle(txtHorizontalInterval.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到水平间距，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到水平间距", ex);
                //throw;
            }
        }

        private void txtVerticalInterval_TextChanged(object sender, EventArgs e)
        {
            try
            {
                BarcodePageSettings.BarcodePaperLayout.VerticalInterval = Convert.ToSingle(txtVerticalInterval.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取不到垂直间距，原因是" + ex.Message);
                //ClsErrorFile.WriteLine("读取不到垂直间距", ex);
                //throw;
            }
        }

        private void chkCustomInterval_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCustomInterval.Checked)
            {
                //只有多行或者多列的情况才能是修改
                if (UpDownNumberOfLine.Value > 1)
                {
                    txtVerticalInterval.Enabled = true;
                }
                else
                {
                    txtVerticalInterval.Enabled = false;
                }

                if (UpDownNumberOfColumn.Value > 1)
                {
                    txtHorizontalInterval.Enabled = true;
                }
                else
                {
                    txtHorizontalInterval.Enabled = false;
                }

            }
            else
            {
                txtVerticalInterval.Enabled = false;
                txtHorizontalInterval.Enabled = true;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            BarcodePageSettings.DrawModelsBackgroundOnPaper(e.Graphics, pictureBox1.Width, pictureBox1.Height);
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                BarcodePageSettings.BarcodePaperLayout.TurnLandScape(false) ;//纵向
            }
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                BarcodePageSettings.BarcodePaperLayout.TurnLandScape(true);//横向
            }
            
        }

        private void comboPrinters_SelectedValueChanged(object sender, EventArgs e)
        {
            loadPaperSize(comboPrinters.Text);
        }

        private void btn_paper_ok_Click(object sender, EventArgs e)
        {
            // 这个用自定义的纸张
            int paper_width;
            int paper_height;
            if (! int.TryParse(txt_paper_width.Text, out paper_width))
            {
                MessageBox.Show("自定义纸张宽度设置不对");
            }
            if (!int.TryParse(txt_paper_height.Text, out paper_height))
            {
                MessageBox.Show("自定义纸张高度设置不对");
            }

            // 这里要设置成英寸。

            PaperSize paperSize = new PaperSize("自定义", (int)(paper_width/25.4*100), (int)(paper_height/25.4*100));

            BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize = paperSize;//设置到

            //在预览的下边显示模板大小
            lblModelSize.Text = "模板大小：  " + BarcodePageSettings.BarcodePaperLayout.ModelWidth.ToString() + " X " + BarcodePageSettings.BarcodePaperLayout.ModelHeight.ToString() + " 毫米";
            //如下显示纸张大小
            lblPaperSize.Text = "纸张大小：  " + BarcodePageSettings.BarcodePaperLayout.PaperWidth.ToString() + " X " + BarcodePageSettings.BarcodePaperLayout.PaperHeight.ToString() + " 毫米";



        }

        private void comboPrinters_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    public class ClsKeyOjbect
    {
        public  string strKey;
        public  object objValue;

        public ClsKeyOjbect(string str, object obj)
        {
            strKey = str;
            objValue = obj;
        }

        public override string ToString()
        {
            return strKey;
            //return base.ToString();
        }

    }

}
