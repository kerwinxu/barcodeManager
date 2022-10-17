using Io.Github.Kerwinxu.LibShapes.Core.Shape;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core
{
    /// <summary>
    /// 纸张尺寸的
    /// </summary>
    public class Paper
    {
        [DescriptionAttribute("纸张宽度"), DisplayName("纸张宽度"), CategoryAttribute("布局")]
        public float PaperWidth { get; set; }

        [DescriptionAttribute("纸张高度"), DisplayName("纸张高度"), CategoryAttribute("布局")]
        public float PaperHeight { get; set; }

        [DescriptionAttribute("上边距"), DisplayName("上边距"), CategoryAttribute("布局")]
        public float Top { get; set; }

        [DescriptionAttribute("左边距"), DisplayName("左边距"), CategoryAttribute("布局")]
        public float Left { get; set; }

        [DescriptionAttribute("右边距"), DisplayName("右边距"), CategoryAttribute("布局")]
        public float Right { get; set; }

        [DescriptionAttribute("下边距"), DisplayName("下边距"), CategoryAttribute("布局")]
        public float Bottom { get; set; }

        [DescriptionAttribute("行数"), DisplayName("行数"), CategoryAttribute("布局")]
        public int Rows { get; set; }

        [DescriptionAttribute("列数"), DisplayName("列数"), CategoryAttribute("布局")]
        public int Cols { get; set; }

        [DescriptionAttribute("水平间隔"), DisplayName("水平间隔"), CategoryAttribute("布局")]
        public float HorizontalIntervalDistance { get; set; }

        [DescriptionAttribute("竖直间隔"), DisplayName("竖直间隔"), CategoryAttribute("布局")]
        public float VerticalIntervalDistance { get; set; }

        [DescriptionAttribute("模板宽度"), DisplayName("模板宽度"), CategoryAttribute("布局")]
        public float ModelWidth { get; set; }

        [DescriptionAttribute("模板高度"), DisplayName("模板高度"), CategoryAttribute("布局")]
        public float ModelHeight { get; set; }

        [Browsable(false)]//不在PropertyGrid上显示
        public ShapeEle ModelShape;
        public void createModelShape()
        {
            // 这个是生成一个特殊的
            ModelShape = new ShapeRectangle()
            {
                X = 0,
                Y = 0,
                Width = ModelWidth,
                Height = ModelHeight,
                IsFill = true,        // 填充
                FillColor = Color.White  // 填充白色。
            };
        }

        public Paper()
        {
            createModelShape(); // 默认创建一个空白的。
        }

    }
}
