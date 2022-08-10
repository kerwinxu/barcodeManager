using System;
using System.Collections.Generic;
using System.Text;
using ADOX;
using ADODB;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using Xuhengxiao.MyDataStructure;
using System.Linq;

namespace Xuhengxiao.DataBase
{
    /**
     我这里的表是分3种的，一种是main表，记录信息是[日期，单号，档口号，原先文件名，现在文件名。
     然后有一个表是记录打印数量的,
     最后的一种表是一个文件夹，记录所有打印过的表。

     我这里，从外部导入的表是excel格式，而内部的，我都是保存成csv格式。

     外部excel ------> main表中记录
               ------> 将表信息保存到tables文件夹中。
               ------> 
     关于打印的，记录的新文件名，相关内容和
     * 
     * */


    /**我这个数据库的表分三种
     * 1、main 表，单独的一个，里边的项很简单，有4项，第一项为自增型数据，我拿这个作为表名，一个是日期，一个是单号，一个是档口号,一个是文件名，
     * 2、printedRecords 表，就是打印的记录，有5项，第一个是日期+时间，第二个是表名，第三个是各项的连接，第四个就是打印数量
     * 3、第三类表就是保存用户导入的excel 的数据了。
     * 
     *这个类的成员函数可以是
     *1、addExcelData ，增加一个表，输入是单号，档口号，和相应的选择的excel的DataTable，返回值就是一个表名。
     *2、listMainTable，就是读取main 表，并返回 DataTable
     *3、sellectTable,选择表，输入是表名，返回是DataTable
     *4、sellectzPrintedRecords ，读取记录而已.输入是表名和xml，输出也是DataTable

     * */

    public  class ClsDataBase
    {

        //public  string _strDataSource = Application.StartupPath + "\\myDataBase.mdb";

        /// <summary>
        ///  记录的文件夹
        /// </summary>
        private static  string _str_record_dir = Path.Combine(Application.StartupPath, "record");
        /// <summary>
        /// 主数据库
        /// </summary>
        private static  string _str_record_main_table = Path.Combine(_str_record_dir, "main.csv");

        /// <summary>
        /// 记录打印过的数量
        /// </summary>
        private static string _str_record_printed_table = Path.Combine(_str_record_dir, "printed.csv");

        /// <summary>
        /// 各个表的内容
        /// </summary>
        private static string _str_record_tables_dir = Path.Combine(_str_record_dir, "tables");

        private static string[] main_columns = {"日期", "单号","档口号" ,"源文件名", "现文件名" };

        private static string[] printed_columns = { "日期", "现文件名", "内容", "数量" };


        public ClsDataBase()
        {
            init();

        }

