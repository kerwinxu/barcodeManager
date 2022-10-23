using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Io.Github.Kerwinxu.LibShapes.Core.State
{
    public abstract class State
    {
        /// <summary>
        /// 画布
        /// </summary>
        public UserControlCanvas canvas { get; set; }

        public PointF startPoint { get; set; }


        public State(UserControlCanvas canvas, PointF start_pointF)
        {
            this.canvas = canvas;
            this.startPoint = start_pointF;
        }

        public State(UserControlCanvas canvas)
        {
            this.canvas = canvas;
        }

        public virtual void LeftMouseDown(PointF pointF) { }
        public virtual void LeftMouseMove(PointF pointF) { }
        public virtual void LeftMouseUp(PointF pointF) {}

        public virtual void RightMouseClick(PointF pointF) { }

    }
}
