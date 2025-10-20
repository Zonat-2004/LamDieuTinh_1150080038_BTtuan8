using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace LamDieuTinh_1150080038_BTtuan8
{
    public class Form1 : Form
    {
        // ===== Chuỗi kết nối (CHỈNH LẠI đường dẫn .mdf) =====
        private readonly string _connStr =
            @"Data Source=(LocalDB)\MSSQLLocalDB;
              AttachDbFilename=C:\Users\Admin\source\repos\LamDieuTinh_1150080038_BTtuan8\LamDieuTinh_1150080038_BTtuan8\QuanLyBanSach.mdf;
              Integrated Security=True";

        private ListView lsvDanhSach;
        private TextBox txtMaXB, txtTenXB, txtDiaChi;
        private Button btnRefresh, btnThem, btnCapNhat, btnXoa;

        public Form1()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            this.Text = "Thực hành 4 - Xóa dữ liệu (Lâm Diệu Tinh 1150080038)";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(950, 560);
            this.BackColor = Color.WhiteSmoke;
            this.Font = new Font("Segoe UI", 10F);

            // ===== Header =====
            Panel pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.SeaGreen
            };
            Label lblHeader = new Label
            {
                Text = "QUẢN LÝ NHÀ XUẤT BẢN - XÓA DỮ LIỆU",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 15, FontStyle.Bold),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblHeader);
            this.Controls.Add(pnlHeader);

            // ===== ListView =====
            lsvDanhSach = new ListView
            {
                Location = new Point(20, 80),
                Size = new Size(560, 430),
                View = View.Details,
                FullRowSelect = true,
                GridLines = true,
                HideSelection = false,
                BackColor = Color.White
            };
            lsvDanhSach.Columns.Add("Mã XB", 100);
            lsvDanhSach.Columns.Add("Tên NXB", 220);
            lsvDanhSach.Columns.Add("Địa chỉ", 220);
            lsvDanhSach.SelectedIndexChanged += lsvDanhSach_SelectedIndexChanged;
            this.Controls.Add(lsvDanhSach);

            // ===== GroupBox Thông tin =====
            GroupBox grpChiTiet = new GroupBox
            {
                Text = "Thông tin chi tiết",
                Location = new Point(600, 80),
                Size = new Size(320, 300)
            };
            this.Controls.Add(grpChiTiet);

            Label lblMa = new Label { Text = "Mã XB:", Location = new Point(20, 40), AutoSize = true };
            txtMaXB = new TextBox { Location = new Point(100, 35), Width = 180 };

            Label lblTen = new Label { Text = "Tên NXB:", Location = new Point(20, 85), AutoSize = true };
            txtTenXB = new TextBox { Location = new Point(100, 80), Width = 180 };

            Label lblDiaChi = new Label { Text = "Địa chỉ:", Location = new Point(20, 130), AutoSize = true };
            txtDiaChi = new TextBox
            {
                Location = new Point(100, 125),
                Width = 180,
                Height = 100,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            grpChiTiet.Controls.Add(lblMa);
            grpChiTiet.Controls.Add(txtMaXB);
            grpChiTiet.Controls.Add(lblTen);
            grpChiTiet.Controls.Add(txtTenXB);
            grpChiTiet.Controls.Add(lblDiaChi);
            grpChiTiet.Controls.Add(txtDiaChi);

            // ===== Các nút chức năng =====
            btnThem = new Button
            {
                Text = "➕ Thêm",
                Location = new Point(600, 400),
                Width = 90,
                Height = 40,
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnThem.Click += BtnThem_Click;
            this.Controls.Add(btnThem);

            btnCapNhat = new Button
            {
                Text = "✏️ Sửa",
                Location = new Point(700, 400),
                Width = 90,
                Height = 40,
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnCapNhat.Click += BtnCapNhat_Click;
            this.Controls.Add(btnCapNhat);

            btnXoa = new Button
            {
                Text = "🗑️ Xóa",
                Location = new Point(800, 400),
                Width = 90,
                Height = 40,
                BackColor = Color.Firebrick,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnXoa.Click += BtnXoa_Click;
            this.Controls.Add(btnXoa);

            btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(710, 460),
                Width = 130,
                Height = 40,
                BackColor = Color.MediumSeaGreen,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnRefresh.Click += (s, e) => HienThiDanhSachNXB();
            this.Controls.Add(btnRefresh);

            this.Load += (s, e) => HienThiDanhSachNXB();
        }

        // ========== HIỂN THỊ DANH SÁCH ==========
        private void HienThiDanhSachNXB()
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_HienThiNXB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        lsvDanhSach.Items.Clear();
                        while (reader.Read())
                        {
                            var item = new ListViewItem(reader.GetString(0).Trim());
                            item.SubItems.Add(reader.GetString(1));
                            item.SubItems.Add(reader.IsDBNull(2) ? "" : reader.GetString(2));
                            lsvDanhSach.Items.Add(item);
                        }
                    }
                }
                txtMaXB.Text = txtTenXB.Text = txtDiaChi.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hiển thị danh sách:\n" + ex.Message);
            }
        }

        // ========== CHỌN ITEM ==========
        private void lsvDanhSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lsvDanhSach.SelectedItems.Count == 0) return;
            var item = lsvDanhSach.SelectedItems[0];
            txtMaXB.Text = item.SubItems[0].Text;
            txtTenXB.Text = item.SubItems[1].Text;
            txtDiaChi.Text = item.SubItems[2].Text;
        }

        // ========== THÊM ==========
        private void BtnThem_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_ThemDuLieu", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenNXB", txtTenXB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("✅ Thêm thành công!");
                    HienThiDanhSachNXB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi thêm dữ liệu:\n" + ex.Message);
            }
        }

        // ========== CẬP NHẬT ==========
        private void BtnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_CapNhatThongTin", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());
                    cmd.Parameters.AddWithValue("@TenNXB", txtTenXB.Text.Trim());
                    cmd.Parameters.AddWithValue("@DiaChi", txtDiaChi.Text.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("✏️ Cập nhật thành công!");
                    HienThiDanhSachNXB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi cập nhật:\n" + ex.Message);
            }
        }

        // ========== XÓA ==========
        private void BtnXoa_Click(object sender, EventArgs e)
        {
            if (txtMaXB.Text == "")
            {
                MessageBox.Show("Vui lòng chọn Nhà Xuất Bản cần xóa!");
                return;
            }

            var confirm = MessageBox.Show($"Bạn có chắc muốn xóa NXB [{txtTenXB.Text}] không?",
                                          "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.No) return;

            try
            {
                using (var con = new SqlConnection(_connStr))
                using (var cmd = new SqlCommand("sp_XoaNXB", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MaXB", txtMaXB.Text.Trim());
                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("🗑️ Xóa thành công!");
                    HienThiDanhSachNXB();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Lỗi khi xóa dữ liệu:\n" + ex.Message);
            }
        }
    }
}
