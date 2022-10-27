using BarcodeTerminator;
using Io.Github.Kerwinxu.LibShapes.Core;
using Io.Github.Kerwinxu.LibShapes.Core.Print;
using Io.Github.Kerwinxu.LibShapes.Core.Serialize;
using Io.Github.Kerwinxu.LibShapes.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BarcodeManager
{
    public partial class FrmMain2 : Form
    {
        public FrmMain2()
        {
            InitializeComponent();

            // 画布要自适应大小。
            canvas_resize();
            // 表格控件要自适应大小
            tableLayout_resize();
            // 要整行选取
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true; // 不修改。
            // 初始化打印机
            init_printers();
            // 加载以前的模板文件路径
            load_shapesFileNames();
            // 加载到组合框。
            combo_models_init();
            // 默认选择多行
            this.dataGridView1.MultiSelect = true;

        }

        /// <summary>
        ///  加载过的模型文件路径
        /// </summary>
        private List<string> shapesFileNames;

        private string file_conf = "shapesFileNames.json";

        ISerialize jsonSerialize = new JsonSerialize();
        private void load_shapesFileNames()
        {
            if (System.IO.File.Exists(file_conf))
            {
                shapesFileNames = jsonSerialize.DeserializeObjectFromFile<List<string>>(file_conf);
                
            }
            else
            {
                shapesFileNames = new List<string>();
            }
            
        }

        private void combo_models_init()
        {
            this.combo_models.Items.Clear();
            this.combo_models.Items.AddRange(
                shapesFileNames.Select(x => System.IO.Path.GetFileNameWithoutExtension(x)) // 取得文件名
                .ToArray()
                );
            if (this.combo_models.Items.Count > 0)
            {
                this.combo_models.Text = this.combo_models.Items[0].ToString();
            }
        }

        private void save_shapesFileNames()
        {
            jsonSerialize.SerializeObjectToFile(shapesFileNames, file_conf);
        }


        private void init_printers()
        {
            // 初始化打印机
            // 加载打印机的
            foreach (var item in PrinterSettings.InstalledPrinters)
            {
                combo_printers.Items.Add(item);
            }
            // 这里有一个默认的打印机
            var doc = new PrintDocument();
            combo_printers.Text = doc.DefaultPageSettings.PrinterSettings.PrinterName;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"https://xinyiya.taobao.com/");
        }

        private void canvas_resize()
        {
            int i = 5;
            this.canvas.Location = new Point(i, i);
            this.canvas.Width = this.splitContainer3.Panel1.Width - 2 * i;
            this.canvas.Height = this.splitContainer3.Panel1.Height - 2 * i;
        }

        private void tableLayout_resize()
        {
            int i = 5;
            this.tableLayoutPanel1.Location = new Point(i, i);
            this.tableLayoutPanel1.Width = this.splitContainer3.Panel2.Width - 2 * i;
            this.tableLayoutPanel1.Height = this.splitContainer3.Panel2.Height - 2 * i;
        }


        private void splitContainer3_Panel2_SizeChanged(object sender, EventArgs e)
        {
            tableLayout_resize();
        }


        private void btn_load_excel_Click(object sender, EventArgs e)
        {
            // todo 导入excel表格
            openFileDialog1.Filter = "excel文件|*.xls;*.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var dt = ExcelData.LoadExcel(openFileDialog1.FileName);
                dataGridView1.DataSource = dt; // 这个显示数据
                // 将原先的变量清空
                combo_qty_var_names.Items.Clear();
                // 新的变量名称
                combo_qty_var_names.Items.AddRange(getColumnNames(dt));
                if (dt.Rows.Count > 0) row_selected(0);

            }
            else
            {
                // 如果没有导入文件，这里的数据就算
                dataGridView1.DataSource = null;
            }
        }

        private string[] getColumnNames(DataTable dt)
        {
            List<string> columnNameList = new List<string>();
            foreach (DataColumn col in dt.Columns)
            {
                columnNameList.Add(col.ColumnName);//获取到DataColumn列对象的列名
            }
            return columnNameList.ToArray();
        }

        private void splitContainer3_Panel1_SizeChanged(object sender, EventArgs e)
        {
            canvas_resize();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private Dictionary<string, string> getVars(int index)
        {
            Dictionary<string, string> result = new Dictionary<string, string>(); ;
            // 
            foreach (DataGridViewColumn item in dataGridView1.Columns)
            {
                var column_name = item.HeaderText;
                var value = this.dataGridView1.Rows[index].Cells[column_name].Value.ToString();
                result[column_name] = value;
            }
            return result;
        }

        /// <summary>
        /// 选择单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                row_selected(e.RowIndex);
            }
        }

        /// <summary>
        /// 行选择更改，
        /// </summary>
        /// <param name="index"></param>
        private void row_selected(int index)
        {
            var dict = getVars(index); // 取得变量
            if (this.canvas != null) canvas.setVars(dict); // 将变量发送给画布
            if (!string.IsNullOrEmpty(combo_qty_var_names.Text) && dict.ContainsKey(combo_qty_var_names.Text))
            {
                btn_print_qty.Text = dict[combo_qty_var_names.Text];
            }
            else
            {
                btn_print_qty.Text = string.Empty;
            }
        }

        private void combo_qty_var_names_TextChanged(object sender, EventArgs e)
        {
            if (this.dataGridView1.DataSource != null && this.dataGridView1.CurrentRow.Index > -1)
            {
                row_selected(this.dataGridView1.CurrentRow.Index);
            }
        }

        private void FrmMain2_FormClosed(object sender, FormClosedEventArgs e)
        {
            save_shapesFileNames();
        }

        private void btn_load_model_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "模板文件|*.json";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                load_shapes_model(openFileDialog1.FileName);
                load_shapes_model();
            }
        }

        private void load_shapes_model(string file_path)
        {
            // 这里要判断一下是否有这个路径了
            if (shapesFileNames.Contains(file_path))
            {
                combo_models.SelectedIndex = shapesFileNames.IndexOf(file_path);
            }
            else
            {
                // 添加新的。
                shapesFileNames.Insert(0, file_path);
                combo_models.Items.Insert(0, System.IO.Path.GetFileNameWithoutExtension(file_path));
                combo_models.SelectedIndex = 0;
            }
        }
        /// <summary>
        /// 这个只是从组合框中读取的
        /// </summary>
        private void load_shapes_model()
        {
            if (shapesFileNames.Count > 0)
            {
                string file_path = shapesFileNames[combo_models.SelectedIndex]; // 看看选择的是哪个
                this.canvas.shapes = Shapes.load(file_path);
                this.canvas.zoomToScreen();
                this.canvas.Refresh();

            }

        }



        private void btn_edit_model_Click(object sender, EventArgs e)
        {
            // todo 编辑模板
            // 首先寻找是哪个模板
            string file_path = shapesFileNames.Where(x => x.Contains(combo_models.Text)).FirstOrDefault();
            if (file_path != null)
            {
                FrmBarcodeEdit frmBarcodeEdit = null;
                if (this.dataGridView1.DataSource != null)
                {
                    frmBarcodeEdit = new FrmBarcodeEdit(file_path, (DataTable)this.dataGridView1.DataSource);
                }
                else
                {
                    frmBarcodeEdit = new FrmBarcodeEdit(file_path);
                }

                // 这里打开
                if (frmBarcodeEdit.ShowDialog() == DialogResult.OK)
                {
                    // 这里要更新啊。
                    load_shapes_model(file_path);
                    load_shapes_model();
                }

            }


        }

        private void btn_new_model_Click(object sender, EventArgs e)
        {
            FrmBarcodeEdit frmBarcodeEdit = null;
            if (this.dataGridView1.DataSource != null)
            {
                frmBarcodeEdit = new FrmBarcodeEdit((DataTable)this.dataGridView1.DataSource);
            }
            else
            {
                frmBarcodeEdit = new FrmBarcodeEdit();
            }
            frmBarcodeEdit.ShowDialog();

            // 这里打开
            if ( string.IsNullOrEmpty(frmBarcodeEdit.getModelFileName()))
            {
                // 这里要更新啊。
                load_shapes_model(frmBarcodeEdit.getModelFileName());
                load_shapes_model();
            }
        }
        /// <summary>
        /// 重新读取shapes
        /// </summary>
        /// <returns></returns>
        private Shapes GetShapes()
        {
            if (shapesFileNames.Count == 0)
            {
                return null;
            }
            // 这里取得是哪个
            return Shapes.load(shapesFileNames[combo_models.SelectedIndex]);

        }

        private void btn_increase_print_Click(object sender, EventArgs e)
        {
            // 根据损耗打印
            // 这里首先判断一下是否有这一个列
            if (this.dataGridView1.DataSource == null) return;
            try
            {
                var columns = getColumnNames((DataTable)this.dataGridView1.DataSource);
                if (columns.Contains(combo_qty_var_names.Text))
                {
                    // 这里表示有打印数量，要取得所有的选择的表格
                    var vars = new List<Dictionary<string, string>>();// 变量的集合
                    var nums = new List<int>();                       // 数量的集合
                    var ratio = int.Parse(txt_loss_ratio.Text);       // 上涨的比例
                    foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                    {
                        var dict = getVars(item.Index);
                        vars.Add(dict);
                        int n = int.Parse(dict[combo_qty_var_names.Text]);
                        int n2 = n * ratio / 100; // 损耗
                        if (n2 ==0 && ratio > 0)
                        {
                            n2 = 1;
                        }
                        nums.Add(n+n2);
                    }
                    // 取得模板
                    var _shapes = GetShapes();
                    if (_shapes == null) { MessageBox.Show("没有模板文件");return; }
                    // 发送给打印机
                    PrintItem printItem = new PrintItem();
                    printItem.PrinterName = combo_printers.Text;
                    printItem.Shapes = _shapes;
                    printItem.Valss = vars;
                    printItem.PrintCounts = nums;
                    printItem.isFullPrint = chk_isFull.Checked;
                    // 
                    PrintManagerImpl printManagerImpl = new PrintManagerImpl();
                    printManagerImpl.addPrintItem(printItem);
                }
                else
                {
                    MessageBox.Show("没有打印数量");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
                //throw;
            }
  

        }

        private void btn_all_print_Click(object sender, EventArgs e)
        {
            // 这里首先判断一下是否有这一个列
            if (this.dataGridView1.DataSource == null) return;
            try
            {
                var columns = getColumnNames((DataTable)this.dataGridView1.DataSource);
                if (columns.Contains(combo_qty_var_names.Text))
                {
                    // 这里表示有打印数量，要取得所有的选择的表格
                    var vars = new List<Dictionary<string, string>>();// 变量的集合
                    var nums = new List<int>();                       // 数量的集合
                    var ratio = int.Parse(txt_loss_ratio.Text);       // 上涨的比例
                    foreach (DataGridViewRow item in this.dataGridView1.Rows) // 全部跟损耗的区别是这里是权表
                    {
                        var dict = getVars(item.Index);
                        vars.Add(dict);
                        int n = int.Parse(dict[combo_qty_var_names.Text]);
                        int n2 = n * ratio / 100; // 损耗
                        if (n2 == 0 && ratio > 0)
                        {
                            n2 = 1;
                        }
                        nums.Add(n + n2);
                    }
                    // 取得模板
                    var _shapes = GetShapes();
                    if (_shapes == null) { MessageBox.Show("没有模板文件"); return; }
                    // 发送给打印机
                    PrintItem printItem = new PrintItem();
                    printItem.PrinterName = combo_printers.Text;
                    printItem.Shapes = _shapes;
                    printItem.Valss = vars;
                    printItem.PrintCounts = nums;
                    printItem.isFullPrint = chk_isFull.Checked;
                    // 
                    PrintManagerImpl printManagerImpl = new PrintManagerImpl();
                    printManagerImpl.addPrintItem(printItem);
                }
                else
                {
                    MessageBox.Show("没有打印数量");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
                //throw;
            }

        }

        private void btn_print_2_Click(object sender, EventArgs e)
        {
            // 这里首先判断一下是否有这一个列
            // 这里判断一下有没有数量
            int num = 0;
            if(!int.TryParse(txt_qty2.Text, out num))
            {
                MessageBox.Show("数量不对");
                return;
            }


            if (this.dataGridView1.DataSource == null) return;
            try
            {
                var columns = getColumnNames((DataTable)this.dataGridView1.DataSource);
                if (columns.Contains(combo_qty_var_names.Text))
                {
                    // 这里表示有打印数量，要取得所有的选择的表格
                    var vars = new List<Dictionary<string, string>>();// 变量的集合
                    var nums = new List<int>();                       // 数量的集合
                    foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
                    {
                        var dict = getVars(item.Index);
                        vars.Add(dict);
                        nums.Add(num); // 跟损耗打印的不同是，这里是固定的数量。
                    }
                    // 取得模板
                    var _shapes = GetShapes();
                    if (_shapes == null) { MessageBox.Show("没有模板文件"); return; }
                    // 发送给打印机
                    PrintItem printItem = new PrintItem();
                    printItem.PrinterName = combo_printers.Text;
                    printItem.Shapes = _shapes;
                    printItem.Valss = vars;
                    printItem.PrintCounts = nums;
                    printItem.isFullPrint = chk_isFull.Checked;
                    // 
                    PrintManagerImpl printManagerImpl = new PrintManagerImpl();
                    printManagerImpl.addPrintItem(printItem);
                }
                else
                {
                    MessageBox.Show("没有打印数量");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("错误：" + ex.Message);
                //throw;
            }

        }

        private void combo_models_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_shapes_model();
        }
    }
}
