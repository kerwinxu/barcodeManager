using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using VestShapes;
using Xuhengxiao.MyDataStructure;
using Xuhengxiao.DataBase;

using Xuhengxiao.ImportData;

using Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint;

namespace BarcodeTerminator
{
    public partial class FrmBarcodeEdit : Form
    {
        public clsKeyValue shapesFileData;//保存xml需要用到这个里边的路径信息

        //private  ArrayList arrlistKeyValue;//保存变量信息的。
        public  DataTable CurrentDataTable;
        public  string strTableName;
        

        public FrmBarcodeEdit(string strFileName)
        {
            InitializeComponent();


            userControlCanvas1.Loader(strFileName);

            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);
            //我自己的初始化
            myInit();


        }

        //private string _strTemplet = @"<Shapes xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"><arrlistShapeEle/><BarcodePageSettings><arrModelShapes><string>方形</string><string>圆角矩形</string><string>椭圆形</string><string>CD</string></arrModelShapes><ModelShapes>圆角矩形</ModelShapes><BarcodePaperLayout><strError/><BarcodePaperSize><Height>1169</Height><PaperName>A4</PaperName><RawKind>9</RawKind><Width>827</Width></BarcodePaperSize><NumberOfLine>2</NumberOfLine><NumberOfColumn>2</NumberOfColumn><Top>2</Top><Left>2</Left><Right>2</Right><Bottom>2</Bottom><ModelWidth>102</ModelWidth><ModelHeight>146.5</ModelHeight><CustomModelSize>false</CustomModelSize><HorizontalInterval>2</HorizontalInterval><VerticalInterval>0</VerticalInterval><CornerRadius>2</CornerRadius><CustomDistance>true</CustomDistance></BarcodePaperLayout></BarcodePageSettings><arrlistKeyValue/><Zoom>0.97</Zoom></Shapes>"

        public FrmBarcodeEdit()
        {
            InitializeComponent();

            // 默认的话是打开这个，
            shapesFileData = new clsKeyValue("templet", null);

            //
            myInit();
            

        }
        public FrmBarcodeEdit(clsKeyValue lt , DataTable dataTable , List<clsKeyValue> arrlist , string tableName)
        {
            InitializeComponent();

            //这个需要保存。
            shapesFileData = lt;

            strTableName = tableName;//保存表名

            //加载形状
            userControlCanvas1.Loader(lt.Value);

            LoadDataTable(dataTable);//加载表格
            
            //加载变量
            setKeyValue(arrlist);

            //我自己的初始化
            myInit();



        }

        /// <summary>
        /// 加载DataTable数据
        /// </summary>
        /// <param name="dataTable"></param>
        public  void LoadDataTable(DataTable dataTable)
        {
            try
            {
                if (dataTable != null)
                {
                    CurrentDataTable = dataTable;//这个可以选择变量

                    //如下两个是绑定那个数据
                    bindingNavigator1.BindingSource = bindingSource1;
                    bindingSource1.DataSource = CurrentDataTable;
                    bindingNavigatorPositionItem.Text = "1";//将这个重设为第一条记录

                    //将第一项更新到画布
                    List<clsKeyValue> arrlist = new List<clsKeyValue>();

                    if ((CurrentDataTable.Rows.Count > 0))
                    {
                        //根据列名迭代
                        foreach (DataColumn item in CurrentDataTable.Columns)
                        {
                            clsKeyValue myclsKeyValue = new clsKeyValue(item.Caption, CurrentDataTable.Rows[0][item.Caption].ToString());
                            arrlist.Add(myclsKeyValue);
                        }

                    }

                    //给画布更新变量信息。
                    setKeyValue(arrlist);
                    //更新到画布
                    userControlCanvas1.setKeyValues(arrlist) ;

                    loadPrintedQtytoComboBox();// 判断哪像是数字
                }

            }
            catch (System.Exception ex)
            {
                //ClsErrorFile.WriteLine("模板编辑中加载DataTable数据出现异常",ex);
            	
            }
            
        }

