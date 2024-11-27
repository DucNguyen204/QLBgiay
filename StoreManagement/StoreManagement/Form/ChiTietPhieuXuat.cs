using CrystalDecisions.CrystalReports.Engine;
using StoreManagement.Crystal_Report;
using StoreManagement.DAO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.ReportSource;
using CrystalDecisions.Shared;

namespace StoreManagement
{
    public partial class ChiTietPhieuXuat : Form
    {
        public ChiTietPhieuXuat()
        {
            InitializeComponent();
        }

        private void ChiTietPhieuXuat_Load(object sender, EventArgs e)
        {
            loadData();
            showCBmaHang();
            showCBmaNv();
            showCBmaPn();
            showCBmaSize();
        }

        public bool kiemTraMaCTPX(string mactpx)
        {

            string query = "select * from tblChiTietPhieuXuat where MaCTPX = ('" + mactpx + "')";
            DataTable ketQua = DataProvider.Instance.ExecuteQuery(query);
            return ketQua.Rows.Count > 0;
        }
      
        public void clear()
        {
            txtmact.Text = "";
            txtsl.Text = "";
            txtdongia.Text = "";
        }
        public void loadData()
        {
            string query = "select * from tblChiTietPhieuXuat ";
            dtgrvctpx.DataSource = DataProvider.Instance.ExecuteQuery(query);

        }
        public void showCBmaPn()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"select * from tblPhieuXuat");
            cbmapx.DataSource = data;
            cbmapx.DisplayMember = "MaPX";
            cbmapx.ValueMember = "MaPX";
        }
        public void showCBmaHang()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"select * from tblHang");
            cbmahang.DataSource = data;
            cbmahang.DisplayMember = "TenHang";
            cbmahang.ValueMember = "MaHang";
        }
        public void showCBmaNv()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"select * from tblNhanVien");
            cbmanv.DataSource = data;
            cbmanv.DisplayMember = "TenNV";
            cbmanv.ValueMember = "MaNV";
        }
        public void showCBmaSize()
        {
            DataTable data = DataProvider.Instance.ExecuteQuery(@"select * from tblSize");
            cbSize.DataSource = data;
            cbSize.DisplayMember = "TenSize";
            cbSize.ValueMember = "LoaiSize";
        }
        private void btnthem_Click(object sender, EventArgs e)
        {
            string maCtpx = txtmact.Text;
            string maPx = cbmapx.Text;
            string maHang = cbmahang.Text;
            string maNv = cbmanv.Text;
            string maSize = cbSize.Text;
            string sl = txtsl.Text;
            string donGia = txtdongia.Text;
            if (kiemTraMaCTPX(maCtpx) == true || maCtpx == "")
            {
                MessageBox.Show("Mã trùng hoặc lỗi!!!");
                return;
            }
            else
            {
                string query = "INSERT INTO tblChiTietPhieuXuat (MaCTPN,MaPN,MaHang,MaNV,MaSize,SoLuongNhap,DonGiaNhap)VALUES" +
                    " ('" + maCtpx + "','" + maPx + "','" + maHang + "','" + maNv + "', '" + Size + "','" + sl + "','" + donGia + "') ";
                dtgrvctpx.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            loadData();
            clear();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            string mactpx = txtmact.Text;
            string mapn = cbmapx.SelectedValue.ToString();
            string mahang = cbmahang.SelectedValue.ToString();
            string manv = cbmanv.Text;
            string size = cbSize.SelectedValue.ToString();
            string soluong = txtsl.Text;
            string dongia = txtdongia.Text;

            if (kiemTraMaCTPX(mactpx) == true)
            {
                string query = " UPDATE tblChiTietPhieuXuat " +
                    "SET MaPX = N'" + mapn + "', MaHang = N'" + mahang + "',MaNV = '" + manv + "',MaSize = N'" + size + "',SoLuongXuat = N'" + soluong + "',DonGiaXuat = N'" + dongia +
                    "' Where MaCTPX = '" + mactpx + "' ";
                dtgrvctpx.DataSource = DataProvider.Instance.ExecuteQuery(query);
            }
            else
            {
                MessageBox.Show("Mã hàng không tồn tại!!!");
            }
            loadData();
            clear();
        }
        private void btnxoa_Click(object sender, EventArgs e)
        {
            string mactpx = txtmact.Text;
            string mapx = cbmapx.SelectedValue.ToString();
            string mahang = cbmahang.SelectedValue.ToString();
            string manv = cbmanv.Text;
            string size = cbSize.SelectedValue.ToString();
            string soluong = txtsl.Text;
            string dongia = txtdongia.Text;

            if (kiemTraMaCTPX(mactpx) == false)
            {
                MessageBox.Show("Mã không tồn tại");
            }
            else
            {
                string query = " DELETE  FROM tblChiTietPhieuXuat WHERE MaCTPX = '" + mactpx + "' ";
                if (MessageBox.Show($"Bạn có muốn xóa mặt hàng có mã {mactpx} này không?", "Xác nhận xóa"
                      , MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dtgrvctpx.DataSource = DataProvider.Instance.ExecuteQuery(query);
                    loadData();
                    clear();
                }
            }
        }

        private void dtgrvctpx_SelectionChanged(object sender, EventArgs e)
        {
            if (dtgrvctpx.SelectedRows.Count > 0)
            {
                DataGridViewRow idx = dtgrvctpx.SelectedRows[0];
                txtmact.Text = idx.Cells["MaCTPX"].Value.ToString();
                cbmapx.Text = idx.Cells["MaPX"].Value.ToString();
                cbmahang.Text = idx.Cells["MaHang"].Value.ToString();
                cbmanv.Text = idx.Cells["MaNV"].Value.ToString();
                cbSize.Text = idx.Cells["MaSize"].Value.ToString();
                txtsl.Text = idx.Cells["SoLuongXuat"].Value.ToString();
                txtdongia.Text = idx.Cells["DonGiaXuat"].Value.ToString();
            }
        }
        private void dtgrvctpx_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtgrvctpx.SelectedRows.Count > 0)
    {
        var row = dtgrvctpx.SelectedRows[0];
        txtmact.Text = row.Cells["MaCTPX"]?.Value?.ToString() ?? string.Empty;
        cbmapx.SelectedValue = row.Cells["MaPX"]?.Value;
        cbmahang.SelectedValue = row.Cells["MaHang"]?.Value;
        cbmanv.SelectedValue = row.Cells["MaNV"]?.Value;
        cbSize.SelectedValue = row.Cells["MaSize"]?.Value;
        txtsl.Text = row.Cells["SoLuongXuat"]?.Value?.ToString() ?? string.Empty;
        txtdongia.Text = row.Cells["DonGiaXuat"]?.Value?.ToString() ?? string.Empty;
    }
        }

        private void btntim_Click(object sender, EventArgs e)
        {
            string timkiem = txttimkiem.Text;
            string query = "select * from tblChiTietPhieuXuat WHERE MaPX like '%" + timkiem + "%'";
            dtgrvctpx.DataSource = DataProvider.Instance.ExecuteQuery(query);
        }

        private void txttimkiem_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txttimkiem.Text))
            {
                loadData();
            }
        }
        private void taoForm(string maCTPX)
        {
            frmBaoCaoXuatHang1 frmBaoCaoXuatHang1 = new frmBaoCaoXuatHang1();
            ReportDocument reportDocument = new ReportDocument();
            reportDocument.Load(@"E:\\StoreManagement\\StoreManagement\\StoreManagement\\Crystal Report\\rptBaoCaoPhieuXuat.rpt");
            TableLogOnInfo logOnInfo = new TableLogOnInfo();
            foreach (Table table in reportDocument.Database.Tables)
            {
                logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo.ServerName = "HI";    // Thay thế tên máy chủ
                logOnInfo.ConnectionInfo.DatabaseName = "QLBgiay"; // Thay thế tên cơ sở dữ liệu
                table.ApplyLogOnInfo(logOnInfo);
            }
            // Thiết lập giá trị tham số cho báo cáo
            reportDocument.SetParameterValue("@MaCTPX", maCTPX);

            // Thiết lập nguồn dữ liệu cho Crystal Report Viewer
            frmBaoCaoXuatHang1.crystalReportViewer.ReportSource = reportDocument;
            frmBaoCaoXuatHang1.crystalReportViewer.Refresh();

            // Hiển thị form báo cáo
            frmBaoCaoXuatHang1.ShowDialog();
        }
        private void btIn_Click(object sender, EventArgs e)
        {
            string maCTPX = txtmact.Text.Trim();
            if (!string.IsNullOrEmpty(maCTPX))
            {
                taoForm(maCTPX);
            }
            else
            {
                MessageBox.Show("Mã phiếu xuất không hợp lệ.");
            }
        }
    }
}
