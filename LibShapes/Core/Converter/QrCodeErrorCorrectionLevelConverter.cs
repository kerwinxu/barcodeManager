using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ZXing.QrCode.Internal;

namespace Io.Github.Kerwinxu.LibShapes.Core.Converter
{
    /// <summary>
    /// 
    /// </summary>
    public  class QrCodeErrorCorrectionLevelConverter : StringConverter
    {
        // 1. 一个静态的属性
        public static Dictionary<string, ErrorCorrectionLevel> level = new Dictionary<string, ErrorCorrectionLevel>();

        // 2. 静态的构造方法，主要是构造上边的属性
        static QrCodeErrorCorrectionLevelConverter()
        {
            level.Add("容错7%", ErrorCorrectionLevel.L);
            level.Add("容错15%", ErrorCorrectionLevel.M);
            level.Add("容错25%", ErrorCorrectionLevel.Q);
            level.Add("容错30%", ErrorCorrectionLevel.H);

        }

        //3. 覆盖 GetStandardValuesSupported 方法并返回 true ，表示此对象支持可以从列表中选取的一组标准值。   
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        //4. 
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
            return new StandardValuesCollection(level.Keys);
        }

        //5.
        //如下这样就会变成组合框
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return false;
        }

    }
}
