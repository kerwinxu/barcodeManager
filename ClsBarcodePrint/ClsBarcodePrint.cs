using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Threading;
using System.Management;
using VestShapes;
using Xuhengxiao.MyDataStructure;
using Xuhengxiao.DataBase;
using System.Xml.Serialization;


namespace BarcodeTerminator
{
    /**
    //这个类是打印数据，只是一个变长数组。里边的项才是打印数据
     public class ClsPrintItems
    {
        private ArrayList  _arrlistPrintItem;

        public ArrayList  arrlistQueuePrintItem
        {
            get { return _arrlistPrintItem; }
            set { _arrlistPrintItem = value; }
        }

        private string _strShapesFileName;

        public string ShapesFileName
        {
            get { return _strShapesFileName; }
            set { _strShapesFileName = value; }
        }
        
        
         public void addQueuePrintItem(queuePrintItem item )
        {
            _arrlistPrintItem.Add(item);
        }
        
    }

    /// <summary>
    /// 这个类保存的是三项数据，一个是表名，一个是相关数据，一个是要打印的页数
    /// </summary>
     public class ClsPrintItem
     {
         private string _strTableName;
         /// <summary>
         /// 表名
         /// </summary>
         public string TableName
         {
             get { return _strTableName; }
             set { _strTableName = value; }
         }

         private ArrayList _arrlistRow;
         /// <summary>
         /// 就是没一行的数据
         /// </summary>
         public ArrayList arrlistRow
         {
             get { return _arrlistRow; }
             set { _arrlistRow = value; }
         }

         private int _intPages;
         /// <summary>
         /// 页数
         /// </summary>
         public int Pages
         {
             get { return _intPages; }
             set { _intPages = value; }
         }

    }
     * */

     public class queuePrintItemRowAndPages
     {

        private List<clsKeyValue> _arrlistRow;
        /// <summary>
        /// 每行的数据
        /// </summary>
        [XmlArray]
        [XmlArrayItem(Type = typeof(clsKeyValue))]
        public List<clsKeyValue> arrlistRow
        {
            get { return _arrlistRow; }
            set { _arrlistRow = value; }
        }


        // 取消，因为不支持序列化
        //private Dictionary<string,string> keyValuePairs;
        ///// <summary>
        ///// 每行的数据，用字典表示。
        ///// </summary>
        //public Dictionary<string,string> arrlistRow
        //{
        //    get { return keyValuePairs; }
        //    set { keyValuePairs = value; }
        //}




        private int _intPages;
         /// <summary>
         /// 页数
         /// </summary>
         public int intPages
         {
             get { return _intPages; }
             set { _intPages = value; }
         }
         

         
     }
    

    [Serializable]
    public   class queuePrintItem
    {
        /**这个序列化需要默认的不含参数的构造函数
        public queuePrintItem(string strTableName, string  shapesFileName, ArrayList arrlistRow, int intPages)
        {

            _strShapesFileName = shapesFileName;//文件名
            _intPages = intPages;
            _strTableName = strTableName;
            _arrlistRow = arrlistRow;
        }
         * */
        //我把如下的四个全部搞成属性了。
        //private  XmlDocument _xmlBarcodeModel;//不需要了。
        //private int _intPages;
        private string _strTableName;

        private ArrayList _arrlistqueuePrintItemRowAndPages;
        private string _strShapesFileName;//这个文件是保存形状的文件。

        public string ShapesFileName
        {
            get
            {
                return _strShapesFileName;
            }
            set
            {
                _strShapesFileName = value;
            }
        }
        public string strTableName
        {
            get
            {
                return _strTableName;
            }
            set
            {
                _strTableName = value;
            }
        }

        [XmlArray]
        [XmlArrayItem(Type = typeof(queuePrintItemRowAndPages))]
        public ArrayList arrlistqueuePrintItemRowAndPages
        {
            get
            {
                return _arrlistqueuePrintItemRowAndPages;
            }
            set
            {
                _arrlistqueuePrintItemRowAndPages = value;
            }
        }

        private bool _isFull;
        /// <summary>
        /// 是否充满
        /// </summary>
        public bool IsFull
        {
            get { return _isFull; }
            set { _isFull = value; }
        }

        private int _intCount;
        /// <summary>
        /// 取得个数
        /// </summary>
        public int IntCount
        {
            get
            { 
                if (arrlistqueuePrintItemRowAndPages!=null)
                {
                    return arrlistqueuePrintItemRowAndPages.Count;
                }
                return 0;
            }

        }

        public void addQueuePrintItemRowAndPages(queuePrintItemRowAndPages item)
        {
            if (arrlistqueuePrintItemRowAndPages==null)
            {
                arrlistqueuePrintItemRowAndPages = new ArrayList();
            }
            arrlistqueuePrintItemRowAndPages.Add(item);
        }
        
        /**
        public XmlDocument xmlBarcodeModel
        {
            get
            {
                return _xmlBarcodeModel;
            }
            set
            {
                _xmlBarcodeModel = value;
            }
        }
         * */

        /**如下的被安排到queuePrintItemRowAndPages中了。
        public int intPages
        {
            get
            {
                return _intPages;
            }
            set
            {
                _intPages = value;
            }
        }
        public ArrayList arrlistRow
        {
            get
            {
                return _arrlistRow;
            }
            set
            {
                _arrlistRow = value;
            }
        }
         * */

        //重新toString()方法，我只打算显示 ArrayList arrlistRow, int intPages这两项。
        /**这个方法不能用了，因为可能存在多行的情况,而这个只是对一行而言的,我用如下那个取代这个。
        public override string ToString()
        {
            string strReturn = "";
            if (arrlistRow != null)
            {
                foreach (clsKeyValue keyValue in arrlistRow)
                {
                    strReturn = strReturn + keyValue.Key + ":" + keyValue.Value + "  ";
                }

            }

            //打印页数
            strReturn = strReturn + " 打印页数：" + intPages.ToString();
 
            return strReturn;
        }
         * */

