using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;

using Io.Github.Kerwinxu.BarcodeManager.ClsBarcodePrint;
using Io.Github.Kerwinxu.LibShapes.Core;


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
        private int index;

        /// <summary>
        ///  文件名称
        /// </summary>
        private string modelfileName;

        #endregion

        public FrmBarcodeEdit(string modelFileName):this()
        {
            loadModelFile(modelFileName);
        }

        public FrmBarcodeEdit(string modelFileName, DataTable dt) : this(modelFileName)
        {
            this.CurrentDataTable = dt;
        }




        public FrmBarcodeEdit()
        {
            InitializeComponent();
            // 界面上的初始化放在这里。
            canvasResize();
            toolboxResize();
            // 画布和工具箱之间是有关联的
            this.toolBox.canvas = this.canvas;
            this.canvas.objectSelected += this.toolBox.objectSelected;
            this.canvas.stateChanged += this.toolBox.stateChanged;
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
            // todo 新建一个文档。
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
            // todo 另存为一个文档
        }

        private void 导入EXCEL表格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 导入excel表格
        }

        private void 退出XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 退出
        }

        private void 撤消UToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 撤销
        }

        private void 重复RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 重做
        }

        private void 剪切TToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 剪切
        }

        private void 复制CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 复制
        }

        private void 粘贴PToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 粘贴
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 全选
        }

        private void 删除DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 删除
        }

        private void 讲ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo  放到到屏幕
        }

        private void 向前一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 上前一层
        }

        private void 向后一层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 向后一层
        }

        private void 移到最前ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 移到最前 
        }

        private void 移到最后ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 移动到最后
        }

        private void 分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 分组
        }

        private void 解除分组ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 取消分组
        }

        private void 关于AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // todo 关于
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            // todo 新建
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
            // todo 剪切
        }

        private void 复制CToolStripButton_Click(object sender, EventArgs e)
        {
            // todo 复制
        }

        private void 粘贴PToolStripButton_Click(object sender, EventArgs e)
        {
            // todo 粘贴
        }

        private void 导入EXCEL_Click(object sender, EventArgs e)
        {
            // todo 导入excel表格
        }

        private void toolStripButtonSellectPrinting_Click(object sender, EventArgs e)
        {
            // todo 选择打印机
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            // todo 数据向前
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            // todo 数据移动到最前
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            // todo 数据向后
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            // todo 数据移动到最后
        }

        private void btnTestPrint_Click(object sender, EventArgs e)
        {
            // todo 测试打印，就是打印一个。
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            // todo 按照损耗打印。
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            // todo 手动输入数量进行打印
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
    }
}
