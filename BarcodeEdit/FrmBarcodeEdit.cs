using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using Io.Github.Kerwinxu.LibShapes.Core;
using Io.Github.Kerwinxu.LibShapes.Utils;
using Io.Github.Kerwinxu.LibShapes.Core.Print;
using System.Drawing.Printing;

namespace BarcodeTerminator
{
    public partial class FrmBarcodeEdit : Form
    {
        #region 如下几个是私有变量

        /// <summary>
        ///  表格信息
        /// </summary>
        private DataTable CurrentDataTable;

        /// <summary>
        /// 表格信息的下标
        /// </summary>
        private int index=-1;

        /// <summary>
        ///  文件名称
        /// </summary>
        private string modelfileName;

        /// <summary>
        /// 取得文件名称
        /// </summary>
        /// <returns></returns>
        public string getModelFileName() { return this.modelfileName; }

        #endregion
        /// <summary>
        /// 导入模板文档
        /// </summary>
        /// <param name="modelFileName"></param>
        public FrmBarcodeEdit(string modelFileName):this()
        {
            loadModelFile(modelFileName);
        }

        /// <summary>
        /// 可以导入模板文档以及excel数据
        /// </summary>
        /// <param name="modelFileName"></param>
        /// <param name="dt"></param>
        public FrmBarcodeEdit(string modelFileName, DataTable dt) : this(modelFileName)
        {
            this.CurrentDataTable = dt;
        }

        public FrmBarcodeEdit(DataTable dt) : this()
        {
            this.CurrentDataTable = dt;
            loadDatatable();
        }


        public FrmBarcodeEdit()
        {
            InitializeComponent();
            // 界面上的初始化放在这里。
            canvasResize();
            toolboxResize();
            // 画布和工具箱之间是有关联的
            this.toolBox.canvas = this.canvas;
            this.canvas.objectSelected += this.toolBox.objectSelected;              // 选择更改事件
            this.canvas.stateChanged += this.toolBox.stateChanged;                  // 状态更改事件
            this.toolBox.PropertyValueChanged += this.canvas.propertyValueChanged;  // 属性更改事件
            // 
            init_printers(); // 加载打印机

        }

        /// <summary>
        /// 画布的自动更改尺寸
        /// </summary>
        private void canvasResize()
        {
            int _spacing = 5;
            this.canvas.Location = new Point(_spacing, _spacing);
            this.canvas.Width = this.splitContainer2.Panel1.Width - 2 * _spacing;
            this.canvas.Height = this.splitContainer2.Panel1.Height - 2 * _spacing;

        }

        private void init_printers()
        {
            // 加载打印机的
            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                combo_printers.Items.Add(item);
            }
            // 这里有一个默认的打印机
            var doc = new PrintDocument();
            combo_printers.Text = doc.DefaultPageSettings.PrinterSettings.PrinterName;
        }

        /// <summary>
        /// 工具箱的自动更改尺寸。
        /// </summary>
        private  void toolboxResize()
        {
            int _spacing = 5;
            this.toolBox.Location = new Point(_spacing, _spacing);
            this.toolBox.Width = this.splitContainer2.Panel2.Width - 2 * _spacing;
            this.toolBox.Height = this.splitContainer2.Panel2.Height - 2 * _spacing;
        }

        private void splitContainer2_Panel1_Resize(object sender, EventArgs e)
        {
            canvasResize();
        }

        private void splitContainer2_Panel2_SizeChanged(object sender, EventArgs e)
        {
            toolboxResize();
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadModelFile();
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveModelFile();
        }

