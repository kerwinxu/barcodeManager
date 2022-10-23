using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Paper
{
    /// <summary>
    /// 纸张尺寸的
    /// </summary>
    public class Paper
    {
        /// <summary>
        /// 纸张宽度
        /// </summary>
        [DescriptionAttribute("纸张宽度"), DisplayName("纸张宽度"), CategoryAttribute("纸张")]
        public float PaperWidth { get; set; }

        /// <summary>
        /// 纸张高度
        /// </summary>
        [DescriptionAttribute("纸张高度"), DisplayName("纸张高度"), CategoryAttribute("纸张")]
        public float PaperHeight { get; set; }
        /// <summary>
        /// 上边距
        /// </summary>
        [DescriptionAttribute("上边距"), DisplayName("上边距"), CategoryAttribute("边距")]
        public float Top { get; set; }
        /// <summary>
        /// 左边距
        /// </summary>
        [DescriptionAttribute("左边距"), DisplayName("左边距"), CategoryAttribute("边距")]
        public float Left { get; set; }

        /// <summary>
        /// 右边距
        /// </summary>
        [DescriptionAttribute("右边距"), DisplayName("右边距"), CategoryAttribute("边距")]
        public float Right { get; set; }

        /// <summary>
        /// 下边距
        /// </summary>
        [DescriptionAttribute("下边距"), DisplayName("下边距"), CategoryAttribute("边距")]
        public float Bottom { get; set; }

        /// <summary>
        /// 行数
        /// </summary>
        [DescriptionAttribute("一张纸上的模板的行数"), DisplayName("行数"), CategoryAttribute("模板")]
        public int Rows { get; set; }

        /// <summary>
        /// 列数
        /// </summary>
        [DescriptionAttribute("一张纸上的模板的列数"), DisplayName("列数"), CategoryAttribute("模板")]
        public int Cols { get; set; }

        /// <summary>
        /// 水平间隔
        /// </summary>
        [DescriptionAttribute("模板之间的水平间隔"), DisplayName("水平间隔"), CategoryAttribute("模板")]
        public float HorizontalIntervalDistance { get; set; }

        /// <summary>
        /// 竖直间隔
        /// </summary>
        [DescriptionAttribute("模板之间的竖直间隔"), DisplayName("竖直间隔"), CategoryAttribute("模板")]
        public float VerticalIntervalDistance { get; set; }

        /// <summary>
        /// 模板宽度
        /// </summary>
        [DescriptionAttribute("模板之间的模板宽度"), DisplayName("模板宽度"), CategoryAttribute("模板")]
        public float ModelWidth { get; set; }

        /// <summary>
        /// 模板高度
        /// </summary>
        [DescriptionAttribute("模板之间的模板高度"), DisplayName("模板高度"), CategoryAttribute("模板")]
        public float ModelHeight { get; set; }

        /// <summary>
        /// 横向打印
        /// </summary>
        [DescriptionAttribute("如果页面应横向打印，则为 true；反之，则为 false。 默认值由打印机决定。"), DisplayName("横向打印"), CategoryAttribute("打印机")]
        public bool Landscape { get; set; }

        /// <summary>
        /// 横向方向的角度
        /// </summary>
        [DescriptionAttribute("有效的旋转值为 90 度和 270 度。 如果不支持横向，则唯一有效的旋转值为 0 度"), DisplayName("横向方向的角度"), CategoryAttribute("打印机")]
        public int LandscapeAngle { get; set; }

        /// <summary>
        /// 模板的形状
        /// </summary>
        [Browsable(false)]//不在PropertyGrid上显示
        public ShapeEle ModelShape;

        //public void createModelShape()
        //{
        //    // 这个是生成一个特殊的
        //    ModelShape = new ShapeRectangle()
        //    {
        //        X = 0,
        //        Y = 0,
        //        Width = ModelWidth,
        //        Height = ModelHeight,
        //        IsFill = true,        // 填充
        //        FillColor = Color.White,  // 填充白色。
        //    };
        //}

        public Paper()
        {
            //createModelShape(); // 默认创建一个空白的。
        }

    }
}
