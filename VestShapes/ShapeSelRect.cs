using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Collections;

namespace VestShapes
{

    [Serializable]
    //[ProtoContract]
    public class ShapeSelRect : ShapeGroup
    {
        //这个类好像不用关心放大倍数问题

        //我将已经选择的形状也放在这里，因为这个形状就是选择框
        /**
        [XmlArray(ElementName = "arrlistShapeEle")]
        [XmlArrayItem(Type = typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type = typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode))
        ]
        //public ArrayList arrlistSelShapes = new ArrayList();//已经被选择的形状。
         * */

        public ShapeSelRect(ArrayList arrlistShape)
        {
            //这样赋值才避免因为操作这个arrlistShapeEle而引发arrlistShape问题
            arrlistShapeEle = new ArrayList();
            foreach (ShapeEle item in arrlistShape)
            {
                arrlistShapeEle.Add(item);
            }

            SetSelRectXYWH();

        }
        /// <summary>
        /// 这个的选中只是选中最外边的边框就算选中了
        /// </summary>
        /// <param name="MousePoint"></param>
        /// <returns></returns>
        public override bool isContains(PointF MousePoint)
        {
            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(getGraphicsPath().GetBounds());
            return path.IsVisible(MousePoint);
            //return base.isContains(MousePoint);
        }

        /// <summary>
        /// 改变
        /// </summary>
        /// <param name="strState"></param>
        /// <param name="startPointf"></param>
        /// <param name="endPointf"></param>
        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            if (Count() > 0)
            {
                if (!UserControlCanvas.isAlignGridding)
                {
                    //如果不对齐，就仅仅需要如下传递参数就可以了。
                    foreach (ShapeEle item in arrlistShapeEle)
                    {
                        item.Redim(strState, startPointf, endPointf);

                    }
                }
                else
                {
                    //如果对齐，就得判断了

                    //如下是取得原先的XYWH，不加上ADD的那个。
                    float fltStartShapeElesX = 0;
                    float fltStartShapeElesY = 0;
                    float fltStartShapeElesW = 0;
                    float fltStartShapeElesH = 0;

                    if (Count() > 0)
                    {
                        ArrayList arrlistX = new ArrayList();
                        ArrayList arrlistY = new ArrayList();

                        foreach (ShapeEle item in arrlistShapeEle)
                        {
                            arrlistX.Add(item.X);
                            arrlistY.Add(item.Y);
                            arrlistX.Add(item.X + item.Width);
                            arrlistY.Add(item.Y + item.Height);


                        }

                        arrlistX.Sort();
                        arrlistY.Sort();

                        fltStartShapeElesX = Convert.ToSingle(arrlistX[0].ToString()) * Zoom;
                        fltStartShapeElesY = Convert.ToSingle(arrlistY[0].ToString()) * Zoom;
                        fltStartShapeElesW = (Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString())) * Zoom;//这个获得宽度加上X的数值
                        fltStartShapeElesH = (Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString())) * Zoom;//这个是获得高度加上Y的数值
                    }

                    float fltStartMouseX = startPointf.X;
                    float fltStartMouseY = startPointf.Y;
                    float fltEndMouseX = endPointf.X;
                    float fltEndMouseY = endPointf.Y;

