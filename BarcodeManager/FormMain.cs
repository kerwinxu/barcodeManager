using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Drawing.Printing;
using System.Xml;
using System.IO;
using System.Collections;
using System.Security.Cryptography;
using System.Diagnostics;

using Xuhengxiao.MyDataStructure;
using VestShapes;
using System.Xml.Serialization;

using Xuhengxiao.ImportData;

using Xuhengxiao.DataBase;



namespace BarcodeTerminator
{
    public partial class frmMain : Form
    {
        //打印分辨率
        public static int intDpi;

        //当前选择的数据
        public static ArrayList arrlistSellectRow = new ArrayList();

        //当前正在打印的表的表名
        private string strCurrentTableName = "";//一开始就是空值 

        private bool isAutoPrint=false;//自动打印

        //private int intC = 0;

        private OleDbDataAdapter OleDbDataAdapterCurrentTable;//当前表的OLEDB。

        public ClsXmlApp myClsXmlApp;

        public frmMain()
        {
            InitializeComponent();
            
            loadXmlAPP();

            //自动打印的提示
            toolTipAutoPrint.IsBalloon = true;
           

            //打印间隔初始化
            ClsBarcodePrint.isPrintJiange = chkJianGe.Checked;
            ClsBarcodePrint.strJianGe = txtInterval.Text;



            ClsBarcodePrint.barcodePrinted += new ClsBarcodePrint.BarcodePrinted(ClsBarcodePrint_barcodePrinted);

            ClsErrorFile.DeleteBefore();

            //默认就可以选择多行的
            chkIsMulti.Checked = true;

            
        }



        void ClsBarcodePrint_barcodePrinted(object sender, ClsBarcodePrint.printedEventArgs e)
        {

            timer2.Enabled = true;//因为数据库有延迟，所以用这个定时器来延迟查询更新

            //row new NotImplementedException();
        }

        private void btnNewBarcodeModel_Click(object sender, EventArgs e)
        {

            FrmBarcodeEdit f = new FrmBarcodeEdit();
            f.setKeyValue(arrlistSellectRow);
            f.LoadDataTable((DataTable)dataGridView1.DataSource);
            
            f.ShowDialog();


            //如果返回的不是原先编辑的，就增加，并且也加
            if (isExitXmlFileData(f.shapesFileData.Value))
            {

                addXmlFileData(f.shapesFileData.Value);
            }

            strCurrentTableName = f.strTableName;

            dataGridView1.DataSource = f.CurrentDataTable;

            f.Dispose();

            comboBoxBarcodeModel_SelectedIndexChanged(null, null);//重新刷新一下画布
        }

        private void btnEditBarcodeModel_Click(object sender, EventArgs e)
        {

            //判断是否选择条形码模板
            if (comboBoxBarcodeModel.SelectedItem == null)
            {
                MessageBox.Show("请选择条形码模板");
                return;
            }

            FrmBarcodeEdit f = new FrmBarcodeEdit(((clsKeyValue)comboBoxBarcodeModel.SelectedItem), (DataTable )dataGridView1.DataSource, arrlistSellectRow,strCurrentTableName);
            //f.setKeyValue(arrlistSellectRow);
            f.ShowDialog();
           

            //如果返回的不是原先编辑的，就增加，并且也加
            if (isExitXmlFileData(f.shapesFileData.Value))
            {
                addXmlFileData(f.shapesFileData.Value);
            }

            strCurrentTableName = f.strTableName;

            dataGridView1.DataSource = f.CurrentDataTable;//可能换了数据的
            

            f.Dispose();//释放

            //同时更新画布。
            dataGridViewChangedCell();

            comboBoxBarcodeModel_SelectedIndexChanged(null, null);


        }

        private bool isExitXmlFileData(string strFileName)
        {
            bool isNew = true;
            foreach (clsKeyValue keyvalue in comboBoxBarcodeModel.Items)
            {
                if (keyvalue.Value == strFileName)
                {
                    isNew = false;
                    return isNew;
                }
            }

            return isNew;
        }

        private void addXmlFileData(string strFileName)
        {
            //主要添加两个方向，一个是往选择框里添加，一个是往xmlapp文件中添加。
            clsKeyValue keyvalue = new clsKeyValue(Path.GetFileNameWithoutExtension(strFileName), strFileName);
            comboBoxBarcodeModel.Items.Add(keyvalue);
            comboBoxBarcodeModel.SelectedItem = keyvalue;

            /**如下的也注释掉了。改为序列化。

            //保存早xmlapp文件中。
            //因为会有文件异常，所以

            XmlDocument xmlDocApp = new XmlDocument();
            xmlDocApp.Load(Application.StartupPath + "\\xmlAPP.xml");
            XmlNode myxmlNode = xmlDocApp.SelectNodes("root").Item(0);

            XmlElement xmlEleFileName = xmlDocApp.CreateElement("xmlBarcodeModel");
            XmlAttribute xmlAttributeFileName = xmlDocApp.CreateAttribute("Path");
            xmlAttributeFileName.Value = strFileName;

            xmlEleFileName.Attributes.Append(xmlAttributeFileName);
            myxmlNode.AppendChild(xmlEleFileName);
            xmlDocApp.Save(Application.StartupPath + "\\xmlAPP.xml");
             * */

            if (myClsXmlApp == null)
                myClsXmlApp = new ClsXmlApp();

            myClsXmlApp.addBarcodeModel(strFileName);

            saveXmlApp();

            //同时更新画布。
            dataGridViewChangedCell();

        }

