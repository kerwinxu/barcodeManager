using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
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

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //定义画笔
            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = base.getGraphicsPath();//首先取得没有偏移但有旋转的路径

            //再反转这个个变换
            arrlistMatrix.Reverse();

            if ((arrlistMatrix != null) && (arrlistMatrix.Count > 0))//只有数量大于0才能做如下的
            {
                for (int i = 0; i < arrlistMatrix.Count; i++)
                {
                    path.Transform((Matrix)arrlistMatrix[i]);

                }
            }



            //如下这个就是画边界
            try
            {

                g.DrawPath(_myPen, path);


            }
            catch (Exception ex)
            {
                ////ClsErrorFile.WriteLine(ex);
                //throw;
            }

            //throw new NotImplementedException();
            if (_isFill)
            {
                try
                {

                    g.FillPath(new SolidBrush(_FillColor), path);

                }
                catch (Exception ex)
                {
                    ////ClsErrorFile.WriteLine(ex);
                    //throw;
                }

            }

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
            /**

            //这里自动取得字符串的宽度和高度
            Bitmap bitmap = new Bitmap(100, 100);
            Graphics g = Graphics.FromImage(bitmap);
            g.PageUnit = GraphicsUnit.Millimeter;//因为我所有的都换成毫米单位了。，这里求出来的宽度和高度是以毫米为单位的。
            SizeF MaxSizef = new SizeF(Width, Height);//这个最大大小，用宽度固定，来求长度的方式
            SizeF textSizef = g.MeasureString(_strAllText, _font,MaxSizef);//

            Height = MaxSizef.Height;//只有这个需要求。因为宽度是固定的

            // 销毁
            g.Dispose();
            bitmap.Dispose();
            //base.UpdateWidthHeight();
             * */
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
