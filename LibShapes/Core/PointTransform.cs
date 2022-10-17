using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core
{
    /// <summary>
    /// 这个是虚拟坐标跟画布上的坐标转换的
    /// Offset 是偏移，而Zoom是放大倍数
    /// </summary>
    public class PointTransform
    {
        public float OffsetX { get; set; }

        public float OffsetY { get; set; }

        private float _zoom=1; // 默认值1

        public float Zoom
        {
            get { return _zoom; }
            set { _zoom= value; if (value <= 0) _zoom = 1; } // 如果小于等于0，就用默认值1吧。
        }

        /// <summary>
        /// 这个转成画布的坐标
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public PointF CanvasToVirtualPoint(PointF pointF)
        {
            return new PointF() {
                X = pointF.X / Zoom - OffsetX,          // 这个是加偏移
                Y = pointF.Y / Zoom - OffsetY
            };
        }

        /// <summary>
        /// 这个转成虚拟的坐标
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public PointF VirtualToCanvasPoint(PointF pointF)
        {
            return new PointF()
            {
                X = (pointF.X + OffsetX) * Zoom,      // 这个是减去偏移
                Y = (pointF.Y + OffsetY) * Zoom
            };
        }


        public Matrix GetMatrix()
        {
            Matrix matrix = new Matrix();
            matrix.Translate(OffsetX, OffsetY);
            matrix.Scale(Zoom, Zoom, MatrixOrder.Append);
            return matrix;
        }

    }
}
