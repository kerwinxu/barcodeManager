using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using VestShapes;
using System.Drawing.Printing;
using Xuhengxiao.MyDataStructure;
using System.IO;
using System.Drawing.Imaging;

namespace BarcodeTerminator
{
    public partial class FrmSaveImage : Form
    {

        private ArrayList _arrlistRows;
        /// <summary>
        /// //很多行的数据，不止一行的
        /// </summary>
        public ArrayList arrlistRows
        {
            get { return _arrlistRows; }
            set { _arrlistRows = value; }
        }

        private string _strShapesFileName;
        /// <summary>
        /// Shapes文件路径
        /// </summary>
        public string ShapesFileName
        {
            get { return _strShapesFileName; }
            set { _strShapesFileName = value; }
        }
        


        public FrmSaveImage()
        {
            InitializeComponent();
            loadDPI();
        }

        public FrmSaveImage(string strShapesFileName, ArrayList ayylistRows1)
        {

            InitializeComponent();

            ShapesFileName = strShapesFileName;
            arrlistRows = ayylistRows1;

            loadDPI();

            loadModelSize();



        }

        /// <summary>
        /// 取得模板大小
        /// </summary>
        private void loadModelSize()
        {
            if (ShapesFileName!="")
            {
                try
                {
                    Shapes shapes1 = ClsXmlSerialization.Load<Shapes>(ShapesFileName);
                    txtImageWidth.Text = shapes1.BarcodePageSettings.BarcodePaperLayout.ModelWidth.ToString();
                    txtImageHeight.Text = shapes1.BarcodePageSettings.BarcodePaperLayout.ModelHeight.ToString();
                }
                catch (System.Exception ex)
                {
                    ClsErrorFile.WriteLine("在导出图像中不能加载模板文件",ex);  
                }
                


            }

        }
        
        /// <summary>
        /// 根据默认的打印机取得分辨率
        /// </summary>
        private void loadDPI()
        {
            //这个只是测试打印时取得分辨率而已。
            try
            {
                //这个测试分辨率就是用打印一张纸的方式来测试的

                PrintDocument myPrintDoc = new PrintDocument();
                myPrintDoc.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
                myPrintDoc.DocumentName = "测试分辨率";
                if (ClsBarcodePrint.strPrinterName!="")
                {
                    myPrintDoc.PrinterSettings.PrinterName = ClsBarcodePrint.strPrinterName;//还有设置打印机  
                }

                myPrintDoc.PrintPage += new PrintPageEventHandler(myPrintDoc_PrintPage);               
                

                myPrintDoc.Print();
            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //TextWriter errorWriter = Console.Error;
                //errorWriter.WriteLine(ex.Message);
            }
        }

        void myPrintDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            txtDPIX.Text = e.Graphics.DpiX.ToString();
            txtDPIY.Text = e.Graphics.DpiY.ToString();
            e.Cancel = true;// 取消打印
            //throw new NotImplementedException();
        }

        private void chkPrinteDPI_CheckedChanged(object sender, EventArgs e)
        {
            //可用性跟那个是相反的。
            txtDPIX.Enabled = !chkPrinteDPI.Checked;
            txtDPIY.Enabled = !chkPrinteDPI.Checked;
        }

