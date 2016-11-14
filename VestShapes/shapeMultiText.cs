using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
//using System.Linq;
using System.Text;

namespace VestShapes
{
    /// <summary>
    /// 如下是多行文本，多行文本跟单行文本的区别是多行文本的宽度是不变的，
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class shapeMultiText : shapeSingleText
    {
        public shapeMultiText()
            : base()
        {
            IsStretch = false;

        }

        public override void Draw(Graphics g, List<Matrix> listMatrix)
        {
            RectangleF rect = getGraphicsPath(listMatrix).GetBounds();
            float fltx = rect.X;
            float flty = rect.Y;
            float fltw = rect.Width;
            float flth = rect.Height;

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //如下的这个是偏移些位置


            //如下是先从绘制矩形中的拷贝的，然后再修改
            if (Route != 0)
            {
                PointF pZhongXin = getCentrePoint();
                g.TranslateTransform(pZhongXin.X, pZhongXin.Y, MatrixOrder.Prepend);
                g.RotateTransform((float)Route);
                g.TranslateTransform(-pZhongXin.X, -pZhongXin.Y);
            }

            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            //RectangleF rect = getRect();

            //这里绘图。
            // 字符串格式
            StringFormat sf = new StringFormat();
            sf.Alignment = AlignMent;
            sf.LineAlignment = LineAlignMent;
            g.DrawString(_strAllText, _RealFont, new SolidBrush(_FillColor), new RectangleF(fltx, flty, fltw, flth), sf);

            g.ResetTransform();
            //base.Draw(g, arrlistMatrix);
        }

        /// <summary>
        /// 只所以用这个是因为在这个多行文本中，宽和高是用户画出来的，
        /// 不是根据实际的算的，并且在绘制中要绕开这个，因为这个只是画了一个边框，
        /// 不是实际的文字
        /// </summary>
        /// <returns></returns>
        public override GraphicsPath getGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();

            path.AddRectangle(getRect());

            //做一个变换矩阵加上偏移和旋转
            System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
            //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
            m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
            path.Transform(m);//应用变换矩阵

            return path;
            //return base.getGraphicsPath();
        }




        protected override void UpdateWidthHeight()
        {
            UpdateStrAllText();//首先更新
            //因为这个是多行文本，是不需要自动计算字体高度等的，所以取消了。
        }



        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = Math.Min(p1.X, p2.X);
            _Y = Math.Min(p1.Y, p2.Y);
            _Width = Math.Max(p1.X, p2.X) - _X;
            _Height = Math.Max(p1.Y, p2.Y) - _Y;

            UpdateWidthHeight();
            //base.ShapeInit(p1, p2);
        }


    }
}
