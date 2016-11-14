using System.Windows.Forms;

using System.Xml;




using System.Drawing.Printing;



namespace BarcodeTerminator
{
    public partial class FrmSelectPrinter : Form
    {
        PrintDocument myPrintDoc = new PrintDocument();

        //如下的是返回的参数
        public string strPrinterName;//打印机名字

        public FrmSelectPrinter()
        {
            InitializeComponent();

            //初始化
            //将默认打印机赋值给strPrinterName
            //ClsBarcodePrint.strPrinterName = myPrintDoc.PrinterSettings.PrinterName;

            //将所有打印机名添加到lstPrinterNames
            foreach (object obj in PrinterSettings.InstalledPrinters)
            {
                lstPrinterNames.Items.Add(obj.ToString());

            }


            
        }

        private void btnOk_Click(object sender, System.EventArgs e)
        {
            //首先判断是否有选择打印机。
            if (lstPrinterNames.SelectedIndex<0)
            {
                MessageBox.Show("请选择打印机");
                return;
            }

            //到这里就是已经选择了打印机了，做两件事情
            //1、将打印机赋值给clsBarcodePrint中
            //2、取得分辨率
            //并保存如上两个信息

            strPrinterName  = lstPrinterNames.SelectedItem.ToString();


            /**没有必要取得打印机的分辨率了。
            ClsTestDPI myTestDPI = new ClsTestDPI(ClsBarcodePrint.strPrinterName);

            //如下是保存这个信息


            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(Application.StartupPath + "\\xmlAPP.xml");

            XmlElement xmlElePrinter = (XmlElement)xmlDoc.SelectNodes("//printer").Item(0);
            //xmlElePrinter.SetAttribute("DPIX", clsBarcodePrint.fltDPIX.ToString());
           // xmlElePrinter.SetAttribute("DPIY", clsBarcodePrint.fltDPIY.ToString());
            xmlElePrinter.SetAttribute("PrinterName", ClsBarcodePrint.strPrinterName);

            xmlDoc.Save(Application.StartupPath + "\\xmlAPP.xml");
             * */
            DialogResult = DialogResult.OK;

            this.Dispose();

        }

        private void btnCancel_Click(object sender, System.EventArgs e)
        {
            myPrintDoc.Dispose();
            this.Dispose();//直接销毁
        }
    }
}
