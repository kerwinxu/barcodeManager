using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.IO;
using System.Data;
using Xuhengxiao.MyDataStructure;
using System.Windows.Forms;
using Xuhengxiao.DataBase;


namespace Xuhengxiao.ImportData
{
    public class ClsImportExcel
    {
        //如下的两个是比较重要的返回数据
        public  DataTable loadExcelDataTalbe;//就是将excle 导入这个数据中，
        public string strCurrentTableName;//

        public ClsImportExcel()
        {
            //初始化
            loadExcelDataTalbe = new DataTable();
            strCurrentTableName = "";

            //选择是哪个档口
            FrmSCAndShop myFrmSCAndShop = new FrmSCAndShop();

            if (myFrmSCAndShop.ShowDialog() != DialogResult.OK)//只要判断这个就可以了。
            {
                MessageBox.Show("请输入单号（名）和档口号（名），以方便以后查看");
                return;

            }


            //选择文件
            string strFile;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
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

            //将导入的表导入到数据库,
            ClsDataBase myClsDataBase = new ClsDataBase();
            strCurrentTableName = myClsDataBase.loadExcel(FrmSCAndShop.strSC, FrmSCAndShop.strShop,  strFile, loadExcelDataTalbe);


        }
        
        /// <summary>
        /// 获取EXCEL的表 表名字列 
        /// </summary>
        /// <param name="p_ExcelFile">Excel文件</param>
        /// <returns>数据表</returns>
        private   string[] GetExcelSheetNames(string excelFile)
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
        private   DataTable GetExcelToDataSet(string FileFullPath, bool no_HDR, string SheetName)
        {
            try
            {
                string strConn = GetExcelConnectionString(FileFullPath, no_HDR);
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                DataTable ds = new DataTable();

                //要想获得修改表格的话只能把这个设置为属性
                OleDbDataAdapter OleDbDataAdapterCurrentTable;
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
                //ClsErrorFile.WriteLine(ee);
                //Console.Error.WriteLine(ee.Message);
                //throw new Exception(ee.Message);
                return new DataTable();
            }
        }




        private   System.Data.DataTable GetExcelSheetNames2DataTable(string excelFile)
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

        private   bool IsExcel2007(string strFileName)
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
        private  String GetExcelConnectionString(string excelFile, bool no_HDR)
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
                //ClsErrorFile.WriteLine(ee);
                throw new Exception(ee.Message);
            }
        }
    }
}
