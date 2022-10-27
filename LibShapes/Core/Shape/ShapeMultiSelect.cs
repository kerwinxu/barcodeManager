using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Io.Github.Kerwinxu.LibShapes.Core.Shape
{
    /// <summary>
    /// 选择了多个形状后，就放在这里边。
    /// </summary>
    public class ShapeMultiSelect: ShapeMulti
    {
        public override ShapeEle DeepClone()
        {
            // 首先组建一个新的
            ShapeMultiSelect group = new ShapeMultiSelect();
            if (shapes != null)
            {
                foreach (var item in shapes)
                {
                    group.shapes.Add(item.DeepClone());
                }
            }
            return group;

            //return base.DeepClone();
        }

    }
}
