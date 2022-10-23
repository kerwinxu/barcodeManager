using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 线段
    /// </summary>
    public class ShapeLine : ShapeEle
    {
        public override ShapeEle DeepClone()
        {
            // 这里用json的方式
            string json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<ShapeLine>(json);
            //throw new NotImplementedException();
        }

        public override GraphicsPath GetGraphicsPathWithAngle()
        {
            GraphicsPath path = new GraphicsPath();
            path.AddLine(getX(), getY(), getX()+getWidth(), getY()+getHeight());
            return path;

            //throw new NotImplementedException();
        }

        ///// <summary>
        ///// 是否取得线段的。
        ///// </summary>
        ///// <param name="matrix"></param>
        ///// <param name="mousePointF"></param>
        ///// <returns></returns>
        //public override bool isContains(Matrix matrix, PointF mousePointF)
        //{
        //    // 这里用点到线段的距离来判断的，
        //    var path = GetGraphicsPath(matrix);// 取得路径
        //    var points = path.PathPoints;      // 取得路径上的点
        //    bool b =  SelectStrategy.isNear(mousePointF, points[0], points[1]);
        //    return b;
        //    //return base.isContains(matrix, mousePointF);
             
        //}
    }
}
