using System;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Xml;
using System.Xml.Serialization;

using Xuhengxiao.MyDataStructure;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
//using ProtoBuf;

namespace VestShapes
{
    /// <summary>
    /// 这个类是用来封装图形的，内部还有一个类的信息是纸张信息，
    /// </summary>
    [Serializable]
    ////[ProtoContract]
    public  class Shapes
    {
        [XmlArray]
        [XmlArrayItem(Type=typeof(ShapeLine)),
        XmlArrayItem(Type = typeof(ShapeRect)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type= typeof(ShapeArc)),
        XmlArrayItem(Type = typeof(ShapePie)),
        XmlArrayItem(Type = typeof(ShapeImage)),
        XmlArrayItem(Type = typeof(ShapeStateText)),
        XmlArrayItem(Type = typeof(shapeVarText)),
        XmlArrayItem(Type = typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeBarcode)),
        XmlArrayItem(Type = typeof(ShapeRoundRect)),
        XmlArrayItem(Type = typeof(ShapeGroup)),
        XmlArrayItem(Type = typeof(shapeSingleText)),
        XmlArrayItem(Type = typeof(shapeMultiText))
        ]
        public  ArrayList arrlistShapeEle = new ArrayList();
        /**
                 [XmlArrayItem(Type=typeof(ShapeArc)),
        XmlArrayItem(Type=typeof(ShapeBarcode)),
        XmlArrayItem (Type=typeof(ShapeEle)),
        XmlArrayItem(Type = typeof(ShapeEllipse)),
        XmlArrayItem(Type=typeof(ShapeGroup)),
        XmlArrayItem(Type=typeof(ShapeImage)),
        XmlArrayItem(Type=typeof(ShapeLine)),
        XmlArrayItem(Type=typeof(ShapePie)),
        XmlArrayItem(Type=typeof(ShapeRect))]
        public  ArrayList arrlistShapeEle = new ArrayList(); 
         * */
        //如下的这个要被替换成页面设置
        //[XmlElement (Type=typeof(Paper))]
        //public  Paper barcodePaper = new Paper();
        //如上的这个被如下这个替代了

        //[XmlElement(Type = typeof(ClsPageSettings))]
        [XmlElement]
        public ClsPageSettings BarcodePageSettings=new ClsPageSettings();

        private ArrayList _arrlistKeyValue;
        [XmlArray]
        [XmlArrayItem(Type = typeof(clsKeyValue))]
        public ArrayList arrlistKeyValue
        {
            get
            {
                return _arrlistKeyValue;
            }
            set
            {
                if ((value != null))
                {
                    _arrlistKeyValue = value;
                }
            }


        }

        
        protected   float _Zoom = 1f;

        [XmlIgnore]
        public float fltCanvasWidth;
        [XmlIgnore]
        public float fltCanvasHeight;

        /**
        [XmlIgnore]
        public  bool isDrawDridding;//是否画网格。
        [XmlIgnore]
        public float fltGriddingInterval ;

         * */

        //这个类好像没有分辨率什么事情，因为画图的单位是毫米。
        //protected float _fltDPIX =96 , _fltDPIY = 96;

        /**
        public float DPIX
        {
            get
            {
                return _fltDPIX;
            }
            set
            {
                _fltDPIX = value;

            }
        }

        public float DPIY
        {
            get
            {
                return _fltDPIY;
            }
            set
            {
                _fltDPIY = value;
            }
        }

         **/
        
