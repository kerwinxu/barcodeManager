using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 圆角矩形
    /// </summary>
    public class ShapeRoundedRectangle : ShapeEle
    {

        [DescriptionAttribute("圆角半径"), DisplayName("圆角半径"), CategoryAttribute("布局")]
        public float Radius { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        public ShapeRoundedRectangle()
        {
            // 有些参数默认不能是0
            Radius = 2;
        }

        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeRoundedRectangle>(json);
            //throw new NotImplementedException();
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            var _x = getX();
            var _y = getY();
            var _width = getWidth();
            var _height = getHeight();
            var path = new GraphicsPath();
            // 这里要注意判断圆角半径可能是0的情况。
            path.StartFigure();
            // 上边
            path.AddLine(new PointF(_x + Radius, _y), new PointF(_x + _width - Radius, _y));
            // 右上角
            if(Radius > 0) path.AddArc(new RectangleF(_x + _width - Radius * 2 , _y, Radius* 2, Radius * 2), 270, 90);
            // 右边
            path.AddLine(new PointF(_x + _width, _y + Radius), new PointF(_x + _width, _y + _height - Radius));
            // 右下角
            if (Radius > 0) path.AddArc(new RectangleF(_x + _width - Radius * 2, _y + _height - Radius * 2, Radius*2, Radius*2), 0, 90);
            // 下边
            path.AddLine(new PointF(_x + Radius, _y + _height), new PointF(_x + _width - Radius , _y + _height));
            // 右下角
            if (Radius > 0) path.AddArc(new RectangleF(_x, _y + _height - Radius*2, Radius*2, Radius*2), 90, 90);
            // 左边
            path.AddLine(new PointF(_x, _y + Radius ), new PointF(_x, _y + _height - Radius));
            // 左上角
            if (Radius > 0) path.AddArc(new RectangleF(_x, _y, Radius*2, Radius*2), 180, 90);
            path.CloseFigure();


            return path;

            //return base.GetGraphicsPathWithAngle();
        }
    }
}
