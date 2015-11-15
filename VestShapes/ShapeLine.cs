using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace VestShapes
{
    [Serializable]
    //[ProtoContract]
    public class ShapeLine : ShapeEle
    {
        protected float _X2, _Y2;//直线是两个点的，这个是另一个点

        protected float _X2Add, _Y2Add;

        [CategoryAttribute("布局")]
        [XmlElement]
        public float X2
        {
            get
            {
                return _X2 / _Zoom;
            }
            set
            {
                _X2 = value * _Zoom;
            }
        }

        [CategoryAttribute("布局")]
        [XmlElement]
        public float Y2
        {
            get
            {
                return _Y2 / _Zoom;
            }
            set
            {
                _Y2 = value * _Zoom;
            }
        }


        //重新定义宽度和高度
        public override float Width
        {
            get
            {
                return Math.Abs(X - X2);
            }

        }
        public override float Height
        {
            get
            {
                return Math.Abs(Y - Y2);
            }

        }


        [XmlElement]
        public override float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                //因为更改这个放大倍数，很多东西都要更改，首先保存相关信息
                float fltX, fltY, fltX2, fltY2;

                fltX = X;
                fltY = Y;
                fltX2 = X2;
                fltY2 = Y2;

                //再设置放大率
                base.Zoom = value;


                //再重新计算相关的XYWH。
                X = fltX;
                Y = fltY;
                X2 = fltX2;
                Y2 = fltY2;

            }

        }

        public override ShapeEle DeepClone()
        {
            ShapeLine shapeEle = new ShapeLine();
            shapeEle.Zoom = Zoom;
            shapeEle.X = X;
            shapeEle.Y = Y;
            shapeEle.Width = Width;
            shapeEle.Height = Height;
            shapeEle.isFill = isFill;
            shapeEle.PenColor = PenColor;
            shapeEle.PenWidth = PenWidth;
            shapeEle.PenDashStyle = PenDashStyle;
            shapeEle.Route = Route;

            //如下是子类单独的
            shapeEle.X2 = X2;
            shapeEle.Y2 = Y2;

            return shapeEle;
            //throw new NotImplementedException();
        }

        public override void ShapeInit(PointF p1, PointF p2)
        {
            _X = p1.X;
            _Y = p1.Y;
            _X2 = p2.X;
            _Y2 = p2.Y;
            //base.ShapeInit(p1, p2);
        }

        public override GraphicsPath getGraphicsPathNoOffsetRoute()
        {
            //添加线段
            GraphicsPath path = new GraphicsPath();
            path.AddLine(new PointF(_X + _XAdd, _Y + _YAdd), new PointF(_X2 + _X2Add, _Y2 + _Y2Add));

            return path;
            //return base.getGraphicsPath();
        }

        public override PointF getCentrePoint()
        {
            return new PointF((_X + _XAdd + _X2 + _X2Add) / 2, (_Y + _YAdd + _Y2 + _Y2Add) / 2);
            //return base.getCentrePoint();
        }

        public override void Draw(Graphics g)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            Pen _myPen = new Pen(PenColor, _penWidth);
            _myPen.DashStyle = PenDashStyle;
            g.DrawLine(_myPen, new PointF(_X + _XAdd, _Y + _YAdd), new PointF(_X2 + _X2Add, _Y2 + _Y2Add));
            //throw new NotImplementedException();
        }

        public override PointF[] getRealPoint()
        {
            PointF[] arrPoint = new PointF[2];

            PointF p1 = new PointF(_X + _XAdd, _Y + _YAdd);
            PointF p2 = new PointF(_X2 + _X2Add, _Y2 + _Y2Add);


            double dblRoute = _route + _routeAdd;

            //求中心点
            PointF pointCenter = getCentrePoint();

            //注意这里是顺时针
            arrPoint[0] = PointRotate(pointCenter, p1, dblRoute);
            arrPoint[1] = PointRotate(pointCenter, p2, dblRoute);

            return arrPoint;
            //return base.getRealPoint();
        }

        public override void Redim(string strState, PointF startPointf, PointF endPointf)
        {
            //也是因为这个是一个线段，所以在选择框中，他肯定占用的是选择框四个节点的两个

            PointF[] arrPAdd = new PointF[2];

            switch (strState)
            {
                case "move":
                    this.Move(new PointF(endPointf.X - startPointf.X, endPointf.Y - startPointf.Y));
                    break;
                case "West":
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }
                    break;
                case "East":
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "North":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    break;
                case "South":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    break;
                case "NorthEast":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "SouthWest":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "SouthEast":
                    if (_Y2 > _Y)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X2 > _X)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;
                case "NorthWest":
                    if (_Y > _Y2)
                    {
                        _Y2Add = endPointf.Y - startPointf.Y;
                    }
                    else
                    {
                        _YAdd = endPointf.Y - startPointf.Y;
                    }
                    if (_X > _X2)
                    {
                        _X2Add = endPointf.X - startPointf.X;
                    }
                    else
                    {
                        _XAdd = endPointf.X - startPointf.X;
                    }

                    break;

                default:
                    break;
            }




            //base.Redim(strState, startPointf, endPointf);
        }

        /// <summary>
        /// 线段的移动就是两个点同时移动 
        /// </summary>
        /// <param name="pointF"></param>
        public override void Move(PointF pointF)
        {
            _XAdd = pointF.X;
            _X2Add = pointF.X;
            _YAdd = pointF.Y;
            _Y2Add = pointF.Y;
            //base.Move(pointF);
        }

        public override void ReInit()
        {
            _X += _XAdd;
            _Y += _YAdd;
            _X2 += _X2Add;
            _Y2 += _Y2Add;

            _XAdd = 0;
            _YAdd = 0;
            _X2Add = 0;
            _Y2Add = 0;
            //base.ReInit();
        }

        /**

        public override bool isContains(PointF MousePoint)
        {
            //线段判断跟矩形的判断不一样。
            //两个点有变化
            PointF[] pointfReal = getRealPoint();

            //直线的选择是判断点到线段的距离

            float fltDistance = getPointLineDistance(pointfReal[0], pointfReal[1], MousePoint);

            //如果这个距离小于精度
            if (fltDistance <= fltJingDu)
                return true;

            return false;

            //return base.isContains(MousePoint);
        }
         * */


    }//end class
}