        private void chkShapesSize_CheckedChanged(object sender, EventArgs e)
        {
            //可用性跟那个是相反的。
            txtImageHeight.Enabled = !chkShapesSize.Checked;
            txtImageWidth.Enabled = !chkShapesSize.Checked;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            //首先判断目录是否存在
            if ((txtPath.Text=="")&&(Directory.Exists(txtPath.Text)))
            {
                MessageBox.Show("没有选择目录或者目录为空.");
                return;//当然要返回啦 
            }

            Shapes myShapes;
            try
            {
                myShapes = ClsXmlSerialization.Load<Shapes>(ShapesFileName);
                myShapes.Zoom = 1;//比例为1才能打印
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("不能导入模板文件" + ex.Message);
                ClsErrorFile.WriteLine("不能导入模板文件", ex);
                return;//如果不能取得，那么就直接返回

            }

            int intIndex = 1;//加在文件尾，为了不重复文件面

            //如下是根据用户选择的多行数据进行迭代了
            if (arrlistRows != null)
            {
                foreach (ArrayList arrlistRow in arrlistRows)
                {
                    //arrlistRow只是表示一行的数据的 

                    //如下构造一个bitmap对象。根据图像宽度高度和分辨率

                    Bitmap bitmap;//下边才是赋值

                    try
                    {
                        float fDPIX = Convert.ToSingle(txtDPIX.Text);
                        float fDPIY = Convert.ToSingle(txtDPIY.Text);
                        float fModelWidth = Convert.ToSingle(txtImageWidth.Text);
                        float fModelHeight = Convert.ToSingle(txtImageHeight.Text);

                        //计算要创建的bitmap的宽度和高度
                        int intW = (int)(fDPIX * fModelWidth / 25.4);
                        int intH = (int)(fDPIY * fModelHeight / 25.4);

                        //初始化bitmap
                        bitmap = new Bitmap(intW, intH);
                        bitmap.SetResolution(fDPIX, fDPIY);//设置分辨率

                    }
                    catch (System.Exception ex)
                    {
                        ClsErrorFile.WriteLine("在导出图片中创建图像bitmap不成功", ex);
                        MessageBox.Show("在导出图片中创建图像bitmap不成功" + ex.Message);
                        return;//返回
                    }

                    //给shapes 提供相关的数据
                    myShapes.arrlistKeyValue = arrlistRow;

                    //如下是绘图
                    myShapes.Draw(Graphics.FromImage(bitmap),0,0);

                    //绘图结束了就是保存//还得判断是否有重复
                    string strFileName = "";
                    //先将各项相加
                    foreach (clsKeyValue item in arrlistRow)
                    {
                        strFileName += item.Value;
                        
                    }

                    //过滤特殊字符，
                    strFileName = FilterSpecial(strFileName);
                    
                    //判断文件是否存在

                    while (File.Exists(txtPath.Text+"\\"+strFileName+intIndex.ToString()+"."+comboBoxImageFormat.Text))
                    {
                        intIndex++;// 递增
                    }

                    //如下得到的才是真正的文件名，不重复的
                    strFileName = txtPath.Text + "\\" + strFileName + intIndex.ToString() + "." + comboBoxImageFormat.Text;

                    intIndex++;// 递增

                    //最后才是保存数据
                    
                    try
                    {
                        bitmap.Save(strFileName, getImageFormat());
                    }
                    catch (System.Exception ex)
                    {
                        ClsErrorFile.WriteLine("不能保存图像", ex);
                        MessageBox.Show("不能保存图像" + ex.Message);
                    }

                }
            }

            this.Dispose();
        }

        /// <summary>
        /// 这个方法是取得用户选择的图片格式
        /// </summary>
        /// <returns></returns>
        private ImageFormat getImageFormat()
        {
            //
            switch (comboBoxImageFormat.Text)
            {
                case "Emf":
                    return ImageFormat.Emf;
                case "Bmp":
                    return ImageFormat.Bmp;
                case "Exif":
                    return ImageFormat.Exif;
                case "Gif":
                    return ImageFormat.Gif;
                case "Icon":
                    return ImageFormat.Icon;
                case "Jpeg":
                    return ImageFormat.Jpeg;
                case "Png":
                    return ImageFormat.Png;
                case "Tiff":
                    return ImageFormat.Tiff;
                case "Wmf":
                    return ImageFormat.Wmf;

                default:
                    break;
            }

            return ImageFormat.Png;
        }

        private string FilterSpecial(string strHtml)
        {
            if (string.Empty == strHtml)
            {
                return strHtml;
            }
            string[] aryReg = { "'", "'delete", "?", "<", ">", "%", "\"\"", ",", ".", ">=", "=<", "_", ";", "||", "[", "]", "&", "/", "-", "|", " ", "''" };
            for (int i = 0; i < aryReg.Length; i++)
            {
                strHtml = strHtml.Replace(aryReg[i], string.Empty);
            }
            return strHtml;
        }

        private void btnOpenPath_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                txtPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