        /// <summary>
        /// 这个只是取得arrlistqueuePrintItemRowAndPages中第一项的数据字符串。
        /// </summary>
        public string getString()
        {
            string strReturn = "";

            if((arrlistqueuePrintItemRowAndPages!=null)&& (arrlistqueuePrintItemRowAndPages.Count>0))
            {
                //先取得第一项
                queuePrintItemRowAndPages queuePrintItemRowAndPages1 =(queuePrintItemRowAndPages) arrlistqueuePrintItemRowAndPages[0];
                //foreach (clsKeyValue keyValue in queuePrintItemRowAndPages1.arrlistRow)
                //{
                //    strReturn = strReturn + keyValue.Key + ":" + keyValue.Value + "  ";
                //}
                foreach (var key in queuePrintItemRowAndPages1.arrlistRow)
                {
                    strReturn += key.Key + ":" + key.Value + " ";
                }
                //打印页数
                strReturn = strReturn + " 打印页数：" + queuePrintItemRowAndPages1.intPages.ToString();

            }

            return strReturn;
        }

        /// <summary>
        /// 这个只是移除第一页而已
        /// </summary>
        public void removeFirstPages()
        {
            if (arrlistqueuePrintItemRowAndPages!=null)
            {
                ((queuePrintItemRowAndPages)arrlistqueuePrintItemRowAndPages[0]).intPages--;
                //如果页数为零了，就移除这一项
                if (((queuePrintItemRowAndPages)arrlistqueuePrintItemRowAndPages[0]).intPages == 0)
                    arrlistqueuePrintItemRowAndPages.RemoveAt(0);
            }
        }

        
        
    }

   public    class ClsBarcodePrint
    {


        private static ClsBarcodePrint clsBarcodePrint = null;  // 这个是静态的类。


        public   PrintDocument myPrintDocument = new PrintDocument();
        public  int intPrintPage;//实际打印的页数
        public static Queue<queuePrintItem>  arrlistPrint = new Queue<queuePrintItem>();//表示要打印的队列
        //如下是打印间隔的两项参数
        public static string strJianGe;
        public static bool isPrintJiange;

        private static TimerCallback myTimerCallBack = timeCallBackPrintManaget;
        public static System.Threading.Timer timerPrint = new System.Threading.Timer(myTimerCallBack, null, 0, 500);

        private UserControlCanvas barcodeCanvas=new UserControlCanvas ();//将这个类作为条形码类画布类

        //public static string strPrinterName="";

        private static string _strPrinterName = "";

        public static string strPrinterName
        {
            get { return _strPrinterName; }
            set { 
                _strPrinterName = value;
                //还得取得角度
                try
                {
                    PrintDocument pdoc = new PrintDocument();
                    pdoc.PrinterSettings.PrinterName = _strPrinterName;
                    LandScapeAngle = pdoc.PrinterSettings.LandscapeAngle;//取得这个横向的角度
                }
                catch (System.Exception ex)
                {
                    ClsErrorFile.WriteLine("没有取得横向角度", ex);       	
                }

            }
        }

        public static int LandScapeAngle = 270;
        

        //如下是定义要打印的时候发的消息
        public delegate void BarcodePrinted(object sender,printedEventArgs e);

        public class printedEventArgs : EventArgs
        {
            //以后再添信息内容吧。
        }

        public static  event BarcodePrinted barcodePrinted;
        
        private void OnBarcodePrinted(printedEventArgs e)
        {
            if (barcodePrinted != null)
                barcodePrinted(this, e);

        }



        //如下的准备注释掉，因为没有用了。
        /**
        public  float fltPaperWidth, fltPaperHeight;
        public  bool isContinuousMedias=false;
        public  int intNumOfLine;
        public  float  fltHorizontalRepeatDistance, fltVarticalRepeatDistance;
        public  Image imageBarcode;
        public static float fltDPIX=600, fltDPIY=600;
         * */

        public static bool isZhuCe;//是否已经注册


        private static bool isPrinting = false;//是否打印的。

        //我用定时器来管理打印顺序，并且做出相应事件的。
        private  static   void timeCallBackPrintManaget(Object state)
        {
            //判断是否打印的依据是打印机为空闲，且队列不为空。且不能是正在打印，因为那个会引发冲突
            if (//(PrinterCheck.GetPrinterStatus(strPrinterName) == PrinterCheck.PrinterStatus.空闲)&&
                 (arrlistPrint.Count > 0) && (!isPrinting))
            {
                isPrinting = true; // 首先设置这个。

                if (clsBarcodePrint == null)
                {
                    clsBarcodePrint = new ClsBarcodePrint();
                }

                try
                {
                    //获取第一项，并删除第一项。不清楚原因，但有时候会打印双份，我这里先清除在打印。
                    queuePrintItem qtm = arrlistPrint.Dequeue();
                    clsBarcodePrint.PrintBarcode(qtm);

                }
                catch (Exception ex)
                {
                    ClsErrorFile.WriteLine(ex);
                    //Console.Error.WriteLine(ex.Message);
                }

                isPrinting = false;// 最后取消这个。

            }

        }

        public void addPrintDetails(queuePrintItem printDetails)
        {
            arrlistPrint.Enqueue(printDetails);

        }


        