        private void saveXmlApp()
        {
            //如下是序列化保存。
            try
            {
                using (Stream stream = new FileStream(Application.StartupPath + "\\xmlAPP.xml", FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer xmls = new XmlSerializer(typeof(ClsXmlApp));
                    xmls.Serialize(stream, myClsXmlApp);
                }
            }

            catch (Exception exception)
            {
                ClsErrorFile.WriteLine("保存不成功，原因是" , exception);
                //MessageBox.Show("保存不成功，原因是" + exception.Message);

                if (exception.InnerException.Message != null)
                    ClsErrorFile.WriteLine("xml保存不成功" + exception.InnerException.Message);
                //MessageBox.Show(exception.InnerException.Message);

            }
            finally
            {

            }

        }

        private void picCanvas_Click(object sender, EventArgs e)
        {

        }



        //
        /// <summary>
        /// 加载程序配置，包括打印机名字，是否倒着打印，以及模板的路径
        /// </summary>
        private void loadXmlAPP()
        {
            #region 如下的我打算用序列化来实现
            /**
            if (!File.Exists(Application.StartupPath + "\\xmlAPP.xml"))
            {
                createXmlApp();

            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Application.StartupPath + "\\xmlAPP.xml");
            XmlElement xmlEle = xmlDoc.DocumentElement;
            foreach (XmlElement myxmlNode in xmlEle.ChildNodes)
            {
                string strNodeName = myxmlNode.Name;
                switch (strNodeName)
                {
                    case "printer":

                        //clsBarcodePrint.fltDPIX = clsBarcodePrint.strAttributeValueToFloat(myxmlNode, "DPIX");
                        //clsBarcodePrint.fltDPIY = clsBarcodePrint.strAttributeValueToFloat(myxmlNode, "DPIY");
                        //读取打印机的时候，得判断是否为空，如果为空就设置为默认打印机
                        string strPrintName_ = myxmlNode.GetAttribute("PrinterName");

                        if (strPrintName_ == "")
                        {
                            PrintDocument printDoc = new PrintDocument();
                            ClsBarcodePrint.strPrinterName = printDoc.PrinterSettings.PrinterName;
                        }
                        else
                        {
                            ClsBarcodePrint.strPrinterName = strPrintName_;

                        }
                        lblPrinterName.Text = ClsBarcodePrint.strPrinterName;


                        break;

                    case "xmlBarcodeModel"://条形码模板

                        string strPath = myxmlNode.Attributes["Path"].InnerText;//路径

                        //还得判断这个路径是否是相对路径

                        if (strPath.IndexOf(":") < 0)
                        strPath = Application.StartupPath + "\\" + strPath;

                        //去掉了没有的
                        if (File.Exists(strPath))
                        {
                            clsKeyValue lt = new clsKeyValue(Path.GetFileNameWithoutExtension(strPath), strPath);
                            comboBoxBarcodeModel.Items.Add(lt);
                        }
                        break;
                }
            }

             * */
            #endregion
     

            //如下是用序列化实现的,如果存在文件就反序列化
            if (File.Exists(Application.StartupPath + "\\xmlAPP.xml"))
            {
                try
                {
                    using (Stream stream = new FileStream(Application.StartupPath + "\\xmlAPP.xml", FileMode.Open, FileAccess.Read, FileShare.Read))
                    {

                        XmlSerializer formatter = new XmlSerializer(typeof(ClsXmlApp));

                        myClsXmlApp = formatter.Deserialize(stream) as ClsXmlApp;


                    }

                }
                catch (Exception exception)
                {
                    ClsErrorFile.WriteLine("加载不成功，原因是" , exception);
                    //MessageBox.Show("加载不成功，原因是" + exception.Message);
                }
                finally
                {

                }
            }

            //如下就是添加了。

            if (myClsXmlApp != null)
            {
                PrintDocument printdoc = new PrintDocument();

                //如果没有设置打印机就设置使用默认的打印机
                if (myClsXmlApp.strPrintingName == null)
                {
                    myClsXmlApp.strPrintingName = printdoc.PrinterSettings.PrinterName;
                    
                }

                //显示选择的打印机，通常是用户选择要使用的，
                lblPrinterName.Text = myClsXmlApp.strPrintingName;


                //这个类也使用这个打印机配置
                //如果没有设置打印机，就设置默认的打印机
         
                ClsBarcodePrint.strPrinterName = myClsXmlApp.strPrintingName;

                foreach (string  strPath in myClsXmlApp.arrlistBarcodeModel)
                {
                    //去掉了没有的
                    if (File.Exists(strPath))
                    {
                        clsKeyValue lt = new clsKeyValue(Path.GetFileNameWithoutExtension(strPath), strPath);
                        comboBoxBarcodeModel.Items.Add(lt);
                    }
                    
                }


                //如果有模板，就设置第一个
                if (comboBoxBarcodeModel.Items.Count > 0)
                {
                    comboBoxBarcodeModel.Text = comboBoxBarcodeModel.Items[0].ToString();
                    //MessageBox.Show(comboBoxBarcodeModel.Items[0].ToString());
                }
            }



        }

        private void btnLoadBarcodeModel_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "模板文件 (*.barcodce)|*.barcode|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.InitialDirectory = Application.StartupPath + "\\BarcodeModel";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                addXmlFileData(openFileDialog1.FileName);
            }

            

        }

        private void btnLoadExcel_Click(object sender, EventArgs e)
        {
            ClsImportExcel importExcel = new ClsImportExcel();
            dataGridView1.DataSource = importExcel.loadExcelDataTalbe;//将导入的表赋值给显示表格的dataGridView
            strCurrentTableName = importExcel.strCurrentTableName;
            //将导入的表作为最新的数据更新到listView .
            dataGridViewChangedCell();

            //还得选择哪个是数量
            loadPrintedQtytoComboBox();


            /**
            //选择是哪个档口
            FrmSCAndShop myFrmSCAndShop = new FrmSCAndShop();

            if (myFrmSCAndShop.ShowDialog() != DialogResult.OK)//只要判断这个就可以了。
            {
                MessageBox.Show("请输入单号（名）和档口号（名），以方便以后查看");
                return;

            }


            //选择文件
            string strFile;
            openFileDialog1.Filter = "Excel97-2003 Excel 2007(*.xls *.xlsx)|*.xls;*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                strFile = openFileDialog1.FileName;
            }
            else
            {
                MessageBox.Show("请选择文件");
                return;
            }

            // 首先判断这个excel表格有几页，如果只有一页就直接导入，如果不止一页就让用户选择
            string[] strSheetNames = GetExcelSheetNames(strFile);

            DataTable loadExcelDataTalbe;//就是将excle 导入这个数据中，

            if (strSheetNames.Length == 1)
            {
                //读入表
                loadExcelDataTalbe = GetExcelToDataSet(strFile, false, strSheetNames[0]);
                
            }
            else
            {
                //选择读入那个表
                FrmChooseExcelSheet frm = new FrmChooseExcelSheet(strSheetNames);
                frm.ShowDialog();
                loadExcelDataTalbe = GetExcelToDataSet(strFile, false, FrmChooseExcelSheet.strSheetName);
                
            }

            dataGridView1.DataSource = loadExcelDataTalbe;//将导入的表赋值给显示表格的dataGridView
            //将导入的表作为最新的数据更新到listView .
            dataGridViewChangedCell();

            //将导入的表导入到数据库,
            ClsDataBase myClsDataBase = new ClsDataBase();
            strCurrentTableName = myClsDataBase.loadExcel(FrmSCAndShop.strSC, FrmSCAndShop.strShop, Path.GetFileName(strFile), loadExcelDataTalbe);

             * */

            

        }

