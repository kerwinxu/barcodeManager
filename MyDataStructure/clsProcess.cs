using System;
using System.Collections.Generic;
using System.Text;

namespace Xuhengxiao.MyDataStructure
{
    /// <summary>
    /// 这个类只是应用在win8 系统中，权限控制太严格了。
    /// </summary>
    public  class clsProcess
    {
        public static void Start(string str)
        {
            try
            {
                System.Diagnostics.Process.Start(str);
            }
            catch (System.Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
            	    
            }

        }

    }
}