        /// <summary>
        /// 初始化，判断是否有相关的文件夹或者文件的。
        /// </summary>
        private void init()
        {
            // 如果不存在文件夹就新建。
            DirectoryInfo directoryInfo = new DirectoryInfo(_str_record_dir);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            // 主数据库
            FileInfo fileInfo = new FileInfo(_str_record_main_table);
            if (!fileInfo.Exists)
            {
                // 新建，且编码为UTF8编码。
                File.WriteAllText(_str_record_main_table, string.Join(",", main_columns) + Environment.NewLine,Encoding.UTF8);
            }

            FileInfo fileInfo2 = new FileInfo(_str_record_printed_table);
            //
            if (!fileInfo2.Exists)
            {
                // 新建，且编码为UTF8编码。
                File.WriteAllText(_str_record_printed_table, string.Join(",", printed_columns) + Environment.NewLine, Encoding.UTF8);
            }
            //
            DirectoryInfo directoryInfo2 = new DirectoryInfo(_str_record_tables_dir);
            if (!directoryInfo2.Exists)
            {
                directoryInfo2.Create();
            }

        }
        /// <summary>
        /// 这个是导入一个表后，在本地创建记录的。
        /// </summary>
        /// <param name="strSc"></param>
        /// <param name="strShop"></param>
        /// <param name="strFileName"></param>
        /// <param name="myDataTable"></param>
        /// <returns></returns>
        public string loadExcel(string strSc, string strShop, string strFileName, DataTable myDataTable)
        {
            //  取得不带扩展名的名字。
            string filename = Path.GetFileNameWithoutExtension(strFileName);
            string str_new_file_name = string.Format("{0}-{1}.csv", DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"), filename);
            string file_path = Path.Combine(_str_record_tables_dir, str_new_file_name); // 保存在这里边
            // 然后新建一个csv文件，保存在里边
            using (StreamWriter sw = new StreamWriter(file_path, false, Encoding.UTF8))
            {
                // 首先添加标题
                string [] columns = new string[myDataTable.Columns.Count];
                for (int i = 0; i < myDataTable.Columns.Count; i++)
                {
                    columns[i] = myDataTable.Columns[i].ColumnName;
                }
                sw.Write(string.Join(",", columns) + Environment.NewLine);
                // 然后添加内容
                foreach (DataRow item in myDataTable.Rows)
                {
                    // 每一行
                    string[] line = columns.Select(x => item[x].ToString()).ToArray();
                    sw.Write(string.Join(",", line) + Environment.NewLine);
                }
            }

            // 保存到main表
            appendMainTable(strSc, strShop, strFileName, str_new_file_name);

            return str_new_file_name;
        }
        /// <summary>
        /// 取得主表
        /// </summary>
        /// <returns></returns>
        public DataTable getMainTable()
        {
            return loadCsv(_str_record_main_table);
        }

        /// <summary>
        /// 加载以前打印的某个表
        /// </summary>
        /// <param name="strNewFileName"></param>
        /// <returns></returns>
        public DataTable sellectTable(string strNewFileName)
        {

            return sellectTable(Path.Combine(_str_record_tables_dir, strNewFileName));

        }

        /// <summary>
        /// 加载csv文件
        /// </summary>
        /// <param name="str_csv_file_path"></param>
        /// <returns></returns>

        public DataTable loadCsv(string str_csv_file_path)
        {
            DataTable dataTable = new DataTable();

            if (File.Exists(str_csv_file_path))
            {
                var lines = File.ReadAllLines(str_csv_file_path, Encoding.UTF8);
                // 第一行是列名
                var columns = lines[0].Split(',');
                // 根据列名组件列的标题
                for (int i = 0; i < columns.Length; i++)
                {
                    dataTable.Columns.Add(columns[i], Type.GetType("System.String"));
                }
                // 然后添加数据
                for (int i = 1; i < lines.Length; i++)
                {
                    DataRow dataRow = dataTable.NewRow(); //
                    var items = lines[i].Split(',');
                    for (int j = 0; j < columns.Length; j++)
                    {
                        dataRow[j] = items[j];
                    }
                }
            }
           
            return dataTable;
        }

        /// <summary>
        /// 添加一个打印记录
        /// </summary>
        /// <param name="strNewFileName"></param>
        /// <param name="clsKeyValues"></param>
        /// <param name="num_print"></param>
        public void appendPrintedTable(string strNewFileName, List<clsKeyValue> clsKeyValues, int num_print)
        {
            using (StreamWriter sw = new StreamWriter(_str_record_printed_table, true, Encoding.UTF8))
            {
                //这个只是添加一行的。
                string s = string.Format("{0},{1},{2},{3}",
                    DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"),
                    strNewFileName,
                    string.Join("$$$", clsKeyValues.Select(x => x.Key + ":" + x.Value)),
                    num_print
                    ) + Environment.NewLine;
                sw.Write(s);
            }
        }

        public void appendMainTable(string strSc, string strShop, string strFileName, string newFileName)
        {
            using (StreamWriter sw = new StreamWriter(_str_record_main_table, true, Encoding.UTF8))
            {
                //这个只是添加一行的。
                string s = string.Format("{0},{1},{2},{3},{4}",
                    DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss"),
                    strSc,
                    strShop,
                    strFileName,
                    newFileName
                    ) + Environment.NewLine;
                sw.Write(s);
            }
        }

    }
}