        //如下的方法也准备注释掉，因为不需要了。
        /**
        public  Bitmap xmlToBarcodeImage(XmlDocument xmlBarcodeModel)
        {
            //首先取得纸张的尺寸和DPI
            //再根据纸张尺寸画画布
            //在读取xml模板内容画图
            //最后返回画布就可以了


            //double dwPaparWidth = 40, fltPaperHeight = 40;//默认纸张大小
            //取得纸张尺寸如下

            XmlElement myXmlEleRoot = xmlBarcodeModel.DocumentElement;
            foreach (XmlNode myxmlNode in myXmlEleRoot.ChildNodes)
            {
                string strNodeName = myxmlNode.Name.ToLower();
                switch (strNodeName)
                {

                    case ("paper"):
                        XmlElement xmltemp = (XmlElement)myxmlNode;//用这个转换只是因为xmlNode没有GetAttribute
                       // fltPaperWidth = strAttributeValueToFloat(xmltemp,"PaperWidth");
                        //fltPaperHeight = strAttributeValueToFloat(xmltemp,"PaperHeight");

                        break;
                }
            }


            //建立画布部分

            int iW=(int)(fltPaperWidth*fltDPIX/25.4);
            int iH=(int)(fltPaperHeight*fltDPIY/25.4);
            Bitmap imageCanvas = new Bitmap(iW,iH);
            imageCanvas.SetResolution(fltDPIX, fltDPIY);

            Graphics g = Graphics.FromImage(imageCanvas);
            g.PageUnit = GraphicsUnit.Millimeter;

            //如下被认为可以清晰文字。
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;
            
            g.Clear(Color.White);



            //如下就是在这个画布上绘图了

            foreach (XmlNode myxmlNode in myXmlEleRoot.ChildNodes)
            {
                string strNodeName = myxmlNode.Name.ToLower();
                switch (strNodeName)
                {
                    case "shapes":

                        //如下是循环把形状的属性
                        #region

                        //如下是循环把形状的属性
                        foreach (XmlElement xmlElementShapes in myxmlNode.ChildNodes)
                        {
                            string strShapesName = xmlElementShapes.Name.ToLower();
                            FontStyle myFontStyle;
                            Font font1;
                            float   fX, fY  ,fWidth ,fHeight ;
                            SolidBrush drawBrush;
                            PointF poingStr;

                            switch (strShapesName)
                            {

                                case "statictext":
                                    //首先建立一个静态文本属性对性
                                    #region

                                    //字体风格，加粗倾斜下划线
                                    myFontStyle = new FontStyle();

                                    if (xmlElementShapes.GetAttribute("isBold").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Bold;

                                    }
                                    if (xmlElementShapes.GetAttribute("isItalic").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Italic;
                                    }
                                    if (xmlElementShapes.GetAttribute("isUnderLine").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Underline;
                                    }

                                     font1 = new Font(xmlElementShapes.GetAttribute("FontName"), Convert.ToSingle(xmlElementShapes.GetAttribute("FontSize")),myFontStyle);

                                    //这样字体就读取好了，接下来读取位置。
                                     fX = strAttributeValueToFloat(xmlElementShapes,"X");
                                     fY = strAttributeValueToFloat(xmlElementShapes,"Y") ;
                                     poingStr = new PointF(fX, fY);
                                     drawBrush = new SolidBrush(Color.Black);

                                    g.DrawString(xmlElementShapes.InnerText, font1, drawBrush, poingStr);
                                    drawBrush.Dispose();
                                    font1.Dispose();

                                    break;

                                    #endregion
                                //如下是设置变量文字
                                case "variabletext":
                                    #region
                                    //变量字体跟静态字体没有什么本质区别，唯一的区别是加了后缀

                                    //字体风格，加粗倾斜下划线
                                    myFontStyle = new FontStyle();

                                    if (xmlElementShapes.GetAttribute("isBold").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Bold;

                                    }
                                    if (xmlElementShapes.GetAttribute("isItalic").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Italic;
                                    }
                                    if (xmlElementShapes.GetAttribute("isUnderLine").ToLower() == "true")
                                    {
                                        myFontStyle = myFontStyle | FontStyle.Underline;
                                    }

                                     font1 = new Font(xmlElementShapes.GetAttribute("FontName"), Convert.ToSingle(xmlElementShapes.GetAttribute("FontSize")), myFontStyle);

                                    //这样字体就读取好了，接下来读取位置。
                                     fX = strAttributeValueToFloat(xmlElementShapes,"X");
                                     fY = strAttributeValueToFloat(xmlElementShapes,"Y");
                                     poingStr = new PointF(fX, fY);
                                     drawBrush = new SolidBrush(Color.Black);
                                     string strSuffix = xmlElementShapes.GetAttribute("suffix");

                                    g.DrawString(xmlElementShapes.InnerText+strSuffix, font1, drawBrush, poingStr);
                                    drawBrush.Dispose();

                                    break;
                                    #endregion

                                //如下是条形码
                                case "barcode":
                                    #region

                                    //条形码可能有异常，比如说位数不符等等
                                    try
                                    {
                                 
;
                                        string  strEncoding = xmlElementShapes.GetAttribute("Encoding");
                                        BarcodeLib.TYPE myType =BarcodeLib.TYPE.EAN13;
                                        switch (strEncoding)
                                        {
                                            case "EAN13":
                                                myType = BarcodeLib.TYPE.EAN13;
                                                break;
                                            case "EAN8":
                                                myType = BarcodeLib.TYPE.EAN8;
                                                break;
                                            case "FIM":
                                                myType = BarcodeLib.TYPE.FIM;
                                                break;
                                            case "Codabar":
                                                myType = BarcodeLib.TYPE.Codabar;
                                                break;
                                            case "UPCA":
                                                myType = BarcodeLib.TYPE.UPCA;
                                                break;
                                            case "UPCE":
                                                myType = BarcodeLib.TYPE.UPCE;
                                                break;
                                            case "UPC_SUPPLEMENTAL_2DIGIT":
                                                myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_2DIGIT;
                                                break;
                                            case "UPC_SUPPLEMENTAL_5DIGIT":
                                                myType = BarcodeLib.TYPE.UPC_SUPPLEMENTAL_5DIGIT;
                                                break;
                                            case "CODE39":
                                                myType = BarcodeLib.TYPE.CODE39;
                                                break;
                                            case "CODE39Extended":
                                                myType = BarcodeLib.TYPE.CODE39Extended;
                                                break;
                                            case "CODE128":
                                                myType = BarcodeLib.TYPE.CODE128;
                                                break;
                                            case "CODE128A":
                                                myType = BarcodeLib.TYPE.CODE128A;
                                                break;
                                            case "CODE128B":
                                                myType = BarcodeLib.TYPE.CODE128B;
                                                break;
                                            case "CODE128C":
                                                myType = BarcodeLib.TYPE.CODE128C;
                                                break;
                                            case "ISBN":
                                                myType = BarcodeLib.TYPE.ISBN;
                                                break;
                                            case "Interleaved2of5":
                                                myType = BarcodeLib.TYPE.Interleaved2of5;
                                                break;
                                            case "Standard2of5":
                                                myType = BarcodeLib.TYPE.Standard2of5;
                                                break;
                                            case "Industrial2of5":
                                                myType = BarcodeLib.TYPE.Industrial2of5;
                                                break;
                                            case "PostNet":
                                                myType = BarcodeLib.TYPE.PostNet;
                                                break;
                                            case "BOOKLAND":
                                                myType = BarcodeLib.TYPE.BOOKLAND;
                                                break;
                                            case "JAN13":
                                                myType = BarcodeLib.TYPE.JAN13;
                                                break;
                                            case "MSI_Mod10":
                                                myType = BarcodeLib.TYPE.MSI_Mod10;
                                                break;
                                            case "MSI_2Mod10":
                                                myType = BarcodeLib.TYPE.MSI_2Mod10;
                                                break;
                                            case "MSI_Mod11":
                                                myType = BarcodeLib.TYPE.MSI_Mod11;
                                                break;
                                            case "MSI_Mod11_Mod10":
                                                myType = BarcodeLib.TYPE.MSI_Mod11_Mod10;
                                                break;
                                            case "Modified_Plessey":
                                                myType = BarcodeLib.TYPE.Modified_Plessey;
                                                break;
                                            case "CODE11":
                                                myType = BarcodeLib.TYPE.CODE11;
                                                break;
                                            case "USD8":
                                                myType = BarcodeLib.TYPE.USD8;
                                                break;
                                            case "UCC12":
                                                myType = BarcodeLib.TYPE.UCC12;
                                                break;
                                            case "UCC13":
                                                myType = BarcodeLib.TYPE.UCC13;
                                                break;
                                            case "LOGMARS":
                                                myType = BarcodeLib.TYPE.LOGMARS;
                                                break;
                                            case "ITF14":
                                                myType = BarcodeLib.TYPE.ITF14;
                                                break;
                                            case "TELEPEN":
                                                myType = BarcodeLib.TYPE.TELEPEN;
                                                break;
                                           
                                        }
                                        
                                        BarcodeLib.Barcode bar = new BarcodeLib.Barcode();
                                        fX = strAttributeValueToFloat(xmlElementShapes,"X");
                                        fY = strAttributeValueToFloat(xmlElementShapes,"Y") ;
                                        poingStr = new PointF(fX, fY);
                                        fWidth = strAttributeValueToFloat(xmlElementShapes,"Width") ;
                                        fHeight = strAttributeValueToFloat(xmlElementShapes,"Height");
                                        bar.IncludeLabel = false; ;
                                        if (xmlElementShapes.GetAttribute("isIncludeLabel").ToLower() == "true")
                                        {
                                             bar.IncludeLabel = true;
                                        }

                                        int intBarCodeWidth = (int)(fWidth * g.DpiX / 25.4);
                                        int intBarCodeHeight = (int)(fHeight * g.DpiY / 25.4);

                                        Image myImage = bar.Encode(myType, xmlElementShapes.InnerText, intBarCodeWidth, intBarCodeHeight,g.DpiX,g.DpiY);
                                        g.DrawImage(myImage, poingStr);
                                        bar.Dispose();
                                         


                                    }
                                    catch (Exception e)
                                    {
                                        MessageBox.Show(e.Message);
                                    }

                                    #endregion
                                    break;
                                //如下设置其他形状


                            }
                        }
                        break;

                        #endregion


                }
            }

            //如下是如果没有注册，在左上角写上文字" BarcodeTerminator"
            if (!isZhuCe)
            {
                g.DrawString(" BarcodeTerminator", new Font("Arial", 6), new SolidBrush(Color.Black), new Point(0, 0));

            }


            return imageCanvas;

        }
         * */

