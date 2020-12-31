using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
using System.Text;
using ZXing;

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
        
        public static string[] arrVarName = { "AZTEC", "EAN13","EAN8", "CODE_39", "QR_CODE" };

        public static Dictionary<string, BarcodeFormat> dictBarcode = new Dictionary<string, BarcodeFormat>();

        static BarcodeEncoding()
        {
            //支持这么多编码。
            dictBarcode.Add("AZTEC", BarcodeFormat.AZTEC);
            dictBarcode.Add("CODABAR", BarcodeFormat.CODABAR);
            dictBarcode.Add("CODE_39", BarcodeFormat.CODE_39);
            dictBarcode.Add("CODE_93", BarcodeFormat.CODE_93);
            dictBarcode.Add("CODE_128", BarcodeFormat.CODE_128);
            dictBarcode.Add("DATA_MATRIX", BarcodeFormat.DATA_MATRIX);
            dictBarcode.Add("EAN_8", BarcodeFormat.EAN_8);
            dictBarcode.Add("EAN_13", BarcodeFormat.EAN_13);
            dictBarcode.Add("ITF", BarcodeFormat.ITF);
            dictBarcode.Add("MAXICODE", BarcodeFormat.MAXICODE);
            dictBarcode.Add("PDF_417", BarcodeFormat.PDF_417);
            dictBarcode.Add("QR_CODE", BarcodeFormat.QR_CODE);
            dictBarcode.Add("RSS_14", BarcodeFormat.RSS_14);
            dictBarcode.Add("RSS_EXPANDED", BarcodeFormat.RSS_EXPANDED);
            dictBarcode.Add("UPC_A", BarcodeFormat.UPC_A);
            dictBarcode.Add("UPC_E", BarcodeFormat.UPC_E);
            dictBarcode.Add("All_1D", BarcodeFormat.All_1D);
            dictBarcode.Add("UPC_EAN_EXTENSION", BarcodeFormat.UPC_EAN_EXTENSION);
            dictBarcode.Add("MSI", BarcodeFormat.MSI);
            dictBarcode.Add("PLESSEY", BarcodeFormat.PLESSEY);
            dictBarcode.Add("IMB", BarcodeFormat.IMB);

        }


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
            //return new StandardValuesCollection(arrVarName);
            return new StandardValuesCollection(dictBarcode.Keys);
        }
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }


    }
}