        /// <summary>
        /// 这个方法只是设置变量信息的
        /// </summary>
        /// <param name="arrlist"></param>
        public void setKeyValue(List<clsKeyValue> arrlist)
        {
            List<clsKeyValue> arrlisttemp = new List<clsKeyValue>();
            try
            {
                if (arrlist != null)
                {
                    foreach (var item in arrlist)
                    {
                        arrlisttemp.Add(new clsKeyValue(item.Key, item.Value));
                    }

                    userControlCanvas1.setArrKeyValue(arrlist);

                    userControlCanvas1.Refresh();
                }
            }
            catch (System.Exception ex)
            {
                //ClsErrorFile.WriteLine("设置变量信息不成功",ex);
            	
            }



        }
        


        private void myInit()
        {

            //设置工具可以操作画布的。
            userControlToolBox1.setCanvas(userControlCanvas1);

            //设置画布的尺寸同布局大小
            canvasResie();

            //userControlCanvas1.ZoomPaperToScreen(); // 如果没有图形，这个容易出问题。

            ClsBarcodePrint c = new ClsBarcodePrint();//只是一个启动计时器的

            if (ClsBarcodePrint.strPrinterName == "")
            {
                ClsBarcodePrint.strPrinterName = c.myPrintDocument.PrinterSettings.PrinterName;//默认打印机
            }

            toolStripLabelPrintingName.Text = ClsBarcodePrint.strPrinterName;//默认打印机

        }


        private void frmBarcodeEdit_Load(object sender, EventArgs e)
        {

        }

        private void canvasResie()
        {

            //调节画布的宽和高的
            userControlCanvas1.Location = new Point(1, 1);
            userControlCanvas1.Width = splitContainer2.Panel1.Width - 2;
            userControlCanvas1.Height = splitContainer2.Panel1.Height - 2;

        }

        private void splitContainer2_Panel1_Resize(object sender, EventArgs e)
        {
            canvasResie();
        }

        private void btnLoadXmlBarcodeModel_Click(object sender, EventArgs e)
        {
            loadXmlBarcodeModel();
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        private void loadXmlBarcodeModel()
        {
            //加载文件，并保存信息到shapesFileData
            string strFileName = userControlCanvas1.Loader();//返回的是文件名

            if (strFileName != "")
            {
                shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

                this.Text = "条形码编辑设计 www.xuhengxiao.com 打开文件：" + strFileName;

            }

        }

        private void btnNewBarcodeModel_Click(object sender, EventArgs e)
        {
            //这个我想把它做成，打开一个条形码模板，但关闭前判断是不是这个模板，如果是，就提示保存
            NewBarcodeModel();

        }

        /// <summary>
        /// 新建模板
        /// </summary>
        private void NewBarcodeModel()
        {
            //这个我想把它做成，打开一个条形码模板，但关闭前判断是不是这个模板，如果是，就提示保存

            string strFileName = Application.StartupPath + "\\BarcodeModel\\templet.barcode";

            userControlCanvas1.Loader(strFileName);

            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

        }

        

        private void btnSaveBarcodeModel_Click(object sender, EventArgs e)
        {
            SaveBarcodeModel();
            this.Dispose();
        }

        /// <summary>
        /// 保存
        /// </summary>
        private void SaveBarcodeModel()
        {
            string strFileName;

            while (((shapesFileData.Key == "templet")))
            {
                strFileName = userControlCanvas1.Saver();
                shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

                if (strFileName == "")
                {
                    MessageBox.Show("您没有选择文件");
                    return;
                }

            }

            strFileName = userControlCanvas1.Saver(shapesFileData.Value);

            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);
        }

        private void btnSaveAsBarcodeModel_Click(object sender, EventArgs e)
        {
            SaveAsBarcodeModel();

        }
        /// <summary>
        /// 另存为
        /// </summary>
        private void SaveAsBarcodeModel()
        {
            string strFileName = userControlCanvas1.Saver();
            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            TestPrint();
        }