        /**

        //变量替换，测试成功
        public static XmlDocument populateVariable(XmlDocument xmlDoc)
        {
            //用xpath搜索的方法来替换。
            XmlDocument myxml = xmlDoc;//赋值而已
            foreach (clsKeyValue keyvalue in frmMain.arrlistSellectRow)
            {
                XmlNodeList nodeList1 = myxml.SelectNodes("//VariableText[@variableName='" + keyvalue.Key + "']");

                foreach (XmlNode myXmlNode in nodeList1)
                {
                    myXmlNode.InnerText = keyvalue.Value;
                }
                XmlNodeList nodeList2 = myxml.SelectNodes("//Barcode[@variableName='" + keyvalue.Key + "']");

                foreach (XmlNode myXmlNode in nodeList2)
                {
                    myXmlNode.InnerText = keyvalue.Value;
                }
            }

            return myxml;

        }
         * 
         * */

        
        private  void   PrintBarcode(queuePrintItem printDetails)
        {
            //首先判断是否是充满
            if (printDetails.IsFull)
            {
                //如下就是充满
                //项的情况，所以要判断
                //当个数大于0且不是正在打印中的时候才能打印
                while ((printDetails.IntCount > 0))
                {
                    //如下的才能打印
                    if  (!isprintDocument_PrintPage)
                    {
                        //ArrayList arrlist = ((queuePrintItemRowAndPages)printDetails.arrlistqueuePrintItemRowAndPages[0]).arrlistRow;
                        List<clsKeyValue> arrlist = ((queuePrintItemRowAndPages)printDetails.arrlistqueuePrintItemRowAndPages[0]).arrlistRow;
                        int intP = ((queuePrintItemRowAndPages)printDetails.arrlistqueuePrintItemRowAndPages[0]).intPages;
                        int i = PrintBarcode(printDetails.ShapesFileName, arrlist, intP);

                        if (i > 0)
                        {
                            //到这一步就保存打印记录了
                            //记录打印。
                            ClsDataBase myClsDataBase = new ClsDataBase();
                            // TODO
                            //myClsDataBase.appendPrintedTable(printDetails.strTableName, arrlist, i);

                            OnBarcodePrinted(new printedEventArgs());//发出打印消息
                        }

                        printDetails.arrlistqueuePrintItemRowAndPages.RemoveAt(0);//删除第一项因为已经打印了
                    }

                } 

            }
            else
            {
                //到这里就是非充满打印，跟充满打印不同的地方在于这个是一张一张往上打印的
                PrintBarcode2(printDetails);

            }

        }
       /// <summary>
       /// 这个方法是充满打印
       /// </summary>
       /// <param name="printDetails"></param>
        private void PrintBarcode2(queuePrintItem printDetails)
        {
            //深度拷贝，因为在打印事件中需要调用这个方法
            currentQueueItem = ClsXmlSerialization.DeepCopy<queuePrintItem>(printDetails);

            Shapes myShapes;
            //首先也是取得纸张的行数和列数
            try
            {
                myShapes = ClsXmlSerialization.Load<Shapes>(currentQueueItem.ShapesFileName);
            }
            catch (System.Exception ex)
            {
                return;//如果不能取得，那么就直接返回
            	
            }

            int intNumOfLine = myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
            int intNumOfColumn = myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;


            while ((currentQueueItem.IntCount > 0))
            {
                //如下的才能打印
                if (!isprintDocument_PrintPage)
                {
                    isprintDocument_PrintPage = true;
                    //如下的就是具体构造每一个printDocument了
                    myPrintDocument = new PrintDocument();
                    myPrintDocument.DefaultPageSettings.Landscape = myShapes.BarcodePageSettings.BarcodePaperLayout.LandScape;//设置是否横向
                    int intP = ((queuePrintItemRowAndPages)currentQueueItem.arrlistqueuePrintItemRowAndPages[0]).intPages;
                    //设置多少张纸，这个打印份数只是一个数据的，因为如果第一个没有充满纸张，那么就没必要多份了。
                    intPrintPage = (int)(intP / (intNumOfLine * intNumOfColumn));
                    if (intPrintPage == 0)//有时候会出现不足一张的情况
                        intPrintPage = 1;

                    //根据纸张的连续等问题
                    //设置打印机
                    if (strPrinterName != "")
                    {
                        myPrintDocument.PrinterSettings.PrinterName = strPrinterName;
                    }

                    myPrintDocument.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
                    myPrintDocument.DocumentName = "打印条形码";//设置完后可在打印对话框及队列中显示（默认显示document）
                    //打印输出（过程）
                    myPrintDocument.PrintPage += new PrintPageEventHandler(myPrintDocument_PrintPage);

                    myPrintDocument.OriginAtMargins = false;//从位于可打印区域的左上角打印
                    //必须得设置自定义纸张,要不然会存在顶点问题，比如说有些条形啊打印机的打印宽度为4英寸，而实际纸张宽度不是4英寸，就会存在打印时候顶点是打印机的顶点，而不是实际纸张的顶点。
                    myPrintDocument.DefaultPageSettings.PaperSize = myShapes.BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize;//设置纸张
                    //如下是设置边距
                    /**如下的经测试会产生不可预测的偏移
                    myPrintDocument.DefaultPageSettings.Margins.Top = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Top / 0.254);
                    myPrintDocument.DefaultPageSettings.Margins.Bottom = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Bottom / 0.254);
                    myPrintDocument.DefaultPageSettings.Margins.Left = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Left / 0.254);
                    myPrintDocument.DefaultPageSettings.Margins.Right = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Right / 0.254);

                     * */
                    //每次打印
                    myPrintDocument.PrinterSettings.Copies = (short)intPrintPage;//首先就是一次性全部打印完毕，设置份数就是页数.
                    if (myPrintDocument.PrinterSettings.MaximumCopies < intPrintPage)//如果最高能打印的份数比要打印的页数少，就按照最高能打印的份数.
                        myPrintDocument.PrinterSettings.Copies = (short)myPrintDocument.PrinterSettings.MaximumCopies;

                    //如果能多份打印，就需要提前删掉那些，比如说要打印5个，每张纸可以打印2个，那么这里第一次的份数就是2，但是在打印进程中只会减少2个，会造成重复打印
                    ((queuePrintItemRowAndPages)currentQueueItem.arrlistqueuePrintItemRowAndPages[0]).intPages = ((queuePrintItemRowAndPages)currentQueueItem.arrlistqueuePrintItemRowAndPages[0]).intPages - (myPrintDocument.PrinterSettings.Copies - 1) * (intNumOfLine * intNumOfColumn);

                    try
                    {
                       
                        myPrintDocument.Print();
                        //这里发出打印消息。
                        //isprintDocument_PrintPage = true;// 因为上一个打印进程也需要时间，所以这里先设置了。
                        myPrintDocument.Dispose();//释放


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("没有打印成功，原因：" + ex.Message);
                        isprintDocument_PrintPage = false;
                        return;
                    }

                    isprintDocument_PrintPage = false;

                }
            }

            //保存打印记录，这里用比较省事的方法
            ClsDataBase myClsDataBase = new ClsDataBase();
            foreach (queuePrintItemRowAndPages item in printDetails.arrlistqueuePrintItemRowAndPages)
            {
                // TODO
                // myClsDataBase.commandAddPrintedRecord(printDetails.strTableName, item.arrlistRow, item.intPages);
                OnBarcodePrinted(new printedEventArgs());//发出打印消息

            }
        }

