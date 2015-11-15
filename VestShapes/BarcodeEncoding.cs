using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace VestShapes
{

    /// <summary>
    /// 显示条形码编码类型
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class BarcodeEncoding : StringConverter
    {
        //我现在只能用这种静态的方式来搞定这个了。
        public static string[] arrVarName = { "EAN13","EAN8","UPCA","UPCE","UPC_SUPPLEMENTAL_2DIGIT","UPC_SUPPLEMENTAL_5DIGIT",
                                                "CODE39","CODE39Extended","CODE128","CODE128A","CODE128B","CODE128C",
                                                "Codabar","ISBN","Interleaved2of5","Standard2of5","Industrial2of5","PostNet",
                                                "BOOKLAND","JAN13","MSI_Mod10","MSI_2Mod10","MSI_Mod11","MSI_Mod11_Mod10",
                                                "Modified_Plessey","CODE11","USD8","UCC12","UCC13","LOGMARS,","ITF14","CODE93",
                                                "TELEPEN","FIM","QR_CODE"};


        //覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }
        /// <summary>
        /// 覆盖 GetStandardValues 方法并返回填充了标准值的 StandardValuesCollection 。
        /// 创建 StandardValuesCollection 的方法之一是在构造函数中提供一个值数组。
        /// 对于选项窗口应用程序，您可以使用填充了建议的默认文件名的 String 数组。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            return new StandardValuesCollection(arrVarName);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }
}