        /// <summary>
        /// 测试打印
        /// </summary>
        private void TestPrint()
        {

            //如下这个只是复制一个值变量再传递
            List<clsKeyValue> arrlist = new List<clsKeyValue>();//用这个是因为 arrlistSellectRow 也是静态变量，读取的时候会读取到不是想要的值,这里用深拷贝
            if (userControlCanvas1.myShapes.arrlistKeyValue != null)
            {
                foreach (var myKeyValue in userControlCanvas1.myShapes.arrlistKeyValue)
                {
                    clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                    arrlist.Add(kv);
                }

            }

            userControlCanvas1.Saver(Application.StartupPath + "\\barcodeEditTestPrint.barcode");

            //测试的话只打印一页就可以了。
            ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

            queuePrintItem printDetails = new queuePrintItem();
            printDetails.strTableName = "Test";
            printDetails.ShapesFileName = Application.StartupPath + "\\barcodeEditTestPrint.barcode";
            printDetails.IsFull = chkIsFull.Checked;
            queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
            queuePrintItemRowAndPages1.arrlistRow = arrlist;
            queuePrintItemRowAndPages1.intPages = 1;
            printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);


            myBarcodePrint.addPrintDetails(printDetails);
        }
        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            LoadExcel();

        }
        /// <summary>
        /// 导入EXCEL表格
        /// </summary>
        private void LoadExcel()
        {
            ClsImportExcel importExcel = new ClsImportExcel();
            LoadDataTable(importExcel.loadExcelDataTalbe);//将导入的表赋值给显示表格的dataGridView
            strTableName = importExcel.strCurrentTableName;

            //还得选择哪个是数量
            loadPrintedQtytoComboBox();

        }

        private void bindingNavigatorMoveLastItem_TextChanged(object sender, EventArgs e)
        {
        }

        private void bindingNavigatorPositionItem_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int intCurrentLine = Convert.ToInt32(bindingNavigatorPositionItem.Text);//取得当前行

                List<clsKeyValue> arrlist = new List<clsKeyValue>();

                foreach (DataColumn item in CurrentDataTable.Columns)
                {
                    clsKeyValue keyvalue = new clsKeyValue(item.Caption,CurrentDataTable.Rows[intCurrentLine-1][item.Caption].ToString());
                    arrlist.Add(keyvalue);
                }

                //给画布更新变量信息。
                setKeyValue(arrlist);

                //更新要打印数量。
                txtQtyOfWantToPrinted.Text = CurrentDataTable.Rows[intCurrentLine - 1][comboBoxQtyOfWantToPrinted.Text].ToString();

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //throw;
            }
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            string strFileName;

            while (((shapesFileData.Key == "templet")))
            {
                strFileName = userControlCanvas1.Saver();
                shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

                if (strFileName == "")
                {
                    MessageBox.Show("请先保存文件再打印，或许用“测试打印”不用保存");
                    return;
                }

            }

            strFileName = userControlCanvas1.Saver(shapesFileData.Value);

            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

            int intPages = getQtyOfWantToPrinted();//取得打印数量，加上损耗

            if (intPages > 0)
            {
                printBarcode(intPages);
            }

        }

        private void printBarcode(int intPages)
        {

            ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

            //如下的这个有些破坏了封装性,我用这个只是为了打印结束后判断是否自动打印。
            //myBarcodePrint.myPrintDocument.EndPrint += new PrintEventHandler(myPrintDocument_EndPrint);

            //如下如下是将变量深拷贝

            //如下这个只是复制一个值变量再传递
            List<clsKeyValue> arrlist = new List<clsKeyValue>();//用这个是因为 arrlistSellectRow 也是静态变量，读取的时候会读取到不是想要的值,这里用深拷贝
            if (userControlCanvas1.myShapes.arrlistKeyValue != null)
            {
                foreach (var myKeyValue in userControlCanvas1.myShapes.arrlistKeyValue)
                {
                    clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                    arrlist.Add(kv);
                }

            }

            // 这里直接打印吧
            // Shapes shapes, List<List<clsKeyValue>> arr2Data, List<int> printCount, string PrinterName, bool isFull=false
            var shapes = userControlCanvas1.myShapes; //
            List<List<clsKeyValue>> arr2Data = new List<List<clsKeyValue>>();
            arr2Data.Add(arrlist);
            List<int> pages = new List<int>();
            pages.Add(intPages);
            var printerName = string.Empty;
            if (!string.IsNullOrEmpty(toolStripLabelPrintingName.Text)) printerName = toolStripLabelPrintingName.Text;
            bool isFull = chkIsFull.Checked;
            new BarcodePrintImpl().print(shapes, arr2Data, pages, printerName, isFull);


            ////创建打印信息。
            //queuePrintItem printDetails = new queuePrintItem();
            //printDetails.strTableName = strTableName;
            //printDetails.ShapesFileName = (shapesFileData).Value;
            //printDetails.IsFull = chkIsFull.Checked;
            //queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
            //queuePrintItemRowAndPages1.arrlistRow = arrlist;
            //queuePrintItemRowAndPages1.intPages = intPages;
            //printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);
            //myBarcodePrint.addPrintDetails  (printDetails);

        }

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
                
            string strFileName;

            while (((shapesFileData.Key == "templet")))
            {
                strFileName = userControlCanvas1.Saver();
                shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

                if (strFileName == "")
                {
                    MessageBox.Show("请先保存文件再打印，或许用“测试打印”不用保存");
                    return;
                }

            }

            strFileName = userControlCanvas1.Saver(shapesFileData.Value);

            shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

            //userControlCanvas1.Saver(shapesFileData.Value);

            int intPages = getCurrentPrintPages();

            //只有大于零才打印
            if (intPages > 0)
            {
                printBarcode(intPages);

            }
        }


        //取得用户实际要打印的数量
        private int getCurrentPrintPages()
        {
            int intPages = 0;

            try
            {
                if (txtCurrentPrintPage.Text != "")
                {
                    intPages = Convert.ToInt32(txtCurrentPrintPage.Text);
                }
                else
                {

                    MessageBox.Show("请输入本次要打印的页数");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("不能取得打印数量，原因是" + ex.Message);
            }

            return intPages;//返回打印数量
        }

        private void loadPrintedQtytoComboBox()
        {
            //加载变量名。从arrlistSellectRow得到列名。

            if (CurrentDataTable == null)
                return;//如果为空就返回

            comboBoxQtyOfWantToPrinted.Items.Clear();

            foreach (DataColumn item in CurrentDataTable.Columns)
            {
                comboBoxQtyOfWantToPrinted.Items.Add(item.Caption);
                
            }

            //如果表中列名为"数量"，则这个列为默认，其他 的像"qty Qty"都检测是否有
            //因为comboBoxQtyOfWantToPrinted的DropDownStyle为DropDownList，所以可以用如下的让复选框自己选择，
            //如果有这一项就自动选择了,如果为其他形式的则需要添加判断了。
            comboBoxQtyOfWantToPrinted.Text = "数量";
            comboBoxQtyOfWantToPrinted.Text = "Qty";
            comboBoxQtyOfWantToPrinted.Text = "qty";
            comboBoxQtyOfWantToPrinted.Text = "Total Qty";
        }

        private void comboBoxQtyOfWantToPrinted_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intCurrentLine = Convert.ToInt32(bindingNavigatorPositionItem.Text);//取得当前行

                txtQtyOfWantToPrinted.Text = CurrentDataTable.Rows[intCurrentLine - 1][comboBoxQtyOfWantToPrinted.Text].ToString();

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //throw;
            }

        }

        private int getQtyOfWantToPrinted()
        {
            int intPages = 0;
            try
            {
                float fltSunHao = Convert.ToSingle(txtSunHao.Text) / 100;//取得损耗
                intPages = Convert.ToInt32(txtQtyOfWantToPrinted.Text);
                intPages += ((int)(intPages * fltSunHao));//加上损耗

            }
            catch (Exception ex)
            {

                MessageBox.Show("无法取得打印数量或者损耗,原因是：" + ex.Message);
            }

            return intPages;

        }

        private void btnLoadzPreviously_Click(object sender, EventArgs e)
        {
            LoadPreviouslyExCel();
        }

        /// <summary>
        /// 加载以前导入的EXCEL表格
        /// </summary>
        private void LoadPreviouslyExCel()
        {
            //这个依旧是新建一个对话框窗体
            FrmLoadExcelRecords f = new FrmLoadExcelRecords();
            if (f.ShowDialog() == DialogResult.OK)
            {
                strTableName = FrmLoadExcelRecords.strTableName;
            }

            //从数据库中导入这个表
            ClsDataBase myClsDataBase = new ClsDataBase();
            LoadDataTable(myClsDataBase.sellectTable(strTableName));
            //还得选择哪个是数量
            loadPrintedQtytoComboBox();

            //要更新变量名
        }
        private void btnCtrlX_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlX();
        }

        private void btnCtrlC_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlC();
        }

        private void btnCtrlV_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlV();
        }

        private void FrmBarcodeEdit_KeyUp(object sender, KeyEventArgs e)
        {
                /**
                switch (e.KeyCode)
                {
                    case Keys.A:
                        break;
                    case Keys.Add:
                        break;
                    case Keys.Alt:
                        break;
                    case Keys.Apps:
                        break;
                    case Keys.Attn:
                        break;
                    case Keys.B:
                        break;
                    case Keys.Back:
                        break;
                    case Keys.BrowserBack:
                        break;
                    case Keys.BrowserFavorites:
                        break;
                    case Keys.BrowserForward:
                        break;
                    case Keys.BrowserHome:
                        break;
                    case Keys.BrowserRefresh:
                        break;
                    case Keys.BrowserSearch:
                        break;
                    case Keys.BrowserStop:
                        break;
                    case Keys.C:

                        break;
                    case Keys.Cancel:
                        break;
                    case Keys.Capital:
                        break;
                   // case Keys.CapsLock:
                       // break;
                    case Keys.Clear:
                        break;
                    case Keys.Control:
                        break;
                    case Keys.ControlKey:
                        break;
                    case Keys.Crsel:
                        break;
                    case Keys.D:
                        break;
                    case Keys.D0:
                        break;
                    case Keys.D1:
                        break;
                    case Keys.D2:
                        break;
                    case Keys.D3:
                        break;
                    case Keys.D4:
                        break;
                    case Keys.D5:
                        break;
                    case Keys.D6:
                        break;
                    case Keys.D7:
                        break;
                    case Keys.D8:
                        break;
                    case Keys.D9:
                        break;
                    case Keys.Decimal:
                        break;
                    case Keys.Delete:
                       
                        break;
                    case Keys.Divide:
                        break;
                    case Keys.Down:
                        break;
                    case Keys.E:
                        break;
                    case Keys.End:
                        break;
                    case Keys.Enter:
                        break;
                    case Keys.EraseEof:
                        break;
                    case Keys.Escape:
                        break;
                    case Keys.Execute:
                        break;
                    case Keys.Exsel:
                        break;
                    case Keys.F:
                        break;
                    case Keys.F1:
                        break;
                    case Keys.F10:
                        break;
                    case Keys.F11:
                        break;
                    case Keys.F12:
                        break;
                    case Keys.F13:
                        break;
                    case Keys.F14:
                        break;
                    case Keys.F15:
                        break;
                    case Keys.F16:
                        break;
                    case Keys.F17:
                        break;
                    case Keys.F18:
                        break;
                    case Keys.F19:
                        break;
                    case Keys.F2:
                        break;
                    case Keys.F20:
                        break;
                    case Keys.F21:
                        break;
                    case Keys.F22:
                        break;
                    case Keys.F23:
                        break;
                    case Keys.F24:
                        break;
                    case Keys.F3:
                        userControlCanvas1.ZoomPaperToScreen();
                        break;
                    case Keys.F4:
                        break;
                    case Keys.F5:
                        break;
                    case Keys.F6:
                        break;
                    case Keys.F7:
                        break;
                    case Keys.F8:
                        break;
                    case Keys.F9:
                        break;
                    case Keys.FinalMode:
                        break;
                    case Keys.G:
                        break;
                    case Keys.H:
                        break;
                    case Keys.HanguelMode:
                        break;

                    case Keys.HanjaMode:
                        break;
                    case Keys.Help:
                        break;
                    case Keys.Home:
                        break;
                    case Keys.I:
                        break;
                    case Keys.IMEAccept:
                        break;

                    case Keys.IMEConvert:
                        break;
                    case Keys.IMEModeChange:
                        break;
                    case Keys.IMENonconvert:
                        break;
                    case Keys.Insert:
                        break;
                    case Keys.J:
                        break;
                    case Keys.JunjaMode:
                        break;
                    case Keys.K:
                        break;

                    case Keys.KeyCode:
                        break;
                    case Keys.L:
                        break;
                    case Keys.LButton:
                        break;
                    case Keys.LControlKey:
                        break;
                    case Keys.LMenu:
                        break;
                    case Keys.LShiftKey:
                        break;
                    case Keys.LWin:
                        break;
                    case Keys.LaunchApplication1:
                        break;
                    case Keys.LaunchApplication2:
                        break;
                    case Keys.LaunchMail:
                        break;
                    case Keys.Left:
                        break;
                    case Keys.LineFeed:
                        break;
                    case Keys.M:
                        break;
                    case Keys.MButton:
                        break;
                    case Keys.MediaNextTrack:
                        break;
                    case Keys.MediaPlayPause:
                        break;
                    case Keys.MediaPreviousTrack:
                        break;
                    case Keys.MediaStop:
                        break;
                    case Keys.Menu:
                        break;
                    case Keys.Modifiers:
                        break;
                    case Keys.Multiply:
                        break;
                    case Keys.N:
                        break;
                    case Keys.NoName:
                        break;
                    case Keys.None:
                        break;
                    case Keys.NumLock:
                        break;
                    case Keys.NumPad0:
                        break;
                    case Keys.NumPad1:
                        break;
                    case Keys.NumPad2:
                        break;
                    case Keys.NumPad3:
                        break;
                    case Keys.NumPad4:
                        break;
                    case Keys.NumPad5:
                        break;
                    case Keys.NumPad6:
                        break;
                    case Keys.NumPad7:
                        break;
                    case Keys.NumPad8:
                        break;
                    case Keys.NumPad9:
                        break;
                    case Keys.O:
                        break;
                    case Keys.Oem1:
                        break;
                    case Keys.Oem102:
                        break;
                    case Keys.Oem2:
                        break;
                    case Keys.Oem3:
                        break;
                    case Keys.Oem4:
                        break;
                    case Keys.Oem5:
                        break;
                    case Keys.Oem6:
                        break;
                    case Keys.Oem7:
                        break;
                    case Keys.Oem8:
                        break;

                    case Keys.OemClear:
                        break;

                    case Keys.OemMinus:
                        break;

                    case Keys.OemPeriod:
                        break;
                   
                        break;
                    case Keys.Oemcomma:
                        break;
                    case Keys.Oemplus:
                        break;
                    
                        break;
                    case Keys.P:
                        break;
                    case Keys.Pa1:
                        break;
                    case Keys.Packet:
                        break;
                    case Keys.PageDown:
                        break;
                    case Keys.PageUp:
                        break;
                    case Keys.Pause:
                        break;
                    case Keys.Play:
                        break;
                    case Keys.Print:
                        break;
                    case Keys.PrintScreen:
                        break;
                    case Keys.ProcessKey:
                        break;
                    case Keys.Q:
                        break;
                    case Keys.R:
                        break;
                    case Keys.RButton:
                        break;
                    case Keys.RControlKey:
                        break;
                    case Keys.RMenu:
                        break;
                    case Keys.RShiftKey:
                        break;
                    case Keys.RWin:
                        break;
                    //case Keys.Return:
                        //break;
                    case Keys.Right:
                        break;
                    case Keys.S:
                        break;
                    case Keys.Scroll:
                        break;
                    case Keys.Select:
                        break;
                    case Keys.SelectMedia:
                        break;
                    case Keys.Separator:
                        break;
                    case Keys.Shift:
                        break;
                    case Keys.ShiftKey:
                        break;
                    case Keys.Sleep:
                        break;
                    case Keys.Space:
                        break;
                    case Keys.Subtract:
                        break;
                    case Keys.T:
                        break;
                    case Keys.Tab:
                        break;
                    case Keys.U:
                        break;
                    case Keys.Up:
                        break;
                    case Keys.V:

                        break;
                    case Keys.VolumeDown:
                        break;
                    case Keys.VolumeMute:
                        break;
                    case Keys.VolumeUp:
                        break;
                    case Keys.W:
                        break;
                    case Keys.X:

                        
                        break;
                    case Keys.XButton1:
                        break;
                    case Keys.XButton2:
                        break;
                    case Keys.Y:
                        break;
                    case Keys.Z:
                        break;
                    case Keys.Zoom:
                        userControlCanvas1.Zoom = userControlCanvas1.Zoom * 2;
                        break;
                    default:
                        break;

            }
                 * */
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadXmlBarcodeModel();
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewBarcodeModel();
        }

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAsBarcodeModel();
        }

        private void 导入EXCEL表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadExcel();
        }

        private void 查看以前导入的ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadPreviouslyExCel();
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            NewBarcodeModel();
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            loadXmlBarcodeModel();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveBarcodeModel();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveBarcodeModel();
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlX();
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlC();
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlV();
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.deleteSelectShapeEle();
        }

        private void 查看以前导入的toolStripButton2_Click(object sender, EventArgs e)
        {
            LoadPreviouslyExCel();
        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlX();
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlC();
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlV();
        }

        private void 讲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.ZoomPaperToScreen();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            TestPrint();
        }

        private void toolStripComboBoxQtyOfWantToPrinted_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int intCurrentLine = Convert.ToInt32(bindingNavigatorPositionItem.Text);//取得当前行

                txtQtyOfWantToPrinted.Text = CurrentDataTable.Rows[intCurrentLine - 1][comboBoxQtyOfWantToPrinted.Text].ToString();

            }
            catch (Exception ex)
            {
                //ClsErrorFile.WriteLine(ex);
                //throw;
            }
        }

        private void 撤消UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlZ();
        }

        private void 重复RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlY();
        }

        private void 向前一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.forward();
        }

        private void 向后一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.backward();
        }

        private void 移到最前ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.forward2();
        }

        private void 移到最后ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.backward2();
        }

        private void 分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.doGroup();
        }

        private void 解除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.DeGroup();
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            userControlCanvas1.CtrlA();
        }

        private void 帮助LToolStripButton_Click(object sender, EventArgs e)
        {
            (new BarcodeTerminator.FrmHelp()).ShowDialog(); 
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (new BarcodeTerminator.FrmHelp()).ShowDialog(); 
        }

        private void 编辑EToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 我自己编写的关闭方法
        /// </summary>
        private void myExit()
        {
            if (userControlCanvas1.isNeedSave())
            {
                //提示用户还没有保存

                DialogResult dr = MessageBox.Show("您没有保存，请问要保存吗?", "保存", MessageBoxButtons.YesNoCancel);

                if (dr == DialogResult.Yes)
                {
                    string strFileName;

                    while (((shapesFileData.Key == "templet")))
                    {
                        strFileName = userControlCanvas1.Saver();
                        shapesFileData = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);

                        if (strFileName == "")
                        {
                            MessageBox.Show("您没有选择文件");
                            return;
                        }

                    }

                    strFileName = userControlCanvas1.Saver(shapesFileData.Value);
                    this.Dispose();//关闭

                }
                else if (dr == DialogResult.No)
                {
                    //不保存就是直接关闭了
                    this.Dispose();
                }
                else
                {
                    //到这里就是选择的是取消了，什么也不用做
                    return;
                }


            }
            else
            {
                this.Dispose();
            }

        }

        
        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myExit();
        }

        private void FrmBarcodeEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            //myInit();
        }

        private void 导入EXCEL_Click(object sender, EventArgs e)
        {
            LoadExcel();
        }

        private void toolStripButtonSellectPrinting_Click(object sender, EventArgs e)
        {
            //这个是选择打印机
            //我自制力一个对话框来选择打印机，并且自动取得分辨率.

            FrmSelectPrinter f = new FrmSelectPrinter();
            if (f.ShowDialog() == DialogResult.OK)
            {

                toolStripLabelPrintingName.Text = f.strPrinterName;

                //设置到
                ClsBarcodePrint.strPrinterName = f.strPrinterName;


            }
        }





    }
}
