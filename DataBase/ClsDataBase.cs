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

namespace Xuhengxiao.DataBase
{
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
        // TODO ,以后换成Sqlite数据库，这个
        //public  string _strDataSource = Application.StartupPath + "\\myDataBase.mdb";

        public ClsDataBase()
        {
            ////如果文件不存在就创建
            //if (!File.Exists(_strDataSource))
            //{
            //    ADOX.Catalog catalog = new Catalog();
            //    catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +_strDataSource + ";Jet OLEDB:Engine Type=5");
            //}

            ////如果表不存在就创建。一个是main 表，另一个是printedRecords 表。我是用如下比较暴力的方式判断的，入托表存在，就会异常退出。
            //commandExecuteNonQuery("create table main ( 表名 integer  identity(1,1) primary key , 导入时间 Date , 单号 text(50),档口 text(50) , 文件名 text(50)  );");
            ////commandExecuteNonQuery("create table printedRecords ( 打印时间 Date , 表名 text(50) , 数据 memo , 打印数量 long);");//这个不需要了

        }

        public ClsDataBase(string strDataSource)
        {
            //_strDataSource = strDataSource;

            ////如果文件不存在就创建
            //if (!File.Exists(strDataSource))
            //{
            //    ADOX.Catalog catalog = new Catalog();
            //    catalog.Create("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _strDataSource + ";Jet OLEDB:Engine Type=5");
            //}

            ////如果表不存在就创建。一个是main 表，另一个是printedRecords 表。我是用如下比较暴力的方式判断的，入托表存在，就会异常退出。
            //commandExecuteNonQuery("create table main ( 表名 integer  identity(1,1) primary key , 导入时间 Date , 单号 text(50),档口 text(50) , 文件名 text(50)  );");
            ////commandExecuteNonQuery("create table printedRecords ( 打印时间 Date , 表名 text(50) , 数据 memo , 打印数量 long);");//这个不需要了


        }

        //对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。对于所有其他类型的语句，返回值为 -1。如果发生回滚，返回值也为 -1。
        //public  int  commandExecuteNonQuery(string strSql)
        //{
        //    using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _strDataSource + ";Jet OLEDB:Engine Type=5"))
        //    {

        //        try
        //        {
        //            OleDbCommand command = new OleDbCommand(strSql);
        //            command.Connection = connection;

        //            // Open the connection and execute the insert myCommand.
        //            connection.Open();
        //            return  command.ExecuteNonQuery();

        //        }
        //        catch (Exception ex)
        //        {
        //            ClsErrorFile.WriteLine(ex);

        //            //TextWriter errorWriter = Console.Error;
        //            //errorWriter.WriteLine(ex.Message);
        //            return -1;
                    
        //        }
        //        // The connection is automatically closed when the
        //        // code exits the using block.
        //    }


        //}

        ////如下是导入一个表
        //public string loadExcel(string strSc, string strShop , string strFileName, DataTable myDataTable)
        //{
        //    //先取得自增值作为表名
        //    string strIDENTITY = strGetIDENTITY(strSc, strShop , strFileName);
        //    //如下就是创建表表名
        //    string strCreateTable = "create table " + strIDENTITY + " ( ";

        //    //根据myDataTable中的列的数据构造SQL，得判断类型的
        //    foreach (DataColumn myDataColum in myDataTable.Columns)
        //    {
        //        // 这个是创建表的SQL:create table printedRecords ( loadDate Date , TableName text(50) , data memo , printedQty long)
        //        //首先加上列名，其次是数据类型
        //        //因为标题可能有特殊符号，所以要加方括号
        //        strCreateTable = strCreateTable +"["+ myDataColum.Caption+"]" + "  ";//需要空格隔开
               
        //        switch (myDataColum.DataType.ToString())
        //        {
        //           /**
        //            case "System.Double":
        //                strCreateTable = strCreateTable + "int ,";
        //                break;
        //            case "System.DateTime":
        //                strCreateTable = strCreateTable + "Date ,";
        //                break;
        //            * */
        //            default:
        //                strCreateTable = strCreateTable + "text(50),";//一般50个字符是足够了。
        //                break;
        //        }
        //    }

        //    //需要把最后那个","去掉
        //    strCreateTable = strCreateTable.Substring(0, strCreateTable.LastIndexOf(","));
        //    strCreateTable = strCreateTable + ")";//最后补上这个圆括号

        //    //到这一步就已经做好了SQL语句了,如下是建立了这个表。
        //    //commandExecuteNonQuery(strCreateTable);

        //    //如下是将这个增加到main 中。在strGetIDENTITY的中就已经增加到main中了。

        //    //如下是用插入的方式一个一个的拷贝数据
        //    // "insert into main ( 导入时间 , 单号 ,档口 , 文件名 )  values ( #" + DateTime.Now + "# ,\"" + strSc + "\" ,\"" + strShop + "\"  , \""+strFileName+"\");";

