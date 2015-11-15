using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{
    /// <summary>
    /// 这个组合图形实质上就是将几个图形组合起来。但这个类只是保存信息的，操作的部分是归画布的。
    /// 从各方面来将，这个继承自 ShapeSelRect 比较好些。省了拷贝一大堆代码，但这个是不显示边框的，所以多态掉Draw。
    /// </summary>
    [Serializable]
    //[ProtoContract]
    public class ShapeGroup : ShapeEle
    {
        [XmlArray]
        [XmlArrayItem(Type = typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type = typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode)),
        XmlArrayItem(Type = typeof(ShapeRoundRect)),
        XmlArrayItem(Type = typeof(ShapeGroup)),
        XmlArrayItem(Type = typeof(shapeSingleText)),
        XmlArrayItem(Type = typeof(shapeMultiText))

        ]
        public ArrayList arrlistShapeEle = new ArrayList();


        public ShapeGroup()
        {

        }

        /**如下会出现错误，因为在群组中，也是有旋转的
        public override bool isContains(PointF MousePoint)
        {
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    if (item.isContains(MousePoint))
                    {
                        return true;//只要有一个返回真就可以了。
                    }
                }

            }

            return false;
            //return base.isContains(MousePoint);
        }
         * */
        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            GraphicsPath path = new GraphicsPath();
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    try
                    {
                        path.AddPath(item.getGraphicsPath(), true);//这个群组不便宜旋转的是内部旋转和偏移的。 
                    }
                    catch (Exception ex)
                    {
                        ////ClsErrorFile.WriteLine("这个不明原因", ex);
                        //throw;
                    }

                }

            }
            return path;
            //return base.getGraphicsPathNoOffsetRoute();
        }



        public ShapeGroup(ArrayList arrlist)
        {
            foreach (ShapeEle item in arrlist)
            {
                arrlistShapeEle.Add(item);

            }
            SetSelRectXYWH();
        }

        public override bool updateVarValue(ArrayList arrlistKeyValue)
        {
            bool isS = false;
            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    if (item.updateVarValue(arrlistKeyValue))
                    {
                        isS = true;
                    }

                }

            }
            return isS;
            //return base.updateVarValue(arrlistKeyValue);
        }

        /**
        public override GraphicsPath getGraphicsPath()
        {
            GraphicsPath path = new GraphicsPath();

            if (Count()>0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    try
                    {
                        path.AddPath(item.getGraphicsPath(), false);
                    }
                    catch (Exception ex)
                    {
                        //ClsErrorFile.WriteLine(ex);
                        //throw;
                    }
                    
                }

                //做一个变换矩阵加上偏移和旋转
                System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
                path.Transform(m);//应用变换矩阵
                
            }

            return path;
        }

         * */

        public override void Draw(Graphics g, ArrayList arrlistMatrix)
        {

            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    ArrayList arr2 = new ArrayList();

                    for (int i = 0; i < arrlistMatrix.Count; i++)
                    {
                        arr2.Add(((System.Drawing.Drawing2D.Matrix)arrlistMatrix[i]).Clone());
                    }

                    //加上这个群组的旋转
                    System.Drawing.Drawing2D.Matrix m = new System.Drawing.Drawing2D.Matrix();
                    //g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                    m.RotateAt((float)(_route + _routeAdd), getCentrePoint());
                    arr2.Add(m);//加上群组的旋转
                    item.Draw(g, arr2);
                }

                //如果这个有填充就填充
                if (isFill)
                {
                    try
                    {
                        GraphicsPath path = getGraphicsPath();
                        path.CloseAllFigures();

                        for (int i = 0; i < arrlistMatrix.Count; i++)
                        {
                            path.Transform(((System.Drawing.Drawing2D.Matrix)arrlistMatrix[i]));
                        }

                        g.FillPath(new SolidBrush(_FillColor), path);

                    }
                    catch (Exception ex)
                    {
                        ////ClsErrorFile.WriteLine(ex);
                        //throw;
                    }

                }

            }

            //base.Draw(g, arrlistMatrix);
        }

        public override void Draw(Graphics g, float fltKongX, float fltKongY)
        {

            if (Count() > 0)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {

                    //如下是群组的角度
                    if (Route != 0)
                    {

                        RectangleF rect = getRect();
                        //如下的这个是偏移些位置
                        g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                        g.TranslateTransform(rect.X + rect.Width / 2, rect.Y + rect.Height / 2, MatrixOrder.Prepend);
                        g.RotateTransform((float)Route);
                        g.TranslateTransform(-rect.X, -rect.Y);
                        g.TranslateTransform(-fltKongX - rect.Width / 2, -fltKongY - rect.Height / 2);
                    }
                    item.Draw(g, fltKongX, fltKongY);
                    //g.ResetTransform();//恢复原先的坐标系。
                }


            }
            SetSelRectXYWH();//仅仅计算就可以了。

        }

        public override ShapeEle DeepClone()
        {
            ArrayList arrlist = new ArrayList();

            foreach (ShapeEle item in arrlistShapeEle)
            {
                arrlist.Add(item.DeepClone());
            }


            ShapeGroup shapeEle = new ShapeGroup(arrlist);

            return shapeEle;

            //throw new NotImplementedException();
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
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Redim(strState, startPointf, endPointf);

                }

            }
            //base.Redim(strState, startPointf, endPointf);
        }


        /// <summary>
        /// 改变大小
        /// </summary>
        /// <param name="rect"></param>
        public override void Resize(RectangleF rect)
        {
            //首先判断是否已经选择图形

            if (Count() > 0)
            {

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Resize(rect);
                }

            }

            SetSelRectXYWH();
            //base.Resize(rect);
        }

        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            //首先判断是否已经选择图形

            if (Count() > 0)
            {

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Move(pointF);
                }

            }

            SetSelRectXYWH();

            //base.Move(pointF);
        }




        /// <summary>
        /// 设置选择框的四周，因为选择的图形不一定一个，所以需要这个设置，已有如果需要添加选择或者删除选择的时候，再一次用这个函数就可以了。
        /// 
        /// </summary>
        /// <param name="arrlistShape"></param>
        public void SetSelRectXYWH()
        {

            RectangleF rect = getGraphicsPath().GetBounds();//取得边框
            _X = rect.X;
            _Y = rect.Y;
            _Width = rect.Width;
            _Height = rect.Height;
            return;

            #region 如下的不需要了，仅仅为了增加破解难度


            //首先初始化
            _X = 0;
            _Y = 0;
            _Width = 0;
            _Height = 0;

            if (Count() > 0)
            {
                ArrayList arrlistX = new ArrayList();
                ArrayList arrlistY = new ArrayList();

                foreach (ShapeEle item in arrlistShapeEle)
                {
                    //将这个所有点的坐标加到这里边去
                    foreach (PointF p1 in item.getRealPoint())
                    {
                        arrlistX.Add(p1.X);
                        arrlistY.Add(p1.Y);
                    }

                    /**
                    arrlistX.Add(item.getRealX() + item.getXAdd());
                    arrlistX.Add(item.getRealX() + item.getXAdd() + item.getRealWidth() + item.getWidthAdd());
                    arrlistY.Add(item.getRealY() + item.getYAdd());
                    arrlistY.Add(item.getRealY() + item.getYAdd() + item.getRealHeight() + item.getHeightAdd());
                     * */
                }

                arrlistX.Sort();
                arrlistY.Sort();

                _X = Convert.ToSingle(arrlistX[0].ToString());
                _Y = Convert.ToSingle(arrlistY[0].ToString());
                _Width = Convert.ToSingle(arrlistX[arrlistX.Count - 1].ToString()) - _X;
                _Height = Convert.ToSingle(arrlistY[arrlistY.Count - 1].ToString()) - _Y;
            }

            #endregion

        }


        public override void ReInit()
        {
            if (arrlistShapeEle != null)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.ReInit();

                }

            }
            //他也要移动
            base.ReInit();

            SetSelRectXYWH();
        }

        public void addShapeEle(ShapeEle shapeEle)
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Add(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void addShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistShapeEle.Add(item);

            }
            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEle(ShapeEle shapeEle)
        {
            if (arrlistShapeEle != null)
                arrlistShapeEle.Remove(shapeEle);

            SetSelRectXYWH();//重新计算
        }

        public void removeShapeEles(ArrayList arrlistSel)
        {
            foreach (ShapeEle item in arrlistSel)
            {
                arrlistShapeEle.Remove(arrlistSel);

            }
            SetSelRectXYWH();//重新计算
        }


        /// <summary>
        /// 这个方法仅仅是返回已经选择的形状的个数
        /// </summary>
        /// <returns></returns>
        /// 
        public int Count()
        {
            if (arrlistShapeEle != null)
                return arrlistShapeEle.Count;
            return 0;
        }

        [XmlElement]
        public override float Zoom
        {
            get
            {
                return base.Zoom;
            }
            set
            {
                //要迭代这个群组内的所有形状。如果这个群组中包含群组，那么这个群组也会迭代的。
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    item.Zoom = value;

                }

                base.Zoom = value;
            }
        }

    }

}
