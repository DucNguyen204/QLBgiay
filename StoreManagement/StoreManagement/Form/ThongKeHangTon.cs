using StoreManagement.DAO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StoreManagement
{
    public partial class ThongKeHangTon : Form
    {
        public ThongKeHangTon()
        {
            InitializeComponent();
            LoadPieChart();
       
        }

        private void LoadPieChart()
        {
            chart1.Series.Clear();
            chart1.BackColor = Color.FromArgb(46, 66, 82); // Nền tối
            chart1.ChartAreas[0].BackColor = Color.FromArgb(46, 66, 82); // Nền vùng vẽ
            chart1.ChartAreas[0].ShadowColor = Color.Gray;

            // Tạo Series
            Series series = new Series
            {
                Name = "TongTienXuat",
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Pie,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                LabelForeColor = Color.White // Màu chữ trắng
            };
            chart1.Series.Add(series);

            // Tiêu đề biểu đồ
            chart1.Titles.Clear();
            Title chartTitle = new Title
            {
                Text = "Tổng Tiền Xuất Hàng",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White, // Màu chữ tiêu đề
                BackColor = Color.Transparent,
                Alignment = ContentAlignment.TopCenter
            };
            chart1.Titles.Add(chartTitle);

            // Chú thích (legend)
            chart1.Legends[0].BackColor = Color.FromArgb(46, 66, 82);
            chart1.Legends[0].ForeColor = Color.White;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            int selectedMonth = dtpThangNam.Value.Month;
            int selectedYear = dtpThangNam.Value.Year;

            // Thực hiện thống kê
            ThongKeTheoThangNam(selectedMonth, selectedYear);
        }

        private void ThongKeTheoThangNam(int month, int year)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=HI;Initial Catalog=QLBgiay;Integrated Security=True"))
            {
                conn.Open();

                // Truy vấn để tính tổng số tiền xuất hàng theo nhân viên trong tháng và năm đã chọn
                string query = @"
                    SELECT p.MaNV, nv.TenNV, SUM(ctpx.SoLuongXuat * ctpx.DonGiaXuat) AS TongTienXuat
                    FROM tblPhieuXuat p
                    INNER JOIN tblChiTietPhieuXuat ctpx ON p.MaPX = ctpx.MaPX
                    INNER JOIN tblNhanVien nv ON p.MaNV = nv.MaNV
                    WHERE MONTH(p.NgayXuat) = @Month AND YEAR(p.NgayXuat) = @Year
                    GROUP BY p.MaNV, nv.TenNV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Hiển thị dữ liệu trong DataGridView (tuỳ chọn)
                dataGridView1.DataSource = dataTable;

                // Cập nhật biểu đồ
                UpdateChart(dataTable);
            }
        }

        private void UpdateChart(DataTable dataTable)
        {
            // Xóa các series cũ
            chart1.Series.Clear();

            // Tạo series mới cho biểu đồ tròn
            Series series = new Series
            {
                Name = "TongTienXuat",
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Pie
            };

            chart1.Series.Add(series);

            // Thêm dữ liệu vào biểu đồ
            foreach (DataRow row in dataTable.Rows)
            {
                string tenNV = row["TenNV"].ToString();  // Lấy tên nhân viên từ dữ liệu
                decimal tongTienXuat = Convert.ToDecimal(row["TongTienXuat"]);
                series.Points.AddXY(tenNV, tongTienXuat);  // Thêm tên nhân viên vào label của điểm dữ liệu
            }

            // Thêm tiêu đề biểu đồ
            chart1.Titles.Clear();
            chart1.Titles.Add($"Tổng tiền xuất hàng theo nhân viên - {dtpThangNam.Value:MM/yyyy}");
        }

        private void ThongKeHangTon_Load(object sender, EventArgs e)
        {
            // Xử lý khi form được load
        }

        private void btnThongKe1_Click(object sender, EventArgs e)
        {
            int selectedMonth = dtpThangNam1.Value.Month;
            int selectedYear = dtpThangNam1.Value.Year;

            // Execute the statistics for "Nhap" data
            ThongKeNhapTheoThangNam(selectedMonth, selectedYear);
        }
        private void LoadDoughnutChart()
        {
            chart2.Series.Clear();

            // Create a new series for doughnut chart
            Series series = new Series
            {
                Name = "TongTienNhap",
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut
            };

            chart2.Series.Add(series);

            // Set titles and format
            chart2.Titles.Clear();
            Title chartTitle = new Title
            {
                Text = "Thống kê tổng tiền nhập hàng theo nhân viên",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White, // Màu chữ tiêu đề
                BackColor = Color.Transparent,
                Alignment = ContentAlignment.TopCenter
            };
            chart2.Titles.Add(chartTitle);

            // Nền và giao diện biểu đồ
            chart2.BackColor = Color.FromArgb(46, 66, 82); // Nền tối giống chart1
            chart2.ChartAreas[0].BackColor = Color.FromArgb(46, 66, 82);
            chart2.Legends[0].BackColor = Color.FromArgb(46, 66, 82);

            // Màu chữ
            chart2.Titles[0].ForeColor = Color.White;
            chart2.Legends[0].ForeColor = Color.White;
        }
        private void ThongKeNhapTheoThangNam(int month, int year)
        {
            using (SqlConnection conn = new SqlConnection(@"Data Source=HI;Initial Catalog=QLBgiay;Integrated Security=True"))
            {
                conn.Open();

                // Query to get the total imported amount by employee for the selected month and year
                string query = @"
            SELECT p.MaNV, nv.TenNV, SUM(ctpn.SoLuongNhap * ctpn.DonGiaNhap) AS TongTienNhap
            FROM tblPhieuNhap p
            INNER JOIN tblChiTietPhieuNhap ctpn ON p.MaPN = ctpn.MaPN
            INNER JOIN tblNhanVien nv ON p.MaNV = nv.MaNV
            WHERE MONTH(p.NgayNhap) = @Month AND YEAR(p.NgayNhap) = @Year
            GROUP BY p.MaNV, nv.TenNV";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Month", month);
                cmd.Parameters.AddWithValue("@Year", year);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Optional: Display data in DataGridView
                dataGridView2.DataSource = dataTable;

                // Update radar chart with data
                UpdateDoughnutChart(dataTable);
            }
        }
        private void UpdateDoughnutChart(DataTable dataTable)
        {
            // Xóa các series cũ
            chart2.Series.Clear();

            // Tạo series mới
            Series series = new Series
            {
                Name = "TongTienNhap",
                IsValueShownAsLabel = true,
                ChartType = SeriesChartType.Doughnut
            };

            chart2.Series.Add(series);

            // Đặt bảng màu giống chart1 (hoặc định nghĩa màu thủ công)
            Color[] colors = { Color.FromArgb(255, 87, 34), Color.FromArgb(33, 150, 243), Color.FromArgb(76, 175, 80),
                       Color.FromArgb(255, 193, 7), Color.FromArgb(156, 39, 176) };

            int colorIndex = 0;

            // Thêm dữ liệu vào biểu đồ
            foreach (DataRow row in dataTable.Rows)
            {
                string tenNV = row["TenNV"].ToString();
                decimal tongTienNhap = Convert.ToDecimal(row["TongTienNhap"]);
                DataPoint point = new DataPoint();
                point.SetValueXY(tenNV, tongTienNhap);
                point.Color = colors[colorIndex % colors.Length]; // Lặp lại màu nếu cần
                colorIndex++;
                series.Points.Add(point);
            }

            // Xóa các tiêu đề cũ
            chart2.Titles.Clear();

            // Thêm tiêu đề mới
            Title chartTitle = new Title
            {
                Text = $"Tổng tiền nhập hàng theo nhân viên - {dtpThangNam1.Value:MM/yyyy}",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White, // Màu chữ
                BackColor = Color.Transparent,
                Alignment = ContentAlignment.TopCenter
            };

            chart2.Titles.Add(chartTitle);

            // Định dạng biểu đồ
            chart2.BackColor = Color.FromArgb(46, 66, 82); // Nền tối giống chart1
            chart2.ChartAreas[0].BackColor = Color.FromArgb(46, 66, 82);
            chart2.Legends[0].BackColor = Color.FromArgb(46, 66, 82);

            chart2.Legends[0].ForeColor = Color.White; // Màu chữ chú thích

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dtpThangNam1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
