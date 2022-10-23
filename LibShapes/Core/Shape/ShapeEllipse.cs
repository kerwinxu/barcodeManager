using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 椭圆
    /// </summary>
    public class ShapeEllipse : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeEllipse>(json);
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();

            var rect = new System.Drawing.RectangleF()
            {
                X = getX(),
                Y = getY(),
                Width = getWidth(),
                Height = getHeight()
            };
            var rect2 = correctRectangle(rect);

            path.AddEllipse(rect2); // 跟矩形不同的是这里的是AddEllipse
            return path;

            //return base.GetGraphicsPathWithAngle();
        }
    }
}