        void myPrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            //isprintDocument_PrintPage = true;
            //如下就是构造图形了。
            Shapes myShapes;
            //首先也是取得纸张的行数和列数
            try
            {
                myShapes = ClsXmlSerialization.Load<Shapes>(currentQueueItem.ShapesFileName);
                myShapes.Zoom = 1;//比例为1才能打印
            }
            catch (System.Exception ex)
            {
                return;//如果不能取得，那么就直接返回

            }

            int intNumOfLine = myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
            int intNumOfColumn = myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;

            Graphics g=e.Graphics;

            //因为打印条形码会有每行有多个条形码，还有其他的间距问题。这里不用考虑页面边距之类的问题，因为这个已经在打印类中考虑了。
            for (int i = 0; i < myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn; i++)
            {
                for (int j = 0; j < myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine; j++)
                {
                    //我将页面边距加到这里来了
                    float fx = myShapes.BarcodePageSettings.BarcodePaperLayout.Left + i * (myShapes.BarcodePageSettings.BarcodePaperLayout.ModelWidth + myShapes.BarcodePageSettings.BarcodePaperLayout.HorizontalInterval);
                    float fy = myShapes.BarcodePageSettings.BarcodePaperLayout.Top + j * (myShapes.BarcodePageSettings.BarcodePaperLayout.ModelHeight + myShapes.BarcodePageSettings.BarcodePaperLayout.VerticalInterval);
                    

                    if (currentQueueItem.IntCount>0)
                    {
                        //ArrayList arrlist = ((queuePrintItemRowAndPages)currentQueueItem.arrlistqueuePrintItemRowAndPages[0]).arrlistRow;
                        List<clsKeyValue> arrlist = ((queuePrintItemRowAndPages)currentQueueItem.arrlistqueuePrintItemRowAndPages[0]).arrlistRow;
                        if (arrlist.Count>0)
                        {
                            myShapes.arrlistKeyValue = arrlist;
                        }
                        myShapes.DrawShapes(g, fx, fy);
                        currentQueueItem.removeFirstPages();// 消减第一个而已
                    }
                    else
                    {
                        break;//如果小于等于0就表示没有项目了
                    }

                }

            }

