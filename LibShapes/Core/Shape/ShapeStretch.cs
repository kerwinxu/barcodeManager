using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    ///  拉伸
    /// </summary>
    public class ShapeStretch : ShapeEle
    {


        [DescriptionAttribute("拉伸"), DisplayName("拉伸"), CategoryAttribute("外观")]
        public bool isStretch { get; set; }


        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeStretch>(json);
            //throw new NotImplementedException();
        }
    }
}