                    float fx1 = (float)Math.Round((fltStartShapeElesX + fltEndMouseX - fltStartMouseX) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesX;
                    float fy1 = (float)Math.Round((fltStartShapeElesY + fltEndMouseY - fltStartMouseY) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesY;
                    float fw1 = (float)Math.Round((fltStartShapeElesW + fltEndMouseX - fltStartMouseX) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesW;
                    float fh1 = (float)Math.Round((fltStartShapeElesH + fltEndMouseY - fltStartMouseY) / UserControlCanvas.GriddingInterval / Zoom, 0) * UserControlCanvas.GriddingInterval * Zoom - fltStartShapeElesH;


                    RectangleF rect = new RectangleF();

                    switch (strState)
                    {
                        case "move":
                            this.Move(new PointF(fx1, fy1));
                            break;
                        case "West":
                            rect.X = fx1;
                            rect.Width = -fx1;
                            this.Resize(rect);
                            break;
                        case "East":
                            rect.Width = fw1;
                            this.Resize(rect);
                            break;
                        case "North":
                            rect.Y = fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;
                        case "South":
                            rect.Height = fh1;
                            this.Resize(rect);
                            break;
                        case "NorthEast":
                            rect.Width = fw1;
                            rect.Y = fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;
                        case "SouthWest":
                            rect.X = fx1;
                            rect.Width = -fx1;
                            rect.Height = fy1;
                            this.Resize(rect);
                            break;
                        case "SouthEast":
                            rect.Width = fw1;
                            rect.Height = fh1;
                            this.Resize(rect);
                            break;
                        case "NorthWest":
                            rect.X = fx1;
                            rect.Width = -fx1;
                            rect.Y = fy1;
                            rect.Height = -fy1;
                            this.Resize(rect);
                            break;

                        default:
                            break;
                    }

                    /**备份
                     * 
                    switch (strState)
                    {
                        case "move":
                            this.Move(new PointF(fx1, fy1));
                            break;
                        case "West":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            this.Resize(rect);
                            break;
                        case "East":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            this.Resize(rect);
                            break;
                        case "North":
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;
                        case "South":
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "NorthEast":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;
                        case "SouthWest":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "SouthEast":
                            rect.Width = fltEndMouseX - fltStartMouseX;
                            rect.Height = fltEndMouseY - fltStartMouseY;
                            this.Resize(rect);
                            break;
                        case "NorthWest":
                            rect.X = fltEndMouseX - fltStartMouseX;
                            rect.Width = fltStartMouseX - fltEndMouseX;
                            rect.Y = fltEndMouseY - fltStartMouseY;
                            rect.Height = fltStartMouseY - fltEndMouseY;
                            this.Resize(rect);
                            break;

                        default:
                            break;
                     * */




                }

            }
            //base.Redim(strState, startPointf, endPointf);
        }

        /**
        /// <summary>
        /// 移动，需要考虑是否对齐
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            if (UserControlCanvas.isAlignGridding)
            {
                _XAdd += (X + pointF.X) / UserControlCanvas.GriddingInterval * UserControlCanvas.GriddingInterval -X;
                _YAdd += (Y + pointF.Y) / UserControlCanvas.GriddingInterval * UserControlCanvas.GriddingInterval -Y;

                base.Move(new PointF(_XAdd, _YAdd));

            }
            else  
            {
                //如果没有选择对齐，那么就直接用这个参数调用父类的移动。
                base.Move(pointF);
            }   
        }

         * */


        /// <summary>
        /// 默认的构造函数  //这个的构造函数就是定义画笔的
        /// </summary>
        public ShapeSelRect()
            : base()
        {
            PenColor = Color.Red;
            PenWidth = 1;
            PenDashStyle = DashStyle.Dot;

        }

        public override void Draw(Graphics g, List<Matrix> listMatrix)
        {

            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;
            //定义画笔
            Pen _myPen = new Pen(PenColor, 1f);//这个选择框画笔固定宽度是
            _myPen.DashStyle = PenDashStyle;

            GraphicsPath path = new GraphicsPath();
            path.AddRectangle(getGraphicsPath(listMatrix).GetBounds());


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

            //base.Draw(g, arrlistMatrix);
        }