            isprintDocument_PrintPage = false;
            //throw new NotImplementedException();
        }


        public static queuePrintItem currentQueueItem;//当前打印的，在非充满中需要这个

        //变量替换，测试成功，如下的准备注释掉，因为不需要了。
        /**
        public static XmlDocument populateVariable(XmlDocument xmlDoc, ArrayList arrlistRow)
        {

            //有时候会遇到没arrlistRow为空的情况,那样子就不用变量替换了.
            if (arrlistRow == null)
                return xmlDoc;

            //用xpath搜索的方法来替换。
            XmlDocument myxml = xmlDoc;//赋值保存而已
            
            foreach (clsKeyValue keyvalue in arrlistRow)
            {
                XmlNodeList nodeList1 = myxml.SelectNodes("//VariableText[@variableName='" + keyvalue.Key + "']");

                foreach (XmlNode myXmlNode in nodeList1)
                {
                    myXmlNode.InnerText = keyvalue.Value;
                }
                XmlNodeList nodeList2 = myxml.SelectNodes("//Barcode[@variableName='" + keyvalue.Key + "']");

                foreach (XmlNode myXmlNode in nodeList2)
                {
                    myXmlNode.InnerText = keyvalue.Value;
                }
            }

            return myxml;

        }

         * */

        
        
