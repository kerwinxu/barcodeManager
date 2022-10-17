using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public class StateStandby : State
    {

        public StateStandby(UserControlCanvas canvas, PointF start_pointF):base(canvas,start_pointF)
        {
            
        }

        public StateStandby(UserControlCanvas canvas) : base(canvas)
        {

        }

        public override void LeftMouseDown(MouseEventArgs e)
        {
            // 首先看看是否有选择的图形


            //base.LeftMouseDown(e);
        }

    }
}