        [XmlElement]
        public float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                _Zoom = value;
                if (arrlistShapeEle != null)
                {
                    foreach (ShapeEle item in arrlistShapeEle)
                    {
                        item.Zoom = _Zoom;
                    }
                }
            }
        }
        [XmlIgnore]
        public   static  bool isZhuCe=false;

        [XmlIgnore]
        public static string StrCode;

        /// <summary>
        /// 读取机器码的
        /// </summary>
        public static void setStrCode()
        {
            ClsZhuCe.setStrCode();
            isZhuCe = ClsZhuCe.isZhuCe;//设置注册信息
            
        }
        
        public Shapes()
        {
            //空白的
            BarcodePageSettings = new ClsPageSettings();
            arrlistKeyValue = new ArrayList();
            arrlistShapeEle = new ArrayList();

        }




        /// <summary>
        /// 添加一个形状
        /// </summary>
        /// <param name="shapeEle"></param>
        public void addShapeEle(ShapeEle shapeEle)
        {
            arrlistShapeEle.Add(shapeEle);
            
        }

        public void addShapeEle(ShapeEle shapeEle, int index)
        {
            arrlistShapeEle.Insert(index, shapeEle);
        }

        public int IndexOf(ShapeEle shapeEle)
        {
            return arrlistShapeEle.IndexOf(shapeEle);
        }

        public int Count()
        {
            return arrlistShapeEle.Count;
        }


        /// <summary>
        /// 删除一个形状
        /// </summary>
        /// <param name="shapeEle"></param>
        public void deleteShapeEle(ShapeEle shapeEle)
        {
            arrlistShapeEle.Remove(shapeEle);
        }

        /// <summary>
        /// 根据一个点来返回选择的形状
        /// </summary>
        /// <param name="pointF"></param>
        /// <returns></returns>
        public ShapeEle getSelectShapeEle(PointF pointF)
        {


            //判断是否在其中一个的图形内。
            if ((arrlistShapeEle != null) && (arrlistShapeEle.Count > 0))
            {
                for (int i = arrlistShapeEle.Count - 1; i > -1; i--)//从最后的判断
                {
                    //只所以需要判断而不是直接设置是为了以后多选的情况，比如说按ctrl 键，如果在这里直接设置的话容易覆盖原先已经选择的。
                    bool  isS = ((ShapeEle)arrlistShapeEle[i]).isContains(new PointF(pointF.X, pointF.Y));
                    if (isS)
                    {
                        ((ShapeEle)arrlistShapeEle[i]).isSelect = true;
                        return (ShapeEle)arrlistShapeEle[i];//这个点只判断最上边的图形就可以了。
                    }

                }

            }

            return null ;//返回选择的图形，如果没有，里边就没有数据
        }



        /**如下的这个没有注册部分，不能用
        /// <summary>
        /// 这个方法仅仅没有偏移的，如果需要偏移，请用他的重载方法。
        /// </summary>
        /// <param name="g"></param>
        public void Draw(Graphics g)
        {
            //如果需要绘制刻度尺，那么这里就用一个双缓冲来实现。
            //绘制条形码纸范围
            SolidBrush brush = new SolidBrush(Color.White);

            //g.FillRectangle(brush, 0f, 0f, barcodePaper.Width *Zoom, barcodePaper.Height*Zoom);
            try
            {
                BarcodePageSettings.DrawModelBackground(g, 0, 0, Zoom);


                if (arrlistShapeEle != null)
                {
                    foreach (ShapeEle item in arrlistShapeEle)
                    {
                        //将变量替换到相应的值后再绘图
                        if ((arrlistKeyValue != null)&&(arrlistKeyValue.Count>0))
                        {
                            item.updateVarValue(arrlistKeyValue);
                        }

                        item.Draw(g);
                    }

                }

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }


        }
         * */

        /// <summary>
        /// 这个方法是在程序运行的时候验证是否已经注册了。
        /// </summary>
        private void isYanZhengZhuce()
        {
            //这个方法是在程序运行的时候验证是否已经注册了。

            //首先获得机器码。
            string strCode = (new Xuhengxiao.Hardware.clsHardwareInfo()).GetPcCode();

            //在读取保存的注册码
            string strKey = "";
            if (File.Exists(Application.StartupPath + "\\key.txt"))
            {
                using (StreamReader sr = new StreamReader(Application.StartupPath + "\\key.txt"))
                {
                    strKey = sr.ReadToEnd();
                }
            }

            //解码注册码
            string strJieMa = "";

            foreach (string str in mySplit(strKey, 172))
            {
                strJieMa += Decrypt(str);
            }


            //验证字符串是否相同
            if (strCode == strJieMa)
            {

            }



        }

        private static  string Decrypt(string base64code) //解密
        {
            try
            {

                //Create a UnicodeEncoder to convert between byte array and string.
                UnicodeEncoding ByteConverter = new UnicodeEncoding();

                //Create a new instance of RSACryptoServiceProvider to generate
                //public and private key data.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();
                string strPrivateKey = @"<RSAKeyValue><Modulus>qnfhMY6XO+fJDmd84nbAyH51xR3gb8ow7GWr3RPl172sYnCCTprCgSg2Y7HexH43p38WHk6bR1hdkic2cYIcz7gqrLs3CsY/YlxljJQ0MGjfeK+OY1L2tB482cE/wjVKAbCG5J+4vzo13S+whKHxsvlkGRM5KpDHyd0ZnE37V8k=</Modulus><Exponent>AQAB</Exponent><P>7W3IhAKh8njPL4XeIf9xjX2HqIgWUS1aIcIEr7bXY5ey53aw47yfkixSudeSZolJMPpGC+GO6hIyEmznlB63iw==</P><Q>t81LaijAd3Utn7xX/QQ/x9c8ijWgyeWQVWyA4F+7Ay6O5Ztke4ufJq6VFslpI0CDe4DUrp2gBtqEAjN/XZB4ew==</Q><DP>UtX3nF8Sw3b0yh7JdlEZ/ARs3RbFuoK5LIf1fJytHxkhGPJnGr2Hasc+AYq9kDqbp5PZ9nE2nGHGyHjoftwMqw==</DP><DQ>Uzx+TZoc5zxCqBcURbnZ5HddrD1zDluOzJCxoGrZ9yvrfKGtlKF7NnpTfBlEKrm5kYGbT2SEpvXoWFLX+BhH5w==</DQ><InverseQ>xKYnwi/1O57Na9fS0GJHxy5/BXdEwqZ7KSeZsftFxrUiO60meb5yFN6MnGANE0A6pqf0tBLgciK8muJVYg7Tsg==</InverseQ><D>Qc7NrKfzUjkEsP7ag0J84emP5WzHO+C+SkRluI755/NdHRN5+oZcGChB9vKvoQNo0MyK6WBHKZ+/X7Crn94u6I7+1+owWeppsd5uie3rruMIZOzaUeGxmiNXsMDZuY7r5aQVb/zccX9+ccMk6DPfE1UVjTsLcUwg8t4tjJ/49lE=</D></RSAKeyValue>";

                RSA.FromXmlString(strPrivateKey);

                byte[] encryptedData;
                byte[] decryptedData;
                encryptedData = Convert.FromBase64String(base64code);

                //Pass the data to DECRYPT, the private key information 
                //(using RSACryptoServiceProvider.ExportParameters(true),
                //and a boolean flag specifying no OAEP padding.
                decryptedData = RSADecrypt(encryptedData, RSA.ExportParameters(true), false);



                //Display the decrypted plaintext to the console. 
                return ByteConverter.GetString(decryptedData);
            }
            catch (Exception exc)
            {
                //Exceptions.LogException(exc);
                //ClsErrorFile.WriteLine(exc.Message);
                ClsErrorFile.WriteLine("",exc);
                //Console.Error.WriteLine(exc.Message);
                return "";
            }
        }

        private static  byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            try
            {
                //Create a new instance of RSACryptoServiceProvider.
                RSACryptoServiceProvider RSA = new RSACryptoServiceProvider();

                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                return RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            //Catch and display a CryptographicException  
            //to the console.
            catch (CryptographicException e)
            {
                //Exceptions.LogException(e);
                //Console.Error.WriteLine(e.Message);
                //ClsErrorFile.WriteLine(e.Message);
                ClsErrorFile.WriteLine("", e);
                return null;
            }

        }


        private static  Array mySplit(string str, int splitLength)
        {
            //如下是计算能分割成多少份
            int n = str.Length / splitLength;
            if (str.Length % splitLength != 0)
                n++;
            string[] strReturn = new string[n];//只是返回n个元素的数组而已。

            string strShengYu = str;//每次截取后剩下的字符串。
            for (int i = 0; i < n; i++)
            {
                int intJieQuQty = splitLength;
                if (strShengYu.Length < splitLength)
                    intJieQuQty = strShengYu.Length;//如果剩余的字符串不够截取的，就只是截取剩余的长度
                strReturn[i] = strShengYu.Substring(0, intJieQuQty);
                strShengYu = strShengYu.Remove(0, intJieQuQty);
            }


            return strReturn;
        }




        /// <summary>
        /// 画图，包含纸张背景
        /// </summary>
        /// <param name="g"></param>
        /// <param name="arrlistMatrix"></param>
        public void Draw(Graphics g, ArrayList arrlistMatrix)
        {

            try
            {
                //偏移

                BarcodePageSettings.DrawModelBackground(g, 0, 0, Zoom, arrlistMatrix);//绘制模板的背景

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine(ex);
                //throw;
            }


            DrawShapes(g, arrlistMatrix);


            /** 
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下被认为可以清晰文字。
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
             * */

            //如果需要绘制刻度尺，那么这里就用一个双缓冲来实现。
            //绘制条形码纸范围
            //SolidBrush brush = new SolidBrush(Color.White);



            //如下是画一个条形码模板的底
            //g.FillRectangle(brush, 0f, 0f, barcodePaper.Width * Zoom, barcodePaper.Height * Zoom);


        }

        /// <summary>
        /// 画图，可以设置偏移，这个包含背景的
        /// </summary>
        /// <param name="g"></param>
        /// <param name="KongX"></param>
        /// <param name="KongY"></param>
        public void Draw(Graphics g, float KongX, float KongY)
        {

            ArrayList arrlist = new ArrayList();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(KongX, KongY);
            arrlist.Add(m);

            Draw(g, arrlist);//调用这个进行绘图

            //如下的相当于被注释掉了
            if (false)
            {
                /** 
           //单位一定要是MM。
           g.PageUnit = GraphicsUnit.Millimeter;

           //如下被认为可以清晰文字。
           g.SmoothingMode = SmoothingMode.HighQuality;
           g.InterpolationMode = InterpolationMode.HighQualityBicubic;
           g.PixelOffsetMode = PixelOffsetMode.HighQuality;
           g.CompositingQuality = CompositingQuality.HighQuality;
            * */

                //如果需要绘制刻度尺，那么这里就用一个双缓冲来实现。
                //绘制条形码纸范围
                //SolidBrush brush = new SolidBrush(Color.White);



                //如下是画一个条形码模板的底
                //g.FillRectangle(brush, 0f, 0f, barcodePaper.Width * Zoom, barcodePaper.Height * Zoom);
                try
                {
                    //偏移
                    g.TranslateTransform(KongX, KongY, MatrixOrder.Prepend);
                    BarcodePageSettings.DrawModelBackground(g, 0, 0, Zoom);//绘制模板的背景
                    g.TranslateTransform(-KongX, -KongY);
                    g.ResetTransform();//恢复原先的坐标系。
                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //throw;
                }


                DrawShapes(g, KongX, KongY);


                #region //如下的已经被注释掉了。
                /**
            if (!isZhuCe)
            {
                //要留出空白来
                ShapeRect shapeRect = new ShapeRect();
                shapeRect.X = 5;
                shapeRect.Y = 5;
                shapeRect.isFill = true;
                shapeRect.FillColor = Color.White;

                ShapeStateText stt = new ShapeStateText();
                stt.Text = "没有注册";
                stt.X = 5;
                stt.Y = 5;
                stt.TextFont = new Font("Arial", 10);

                shapeRect.Height = stt.Height;
                shapeRect.Width = stt.Width;

                shapeRect.Zoom = Zoom;
                stt.Zoom = Zoom;

                //偏移
                g.TranslateTransform(KongX, KongY, MatrixOrder.Prepend);
                shapeRect.Draw(g);
                g.TranslateTransform(-KongX, -KongY);
                g.ResetTransform();//恢复原先的坐标系。

                g.TranslateTransform(KongX, KongY, MatrixOrder.Prepend);
                stt.Draw(g);
                g.TranslateTransform(-KongX, -KongY);
                g.ResetTransform();//恢复原先的坐标系。
                
            }
             * *.



            /**如下的不能适合组合的情况，

            if (arrlistShapeEle != null)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    //

                    //将变量替换到相应的值后再绘图
                    if (arrlistKeyValue != null)
                    {
                        item.updateVarValue(arrlistKeyValue);
                    }


                    g.TranslateTransform(KongX, KongY, MatrixOrder.Prepend);
                    
                    item.Draw(g);

                    g.TranslateTransform(-KongX, -KongY);
                    g.ResetTransform();

                }

            }
             * */
                #endregion
                
            }

           

        }

        /// <summary>
        /// 画图，不包含背景
        /// </summary>
        /// <param name="g"></param>
        /// <param name="arrlistMatrix"></param>
        public  void DrawShapes(Graphics g, ArrayList arrlistMatrix)
        {
            //单位一定要是MM。
            g.PageUnit = GraphicsUnit.Millimeter;


            //如下被认为可以清晰文字。g
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //文字抗锯齿
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //设置高质量插值法
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            //g.SmoothingMode = SmoothingMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //
            if (arrlistShapeEle != null)
            {
                foreach (ShapeEle item in arrlistShapeEle)
                {
                    //
                    //将变量替换到相应的值后再绘图
                    if ((arrlistKeyValue != null))
                    {
                        item.updateVarValue(arrlistKeyValue);
                    }

                    item.Draw(g, arrlistMatrix);
                   

                }

            }
            


            //如下是判断注册的
            #region
            if (!isZhuCe)
            {
                //要留出空白来
                ShapeRect shapeRect = new ShapeRect();
                shapeRect.X = 5;
                shapeRect.Y = 5;
                shapeRect.isFill = true;
                shapeRect.FillColor = Color.White;

                shapeSingleText stt = new shapeSingleText();
                stt.DefaultText = "没有注册";
                stt.PreFix = "";
                stt.Suffix = "";

                stt.X = 5;
                stt.Y = 5;
                //stt.TextFont = new Font("Arial", 4);

                shapeRect.Height = stt.Height;
                shapeRect.Width = stt.Width;

                shapeRect.Zoom = Zoom;
                stt.Zoom = Zoom;

                //偏移

                shapeRect.Draw(g, arrlistMatrix);

                stt.Draw(g, arrlistMatrix);

            }
            #endregion 

        }

        /// <summary>
        /// 在一个指定位置绘制所有形状。
        /// </summary>
        /// <param name="g"></param>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        public void DrawShapes(Graphics g, float fltKongX, float fltKongY)
        {


            ArrayList arrlist = new ArrayList();
            System.Drawing.Drawing2D.Matrix m = new Matrix();
            m.Translate(fltKongX, fltKongY);
            arrlist.Add(m);

            DrawShapes(g, arrlist);//调用这个进行绘图

            //如下的相当于被注释掉了。
            #region
            if (false)
            {

                //单位一定要是MM。
                g.PageUnit = GraphicsUnit.Millimeter;

                //如下被认为可以清晰文字。
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;

                //
                DrawShapes(g, arrlistShapeEle, fltKongX, fltKongY);


                //如下是判断注册的

                if (!isZhuCe)
                {
                    //要留出空白来
                    ShapeRect shapeRect = new ShapeRect();
                    shapeRect.X = 5;
                    shapeRect.Y = 5;
                    shapeRect.isFill = true;
                    shapeRect.FillColor = Color.White;

                    ShapeStateText stt = new ShapeStateText();
                    stt.Text = "没有注册";
                    stt.X = 5;
                    stt.Y = 5;
                    stt.TextFont = new Font("Arial", 10);

                    shapeRect.Height = stt.Height;
                    shapeRect.Width = stt.Width;

                    shapeRect.Zoom = Zoom;
                    stt.Zoom = Zoom;

                    //偏移
                    g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                    shapeRect.Draw(g);
                    g.TranslateTransform(-fltKongX, -fltKongY);
                    g.ResetTransform();//恢复原先的坐标系。

                    g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                    stt.Draw(g);
                    g.TranslateTransform(-fltKongX, -fltKongY);
                    g.ResetTransform();//恢复原先的坐标系。

                }



            }
            #endregion

        }

        /// <summary>
        /// 如下是根据图形数组绘图，可以递归群组的
        /// </summary>
        /// <param name="g"></param>
        /// <param name="arraylist"></param>
        /// <param name="fltKongX"></param>
        /// <param name="fltKongY"></param>
        private  void DrawShapes(Graphics g, ArrayList arraylist , float fltKongX , float fltKongY )
        {
            if (arraylist != null)
            {
                foreach (ShapeEle item in arraylist)
                {
                    //
                    //将变量替换到相应的值后再绘图
                    if (arrlistKeyValue != null)
                    {
                        item.updateVarValue(arrlistKeyValue);
                    }

                    item.Draw(g, fltKongX, fltKongY);
                    /**
                    //是组合的话，就得迭代。，但这个组合也是得画的。
                    if (item.GetType().Name == "ShapeGroup")
                    {
                        DrawShapes(g, ((ShapeGroup)item).arrlistShapeEle, fltKongX, fltKongY);
                        item.Draw(g);
                    }
                    else
                    {
                        //如下的这个是偏移些位置
                        g.TranslateTransform(fltKongX, fltKongY, MatrixOrder.Prepend);
                        item.Draw(g);//真正的绘图。
                        //如下的这个是恢复原先的，负数.
                        g.TranslateTransform(-fltKongX, -fltKongY);
                        g.ResetTransform();//恢复原先的坐标系。
                    }
                     * */

                }

            }



        }

        /// <summary>
        /// 根据一个矩形返回被选择的形状
        /// </summary>
        /// <param name="rect"></param>
        /// <returns></returns>
        public ArrayList getSelectShapeEle(RectangleF rect)
        {
            ArrayList arraylistSel = new ArrayList();

            //首先是判断是否有选择到其中的。
            foreach (ShapeEle item in arrlistShapeEle)
            {
                bool isS = item.isContains(rect);
                //如果有被选择的，当然首先是更改状态为选择
                if (isS)
                {
                    item.isSelect = true;//判断位啦,好像这个也没有什么用。
                    arraylistSel.Add(item);

                }

            }


            return arraylistSel;
        }
    }

    /// <summary>
    /// 纸张类，因为我做的这个矢量绘图主要用来一比一打印的，所以需要这个纸张类。
    /// </summary>
    [Serializable]
    public class Paper
    {
        private float _fltPaperWidth=40, _fltPaperHeight=40;//纸张的宽度和高度
        //private bool _isContinuousMedias = false;//是否是连续纸张
        private int _NumOfLables=2;//每行的条形码纸个数
        private float _fltHorizontalRepeatDistance=2f;//水平间距
        //private float _fltVarticalRepeatDistance=2f;//垂直间距

        //只有设置成如下的属性才能被属性选择器认识
        [DescriptionAttribute("纸张宽度"), CategoryAttribute("布局")]
        public float Width
        {
            get
            {
                return _fltPaperWidth;
            }
            set
            {
                _fltPaperWidth = value;
            }
        }
        [DescriptionAttribute("纸张高度"), CategoryAttribute("布局")]
        public float Height
        {
            get
            {
                return _fltPaperHeight;
            }
            set
            {
                _fltPaperHeight = value;
            }
        }

        [DescriptionAttribute("每行条形码纸个数"), CategoryAttribute("布局")]
        public int NumOfLables
        {
            get
            {
                return _NumOfLables;
            }
            set
            {
                _NumOfLables = value;
            }
        }

        [DescriptionAttribute("条形码纸之间的水平间距"), CategoryAttribute("布局")]
        public float HorizontalRepeatDistance
        {
            get
            {
                return _fltHorizontalRepeatDistance;
            }
            set
            {
                _fltHorizontalRepeatDistance = value;
            }
        }
        /**我暂时不想搞这个连续纸张，因为连续纸张只是高度加上垂直间隔，让用户直接设置高度增加就可以了。
        [DescriptionAttribute("条形码纸之间的垂直间距"), CategoryAttribute("布局")]
        public float VarticalRepeatDistance
        {
            get
            {
                return _fltVarticalRepeatDistance;
            }
            set
            {
                _fltVarticalRepeatDistance = value;
            }
        }

        [DescriptionAttribute("是否是连续纸张"), CategoryAttribute("布局")]
        public bool IsContinuousMedias
        {
            get
            {
                return _isContinuousMedias;
            }
            set
            {
                _isContinuousMedias = value;
            }

        }
         * */

    }
}
