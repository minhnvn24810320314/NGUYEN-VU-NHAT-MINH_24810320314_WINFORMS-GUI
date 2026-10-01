using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NGUYEN_VU_NHAT_MINH_24810320314_WINFORMS_GUI
{
    // Model Sản Phẩm
    public class Product
    {
        public string ProductId { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string ImagePath { get; set; } = string.Empty;
    }

    // Model Danh Mục
    public class CategoryItem
    {
        public string CategoryId { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
    }

    public partial class Form1 : Form
    {
        private BindingList<Product> _productList = new BindingList<Product>();
        private BindingSource _bindingSource = new BindingSource();
        private ErrorProvider _errorProvider = new ErrorProvider();

        // Controls
        private TableLayoutPanel mainLayout = new TableLayoutPanel();
        private TextBox txtProductId = new TextBox();
        private TextBox txtProductName = new TextBox();
        private TextBox txtUnitPrice = new TextBox();
        private TextBox txtQuantity = new TextBox();
        private TextBox txtSearch = new TextBox();
        private ComboBox cboCategory = new ComboBox();
        private PictureBox picAvatar = new PictureBox();
        private DataGridView dgvProducts = new DataGridView();
        private StatusStrip statusStrip = new StatusStrip();
        private ToolStripStatusLabel lblStatus = new ToolStripStatusLabel();
        private MenuStrip menuStrip = new MenuStrip();

        public Form1()
        {
            InitializeComponent();
            BuildDynamicUI();
            InitDataBinding();
        }

        private void BuildDynamicUI()
        {
            this.Text = "TechMart Product Manager - Nguyễn Vũ Nhật Minh (24810320314)";
            this.Size = new Size(1100, 650);
            this.StartPosition = FormStartPosition.CenterScreen;

            // MenuStrip (TC05/Export CSV)
            var fileMenu = new ToolStripMenuItem("File (&F)");
            var exportItem = new ToolStripMenuItem("Export CSV", null, ExportCSV_Click) { ShortcutKeys = Keys.Control | Keys.E };
            var exitItem = new ToolStripMenuItem("Exit", null, (s, e) => Application.Exit()) { ShortcutKeys = Keys.Control | Keys.X };
            fileMenu.DropDownItems.Add(exportItem);
            fileMenu.DropDownItems.Add(exitItem);
            menuStrip.Items.Add(fileMenu);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            // StatusStrip
            statusStrip.Items.Add(lblStatus);
            this.Controls.Add(statusStrip);

            // TableLayoutPanel chính (TC01 Responsive Layout)
            mainLayout.Dock = DockStyle.Fill;
            mainLayout.ColumnCount = 2;
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 65F));
            mainLayout.RowCount = 1;
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            // KHUNG TRÁI: Nhập liệu
            GroupBox gbInput = new GroupBox { Text = "Thông Tin Sản Phẩm", Dock = DockStyle.Fill, Padding = new Padding(10) };
            TableLayoutPanel inputPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 8
            };
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            inputPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

            inputPanel.Controls.Add(new Label { Text = "Mã SP:", Anchor = AnchorStyles.Left }, 0, 0);
            inputPanel.Controls.Add(txtProductId, 1, 0); txtProductId.Dock = DockStyle.Fill;

            inputPanel.Controls.Add(new Label { Text = "Tên SP:", Anchor = AnchorStyles.Left }, 0, 1);
            inputPanel.Controls.Add(txtProductName, 1, 1); txtProductName.Dock = DockStyle.Fill;

            inputPanel.Controls.Add(new Label { Text = "Danh Mục:", Anchor = AnchorStyles.Left }, 0, 2);
            cboCategory.Dock = DockStyle.Fill;
            cboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            inputPanel.Controls.Add(cboCategory, 1, 2);

            inputPanel.Controls.Add(new Label { Text = "Đơn Giá:", Anchor = AnchorStyles.Left }, 0, 3);
            inputPanel.Controls.Add(txtUnitPrice, 1, 3); txtUnitPrice.Dock = DockStyle.Fill;

            inputPanel.Controls.Add(new Label { Text = "Số Lượng:", Anchor = AnchorStyles.Left }, 0, 4);
            inputPanel.Controls.Add(txtQuantity, 1, 4); txtQuantity.Dock = DockStyle.Fill;

            inputPanel.Controls.Add(new Label { Text = "Hình Ảnh:", Anchor = AnchorStyles.Left }, 0, 5);
            FlowLayoutPanel imgPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            picAvatar.Size = new Size(100, 80);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.BorderStyle = BorderStyle.FixedSingle;
            Button btnChooseImg = new Button { Text = "Chọn Ảnh", AutoSize = true };
            btnChooseImg.Click += BtnChooseImg_Click;
            imgPanel.Controls.Add(picAvatar);
            imgPanel.Controls.Add(btnChooseImg);
            inputPanel.Controls.Add(imgPanel, 1, 5);

            // Nút bấm hành động
            FlowLayoutPanel btnPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight };
            Button btnAdd = new Button { Text = "Thêm Mới", Width = 80, Height = 30 };
            Button btnDelete = new Button { Text = "Xóa", Width = 80, Height = 30 };
            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnPanel.Controls.Add(btnAdd);
            btnPanel.Controls.Add(btnDelete);
            inputPanel.Controls.Add(btnPanel, 1, 6);

            gbInput.Controls.Add(inputPanel);

            // KHUNG PHẢI: Bảng dữ liệu DataGridView
            GroupBox gbData = new GroupBox { Text = "Danh Sách Sản Phẩm", Dock = DockStyle.Fill, Padding = new Padding(10) };
            TableLayoutPanel rightPanel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 2 };
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            rightPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            FlowLayoutPanel searchPanel = new FlowLayoutPanel { Dock = DockStyle.Fill };
            searchPanel.Controls.Add(new Label { Text = "Tìm kiếm tên SP:", AutoSize = true, Anchor = AnchorStyles.Left });
            txtSearch.Width = 200;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            searchPanel.Controls.Add(txtSearch);
            rightPanel.Controls.Add(searchPanel, 0, 0);

            // DataGridView (TC03 DataBinding)
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.MultiSelect = false;
            dgvProducts.SelectionChanged += DgvProducts_SelectionChanged;

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductId", HeaderText = "Mã SP", Width = 80 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ProductName", HeaderText = "Tên Sản Phẩm", Width = 150 });
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Category", HeaderText = "Danh Mục", Width = 100 });
            var colPrice = new DataGridViewTextBoxColumn { DataPropertyName = "UnitPrice", HeaderText = "Đơn Giá (VNĐ)", Width = 120 };
            colPrice.DefaultCellStyle.Format = "N0";
            dgvProducts.Columns.Add(colPrice);
            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Quantity", HeaderText = "Số Lượng", Width = 80 });

            rightPanel.Controls.Add(dgvProducts, 0, 1);
            gbData.Controls.Add(rightPanel);

            mainLayout.Controls.Add(gbInput, 0, 0);
            mainLayout.Controls.Add(gbData, 1, 0);

            this.Controls.Add(mainLayout);
            mainLayout.BringToFront();

            // Khởi tạo ComboBox Danh mục
            cboCategory.DataSource = new[]
            {
                new CategoryItem { CategoryId = "CAT01", CategoryName = "Điện thoại" },
                new CategoryItem { CategoryId = "CAT02", CategoryName = "Laptop" },
                new CategoryItem { CategoryId = "CAT03", CategoryName = "Phụ kiện" }
            };
            cboCategory.DisplayMember = "CategoryName";
            cboCategory.ValueMember = "CategoryId";
        }

        private void InitDataBinding()
        {
            // Bổ sung dữ liệu mẫu ban đầu
            _productList.Add(new Product
            {
                ProductId = "SP001",
                ProductName = "iPhone 15 Pro Max",
                Category = "Điện thoại",
                UnitPrice = 29990000,
                Quantity = 15
            });

            _productList.Add(new Product
            {
                ProductId = "SP002",
                ProductName = "MacBook Pro M3",
                Category = "Laptop",
                UnitPrice = 45990000,
                Quantity = 8
            });

            _productList.Add(new Product
            {
                ProductId = "SP003",
                ProductName = "Tai nghe AirPods Pro 2",
                Category = "Phụ kiện",
                UnitPrice = 5990000,
                Quantity = 30
            });

            // Gán nguồn dữ liệu vào BindingSource
            _bindingSource.DataSource = _productList;
            dgvProducts.DataSource = _bindingSource;

            // Cập nhật thanh trạng thái StatusStrip khi danh sách thay đổi
            _productList.ListChanged += (s, e) => lblStatus.Text = $"Tổng số sản phẩm: {_productList.Count}";
            lblStatus.Text = $"Tổng số sản phẩm: {_productList.Count}";
        }

        // TC02 Validation kiểm tra dữ liệu
        private bool ValidateInput()
        {
            bool valid = true;
            _errorProvider.Clear();

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                _errorProvider.SetError(txtProductName, "Tên SP không được để trống!");
                valid = false;
            }
            if (!decimal.TryParse(txtUnitPrice.Text, out decimal p) || p <= 0)
            {
                _errorProvider.SetError(txtUnitPrice, "Đơn giá phải > 0!");
                valid = false;
            }
            if (!int.TryParse(txtQuantity.Text, out int q) || q < 0)
            {
                _errorProvider.SetError(txtQuantity, "Số lượng phải >= 0!");
                valid = false;
            }
            return valid;
        }

        // TC04 Mở file ảnh
        private void BtnChooseImg_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.jpeg" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    picAvatar.ImageLocation = ofd.FileName;
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            _productList.Add(new Product
            {
                ProductId = string.IsNullOrWhiteSpace(txtProductId.Text) ? $"SP{_productList.Count + 1:D3}" : txtProductId.Text,
                ProductName = txtProductName.Text.Trim(),
                Category = cboCategory.Text,
                UnitPrice = decimal.Parse(txtUnitPrice.Text),
                Quantity = int.Parse(txtQuantity.Text),
                ImagePath = picAvatar.ImageLocation ?? ""
            });
            ClearInput();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product p)
            {
                if (MessageBox.Show($"Bạn có muốn xóa {p.ProductName}?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    _productList.Remove(p);
                    ClearInput();
                }
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string kw = txtSearch.Text.Trim().ToLower();
            _bindingSource.DataSource = string.IsNullOrEmpty(kw)
                ? _productList
                : new BindingList<Product>(_productList.Where(p => p.ProductName.ToLower().Contains(kw)).ToList());
        }

        private void ExportCSV_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV File|*.csv", FileName = "Products.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder("MaSP,TenSP,DanhMuc,DonGia,SoLuong\n");
                    foreach (var p in _productList)
                        sb.AppendLine($"{p.ProductId},{p.ProductName},{p.Category},{p.UnitPrice},{p.Quantity}");
                    File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Xuất file CSV thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow?.DataBoundItem is Product p)
            {
                txtProductId.Text = p.ProductId;
                txtProductName.Text = p.ProductName;
                cboCategory.Text = p.Category;
                txtUnitPrice.Text = p.UnitPrice.ToString("G0");
                txtQuantity.Text = p.Quantity.ToString();
                if (!string.IsNullOrEmpty(p.ImagePath) && File.Exists(p.ImagePath))
                    picAvatar.ImageLocation = p.ImagePath;
                else
                    picAvatar.Image = null;
            }
        }

        private void ClearInput()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtUnitPrice.Clear();
            txtQuantity.Clear();
            picAvatar.Image = null;
            _errorProvider.Clear();
        }
    }
}