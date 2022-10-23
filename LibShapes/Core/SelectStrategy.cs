using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core
{
    /// <summary>
    /// 选择策略，主要是两种，
    /// </summary>
    public  class SelectStrategy
    {
        /// <summary>
        /// 靠近在这个范围内就算选择了。
        /// </summary>
        private static float tolerance = 0.5f;

        /// <summary>
        /// 两个点是否距离足够近
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool isNear(PointF p1, PointF p2)
        {
            return Utils.DistanceCalculation.distance(p1, p2) <= tolerance;
        }

        /// <summary>
        /// 一个点跟一个线段是否靠的非常近。
        /// </summary>
        /// <param name="p0"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static bool isNear(PointF p0, PointF p1, PointF p2)
        {
            return Utils.DistanceCalculation.pointToLine(p0, p1, p2) <= tolerance;
        }
    }
}
