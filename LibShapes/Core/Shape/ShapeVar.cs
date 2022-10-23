using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 变量，主要是支持外部输入变量
    /// </summary>
    public class ShapeVar : ShapeStretch
    {

        [DescriptionAttribute("对应excel中的一列数据"), DisplayName("变量名"), CategoryAttribute("变量")]
        public string VarName { get; set; }


        [Browsable(false)]//不在PropertyGrid上显示
        public string VarValue { get; set; }

        [DescriptionAttribute("没有指定变量时的文本"), DisplayName("文本"), CategoryAttribute("文本")]
        public string StaticText { get; set; }


        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeVar>(json);
            //throw new NotImplementedException();
        }

        public override  void setVals(Dictionary<string, string> vars)
        {
            //首先判断是否有这个
            if (vars.ContainsKey(VarName))
            {
                VarName = vars[VarName]; // 这个变量的值
            }
            else
            {
                VarValue = string.Empty;         // 没有是空字符串。
            }
        }

        /// <summary>
        /// 取得文本
        /// </summary>
        /// <returns></returns>
        public virtual string getText()
        {
            return string.IsNullOrEmpty(this.VarName) ? StaticText : this.VarValue;
        }

        public override bool Equals(object obj)
        {
            var shape = obj as ShapeVar;
            if (shape == null) return false; // 转换失败就是不同啦

            return base.Equals(obj) && this.VarName == shape.VarName;
        }

    }
}