        /**
        /// <summary>
        /// 设置选择框的四周，因为选择的图形不一定一个，所以需要这个设置，已有如果需要添加选择或者删除选择的时候，再一次用这个函数就可以了。
        /// 
        /// </summary>
        /// <param name="arrlistShape"></param>
        public void SetSelRectXYWH()
        {
            //首先初始化
            _X = 0;
            _Y = 0;
            _Width = 0;
            _Height = 0;

            if (Count() > 0)
            {
                ArrayList arrlistX = new ArrayList();
                ArrayList arrlistY = new ArrayList();

                foreach (ShapeEle item in arrlistSelShapes)
                {
                    //将这个所有点的坐标加到这里边去
                    foreach (PointF p1 in item.getRealPoint())
                    {
                        arrlistX.Add(p1.X);
                        arrlistY.Add(p1.Y);
                    }

                }

                arrlistX.Sort();
                arrlistY.Sort();

                _X = Convert.ToSingle(arrlistX[0].ToString());
                _Y = Convert.ToSingle(arrlistY[0].ToString());
                _Width = Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString()) - _X;
                _Height = Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString()) - _Y;
            }

        }

         * */



        /// <summary>
        /// 根据一个点来返回选择的形状
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public ShapeEle getSelectShapeEle(PointF pointF)
        {


            //判断是否在其中一个的图形内。
            if ((arrlistShapeEle != null) && (arrlistShapeEle.Count > 0))
            {
                for (int i = arrlistShapeEle.Count - 1; i > -1; i--)//从最后的判断
                {
                    //只所以需要判断而不是直接设置是为了以后多选的情况，比如说按ctrl 键，如果在这里直接设置的话容易覆盖原先已经选择的。
                    bool isS = ((ShapeEle)arrlistShapeEle[i]).isContains(new PointF(pointF.X, pointF.Y));
                    if (isS)
                    {
                        ((ShapeEle)arrlistShapeEle[i]).isSelect = true;
                        return (ShapeEle)arrlistShapeEle[i];//这个点只判断最上边的图形就可以了。
                    }

                }

            }

            return null;//返回选择的图形，如果没有，里边就没有数据
        }

        /**
        /// <summary>
        /// 这个方法仅仅是返回已经选择的形状的个数
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            if (arrlistSelShapes != null)
                return arrlistSelShapes.Count;
            return 0;
        }
         * */
        /**
        public override void ReInit()
        {
            if (arrlistSelShapes != null)
            {
                foreach (ShapeEle item in arrlistSelShapes)
                {
                    item.ReInit();

                }

            }
            //他也要移动
            base.ReInit();

        }

        public void addShapeEle(ShapeEle shapeEle)
        {
            if (arrlistSelShapes != null)
                arrlistSelShapes.Add(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void addShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistSelShapes.Add(item);

            }
            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEle(ShapeEle shapeEle)
        {
            if (arrlistSelShapes != null)
                arrlistSelShapes.Remove(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistSelShapes.Remove(arrlistSel);

            }
            SetSelRectXYWH();//重新计算
        }

         * */


        /// <summary>
        /// 取消所有选择
        /// </summary>

        public void cancelAllSelect()
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Clear();
            //还有XYWH清零
            X = 0;
            Y = 0;
            Width = 0;
            Height = 0;
        }

        /// <summary>
        /// 这个方法就是当用户鼠标点在这个更改框范围内的时候，判断是在什么位置。
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public string strOver(PointF pointF)
        {
            //默认是移动
            string strFangxiang = "move";//

            //首先判断是否是正东南西北

            if (Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)
                strFangxiang = "West";
            if (Math.Abs((_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)
                strFangxiang = "East";
            if (Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)
                strFangxiang = "North";
            if (Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)
                strFangxiang = "South";

            //再判断是不是东南方向之类的
            if (((Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "NorthEast";
            if (((Math.Abs((_Y + _YAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "NorthWest";
            if (((Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)) && (((Math.Abs(_X + _XAdd + _Width + _WidthAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "SouthEast";
            if (((Math.Abs((_Y + _YAdd + _Height + _HeightAdd) - pointF.Y) < fltJingDu)) && ((Math.Abs((_X + _XAdd) - pointF.X) < fltJingDu)))
                strFangxiang = "SouthWest";

            return strFangxiang;
        }

        //public  enum enumFangXiang { North=1 , South , East , West , NorthEast , SouthEast , SouthWest , NorthWest ,Inside};




    }
}
