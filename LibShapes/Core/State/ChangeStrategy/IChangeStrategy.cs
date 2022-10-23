using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.State.ChangeStrategy
{
    /// <summary>
    /// 更改策略
    /// </summary>
    interface IChangeStrategy
    {
        /// <summary>
        /// 是否合适这个策略
        /// </summary>
        /// <param name="pointFs"></param>
        /// <param name="start_pointF"></param>
        /// <returns></returns>
        bool isRight(PointF [] pointFs, PointF start_pointF);


        /// <summary>
        /// 这个策略的执行，
        /// </summary>
        /// <param name="shape"></param>
        void action(ShapeEle shape, PointF start_pointF, PointF end_pointF);
    }
}