        private void loadPrintedQtytoComboBox()
        {
            //加载变量名。从arrlistSellectRow得到列名。

            for (int i = 0; i < frmMain.arrlistSellectRow.Count; i++)
            {
                string str = ((clsKeyValue)(frmMain.arrlistSellectRow[i])).Key;
                comboBoxQtyOfWantToPrinted.Items.Add(str);
            }

            //如果表中列名为"数量"，则这个列为默认，其他 的像"qty Qty"都检测是否有
            //因为comboBoxQtyOfWantToPrinted的DropDownStyle为DropDownList，所以可以用如下的让复选框自己选择，
            //如果有这一项就自动选择了,如果为其他形式的则需要添加判断了。
            comboBoxQtyOfWantToPrinted.Text = "数量";
            comboBoxQtyOfWantToPrinted.Text = "Qty";
            comboBoxQtyOfWantToPrinted.Text = "qty";
            comboBoxQtyOfWantToPrinted.Text = "Total Qty";
        }

        public void dataGridViewChangedCell()
        {
            //

            //因为存在点击列名而导致的异常，所以需要判断一下。
            if (dataGridView1.SelectedCells.Count > 0)
            {
                arrlistSellectRow.Clear();//当然首先是清空了。

                string strData = "";//查询打印记录时用


                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    clsKeyValue keyvalue = new clsKeyValue(dataGridView1.Columns[i].Name, dataGridView1.CurrentRow.Cells[i].Value.ToString());
                    arrlistSellectRow.Add(keyvalue);

                    strData = strData + dataGridView1.CurrentRow.Cells[i].Value.ToString();//就是所有项连接起来

                }


                //导入打印记录,判断是根据表名和数据，
                ClsDataBase myClsDataBase = new ClsDataBase();
                dataGridViewPrintedRecords.DataSource = myClsDataBase.commandSelectPrintedRecord(strCurrentTableName, arrlistSellectRow);

                //更新图片
                XmlDocument xmlDoc = new XmlDocument();

                //如下得判断路径是否是绝对路径

                //值深度拷贝
                ArrayList arrlisttemp = new ArrayList();
                if (arrlistSellectRow != null)
                {
                    foreach (clsKeyValue item in arrlistSellectRow)
                    {
                        clsKeyValue clskeyvalue1 = new clsKeyValue(item.Key, item.Value);
                        arrlisttemp.Add(clskeyvalue1);
                    }
                }

                //还得判断是否有选择的条形码模板


                userControlCanvas1.setArrKeyValue(arrlisttemp);
                userControlCanvas1.Refresh();
                /**

                {

                    if (comboBoxBarcodeModel.SelectedItem != null)
                    {

                        string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
                        //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                        if (strPathName.IndexOf(":") < 0)
                        {
                            //还有得判断首字符是不是"\\"，如果不是就加上
                            if (strPathName.Substring(0) == "\\")
                            {
                                strPathName = Application.StartupPath + strPathName;
                            }
                            else
                            {
                                strPathName = Application.StartupPath + "\\" + strPathName;
                            }
                        }

                        ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

                        //如下是画布设置
                        userControlCanvas1.Loader(strPathName);

                        //根据纸张的大小选择放大率
                        calculateCanvas();

                        userControlCanvas1.setArrKeyValue(arrlisttemp);

                        userControlCanvas1.ZoomPaperToScreen();

                        userControlCanvas1.Refresh();

                        //picCanvas.Image = myBarcodePrint.xmlToBarcodeImage(clsBarcodePrint.populateVariable(xmlDoc, frmMain.arrlistSellectRow));
                    }
                }
                 * */
                

                //得更新要打印数量
                //加载变量名。从arrlistSellectRow得到列名。
                comboBoxVaribaleName_SelectedIndexChanged(this, new EventArgs());


            }

        }

        /// <summary>
        /// 获取EXCEL的表 表名字列 
        /// </summary>
        /// <param name="p_ExcelFile">Excel文件</param>
        /// <returns>数据表</returns>
        private string[] GetExcelSheetNames(string excelFile)
        {
            System.Data.DataTable dt = null;

            try
            {

                dt = GetExcelSheetNames2DataTable(excelFile);
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }
                //Array.Reverse(excelSheets);//需要反转数组，才能与文件中实际的顺序相吻合
                return excelSheets;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
            finally
            {
                if (dt != null)
                {
                    dt.Dispose();
                }
            }


        }
        private DataTable GetExcelToDataSet(string FileFullPath, bool no_HDR, string SheetName)
        {
            try
            {
                string strConn = GetExcelConnectionString(FileFullPath, no_HDR);
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable ds = new DataTable();

                //要想获得修改表格的话只能把这个设置为属性
                OleDbDataAdapterCurrentTable = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn);
                OleDbDataAdapterCurrentTable.Fill(ds);

                conn.Close();
                return ds;

                //如下是原先的，可以看到他返回值
                /**
                OleDbDataAdapter odda = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", SheetName), conn);
                //("select * from [Sheet1$]", conn);
                odda.Fill(ds);
                conn.Close();
                return ds;
                 * */
            }
            catch (Exception ee)
            {
                ClsErrorFile.WriteLine(ee);
                //Console.Error.WriteLine(ee.Message);
                //throw new Exception(ee.Message);
                return new DataTable();
            }
        }




        private System.Data.DataTable GetExcelSheetNames2DataTable(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                string strConn = GetExcelConnectionString(excelFile, true);
                objConn = new OleDbConnection(strConn);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                return dt;
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        private bool IsExcel2007(string strFileName)
        {
            bool isExcel2007File;
            switch (Path.GetExtension(strFileName))
            {
                case ".xls":
                    isExcel2007File = false;
                    break;
                case ".xlsx":
                    isExcel2007File = true;
                    break;
                default:
                    throw new Exception("你要檢查" + strFileName + "是2007版本的Excel文件還是之前版本的Excel文件，但是這個文件不是一個有效的Excel文件。");

            }

            return isExcel2007File;

        }
        /// <summary>
        /// Excel文件在服務器上的OLE連接字符串
        /// </summary>
        /// <param name="excelFile">Excel文件在服務器上的路徑</param>
        /// <param name="no_HDR">第一行不是標題：true;第一行是標題：false;</param>
        /// <returns>String</returns>
        public String GetExcelConnectionString(string excelFile, bool no_HDR)
        {

            try
            {
                if (no_HDR)
                {
                    if (IsExcel2007(excelFile))
                    {
                        return "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0; HDR=NO; IMEX=1'"; //此连接可以操作.xls与.xlsx文件
                    }
                    else
                    {
                        return "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0; HDR=NO; IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件

                    }
                }
                else
                {
                    if (IsExcel2007(excelFile))
                    {
                        return "Provider=Microsoft.Ace.OleDb.12.0;" + "data source=" + excelFile + ";Extended Properties='Excel 12.0;HDR=YES;  IMEX=1'"; //此连接可以操作.xls与.xlsx文件
                    }
                    else
                    {
                        return "Provider=Microsoft.Jet.OleDb.4.0;" + "data source=" + excelFile + ";Extended Properties='Excel 8.0;HDR=YES; IMEX=1'"; //此连接只能操作Excel2007之前(.xls)文件

                    }

                }
            }
            catch (Exception ee)
            {
                throw new Exception(ee.Message);
            }
        }

        private void chkDataGridViewReadOnly_CheckedChanged(object sender, EventArgs e)
        {
            //dataGridView1.ReadOnly = chkDataGridViewReadOnly.Checked;//只要这样设置就可以了。
        }

        private void printBarcode(int intPages)
        {

            ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

            //如下的这个有些破坏了封装性,我用这个只是为了打印结束后判断是否自动打印。
            //myBarcodePrint.myPrintDocument.EndPrint += new PrintEventHandler(myPrintDocument_EndPrint);

            //如下是添加到打印队列
            ArrayList arrlist = new ArrayList();//用这个是因为 arrlistSellectRow 也是静态变量，读取的时候会读取到不是想要的值,这里用深拷贝
            if (arrlistSellectRow != null)
            {
                foreach (clsKeyValue myKeyValue in arrlistSellectRow)
                {
                    clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                    arrlist.Add(kv);
                }

            }

            //创建打印信息。
            queuePrintItem printDetails = new queuePrintItem();
            printDetails.strTableName = strCurrentTableName;
            //如下是取得模板的路径
            string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
            //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
            if (strPathName.IndexOf(":") < 0)
            {
                //还有得判断首字符是不是"\\"，如果不是就加上
                if (strPathName.Substring(0) == "\\")
                {
                    strPathName = Application.StartupPath + strPathName;
                }
                else
                {
                    strPathName = Application.StartupPath + "\\" + strPathName;
                }
            }

            printDetails.ShapesFileName = strPathName;
            printDetails.IsFull = chkIsFull.Checked;
            queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
            queuePrintItemRowAndPages1.arrlistRow = arrlist;
            queuePrintItemRowAndPages1.intPages = intPages;
            printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);

            //queuePrintItem printDetails = new queuePrintItem(strCurrentTableName, ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value, arrlist, intPages);
            myBarcodePrint.addPrintDetails(printDetails);
        }

        
        /// <summary>
        /// 自动打印
        /// </summary>
        private void AutoprintBarcode()
        {
            int intPages = 0;
            //得判断是否是自动打印，如果是自动打印，就直接用EXCEL中的数量
            try
            {
                if (txtQtyOfWantToPrinted.Text != "")
                {
                    intPages = Convert.ToInt32(txtQtyOfWantToPrinted.Text);
                }
                else
                {

                    MessageBox.Show("无法找到要打印的页数");
                    return;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("不能取得要打印数量，原因是" + ex.Message);
                return;//直接返回
            }

            //如下是取得损耗
            float fltSunHao = 0;
            try
            {
                if (chkSunHao.Checked)
                {
                    fltSunHao = Convert.ToSingle(txtSunHao.Text)/100;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("不能取得损耗数量，原因是" + ex.Message);
                return;//直接返回
            }

            intPages += ((int)(intPages * fltSunHao));

            printBarcode(intPages);
            /**如下的已经保存到如上调用的一个方法里了。
            ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

            //如下的这个有些破坏了封装性,我用这个只是为了打印结束后判断是否自动打印。
            //myBarcodePrint.myPrintDocument.EndPrint += new PrintEventHandler(myPrintDocument_EndPrint);

            //如下是添加到打印队列
            ArrayList arrlist = new ArrayList ();//用这个是因为 arrlistSellectRow 也是静态变量，读取的时候会读取到不是想要的值,这里用深拷贝
            if (arrlistSellectRow != null)
            {
                foreach (clsKeyValue myKeyValue in arrlistSellectRow)
                {
                    clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                    arrlist.Add(kv);
                }

            }

            queuePrintItem printDetails = new queuePrintItem(strCurrentTableName, ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value, arrlist, intPages);
            myBarcodePrint.addPrintDetails(printDetails);
             * */


        }


        /// <summary>
        /// 根据用户要求的的实际数量打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {
            int intPages = getCurrentPrintPages();//这个数量是钉死的

            if (chkIsMulti.Checked)
            {
                //创建打印信息。
                queuePrintItem printDetails = new queuePrintItem();
                printDetails.strTableName = strCurrentTableName;
                //如下是取得模板的路径
                string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
                //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                if (strPathName.IndexOf(":") < 0)
                {
                    //还有得判断首字符是不是"\\"，如果不是就加上
                    if (strPathName.Substring(0) == "\\")
                    {
                        strPathName = Application.StartupPath + strPathName;
                    }
                    else
                    {
                        strPathName = Application.StartupPath + "\\" + strPathName;
                    }
                }

                printDetails.ShapesFileName = strPathName;//设置目录

                printDetails.IsFull = chkIsFull.Checked;

                try
                {
                    //得根据选中的行迭代
                    if (dataGridView1.SelectedRows.Count>0)
                    {
                        foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                        {
                            ArrayList arrlist = new ArrayList();

                            //如下是构造数据的
                            for (int i = 0; i < dataGridView1.ColumnCount; i++)
                            {
                                clsKeyValue keyvalue = new clsKeyValue(dataGridView1.Columns[i].Name, item.Cells[i].Value.ToString());
                                arrlist.Add(keyvalue);

                            }
                            //如下就是构造打印了。

                            queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
                            queuePrintItemRowAndPages1.arrlistRow = arrlist;
                            queuePrintItemRowAndPages1.intPages = intPages;
                            printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);

                            //queuePrintItem printDetails = new queuePrintItem(strCurrentTableName, ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value, arrlist, intPages);

                        }

                    }
                    else
                    {

                        queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
                        queuePrintItemRowAndPages1.arrlistRow = null;
                        queuePrintItemRowAndPages1.intPages = intPages;
                        printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);

                    }


                    ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();
                    myBarcodePrint.addPrintDetails(printDetails);


                }
                catch (System.Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                }


            }
            else
            {

                //只有大于零才打印
                if (intPages > 0)
                {
                    printBarcode(intPages);

                }

            }

            /**如下的会产生问题，比如说在读取打印数量后已经抛出来一场，但结果还是打印了。
             * 
            timer1.Enabled = false;
            int intPages = 0;
            
            try
            {
                if (txtCurrentPrintPage.Text != "")
                {
                    intPages = Convert.ToInt32(txtCurrentPrintPage.Text);
                    printBarcode(intPages);
                }
                else
                {

                    MessageBox.Show("请输入本次要打印的页数");
                    return;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("不能取得打印数量，原因是" + ex.Message);
            }
             * */
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


        void myPrintDocument_EndPrint(object sender, PrintEventArgs e)
        {




        }

        private void btnTestPrint_Click(object sender, EventArgs e)
        {
            try
            {
                //timer1.Enabled = false;//这个好像是没有必要

                ArrayList arrlist = new ArrayList();//用这个是因为 arrlistSellectRow 也是静态变量，读取的时候会读取到不是想要的值,这里用深拷贝
                if (arrlistSellectRow != null)
                {
                    foreach (clsKeyValue myKeyValue in arrlistSellectRow)
                    {
                        clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                        arrlist.Add(kv);
                    }
                }
                //测试的话只打印一页就可以了。
                ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

                queuePrintItem printDetails = new queuePrintItem();
                printDetails.strTableName = strCurrentTableName;//当前表
                //如下是取得模板的路径
                string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;

                //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                if (strPathName.IndexOf(":") < 0)
                {
                    //还有得判断首字符是不是"\\"，如果不是就加上
                    if (strPathName.Substring(0) == "\\")
                    {
                        strPathName = Application.StartupPath + strPathName;
                    }
                    else
                    {
                        strPathName = Application.StartupPath + "\\" + strPathName;
                    }
                }

                printDetails.ShapesFileName = strPathName;
                printDetails.IsFull = chkIsFull.Checked;
                queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
                queuePrintItemRowAndPages1.arrlistRow = arrlist;
                queuePrintItemRowAndPages1.intPages = 1;
                printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);

                //queuePrintItem printDetails = new queuePrintItem("Test", ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value, arrlist, 1);
                // 添加到保存信息中。
                myBarcodePrint.addPrintDetails(printDetails);

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine("测试打印出现异常",ex);
                //throw;
            }
          

        }

        private void btnTestDPI_Click(object sender, EventArgs e)
        {
            //这个测试分辨率就是用打印一张纸的方式来测试的

            PrintDocument printDocTestDPI = new PrintDocument();
            printDocTestDPI.DocumentName = "测试分辨率";
            printDocTestDPI.PrinterSettings.PrinterName = lblPrinterName.Text;//还有设置打印机
            printDocTestDPI.PrintPage += new PrintPageEventHandler(printDocTestDPI_PrintPage);

            printDocTestDPI.Print();
        }


        //如下的这个只是取得分辨率
        //如下的这个可以被注释掉了
        void printDocTestDPI_PrintPage(object sender, PrintPageEventArgs e)
        {

            //分辨率可以从e中得到
            //clsBarcodePrint.fltDPIX = e.Graphics.DpiX;
            //clsBarcodePrint.fltDPIY = e.Graphics.DpiY;

            //保存到xmlAPP.xml中
            //首先判断文件是否在，如果不在则新建
            if (!File.Exists(Application.StartupPath + "\\xmlAPP.xml"))
            {
                FileStream fs = File.Create(Application.StartupPath + "xmlAPP.xml");
                fs.Close();//关闭文件
                createXmlApp();
            }

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(Application.StartupPath + "\\xmlAPP.xml");

            XmlElement xmlElePrinter = (XmlElement)xmlDoc.SelectNodes("//printer").Item(0);
            xmlElePrinter.SetAttribute("DPIX", e.Graphics.DpiX.ToString());
            xmlElePrinter.SetAttribute("DPIY", e.Graphics.DpiY.ToString());

            xmlDoc.Save(Application.StartupPath + "\\xmlAPP.xml");

            e.Cancel = true;
            e.HasMorePages = false;


            //throw new NotImplementedException();
        }
        //如下的这个可以注释掉了。
        public void createXmlApp()
        {
            // 首先创建文件
            FileStream fs = File.Create(Application.StartupPath + "\\xmlAPP.xml");
            fs.Close();//关闭文件

            //创建 XmlDocument 以便操作
            XmlDocument xmlDoc = new XmlDocument();

            //xml 唯一的根，我设置的根都是root 
            XmlElement xmlEleRoot = xmlDoc.CreateElement("root");

            // 这个配置还有一个是必须的，就是打印机的DPI, 其他的都不是必要的
            XmlElement xmlElePrinter = xmlDoc.CreateElement("printer");

            //打印机名称，这里直接设置默认打印机
            PrintDocument printDoc = new PrintDocument();
            xmlElePrinter.SetAttribute("PrinterName", printDoc.PrinterSettings.PrinterName);
            printDoc.Dispose();//释放资源

            //DPI有两个
            xmlElePrinter.SetAttribute("DPIX", "600");
            xmlElePrinter.SetAttribute("DPIY", "600");

            //如下是两个添加操作了
            xmlEleRoot.AppendChild(xmlElePrinter);
            xmlDoc.AppendChild(xmlEleRoot);

            //保存操作
            xmlDoc.Save(Application.StartupPath + "\\xmlAPP.xml");

        }

        private void button1_Click(object sender, EventArgs e)
        {
            createXmlApp();
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            FrmHelp f = new FrmHelp();
            f.ShowDialog();
            f.Dispose();
        }

        private void btnSelectPrinter_Click(object sender, EventArgs e)
        {
            //这个是选择打印机
            //我自制力一个对话框来选择打印机，并且自动取得分辨率.

            FrmSelectPrinter f = new FrmSelectPrinter();
            if (f.ShowDialog() == DialogResult.OK)
            {
                if (myClsXmlApp == null)
                    myClsXmlApp = new ClsXmlApp();
                
                //这里要保存的
                myClsXmlApp.strPrintingName = f.strPrinterName;

                lblPrinterName.Text = f.strPrinterName;

                //设置到
                ClsBarcodePrint.strPrinterName = f.strPrinterName;

                saveXmlApp();//保存

            }

        }

        private void comboBoxVaribaleName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //在这个更改时就更改相应的变量 
            foreach (clsKeyValue keyvalue in frmMain.arrlistSellectRow)
            {
                if (keyvalue.Key == comboBoxQtyOfWantToPrinted.Text)
                {
                    txtQtyOfWantToPrinted.Text = keyvalue.Value;
                    return;
                }
            }
        }


        private void btnLoadzPreviously_Click(object sender, EventArgs e)
        {
            //这个依旧是新建一个对话框窗体
            FrmLoadExcelRecords f = new FrmLoadExcelRecords();
            if (f.ShowDialog() == DialogResult.OK)
            {
                strCurrentTableName = FrmLoadExcelRecords.strTableName;
            }

            //从数据库中导入这个表
            ClsDataBase myClsDataBase = new ClsDataBase();
            dataGridView1.DataSource = myClsDataBase.getUserLoadTable(strCurrentTableName);

            //还得选择哪个是数量
            loadPrintedQtytoComboBox();
        }

        //当数据源改变的时候发生
        private void dataGridViewPrintedRecords_DataSourceChanged(object sender, EventArgs e)
        {
            //就是计算打印了多少数量。
            //我的方式就是循环“打印数量”这一列的所有行。
            int intPrintedQty = 0;

            for (int i = 0; i < dataGridViewPrintedRecords.Rows.Count; i++)
            {
                //如下是假设第二列为"打印数量"，如果不是就要做相应的改变
                intPrintedQty = intPrintedQty + Convert.ToInt32(dataGridViewPrintedRecords.Rows[i].Cells[1].Value.ToString());
            }

            txtAlreadyPrinted.Text = intPrintedQty.ToString();

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {

            dataGridViewChangedCell();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblQtyWaitPrint.Text = ClsBarcodePrint.arrlistPrint.Count.ToString();//打印队列中的数量

            //dataGridViewChangedCell();//主要是更新记录用，貌似没有必要用这个不断更新记录

            //如果没有打印的了就停止吧
            //判断是否打印的依据是打印机为空闲，且队列不为空。
            if ((PrinterCheck.GetPrinterStatus(ClsBarcodePrint.strPrinterName) == PrinterCheck.PrinterStatus.空闲)
                && (ClsBarcodePrint.arrlistPrint.Count == 0))
            {
               
                //当这个计时器运行的时候就是自动打印,如上的就是自动打印了。

                    if ((dataGridView1.CurrentCellAddress.Y < dataGridView1.RowCount - 2)&&(isAutoPrint))
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.CurrentCellAddress.Y + 1].Cells[0];
                        dataGridViewChangedCell();//更新记录
                         AutoprintBarcode();
                    }
                    else
                    {
                        //chkAutoPrinter.Checked = false;//这个没有了。
                        timer1.Enabled = false;
                        isAutoPrint = false;

                    }
            }
        }

        private void  isYanZhengZhuce()
        {


            
        }
        /**
        private string Decrypt(string base64code) //解密
        {
            try
            {

                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                string strPrivateKey = @"<RSAKeyValue><Modulus>qnfhMY6XO+fJDmd84nbAyH51xR3gb8ow7GWr3RPl172sYnCCTprCgSg2Y7HexH43p38WHk6bR1hdkic2cYIcz7gqrLs3CsY/YlxljJQ0MGjfeK+OY1L2tB482cE/wjVKAbCG5J+4vzo13S+whKHxsvlkGRM5KpDHyd0ZnE37V8k=</Modulus><Exponent>AQAB</Exponent><P>7W3IhAKh8njPL4XeIf9xjX2HqIgWUS1aIcIEr7bXY5ey53aw47yfkixSudeSZolJMPpGC+GO6hIyEmznlB63iw==</P><Q>t81LaijAd3Utn7xX/QQ/x9c8ijWgyeWQVWyA4F+7Ay6O5Ztke4ufJq6VFslpI0CDe4DUrp2gBtqEAjN/XZB4ew==</Q><DP>UtX3nF8Sw3b0yh7JdlEZ/ARs3RbFuoK5LIf1fJytHxkhGPJnGr2Hasc+AYq9kDqbp5PZ9nE2nGHGyHjoftwMqw==</DP><DQ>Uzx+TZoc5zxCqBcURbnZ5HddrD1zDluOzJCxoGrZ9yvrfKGtlKF7NnpTfBlEKrm5kYGbT2SEpvXoWFLX+BhH5w==</DQ><InverseQ>xKYnwi/1O57Na9fS0GJHxy5/BXdEwqZ7KSeZsftFxrUiO60meb5yFN6MnGANE0A6pqf0tBLgciK8muJVYg7Tsg==</InverseQ><D>Qc7NrKfzUjkEsP7ag0J84emP5WzHO+C+SkRluI755/NdHRN5+oZcGChB9vKvoQNo0MyK6WBHKZ+/X7Crn94u6I7+1+owWeppsd5uie3rruMIZOzaUeGxmiNXsMDZuY7r5aQVb/zccX9+ccMk6DPfE1UVjTsLcUwg8t4tjJ/49lE=</D></RSAKeyValue>";
        
                RSA.FromXmlString(strPrivateKey);

                byte[] encryptedData;
                byte[] decryptedData;
                encryptedData = Convert.FromBase64String(base64code);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);



                //Display the decrypted plaintext to the console. 
                return ByteConverter.GetString(decryptedData);
            }
            catch (Exception exc)
            {
                ClsErrorFile.WriteLine(exc);
                //Exceptions.LogException(exc);
                //Console.Error.WriteLine(exc.Message);
                return "";
            }
        }
         * */

        private byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                ClsErrorFile.WriteLine(e);

                //Exceptions.LogException(e);
                //Console.Error.WriteLine(e.Message);

                return null;
            }

        }


        private Array mySplit(string str, int splitLength)
        {
            //如下是计算能分割成多少份
            int n = str.Length / splitLength;
            if (str.Length % splitLength != 0)
                n++;
            string[] strReturn = new string[n];//只是返回n个元素的数组而已。

            string strShengYu = str;//每次截取后剩下的字符串。
            for (int i = 0; i < n; i++)
            {
                int intJieQuQty = splitLength;
                if (strShengYu.Length < splitLength)
                    intJieQuQty = strShengYu.Length;//如果剩余的字符串不够截取的，就只是截取剩余的长度
                strReturn[i] = strShengYu.Substring(0, intJieQuQty);
                strShengYu = strShengYu.Remove(0, intJieQuQty);
            }


            return strReturn;
        }


        private void chkJianGe_CheckedChanged(object sender, EventArgs e)
        {
            //就是几个控件的可用性跟这个checkbox的选择相同，那几个空间默认是不可用的
            txtInterval.Enabled = chkJianGe.Checked;
            ClsBarcodePrint.isPrintJiange = chkJianGe.Checked;//设置自动打印
            ClsBarcodePrint.strJianGe = txtInterval.Text;
        }

        private void btnQueuePrint_Click(object sender, EventArgs e)
        {
            FrmQueuePrint f = new FrmQueuePrint();
            f.ShowDialog();
        }

        private void btnZhuCe_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://shop34192387.taobao.com/");
        }

        private void splitContainer3_Panel1_Resize(object sender, EventArgs e)
        {
            userControlCanvas1.Location = new Point(2, 2);
            userControlCanvas1.Width = splitContainer3.Panel1.Width - 4;
            userControlCanvas1.Height = splitContainer3.Panel1.Height - 4;
            userControlCanvas1.ZoomPaperToScreen();
            //calculateCanvas();

        }

        private void calculateCanvas()
        {
            float ZoomW = userControlCanvas1.Width/96*25.4f / userControlCanvas1.myShapes.BarcodePageSettings.BarcodePaperLayout.PaperWidth;
            float ZoomH = userControlCanvas1.Height / 96 * 25.4f / userControlCanvas1.myShapes.BarcodePageSettings.BarcodePaperLayout.PaperHeight;
            if (ZoomW < ZoomH)
            {
                userControlCanvas1.Zoom = ZoomW;
            }
            else
            {
                userControlCanvas1.Zoom = ZoomH;
            }
        }

        private void comboBoxBarcodeModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBarcodeModel.SelectedItem != null)
            {

                string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
                //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                if (strPathName.IndexOf(":") < 0)
                {
                    //还有得判断首字符是不是"\\"，如果不是就加上
                    if (strPathName.Substring(0) == "\\")
                    {
                        strPathName = Application.StartupPath + strPathName;
                    }
                    else
                    {
                        strPathName = Application.StartupPath + "\\" + strPathName;
                    }
                }

                ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();

                //如下是画布设置
                userControlCanvas1.Loader(strPathName);

                //根据纸张的大小选择放大率
                //calculateCanvas();

                //userControlCanvas1.setArrKeyValue(arrlisttemp);

                userControlCanvas1.ZoomPaperToScreen();

                userControlCanvas1.Refresh();

                //picCanvas.Image = myBarcodePrint.xmlToBarcodeImage(clsBarcodePrint.populateVariable(xmlDoc, frmMain.arrlistSellectRow));
            }

            //dataGridViewChangedCell();
        }

        /// <summary>
        /// 按损耗打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint2_Click(object sender, EventArgs e)
        {
            //如果是选择多行，每一项都得判断有多少数据的
            if (chkIsMulti.Checked)
            {
                try
                {
                    queuePrintItem printDetails = new queuePrintItem();
                    printDetails.strTableName = strCurrentTableName;
                    //如下是取得模板的路径
                    string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
                    //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                    if (strPathName.IndexOf(":") < 0)
                    {
                        //还有得判断首字符是不是"\\"，如果不是就加上
                        if (strPathName.Substring(0) == "\\")
                        {
                            strPathName = Application.StartupPath + strPathName;
                        }
                        else
                        {
                            strPathName = Application.StartupPath + "\\" + strPathName;
                        }
                    }

                    printDetails.ShapesFileName = strPathName;
                    printDetails.IsFull = chkIsFull.Checked;

                    //得根据选中的行迭代DataGridViewTextBoxCell
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        ArrayList arrlist = new ArrayList();
                        int intPages = 0;//默认打印0页

                        //如下是构造数据的
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            clsKeyValue keyvalue = new clsKeyValue(dataGridView1.Columns[i].Name, item.Cells[i].Value.ToString());
                            arrlist.Add(keyvalue);

                            //判断哪个是数量
                            if (dataGridView1.Columns[i].Name == comboBoxQtyOfWantToPrinted.Text)
                            {
                                try
                                {
                                    intPages = Convert.ToInt32(item.Cells[i].Value.ToString());
                                }
                                catch (System.Exception ex)
                                {
                                    MessageBox.Show("读取不到数量，请在“要打印数量”后选择一项作为数量，这个是自动读取的，错误原因是："+ex.Message);
                                    ClsErrorFile.WriteLine("读取不到数量，请在“要打印数量”后选择一项作为数量，这个是自动读取的，错误原因是：" + ex.Message);
                                    return;
                                }
                            }

                        }
                        //如下就是构造打印了。

                        //创建打印信息。

                        queuePrintItemRowAndPages queuePrintItemRowAndPages1 = new queuePrintItemRowAndPages();
                        queuePrintItemRowAndPages1.arrlistRow = arrlist;
                        queuePrintItemRowAndPages1.intPages = intPages;
                        printDetails.addQueuePrintItemRowAndPages(queuePrintItemRowAndPages1);

                    }

                    ClsBarcodePrint myBarcodePrint = new ClsBarcodePrint();
                    myBarcodePrint.addPrintDetails(printDetails);
                    
                }
                catch (System.Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                }
            }
            else
            {
                int intPages = getQtyOfWantToPrinted();//取得打印数量，加上损耗

                if (intPages > 0)
                {
                    timer1.Enabled = false;
                    printBarcode(intPages);
                }


            }

            /**
             * 如下的会产生问题，比如说在读取打印数量时已经跑出异常了，但是还是系统还是崩溃了,所以加上如上判断

            try
            {
                float fltSunHao = Convert.ToSingle(txtSunHao.Text)/100;
                int intPages = Convert.ToInt32(txtQtyOfWantToPrinted.Text);
                intPages += ((int)(intPages * fltSunHao));
                printBarcode(intPages);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
             * */
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

                MessageBox.Show("无法取得打印数量或者损耗,原因是："+ex.Message);
            }

            return intPages;

        }

        private void chkAutoPrinter_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void btnAutoPrint_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;//计时器开始运行
            isAutoPrint = true;//设置自动打印标识
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            //我现在只能用这个定时器不断查询的方式来更新打印记录了，因为存在数据库延迟。
            /**
            try
            {
                arrlistSellectRow.Clear();//当然首先是清空了。

                string strData = "";//查询打印记录时用

                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    clsKeyValue keyvalue = new clsKeyValue(dataGridView1.Columns[i].Name, dataGridView1.CurrentRow.Cells[i].Value.ToString());
                    arrlistSellectRow.Add(keyvalue);

                    strData = strData + dataGridView1.CurrentRow.Cells[i].Value.ToString();//就是所有项连接起来

                }


                //导入打印记录,判断是根据表名和数据，
                ClsDataBase myClsDataBase = new ClsDataBase();
                dataGridViewPrintedRecords.DataSource = myClsDataBase.commandSelectPrintedRecord(strCurrentTableName, arrlistSellectRow);

            }
            catch (Exception ex)
            {

                Console.Error.WriteLine(ex.Message);
            }
             * */


            //用如下的这个，不用老是对arrlistSellectRow进行赋值操作
            try
            {
                if (arrlistSellectRow != null)
                {
                    string strData = "";
                    foreach (clsKeyValue item in arrlistSellectRow)
                    {
                        strData += item.Value;

                    }

                    //导入打印记录,判断是根据表名和数据，
                    ClsDataBase myClsDataBase = new ClsDataBase();
                    dataGridViewPrintedRecords.DataSource = myClsDataBase.commandSelectPrintedRecord(strCurrentTableName, arrlistSellectRow);
                }
          

            }
            catch (Exception exception)
            {
                ClsErrorFile.WriteLine(exception);

                //Console.Error.WriteLine(exception.Message);
            }
 


        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //如果是可以编辑就更改
            try
            {
                if (!dataGridView1.ReadOnly)
                {
                    OleDbDataAdapterCurrentTable.Update((DataTable)dataGridView1.DataSource);
                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //Console.Error.WriteLine(ex.Message);
                //throw;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private void splitContainer4_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void userControlCanvas1_Resize(object sender, EventArgs e)
        {
            userControlCanvas1.ZoomPaperToScreen();
           
        }

        private void chkIsMulti_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.MultiSelect = chkIsMulti.Checked;//
        }

        private void btnSaveImage_Click(object sender, EventArgs e)
        {
            //这个到处图像跟打印部分绝大部分是相同的，如下的是先拷贝再修改的。



            ArrayList arrlistRows = new ArrayList();


            if (chkIsMulti.Checked)
            {
                //创建打印信息。
                try
                {

                    //得根据选中的行迭代
                    foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                    {
                        ArrayList arrlist = new ArrayList();

                        //如下是构造数据的
                        for (int i = 0; i < dataGridView1.ColumnCount; i++)
                        {
                            clsKeyValue keyvalue = new clsKeyValue(dataGridView1.Columns[i].Name, item.Cells[i].Value.ToString());
                            arrlist.Add(keyvalue);

                        }

                        arrlistRows.Add(arrlist);
                    }

                   
                }
                catch (System.Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    return;
                }



            }
            else
            {
                ArrayList arrlist = new ArrayList();
                
                if (arrlistSellectRow != null)
                {
                    foreach (clsKeyValue myKeyValue in arrlistSellectRow)
                    {
                        clsKeyValue kv = new clsKeyValue(myKeyValue.Key, myKeyValue.Value);
                        arrlist.Add(kv);
                    }

                }
                arrlistRows.Add(arrlist);

            }

            FrmSaveImage f = new FrmSaveImage(((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value,arrlistRows);
            f.ShowDialog();//打开窗体

            
        }

        private void btnUpdateModel_Click(object sender, EventArgs e)
        {

            if (comboBoxBarcodeModel.SelectedItem != null)
            {

                string strPathName = ((clsKeyValue)comboBoxBarcodeModel.SelectedItem).Value;
                //还得判断是否是相对路径，我的简单判断方法是是否有冒号":"
                if (strPathName.IndexOf(":") < 0)
                {
                    //还有得判断首字符是不是"\\"，如果不是就加上
                    if (strPathName.Substring(0) == "\\")
                    {
                        strPathName = Application.StartupPath + strPathName;
                    }
                    else
                    {
                        strPathName = Application.StartupPath + "\\" + strPathName;
                    }
                }


                //如下是保存
                userControlCanvas1.Saver(strPathName);

                //刷新
                //userControlCanvas1.Refresh();

                //picCanvas.Image = myBarcodePrint.xmlToBarcodeImage(clsBarcodePrint.populateVariable(xmlDoc, frmMain.arrlistSellectRow));
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

    }

    /**已经专门找来一个类来保存这个类型。
    public class clsKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }


        public clsKeyValue(string pKey, string pValue)
        {
            this.Key = pKey;
            this.Value = pValue;
        }
        public override string ToString()
        {
            return this.Key;
        }
    }
     * */




}