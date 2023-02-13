using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Utils
{
    /// <summary>
    /// 距离计算的类，
    /// </summary>
    public  class DistanceCalculation
    {
        /// <summary>
        /// 选择的容忍度
        /// </summary>
        public static float select_tolerance = 4;

        /// <summary>
        /// 两点之间的距离
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static float distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        /// <summary>
        /// 点到线段的距离
        /// </summary>
        /// <param name="p0">点</param>
        /// <param name="p1">线段的点1</param>
        /// <param name="p2">线段的点2</param>
        /// <returns></returns>
        public static float pointToLine(PointF p0, PointF p1, PointF p2)
        {
   
            float a = distance(p1, p2); // 求出这个线段的长度
            float b = distance(p0, p1); // 这个点跟线段的点1的长度
            float c = distance(p0, p2); // 这个点跟线段的点2的长度
            // 这里分几种情况
            if (c*c >= a*a + b*b)
            {
                // 如果这个特别的长，那么这个点就不考虑距离了
                return b;
            }else if(b*b >= a * a + c * c)
            {
                // 同理
                return c;
            }
            else
            {
                float p = (a + b + c) / 2; // 半周长
                double s = Math.Sqrt(p * (p - a) * (p - b) * (p - c)); // 海伦公式求面积
                return (float)(2 * s / a); // 返回点到线的距离
            }

        }
    }
}
