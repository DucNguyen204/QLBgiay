using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Forms;
using app = Microsoft.Office.Interop.Excel.Application;
using ExcelDataReader;

namespace StoreManagement
{
    public partial class Hang : Form
    {
        private static string constr = @"Data Source=HI;Initial Catalog=QLBgiay;Integrated Security=True;";
        public Hang()
        {
            InitializeComponent();
            
        }
        private void Hang_Load(object sender, EventArgs e)
        {
            loadData();
            loadCbLoaiHang();
            loadCbNcc();
            loadCbSize();
        }
        private void loadData()
        {
            using(SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_hienTTHang", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using(SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dtgrvhang.DataSource = dt;
                        }
                    }
                    }catch (Exception ex) {
                    MessageBox.Show("Lỗi : " + ex);
                     }
            }
        }
        private void loadCbNcc()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    string query = "SELECT *FROM tblNhaCC";
                    using(SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using(SqlDataAdapter adapter=new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            if(dt.Rows.Count > 0)
                            {
                                cbmanhacc.DataSource = dt;
                                cbmanhacc.DisplayMember = "TenNCC";
                                cbmanhacc.ValueMember = "MaNhaCC";
                            }
                        }
                    }
                }catch(Exception e)
                {
                    MessageBox.Show("Lỗi khi tải nhà cung cấp: " + e.Message);
                }
            }
        }
        private void loadCbLoaiHang()
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    string query = "SELECT *FROM tblLoaiSanPham";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                cbmaloai.DataSource = dt;
                                cbmaloai.DisplayMember = "TenLoai";
                                cbmaloai.ValueMember = "MaLoai";
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Lỗi khi tải loại sản phẩm: " + e.Message);
                }
            }
        }

        public void loadCbSize()
        {
         
            string query = "SELECT MaSize, LoaiSize FROM tblSize";

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        cbSize.DisplayMember = "LoaiSize"; // Field to display
                        cbSize.ValueMember = "MaSize";     // Field to use as value
                        cbSize.DataSource = dt;
                    }
                }
            }
        }

        public void clear()
        {
            txtmahang.Text = "";
            txttenhang.Text = "";
            txtsoluong.Text = "";
            txtdongia.Text = "";


        }
        private bool KiemTraMaHang(string maHang)
        {
            using(SqlConnection con = new SqlConnection(constr)) {
                try {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM tblHang WHERE MaHang = @MaHang";
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@MaHang", maHang);
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                } catch (Exception ex) {
                    MessageBox.Show("Lỗi khi kiểm tra mã hàng:"+ ex.Message);
                    return false;
                }
            }
        }

        

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (KiemTraMaHang(txtmahang.Text))
            {
                MessageBox.Show("Mã sản phẩm đã tồn tại!!!\r\nVui lòng chọn mã sản phẩm khác");
                return;
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_ThemHang", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaHang", txtmahang.Text);
                        cmd.Parameters.AddWithValue("@TenHang", txttenhang.Text);
                        cmd.Parameters.AddWithValue("@MaNhaCC", cbmanhacc.SelectedValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@MaLoai", cbmaloai.SelectedValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SoLuong", int.Parse(txtsoluong.Text));
                        cmd.Parameters.AddWithValue("@MaSize", cbSize.SelectedValue ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@DonGia", decimal.Parse(txtdongia.Text));
                        cmd.ExecuteNonQuery();
                    }
                    loadData();
                    clear();
                    MessageBox.Show("Thêm sản phẩm thành công!"); // Thông báo thành công
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi thêm sản phẩm: " + ex);
                }
            }

        }

        private void btExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

         private void btnsua_Click(object sender, EventArgs e)
        {
            string mahang = txtmahang.Text;
            string manhacc = cbmanhacc.SelectedValue?.ToString();
            string maloai = cbmaloai.SelectedValue?.ToString();
            string size = cbSize.SelectedValue?.ToString();
            string soluong = txtsoluong.Text;
            string dongia = txtdongia.Text;

            if (KiemTraMaHang(mahang))
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        con.Open();
                        string query = "UPDATE tblHang " +
                                       "SET MaNhaCC = @MaNhaCC, MaLoai = @MaLoai, MaSize = @MaSize, SoLuong = @SoLuong, DonGia = @DonGia " +
                                       "WHERE MaHang = @MaHang";

                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@MaHang", mahang);
                            cmd.Parameters.AddWithValue("@MaNhaCC", (object)manhacc ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaLoai", (object)maloai ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@MaSize", (object)size ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@SoLuong", soluong);
                            cmd.Parameters.AddWithValue("@DonGia", dongia);

                            int rowsAffected = cmd.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Cập nhật sản phẩm thành công.");
                                loadData();
                                clear();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sản phẩm có mã hàng này.");
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
                MessageBox.Show("Mã hàng không tồn tại.");
            }
        }
        private void btnxoa_Click(object sender, EventArgs e)
        {
           
        }

        private void XoaHang(string mahang)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                try
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_XoaHang", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaHang", mahang);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Xóa thành công.");
                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy mã hàng hợp lệ.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xóa mặt hàng: " + ex.Message);
                }
            }
        }



     
        //Kiểm tra txtDongia chỉ có thể nhập số nguyên
        private void txtdongia_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && char.IsControl(e.KeyChar)) {
                e.Handled = true;
            }
        }

        private void dtgrvhang_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgrvhang.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dtgrvhang.SelectedRows[0];

                txtmahang.Text = selectedRow.Cells["MaHang"].Value.ToString();
                cbmanhacc.Text = selectedRow.Cells["TenNCC"].Value.ToString();
                cbmaloai.Text = selectedRow.Cells["TenLoai"].Value.ToString();
                cbSize.Text = selectedRow.Cells["LoaiSize"].Value.ToString();
                txtsoluong.Text = selectedRow.Cells["SoLuong"].Value.ToString();
                txtdongia.Text = selectedRow.Cells["DonGia"].Value.ToString();
                txttenhang.Text = selectedRow.Cells["TenHang"].Value.ToString();
                
            }
        }

        private void btntimkiem_Click(object sender, EventArgs e)
        {
            string search = txttim.Text.Trim();
            if(string.IsNullOrEmpty(search) ) {
                MessageBox.Show("Vui lòng nhập tên sản phẩm để tìm kiếm!!!");
                return;
            }
            using (SqlConnection conn = new SqlConnection(constr))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_TimKiemHang",conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@MaHang", (object)search ?? DBNull.Value);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            dtgrvhang.DataSource = dt;
                        }
                    }
                
                }catch (Exception ex) {
                    MessageBox.Show("Lỗi khi tìm kiếm mặt hàng :" + ex);
                }
            }
        }

        private void txttim_TextChanged(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txttim.Text)) {
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
                        dtgrvhang.DataSource = dataTable; // Gán dữ liệu vào DataGridView
                    }
                }
                MessageBox.Show("Import dữ liệu từ Excel thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi import dữ liệu từ Excel: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

      

        private void btnExport_Click_1(object sender, EventArgs e)
        {
            try
            {

                MessageBox.Show("Máy bạn phải có ổ D");
                xuatfileExcel(dtgrvhang, @"E:\", "Workbook");
                MessageBox.Show("Xuất file thành công. ", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Information);
                System.Diagnostics.Process.Start("Excel.exe", @"E:\Workbook.xlsx");
            }
            catch (Exception ex)
            {

                MessageBox.Show("Xuất file không thành công", "Lưu ý", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnIn_Click_1(object sender, EventArgs e)
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
