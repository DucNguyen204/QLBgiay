using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StoreManagement.Crystal_Report
{
    public partial class trangchunhanvien : Form
    {
        public trangchunhanvien()
        {
            InitializeComponent();
            timer1.Interval = 1000; // Cập nhật mỗi giây
            timer1.Tick += Timer1_Tick;
            timer1.Start();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            // Hiển thị giờ và ngày hiện tại trong một Label
            lbtime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }
        private void Clear_panel()
        {
            // Giả sử main là một Panel, bạn có thể xóa các điều khiển con của nó như sau:
            main.Controls.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Clear_panel();
            Hang hang = new Hang()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            main.Controls.Add(hang);
            hang.Show();
        }


        private void Button_MouseDown(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.Purple; // Thay đổi màu nền khi nhấn chuột
            }
        }

        // Sự kiện khi nhả chuột ra
        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.DarkSlateGray; // Trả lại màu nền ban đầu
            }
        }

        // Sự kiện khi chuột rời khỏi button
        private void Button_MouseLeave(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                btn.BackColor = Color.DarkSlateGray; // Màu nền ban đầu khi chuột rời khỏi button
            }
        }
        private void main_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnhangton_Click(object sender, EventArgs e)
        {
            Clear_panel();
            ThongKeHangTon tk = new ThongKeHangTon()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            main.Controls.Add(tk);
            tk.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Clear_panel();
            NhaCungCap ncc = new NhaCungCap()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            main.Controls.Add(ncc);
            ncc.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clear_panel();
            NhapHang nh = new NhapHang()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            main.Controls.Add(nh);
            nh.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clear_panel();
            XuatHang xh = new XuatHang()
            {
                Dock = DockStyle.Fill,
                TopLevel = false,
                TopMost = true
            };
            main.Controls.Add(xh);
            xh.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DangNhap dn = new DangNhap();
            dn.Show();
            this.Close();
        }

        private void trangchunhanvien_Load(object sender, EventArgs e)
        {
            lbtime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