        //    string strInsertSqlPrefix = "insert into " + strIDENTITY + "(";

        //    //根据myDataTable中的列的数据构造SQL，得判断类型的
        //    foreach (DataColumn myDataColum in myDataTable.Columns)
        //    {
        //        // 这个是创建表的SQL:create table printedRecords ( loadDate Date , TableName text(50) , data memo , printedQty long)
        //        //首先加上列名，其次是数据类型
        //        //因为标题可能有特殊符号，所以要加方括号
        //        strInsertSqlPrefix = strInsertSqlPrefix + "[" + myDataColum.Caption + "]" + "  ,  ";//需要空格隔开

        //    }
        //    //需要把最后那个","去掉
        //    strInsertSqlPrefix = strInsertSqlPrefix.Substring(0, strInsertSqlPrefix.LastIndexOf(","));
        //    strInsertSqlPrefix = strInsertSqlPrefix + ")";//最后补上这个圆括号

        //    strInsertSqlPrefix += "  values ( ";


        //    //如下就是迭代每一行，并且每一行都用一个语句来插入
        //    foreach (DataRow myDataRow in myDataTable.Rows)
        //    {
        //        string strInsertSql = strInsertSqlPrefix;

        //        foreach (object  item in myDataRow.ItemArray)
        //        {
        //            strInsertSql+="\"" + item.ToString() + "\" , ";
                    
        //        }


        //        //需要把最后那个","去掉
        //        strInsertSql = strInsertSql.Substring(0, strInsertSql.LastIndexOf(","));
        //        strInsertSql = strInsertSql + ")";//最后补上这个圆括号

        //        ////如下是运行这个插入
        //        //using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _strDataSource + ";Jet OLEDB:Engine Type=5"))
        //        //{
        //        //    try
        //        //    {
        //        //        connection.Open();//打开连接                        
        //        //        OleDbCommand myCommand = new OleDbCommand();
        //        //        myCommand.CommandText = strInsertSql;
        //        //        myCommand.Connection = connection;
        //        //        myCommand.ExecuteNonQuery();

        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        ClsErrorFile.WriteLine(ex);
        //        //        //TextWriter errorWriter = Console.Error;
        //        //        //errorWriter.WriteLine(ex.Message);
        //        //    }
        //        //    // The connection is automatically closed when the
        //        //    // code exits the using block.
        //        //}
                
        //    }

        //    /**当标题中出现特殊字符的时候，如下的这个导入会不成功
        //    //如下用oldDbAdapater来将EXCEL中的数据拷贝到ACCESS中
        //    using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=myDataBase.mdb;Jet OLEDB:Engine Type=5"))
        //    {
        //        try
        //        {
        //            OleDbDataAdapter adapter = new OleDbDataAdapter();
        //            adapter.SelectCommand = new OleDbCommand("select * from " + strIDENTITY, connection);//select * from table 就是查询所有的
        //            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
        //            connection.Open();
        //            DataTable customers = new DataTable();//要拷贝到这里边
        //            adapter.Fill(customers);

        //            //这个DataRow拷贝只能是像如下这样的对象拷贝
        //            foreach (DataRow myDataRow in myDataTable.Rows)
        //            {
        //                DataRow newRow = customers.NewRow();//DataTable中的数据只能是这样在来获得
        //                newRow.ItemArray = myDataRow.ItemArray;//ItemArray通过一个数组来获取或设置此行的所有值 
        //                customers.Rows.Add(newRow);//添加一行。
        //            }


        //            //code to modify data in dataset here

        //            adapter.Update(customers);//将改动更新到数据源。
        //        }
        //        catch (Exception ex)
        //        {
        //            TextWriter errorWriter = Console.Error;
        //            errorWriter.WriteLine(ex.Message);
        //        }
        //        // The connection is automatically closed when the
        //        // code exits the using block.
        //    }
        //     * */




        //    return strIDENTITY;

        //}

        //private string strGetIDENTITY(string strSc, string strShop , string strFileName)
        //{
        //    string strIDENTITY = "";

        //    //我认为取得表名，也就是自增型数据
        //    using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _strDataSource + ";Jet OLEDB:Engine Type=5"))
        //    {
        //        try
        //        {
        //            connection.Open();//打开连接 


        //            //INSERT INTO main ( loadDate, SC, Shop ) VALUES (#2012/9/19#, "sc", "shop");
        //            string strInsertSql = "insert into main ( 导入时间 , 单号 ,档口 , 文件名 )  values ( #" + DateTime.Now + "# ,\"" + strSc + "\" ,\"" + strShop + "\"  , \""+strFileName+"\");";
        //            OleDbCommand myCommand = new OleDbCommand();
        //            myCommand.CommandText = strInsertSql;
        //            myCommand.Connection = connection;
        //            myCommand.ExecuteNonQuery();