        private void 另存为AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveAsModelFile();
        }

        private void 导入EXCEL表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadexcel();
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            myExit();
        }

        private void 撤消UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.undo();
        }

        private void 重复RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.redo();
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.cut();
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.copy();
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.paste();
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.selectAll();
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.deleteShapes();
        }

        private void 讲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.zoomToScreen();
        }

        private void 向前一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.forward();
        }

        private void 向后一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.backward();
        }

        private void 移到最前ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.forwardToFront();
        }

        private void 移到最后ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.backwardToEnd();
        }

        private void 分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.mergeGroup();
        }

        private void 解除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.canvas.cancelGroup();
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmHelp frmHelp = new FrmHelp();
            frmHelp.ShowDialog();
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            newFile();
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            loadModelFile();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            saveModelFile();
        }

        private void 剪切UToolStripButton_Click(object sender, EventArgs e)
        {
            this.canvas.cut();
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            this.canvas.copy();
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            this.canvas.paste();
        }

        private void 导入EXCEL_Click(object sender, EventArgs e)
        {
            loadexcel();
        }


        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (CurrentDataTable != null && CurrentDataTable.Rows.Count > 0) index -=1;
            updateBinding();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            if (CurrentDataTable != null && CurrentDataTable.Rows.Count > 0) index = 0;
            updateBinding();
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (CurrentDataTable != null && index < CurrentDataTable.Rows.Count-1) index += 1;
            updateBinding();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            if (CurrentDataTable != null && index < CurrentDataTable.Rows.Count - 1) index = CurrentDataTable.Rows.Count - 1;
            updateBinding();
        }

        private void btnTestPrint_Click(object sender, EventArgs e)
        {
            printShapes(1);
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            int num1 = 0;
            if (!int.TryParse(txtQtyOfWantToPrinted.Text, out num1)) { MessageBox.Show("打印数量转换失败"); return; }
            if (num1 == 0) MessageBox.Show("打印数量为0");
            float num2 = 0;
            if (!float.TryParse(txtSunHao.Text, out num2)){ MessageBox.Show("损耗比例转换失败");return; }
            if (num2 > 0) num1 = (int)(num1 * (1 + num2));
            printShapes(num1);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            int num = 0;
            if (!int.TryParse(txtCurrentPrintPage.Text, out num)) { MessageBox.Show("打印数量转换失败"); return; }
            if (num == 0) MessageBox.Show("打印数量为0");
            printShapes(num);
        }

        /// <summary>
        /// 加载模型文件
        /// </summary>
        private void loadModelFile()
        {
            openFileDialog1.Filter = "模板文件|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                loadModelFile(openFileDialog1.FileName);
            }
        }

        private void loadModelFile(string modelFileName)
        {
            this.modelfileName = modelFileName;
            // 然后读取这个文件
            var shapes = Shapes.load(modelFileName);
            // 然后更改画布的
            this.canvas.shapes = shapes;
            this.canvas.Refresh();
        }

        private void saveModelFile(string modelFileName)
        {
            if (this.canvas.shapes != null)
            {
                canvas.shapes.save(modelFileName);
            }
        }

        private void saveModelFile()
        {
            // 这里判断一下是否有名字
            if (string.IsNullOrEmpty(this.modelfileName))
            {
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.DefaultExt = ".json";
                saveFileDialog1.Filter = "模板文件|*.json";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.modelfileName = saveFileDialog1.FileName;
                }

            }
            saveModelFile(this.modelfileName);
        }

        /// <summary>
        /// 新建一个文档
        /// </summary>
        private void newFile()
        {
            //清空数据
            this.modelfileName = null;
            this.CurrentDataTable = null;
            this.index = -1;
            this.canvas.shapes = new Shapes();
        }

        private string [] getColumnNames (DataTable dt)
        {
            List<string> columnNameList = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                columnNameList.Add(col.ColumnName);//获取到DataColumn列对象的列名
            }
            return columnNameList.ToArray();
        }

        private void loadDatatable()
        {
            // 然后这里默认是第一行。
            index = 0;
            bindingNavigatorCountItem.Text = $"/ {CurrentDataTable.Rows.Count}";
            updateBinding();
            // 这里设置有什么打印的变量。
            var var_names = getColumnNames(CurrentDataTable);
            comboBoxQtyOfWantToPrinted.Items.Clear();
            comboBoxQtyOfWantToPrinted.Items.AddRange(var_names);
        }

        private void loadexcel()
        {
            // 读取excel表格
            openFileDialog1.Filter = "excel文件|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentDataTable = ExcelData.LoadExcel(openFileDialog1.FileName);
                loadDatatable();
            }
        }

        /// <summary>
        /// 取得
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private Dictionary<string, string>  getDict(int i)
        {
            var _data = CurrentDataTable.Rows[i];
            // 然后将这个组成字典形式
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (DataColumn item in CurrentDataTable.Columns)
            {
                dict[item.ColumnName] = _data[item.ColumnName].ToString();
            }
            return dict;
        }

        private void updateBinding()
        {
            if (index >= 0)
            {
                // 取得变量
                var dict = getDict(index);
                // 设置变量
                this.canvas.setVars(dict);
                // 更新打印的数量
                if (dict.ContainsKey(comboBoxQtyOfWantToPrinted.Text))
                {
                    txtQtyOfWantToPrinted.Text = dict[comboBoxQtyOfWantToPrinted.Text];
                }
            }
            // 更新几个状态
            if (index < 0 )
            {
                bindingNavigatorPositionItem.Text = "0";
                bindingNavigatorCountItem.Text = "0";

            }
            else
            {
                bindingNavigatorPositionItem.Text = (index + 1).ToString() ;
            }

            bindingNavigatorMoveFirstItem.Enabled = index > 0;
            bindingNavigatorMovePreviousItem.Enabled = index > 0;
            if (CurrentDataTable != null)
            {
                bindingNavigatorMoveNextItem.Enabled = index < CurrentDataTable.Rows.Count - 1;
                bindingNavigatorMoveLastItem.Enabled = index < CurrentDataTable.Rows.Count - 1;
            }
            else
            {
                bindingNavigatorMoveNextItem.Enabled = false;
                bindingNavigatorMoveLastItem.Enabled = false;
            }
            

            
        }



        private void 帮助LToolStripButton_Click(object sender, EventArgs e)
        {
            FrmHelp frmHelp = new FrmHelp();
            frmHelp.ShowDialog();
        }

        /// <summary>
        /// 打印。
        /// </summary>
        /// <param name="num"></param>
        private void printShapes(int num)
        {
            PrintItem printItem = new PrintItem();

            if (index < 0 )
            {
                printItem.Valss.Add(new Dictionary<string, string>()); // 空白的变量
            }
            else
            {
                // 首先构造变量
                var dict = getDict(index);
                printItem.Valss.Add(dict);
            }

            // 然后构造数量
            printItem.PrintCounts.Add(1);
            // 充满打印
            printItem.isFullPrint = chkIsFull.Checked;
            printItem.Shapes = this.canvas.shapes;
            // 添加到
            PrintManagerImpl printManagerImpl = new PrintManagerImpl();
            printManagerImpl.addPrintItem(printItem);

        }

        private void myExit()
        {
            // 不管怎么样，只要有图形，就有保存或者另存为
            if (this.canvas.shapes.lstShapes.Count > 0 && MessageBox.Show("需要保存文件吗?") == DialogResult.OK)
            {
                saveModelFile();
            }
        }

        /// <summary>
        /// 另存为。
        /// </summary>
        private void saveAsModelFile()
        {
   
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.DefaultExt = ".json";
            saveFileDialog1.Filter = "模板文件|*.json";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.modelfileName = saveFileDialog1.FileName;
            }

            saveModelFile(this.modelfileName);
        }



        private void comboBoxQtyOfWantToPrinted_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 这里需要判断是否有这个变量
            if (index >= 0)
            {
                var dict = getDict(index);
                // 如果有这个
                if (dict.ContainsKey(comboBoxQtyOfWantToPrinted.Text))
                {
                    txtQtyOfWantToPrinted.Text = dict[comboBoxQtyOfWantToPrinted.Text];
                }
                else
                {
                    txtQtyOfWantToPrinted.Text = string.Empty;
                }
            }
            else
            {
                txtQtyOfWantToPrinted.Text = string.Empty;
            }
            

        }
    }
}