        private int PrintBarcode(string ShapeFileName, List<clsKeyValue> arrlist, int intPage)
        {
            //首先清零
            intPrintPage = 0;
            try
            {
                barcodeCanvas = new VestShapes.UserControlCanvas();
                barcodeCanvas.Loader(ShapeFileName);//先加载模板

            }
            catch (Exception ex)
            {
                ClsErrorFile.WriteLine("不能加载模板，所以不能打印", ex);

                MessageBox.Show("不能加载模板，所以不能打印，原因是"+ ex.Message);

                return 0;
                //throw;
            }

            barcodeCanvas.setArrKeyValue(arrlist);//这个是导入变量用。

            int intNumOfLine = barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfLine;
            int intNumOfColumn = barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.NumberOfColumn;

            myPrintDocument.DefaultPageSettings.Landscape = barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.LandScape;//设置是否横向
            
            //设置多少张纸
            intPrintPage = (int)(intPage / (intNumOfLine*intNumOfColumn));
            if ((intPage %( intNumOfLine*intNumOfColumn)) != 0)//如果不能整除就多一张纸
                intPrintPage++;
            if (intPrintPage == 0)//有时候会出现不足一张的情况
                intPrintPage = 1;

            int intQty = intPrintPage * intNumOfLine*intNumOfColumn;
            //如上是计算了要打印多少页，因为有些条形码纸一行有多个，需要在这里打印。


            //根据纸张的连续等问题
            //设置打印机
            if (strPrinterName!="")
            {
                myPrintDocument.PrinterSettings.PrinterName = strPrinterName;
            }

            myPrintDocument.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
            myPrintDocument.DocumentName = "打印条形码";//设置完后可在打印对话框及队列中显示（默认显示document）
            //打印输出（过程）
            myPrintDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            
            myPrintDocument.OriginAtMargins = false ;//从位于可打印区域的左上角打印
            //必须得设置自定义纸张,要不然会存在顶点问题，比如说有些条形啊打印机的打印宽度为4英寸，而实际纸张宽度不是4英寸，就会存在打印时候顶点是打印机的顶点，而不是实际纸张的顶点。
            myPrintDocument.DefaultPageSettings.PaperSize = barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.BarcodePaperSize;//设置纸张
            //如下是设置边距
            /**如下的经测试会产生不可预测的偏移
            myPrintDocument.DefaultPageSettings.Margins.Top = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Top / 0.254);
            myPrintDocument.DefaultPageSettings.Margins.Bottom = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Bottom / 0.254);
            myPrintDocument.DefaultPageSettings.Margins.Left = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Left / 0.254);
            myPrintDocument.DefaultPageSettings.Margins.Right = (int)(barcodeCanvas.myShapes.BarcodePageSettings.BarcodePaperLayout.Right / 0.254);

             * */
            //每次打印
            myPrintDocument.PrinterSettings.Copies = (short)intPrintPage;//首先就是一次性全部打印完毕，设置份数就是页数.
            if (myPrintDocument.PrinterSettings.MaximumCopies < intPrintPage)//如果最高能打印的份数比要打印的页数少，就按照最高能打印的份数.
                myPrintDocument.PrinterSettings.Copies = (short)myPrintDocument.PrinterSettings.MaximumCopies;

            try
            {
                isprintDocument_PrintPage = true;

                myPrintDocument.Print();
                //这里发出打印消息。
                
                myPrintDocument.Dispose();//释放

            }
            catch (Exception ex)
            {
                MessageBox.Show("没有打印成功，原因：" + ex.Message);
                isprintDocument_PrintPage = false;
                return 0;
            }

            //如下是判断是否需要打印间隔
            if (isPrintJiange)
            {
                PrintDocument printDocPrintJianGe = new PrintDocument();
                printDocPrintJianGe.PrintController = new StandardPrintController();
                printDocPrintJianGe.DocumentName = "打印间隔";
                printDocPrintJianGe.PrinterSettings.Copies = 1;// 间隔只打一份。
                printDocPrintJianGe.PrintPage += new PrintPageEventHandler(printDocPrintJianGe_PrintPage);

                try
                {
                    printDocPrintJianGe.Print();
                    printDocPrintJianGe.Dispose();//释放资源

                }
                catch (Exception ex)
                {
                    MessageBox.Show("没有打印成功，原因：" + ex.Message);
                    return 0;
                }

            }

            return intQty;//返回实际打印的数量

        }

            /**
        private int  PrintBarcode(XmlDocument xmlBarcodeModel , int intPage)
        {


            //首先清零
            intPrintPage = 0;
            fltPaperHeight = 0;
            fltPaperWidth = 0;
            isContinuousMedias = false;
            intNumOfLine = 1;
  
            imageBarcode = xmlToBarcodeImage(xmlBarcodeModel);//这个调用一次就可以了。

            XmlElement myxmlElePaper=(XmlElement)xmlBarcodeModel.SelectNodes("//paper").Item(0);
            //读取纸张相关信息
            //都得判断是否是空值
            fltPaperWidth = strAttributeValueToFloat(myxmlElePaper, "PaperWidth");
            fltPaperHeight =strAttributeValueToFloat(myxmlElePaper,"PaperHeight");
            intNumOfLine =(int) strAttributeValueToFloat( myxmlElePaper,"NumOfLables");
            fltHorizontalRepeatDistance = strAttributeValueToFloat(myxmlElePaper,"HorizontalRepeatDistance");
            fltVarticalRepeatDistance = strAttributeValueToFloat(myxmlElePaper,"VarticalRepeatDistance");

            //判断是不是连续纸。
            if (myxmlElePaper.GetAttribute("isContinuousMedias").ToLower() == "true")
            {
                isContinuousMedias = true;
            }

            //设置多少张纸
            intPrintPage = (int)(intPage / intNumOfLine);
            if ((intPage % intNumOfLine) != 0)//如果不能整除就多一张纸
                intPrintPage++;
            if (intPrintPage == 0)//有时候会出现不足一张的情况
                intPrintPage = 1;

            int intQty = intPrintPage * intNumOfLine;


            //根据纸张的连续等问题
            //设置打印机
            myPrintDocument.PrinterSettings.PrinterName = strPrinterName;
            myPrintDocument.PrintController = new StandardPrintController();//这个据说可以不显示那个打印进度对框框
            myPrintDocument.DocumentName = "打印条形码";//设置完后可在打印对话框及队列中显示（默认显示document）
            //打印输出（过程）
            myPrintDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);

            //每次打印
            myPrintDocument.PrinterSettings.Copies = (short)intPrintPage;//首先就是一次性全部打印完毕，设置份数就是页数.
            if (myPrintDocument.PrinterSettings.MaximumCopies < intPrintPage)//如果最高能打印的份数比要打印的页数少，就按照最高能打印的份数.
                myPrintDocument.PrinterSettings.Copies =(short )myPrintDocument.PrinterSettings.MaximumCopies;

            try
            {
                myPrintDocument.Print();
                


            }
            catch (Exception ex)
            {
                MessageBox.Show("没有打印成功，原因：" + ex.Message);
                return 0;
            }

            //如下是判断是否需要打印间隔
            if (isPrintJiange)
            {
                PrintDocument printDocPrintJianGe = new PrintDocument();
                printDocPrintJianGe.PrintController = new StandardPrintController();
                printDocPrintJianGe.DocumentName = "打印间隔";
                printDocPrintJianGe.PrintPage += new PrintPageEventHandler(printDocPrintJianGe_PrintPage);

                try
                {
                    printDocPrintJianGe.Print();



                }
                catch (Exception ex)
                {
                    MessageBox.Show("没有打印成功，原因：" + ex.Message);
                    return 0;
                }

            }

            return intQty;//返回实际打印的数量

        }
             * */