        //            myCommand.CommandText = "SELECT @@IDENTITY";
        //            strIDENTITY = myCommand.ExecuteScalar().ToString();


        //        }
        //        catch (Exception ex)
        //        {
        //            ClsErrorFile.WriteLine(ex);
        //            //TextWriter errorWriter = Console.Error;
        //            //errorWriter.WriteLine(ex.Message);
        //        }
        //        // The connection is automatically closed when the
        //        // code exits the using block.
        //    }

        //    return strIDENTITY;

        //}




        //public DataTable commandSelect(string strSelect)
        //{
        //    DataTable customers = new DataTable();

        //    //如下用oldDbAdapater来将EXCEL中的数据拷贝到ACCESS中
        //    using (OleDbConnection connection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + _strDataSource + ";Jet OLEDB:Engine Type=5"))
        //    {
        //        try
        //        {
        //            OleDbDataAdapter adapter = new OleDbDataAdapter();
        //            adapter.SelectCommand = new OleDbCommand(strSelect, connection);//select * from table 就是查询所有的
        //            OleDbCommandBuilder builder = new OleDbCommandBuilder(adapter);
        //            connection.Open();
        //            adapter.Fill(customers);

        //        }
        //        catch (Exception ex)
        //        {
        //            ClsErrorFile.WriteLine(ex);
        //           // TextWriter errorWriter = Console.Error;
        //            //errorWriter.WriteLine(ex.Message);
        //        }
        //        // The connection is automatically closed when the
        //        // code exits the using block.
        //        return customers;

        //    }
        //}


        ////如下是查询用户以前导入的表，输入为单号，返回相应的表
        //public DataTable getUserLoadTable(string strTableName)
        //{
        //    return commandSelect("select * from " + strTableName);
        //}

        ////如下是返回main 表

        //public DataTable getMainTable()
        //{
        //    //我觉得这个倒序会比较好些，也只显示最后的500项目
        //    return commandSelect("select top 500 * from main ORDER BY main.表名 DESC;");
        //}

        //
        /**
         * printedRecords 表，就是打印的记录，有5项，
         * 第一个是日期+时间，
         * 第二个是表名，
         * 第三个是各项的连接（也就是各项当字符串连接起来），
         * 第四个就是打印数量
         * 对于这个printedRecords ，一个是增加操作，一个是查询操作。
         * 
         * */

        //public int commandAddPrintedRecord(string strTableName, ArrayList myArrayList, int intprintedQty)
        //{
        //    //因为access 的表容量有限，如果打印的非常多，access 就会不堪重负。

        //    //我是将每个EXCEL导入的文件的打印数据放在一个单独的表内，命名格式是表名+printedRecords

        //    //首先判断表是否存在，如果不存在则新建，如下的方式是比较暴力的方式，如果存在就异常退出了
        //    commandExecuteNonQuery("create table " + strTableName + "printedRecords ( 打印时间 Date , 表名 text(50) , 数据 memo , 打印数量 long);");

        //    //再将打印数据插入到表


        //    //这个就是查询 printedRecords 表,
        //    string strSQL = "insert into "+strTableName+"printedRecords ( 打印时间  , 表名  , 数据  , 打印数量 ) values ( # ";
        //    strSQL = strSQL + DateTime.Now + "# , ";//时间
        //    strSQL = strSQL+"\"" + strTableName + "\" , ";//表名
            
            
        //    string strData = "";
        //    foreach (clsKeyValue myClsKeyValue in myArrayList)
        //    {
        //        strData = strData + myClsKeyValue.Value;//将所有值连接起来
        //    }

        //    strSQL = strSQL +"\""+ strData + "\" , ";//数据
        //    strSQL = strSQL + intprintedQty.ToString() + " ) ;";
        //    return commandExecuteNonQuery(strSQL);;

        //}

//        public DataTable commandSelectPrintedRecord(string strTableName, ArrayList myArrayList)
//        {
//            /**
//             * SELECT printedRecords.loadDate, printedRecords.TableName, printedRecords.data, printedRecords.printedQty
//FROM printedRecords
//WHERE (((printedRecords.TableName)="18") AND ((printedRecords.data)="fvdsgr"));

//             * 
//             * 
//             * */
//            string strData = "";
//            foreach (clsKeyValue myClsKeyValue in myArrayList)
//            {
//                strData = strData + myClsKeyValue.Value;//将所有值连接起来
//            }

//            string strSQL = "select 打印时间 , 打印数量  from " + strTableName + "printedRecords WHERE (((表名)=\"";
//            strSQL=strSQL+strTableName+"\") AND ((数据)=\""+strData+"\"))";

//            return commandSelect(strSQL);

//        }

    }
}
