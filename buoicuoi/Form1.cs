using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using buoicuoi.Models;

namespace buoicuoi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra và chuyển đổi tuổi từ chuỗi sang số nguyên
            if (!int.TryParse(txttuoi.Text, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi hợp lệ.");
                return;
            }

            using (var context = new Model1())
            {
                // Tạo đối tượng Student mới với thông tin từ các TextBox và ComboBox
                var newStudent = new Student
                {
                    FullName = txtten.Text,
                    Age = age,
                    Major = cbbnganh.SelectedItem?.ToString() ?? string.Empty // Đảm bảo rằng giá trị ngành được lấy đúng cách
                };

                // Thêm sinh viên mới vào DbSet
                context.Students.Add(newStudent);

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Cập nhật lại DataGridView với dữ liệu mới
                var students = context.Students.ToList();
                dgvstudent.DataSource = students;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Lấy tên sinh viên từ TextBox
            string studentName = txtten.Text;

            // Kiểm tra xem tên sinh viên có được nhập không
            if (string.IsNullOrWhiteSpace(studentName))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên cần xóa.");
                return;
            }

            using (var context = new Model1())
            {
                // Tìm sinh viên theo tên
                var studentToRemove = context.Students.FirstOrDefault(s => s.FullName == studentName);

                // Kiểm tra xem sinh viên có tồn tại không
                if (studentToRemove == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên với tên đã nhập.");
                    return;
                }

                // Xóa sinh viên khỏi DbSet
                context.Students.Remove(studentToRemove);

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Cập nhật lại DataGridView với dữ liệu mới
                var students = context.Students.ToList();
                dgvstudent.DataSource = students;

                MessageBox.Show("Xóa sinh viên thành công.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Lấy tên sinh viên từ TextBox
            string studentName = txtten.Text;

            // Kiểm tra xem tên sinh viên có được nhập không
            if (string.IsNullOrWhiteSpace(studentName))
            {
                MessageBox.Show("Vui lòng nhập tên sinh viên cần sửa.");
                return;
            }

            // Kiểm tra và chuyển đổi tuổi từ chuỗi sang số nguyên
            if (!int.TryParse(txttuoi.Text, out int age))
            {
                MessageBox.Show("Vui lòng nhập tuổi hợp lệ.");
                return;
            }

            using (var context = new Model1())
            {
                // Tìm sinh viên theo tên
                var studentToEdit = context.Students.FirstOrDefault(s => s.FullName == studentName);

                // Kiểm tra xem sinh viên có tồn tại không
                if (studentToEdit == null)
                {
                    MessageBox.Show("Không tìm thấy sinh viên với tên đã nhập.");
                    return;
                }

                // Cập nhật thông tin sinh viên
                studentToEdit.Age = age;
                studentToEdit.Major = cbbnganh.Text; // Sử dụng TextBox txtnganh thay vì ComboBox

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Cập nhật lại DataGridView với dữ liệu mới
                var students = context.Students.ToList();
                dgvstudent.DataSource = students;

                MessageBox.Show("Sửa thông tin sinh viên thành công.");
            }
        }

    }
}
