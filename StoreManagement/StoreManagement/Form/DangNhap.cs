using StoreManagement.Crystal_Report;
using StoreManagement.DAO;
using System;
using System.Data;

using System.Windows.Forms;

namespace StoreManagement
{
    public partial class DangNhap : Form
    {
        public DangNhap()
        {
            InitializeComponent();
        }
        private bool KiemTraDangNhap(string taiKhoan, string matKhau)
        {
            string query = "SELECT * FROM tblDangNhap WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { taiKhoan, matKhau });

            return result.Rows.Count > 0;
        }
        private void clear()
        {
            txtTaiKhoan.Text = string.Empty;
            txtMatKhau.Text = string.Empty;
        }
        private int GetMaTK(string taiKhoan, string matKhau)
        {
            string query = "SELECT MaTK FROM tblDangNhap WHERE TaiKhoan = @TaiKhoan AND MatKhau = @MatKhau";
            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] { taiKhoan, matKhau });

            if (result.Rows.Count > 0)
            {
                return Convert.ToInt32(result.Rows[0]["MaTK"]);
            }
            else
            {
                return -1; // Return -1 if login fails
            }
        }
        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            string tk = txtTaiKhoan.Text.Trim();
            string pass = txtMatKhau.Text.Trim();
            int maTK = GetMaTK(tk, pass);

            if (maTK != -1) // Login successful
            {
                MessageBox.Show("Đăng nhập thành công");

                if (maTK >= 1 && maTK <= 3)
                {
                    Trangchu Trangchu = new Trangchu();
                    Trangchu.Show();
                }
                else
                {
                    trangchunhanvien Trangchunhanvien = new trangchunhanvien();
                    Trangchunhanvien.Show();
                }

                this.Hide();
            }
            else
            {
                MessageBox.Show("Mật khẩu tài khoản không đúng!!!\r\nVui lòng đăng nhập lại !");
                clear();
            }

        }


    }
}
