using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 矩形
    /// </summary>
    public class ShapeRectangle : ShapeEle
    {
        // 我将这个部分移动到ShapeEle部分了,默认情况下就是这种。
        //public override GraphicsPath GetGraphicsPathWithAngle()
        //{
        //    GraphicsPath path = new GraphicsPath();
        //    path.AddRectangle(new System.Drawing.RectangleF() {
        //        X = getX(),
        //        Y = getY(),
        //        Width = getWidth(),
        //        Height = getHeight()
        //    });
        //    return path;
        //    //throw new NotImplementedException();
        //}

        public override ShapeEle DeepClone()
        {
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeRectangle>(json);
            //throw new NotImplementedException();
        }
    }
}