        void printDocPrintJianGe_PrintPage(object sender, PrintPageEventArgs e)
        {
            //我现阶段这个打印间隔只是打印字符串而已，以后会实现打印xml

            //如下只是打印一断文字而已
            Font font1 = new Font("Arial", 16);
            Brush drawBrush = new SolidBrush(Color.Black);
            e.Graphics.DrawString(strJianGe, font1, drawBrush, new Point(0, 0));
            drawBrush.Dispose();
            font1.Dispose();

            //throw new NotImplementedException();
        }

       /// <summary>
        /// 这个变量表示正在调用printDocument_PrintPage中
       /// </summary>
        private static  bool isprintDocument_PrintPage = false;

        private    void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            isprintDocument_PrintPage = true;

            //如下就是调用画布的打印条码，这个画布会自动打印的，根据条形码纸的宽和高，间距，每行多少个
            barcodeCanvas.DrawBarcode(e.Graphics);


            //e.PageSettings.PrintableArea
            /**
            //如下是如果没有注册，在左上角写上文字" BarcodeTerminator"
            if (!isZhuCe)
            {
                e.Graphics.DrawString("没有注册", new Font("Arial", 6), new SolidBrush(Color.Black), new Point(0, 0));

            }
             * */


            if (intPrintPage < myPrintDocument.PrinterSettings.Copies)//如果要打印的少于份数
                myPrintDocument.PrinterSettings.Copies = (short)intPrintPage;

            intPrintPage = intPrintPage - myPrintDocument.PrinterSettings.Copies;//减少份数

            e.HasMorePages = true;

            if ((intPrintPage <= 0))//如果打印已经打印完了，那么就设置不用继续打印了。
            {
                e.HasMorePages = false;
            }

            isprintDocument_PrintPage = false;//已经完成了

            /**
            //不连续纸张
            Graphics g = e.Graphics;
            g.PageUnit = GraphicsUnit.Millimeter;
            g.DrawImage(imageBarcode, new PointF(0, 0));

            //如果每行多过2个
            if (intNumOfLine > 1)
            {
                for (int i = 1; i < intNumOfLine; i++)
                {
                    g.DrawImage(imageBarcode, new PointF((fltPaperWidth + fltHorizontalRepeatDistance) * i, 0.0f));

                }
            }

            //判断是否是连续纸张，如果是的话，就加上一段空白,我已经做过测试了，填充黑色背景测试。
            if (isContinuousMedias)
            {
                Bitmap b = new Bitmap(((int)((intNumOfLine * fltPaperWidth + (intNumOfLine - 1) * fltHorizontalRepeatDistance) * fltDPIX / 25.4)), (int)(fltVarticalRepeatDistance * fltDPIX / 25.4));
                b.SetResolution(fltDPIX, fltDPIY);//设置分辨率
                Graphics g2 = Graphics.FromImage(b);
                g2.PageUnit = GraphicsUnit.Millimeter;//设置单位为毫米
                g2.Clear(Color.White);       //填充北京         
                g.DrawImage(b, new PointF(0f, (fltPaperHeight)));//这个就是竖直方向上的间隔。
            }


            if (intPrintPage < myPrintDocument.PrinterSettings.Copies)//如果要打印的少于份数
                myPrintDocument.PrinterSettings.Copies = (short)intPrintPage;

            intPrintPage = intPrintPage - myPrintDocument.PrinterSettings.Copies;//减少份数

            e.HasMorePages = true;

            if ((intPrintPage <= 0))//如果打印已经打印完了，那么就设置不用继续打印了。
            {
                e.HasMorePages = false;
            }
             * */

        }



        //如下的这个也不需要了。

        private   Bitmap imageCut(Bitmap b, int StartX, int StartY, int iWidth, int iHeight)
        {
            if (b == null)
            {
                return null;
            }
            int w = b.Width;
            int h = b.Height;
            if (StartX >= w || StartY >= h)
            {
                return null;
            }
            if (StartX + iWidth > w)
            {
                iWidth = w - StartX;
            }
            if (StartY + iHeight > h)
            {
                iHeight = h - StartY;
            }
            try
            {
                Bitmap bmpOut = new Bitmap(iWidth, iHeight);
                Graphics g = Graphics.FromImage(bmpOut);

                //如下是我添加的，我打算这个用MM单位。
                g.PageUnit = GraphicsUnit.Millimeter;

                g.DrawImage(b, new Rectangle(0, 0, iWidth, iHeight), new Rectangle(StartX, StartY, iWidth, iHeight), GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }

        //只所以需要这个是因为某些属性没有赋值，是空值,这个方法就是默认返回0，而不是空值
        //r如下的方法不需要了。
        /**
        public  static float strAttributeValueToFloat(XmlElement myXmlElement, string strAttributeName)
        {
            float f = 0f;
            if (myXmlElement.GetAttribute(strAttributeName) != "")
                f = Convert.ToSingle(myXmlElement.GetAttribute(strAttributeName));
            return f;
        }
         * */
            
    }

    //如下是判断打印是否结束的
    public static class PrinterCheck
    {
        public enum PrinterStatus
        {
            其他状态 = 1,
            未知,
            空闲,
            正在打印,
            预热,
            停止打印,
            打印中,
            离线
        }

        /// <summary>
        /// 获取打印机的当前状态
        /// </summary>
        /// <param name="PrinterDevice">打印机设备流程内容</param>
        /// <returns>打印机状态</returns>
        public static PrinterStatus GetPrinterStatus(string PrinterDevice)
        {            
            PrinterStatus ret = 0;

            try
            {
                string path = @"win32_printer.DeviceId='" + PrinterDevice + "'";
                ManagementObject printer = new ManagementObject(path);
                printer.Get();
                ret = (PrinterStatus)Convert.ToInt32(printer.Properties["PrinterStatus"].Value);


            }
            catch (Exception ex) 
            {
                ClsErrorFile.WriteLine(ex);
                //Console.Error.WriteLine(ex.Message);
            }

            return ret;
        }
    }
}
