using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace VestShapes
{

    //扇形跟圆弧差不多。
    [Serializable]
    //[ProtoContract]
    public class ShapePie : ShapeArc
    {
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = getRect();
            try
            {
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, StartAngle, EndAngle);
            }
            catch (Exception ex)
            {

                rect.X = _X + _XAdd;
                rect.Y = _Y + _YAdd;
                rect.Width = 10;
                rect.Height = 10;
                path.AddPie(rect.X, rect.Y, rect.Width, rect.Height, StartAngle, EndAngle);
                ////ClsErrorFile.WriteLine("这里是一个扇形出现参数错误，异常处理是构造一个默认宽和高都是10，角度为0和90的扇形", ex);
                //throw;

                //throw;
            }

            return path;
            //return base.getGraphicsPath();
        }

    }

}
