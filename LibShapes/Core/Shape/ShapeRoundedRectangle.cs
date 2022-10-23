using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 圆角矩形
    /// </summary>
    public class ShapeRoundedRectangle : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeRoundedRectangle>(json);
            //throw new NotImplementedException();
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            // todo 圆角矩形实现。
            return base.GetGraphicsPathWithAngle();
        }
    }
}
