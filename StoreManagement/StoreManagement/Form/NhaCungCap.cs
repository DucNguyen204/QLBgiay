using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using app = Microsoft.Office.Interop.Excel.Application;
using ExcelDataReader;

namespace StoreManagement
{
    public partial class NhaCungCap : Form
    {
        private static string constr = @"Data Source=HI;Initial Catalog=QLBgiay;Integrated Security=True;";

        public NhaCungCap()
        {
            InitializeComponent();
        }

        private void NhaCungCap_Load(object sender, EventArgs e)
        {
            loadData();
        }

        private void loadData()
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT * FROM tblNhaCC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dtgrvncc.DataSource = dt;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi hiển thị nhà cung cấp: " + ex.Message);
                }
            }
        }

        public void clear()
        {
            txtmancc.Text = "";
            txttenncc.Text = "";
            txtdienthoai.Text = "";
            txtdiachi.Text = "";
        }

        private bool KiemTraMaNCC(string maNcc)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM tblNhaCC WHERE MaNhaCC = @MaNhaCC";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MaNhaCC", maNcc);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kiểm tra mã nhà cung cấp: " + ex.Message);
                    return false;
                }
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem các TextBox có trống không
            if (string.IsNullOrEmpty(txtmancc.Text) || string.IsNullOrEmpty(txttenncc.Text) ||
                string.IsNullOrEmpty(txtdienthoai.Text) || string.IsNullOrEmpty(txtdiachi.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }
            if (KiemTraMaNCC(txtmancc.Text))
            {
                MessageBox.Show("Mã nhà cung cấp đã tồn tại");
                return;
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemNhaCungCap", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaNCC", txtmancc.Text);
                        cmd.Parameters.AddWithValue("@TenNCC", txttenncc.Text);
                        cmd.Parameters.AddWithValue("@DiaChi", txtdiachi.Text);
                        cmd.Parameters.AddWithValue("@SDT", txtdienthoai.Text);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Thêm nhà cung cấp thành công!");
                            loadData();
                            clear();
                        }
                        else
                        {
                            MessageBox.Show("Thêm nhà cung cấp thất bại!");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm nhà cung cấp: " + ex.Message);
                }
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string maNcc = txtmancc.Text;
            string tenNcc = txttenncc.Text;
            string diaChi = txtdiachi.Text;
            string sdt = txtdienthoai.Text;
            if (KiemTraMaNCC(maNcc))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();
                        string query = "UPDATE tblNhaCC " +
                                       "SET TenNCC = @TenNCC, DiaChi = @DiaChi, DienThoai = @DienThoai WHERE MaNhaCC = @MaNhaCC";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@MaNhaCC", maNcc);
                            cmd.Parameters.AddWithValue("@TenNCC", tenNcc);
                            cmd.Parameters.AddWithValue("@DiaChi", diaChi);
                            cmd.Parameters.AddWithValue("@DienThoai", sdt);
                           

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật nhà cung cấp thành công.");
                                loadData();
                                clear();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy nhà cung cấp có mã hàng này.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật sản phẩm: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhà cung cấp để sửa");
            }
        }

        private void dtgrvncc_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgrvncc.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgrvncc.SelectedRows[0];

                txtmancc.Text = selectedRow.Cells["MaNhaCC"].Value.ToString();
                txttenncc.Text = selectedRow.Cells["TenNCC"].Value.ToString();
                txtdiachi.Text = selectedRow.Cells["DiaChi"].Value.ToString();
                txtdienthoai.Text = selectedRow.Cells["DienThoai"].Value.ToString();
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (dtgrvncc.SelectedRows.Count > 0)
            {
                if (dtgrvncc.SelectedRows[0].Cells["MaNhaCC"].Value != null)
                {
                    string maNcc = dtgrvncc.SelectedRows[0].Cells["MaNhaCC"].Value.ToString();
                    if (MessageBox.Show($"Bạn có muốn xóa mặt hàng có mã {maNcc} này không?", "Xác nhận xóa"
                        , MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        XoaHang(maNcc);
                        loadData();
                        clear();
                    }
                }
            }
        }

        private void XoaHang(string maNhaCC)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaNhaCungCap", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaNCC", maNhaCC);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa thành công.");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã nhà cung cấp hợp lệ.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa nhà cung cấp\r\nBạn vẫn có sản phẩm từ nhà cung cấp này :(( ");
                }
            }
        }

        private void btntim_Click(object sender, EventArgs e)
        {
            string tenNCC = txttimkiem.Text.Trim();


            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemNCC", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@TenNCC", string.IsNullOrEmpty(tenNCC) ? (object)DBNull.Value : tenNCC);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dtgrvncc.DataSource = dt;

                            if (dt.Rows.Count == 0)
                            {
                                MessageBox.Show("Không tìm thấy nhà cung cấp nào phù hợp.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi tìm kiếm loại mặt hàng: " + ex.Message);
                }
            }
        }

        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttimkiem.Text))
            {
                loadData();
            }
        }

        private void xuatfileExcel(DataGridView g, string duongDan, string tentep) // HÀM XUẤT FILE EXCEL
        {
            app obj = new app(); // KHƠI TẠO APP
            obj.Application.Workbooks.Add(Type.Missing); // ĐỐI TƯỢNG OBJ ĐỂ LƯU TRỮ
            obj.Columns.ColumnWidth = 25;
            for (int i = 1; i < g.Columns.Count + 1; i++) // chạy hàng ngang
            {
                obj.Cells[1, i] = g.Columns[i - 1].HeaderText;
            }
            for (int i = 0; i < g.Rows.Count; i++)  // CHẠY HÀNG DỌC
            {
                for (int j = 0; j < g.Columns.Count; j++) // CHẠY HÀNG NGANG
                {
                    if (g.Rows[i].Cells[j].Value != null) // Khác NULL THÌ CHÈN VÀO
                    {
                        obj.Cells[i + 2, j + 1] = g.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            obj.ActiveWorkbook.SaveCopyAs(duongDan + tentep + ".xlsx"); // LƯU TÊN TỆP
            obj.ActiveWorkbook.Saved = true;

        }
        private void btnExport_Click(object sender, EventArgs e)
        {

            try
            {

                MessageBox.Show("Máy bạn phải có ổ D");
                xuatfileExcel(dtgrvncc, @"E:\", "Workbook");
                MessageBox.Show("Xuất file thành công. ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("Excel.exe", @"E:\Workbook.xlsx");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Xuất file không thành công", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private List<string> getListSheet(string urlFile)
        {
            try
            {
                List<string> sheets = new List<string>();
                string connec = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 8.0;HDR=YES;IMEX=1;""", urlFile);
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");
                DbConnection connection = factory.CreateConnection();
                connection.ConnectionString = connec;
                connection.Open();
                System.Data.DataTable dt = connection.GetSchema("Tables");
                connection.Close();
                foreach (DataRow row in dt.Rows)
                {
                    string sheetnames = (string)row["TABLE_NAME"];
                    sheets.Add(sheetnames);
                }
                return sheets;
            }
            catch (Exception)
            {
                return null;
            }
        }
        private void ImportExcelData(string filePath)
        {
            try
            {


                using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        var dataSetConfig = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true // Sử dụng dòng đầu tiên làm tiêu đề cột
                            }
                        };
                        DataSet result = reader.AsDataSet(dataSetConfig);
                        DataTable dataTable = result.Tables[0]; // Lấy bảng đầu tiên trong file Excel
                        dtgrvncc.DataSource = dataTable; // Gán dữ liệu vào DataGridView
                    }
                }
                MessageBox.Show("Import dữ liệu từ Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import dữ liệu từ Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel Files|*.xls;*.xlsx";
            openFileDialog.Title = "Chọn file Excel";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                ImportExcelData(filePath);
            }
        }
    }
}
