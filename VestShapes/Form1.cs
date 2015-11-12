using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Windows.Forms;
using System.Collections;


namespace VestShapes
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();
            userControlToolBox1.setCanvas(userControl11);
            userControl11.Zoom = 2;

            testShapeVarText();// 测试变量是否可以用
            
        }

        /// <summary>
        /// 这个方法仅仅是测试变量的
        /// </summary>
        private void testShapeVarText()
        {
            /**
            clsKeyValue myKeyValue1 = new clsKeyValue("款号", "TT111");
            clsKeyValue myKeyValue2 = new clsKeyValue("品名", "服装");

            ArrayList arrlist = new ArrayList();
            arrlist.Add(myKeyValue1);
            arrlist.Add(myKeyValue2);
            userControl11.setArrKeyValue(arrlist);
             * */


        }




        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            //textBox1.Text += "MouseMove" + Environment.NewLine;
        }



        private void userControl11_MouseMove(object sender, MouseEventArgs e)
        {
            /**
            if (isSelected)
            {

                fltEndMouseX = e.X;
                fltEndMouseY = e.Y;
                int x = (int)(fltEndMouseX - fltStartMouseX);
                int y = (int)(fltEndMouseY - StartY);

                textBox1.Text += fltEndMouseX.ToString() + Environment.NewLine;
                textBox1.Text += fltEndMouseY.ToString() + Environment.NewLine;

                textBox1.Text += x.ToString() + Environment.NewLine;
                textBox1.Text += y.ToString() + Environment.NewLine;


                this.userControl11.Location = new Point(this.userControl11.Location.X + x, this.userControl11.Location.Y + y);
                propertyGrid1.Refresh();
            }

             * */
        }

        private void userControl11_MouseUp(object sender, MouseEventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            userControl11.addShapeEle(new ShapeRect());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            userControl11.Zoom = 2f;
            userControl11.Refresh();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            userControl11.Zoom = 1f;
            userControl11.Refresh();
        }


        /// <summary>
        /// 属性更改时发生的。
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            userControl11.Refresh();
        }

        private void userControl11_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            userControl11.addShapeEle(new ShapeRect());
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            /**
            ArrayList arrlist = new ArrayList();
            PointF p1 = new PointF(10, 100);
            PointF p2 = new PointF(20, 200);
            arrlist.Add(p1);
            arrlist.Add(p2);
            arrlist.Add(new PointF(20, 200));
            arrlist.Add(new PointF(20, 100));

            PointF [] arrPoint=new  PointF[4] ;
            arrPoint[0] = new PointF(10, 100);
            arrPoint[1] = new PointF(10, 200);
            arrPoint[2] = new PointF(20, 200);
            arrPoint[3] = new PointF(20, 100);

            int intInside = ShapeEle.PtInPolygon(new PointF(5, 150), arrPoint );

            //MessageBox.Show(intInside.ToString());
            //MessageBox.Show(ShapeEle.isContaine(arrPoint, new PointF(15, 180)).ToString());


            string str = "124.89874,46.6286V124.9001,46.62973V124.90287,46.62741V124.90409,46.62783V124.90334,46.63028V124.9055,46.63044V124.90629,46.62796V124.90775,46.62792V124.90868,46.63011V124.91089,46.62931V124.90943,46.62751V124.91182,46.62706V124.91257,46.62905V124.91506,46.62812V124.91281,46.6268V124.91459,46.62619V124.91557,46.6277V124.91708,46.62586V124.91468,46.6257V124.91497,46.62477V124.91703,46.62384V124.91497,46.62261V124.91356,46.62387V124.91309,46.62194V124.91506,46.62232V124.91511,46.62081V124.91286,46.62097V124.91271,46.62029V124.91482,46.61975V124.91398,46.61833V124.91187,46.61942V124.91117,46.61872V124.91075,46.61701V124.90746,46.61714V124.90976,46.61878V124.90639,46.61868V124.90559,46.61717V124.90142,46.61772V124.90498,46.61888V124.90212,46.61946V124.90085,46.61807V124.89921,46.61952V124.90151,46.62013V124.90109,46.62152V124.89813,46.62013V124.8979,46.62223V124.90067,46.622V124.90001,46.62351V124.89663,46.62281V124.89663,46.62441V124.89917,46.62445V124.89903,46.62545V124.89588,46.62567V124.89584,46.62702V124.89903,46.62638V124.89982,46.62725";
            string[] s1 = str.Split('V');

            PointF[] ps = new PointF[s1.Length];
            for (int i = 0; i < s1.Length; i++)
            {
                string[] s2 = s1[i].Split(',');
                PointF pf = new PointF(float.Parse(s2[0]), float.Parse(s2[1]));
                ps[i] = pf;
            }

            float f1 = 0;
            float f2 = 0;

            if (ShapeEle.isContaine(ShapeEle.AngleSort(ps), new PointF(124.89874f, 46.6286f)))
            {
                MessageBox.Show("在范围内");
            }
            else
            {
                MessageBox.Show("不在范围内");
            }



             * */

        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            saveFileDialog1.DefaultExt = "shapes";
            saveFileDialog1.Title = "保存图形";
            saveFileDialog1.Filter = "shape files (*.shapes)|*.shapes|All files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                userControl11.Saver(saveFileDialog1.FileName);
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "shapes";
            openFileDialog1.Title = "保存图形";
            openFileDialog1.Filter = "shape files (*.shapes)|*.shapes|All files (*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                userControl11.Loader(openFileDialog1.FileName);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            userControl11.CtrlC();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            userControl11.CtrlV();
        }
    }
}
