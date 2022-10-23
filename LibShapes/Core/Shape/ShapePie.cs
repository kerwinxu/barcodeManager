using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 扇形,跟弧度的区别是没有那个边框吧。
    /// </summary>
    public class ShapePie : ShapeArc
    {
        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapePie>(json);
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            var rect = new System.Drawing.RectangleF()
            {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getHeight(),
            };

            var rect2 = correctRectangle(rect);
            path.AddPie(
                   rect2.X,
                   rect2.Y,
                   rect2.Width,
                   rect2.Height,
                   StartAngle,
                   SweepAngle);

            return path;
            //return base.GetGraphicsPathWithAngle();
        }


    }
}
