using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace yazlab
{
    public partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
            studentUserControl1.teacher = teacherUserControl1;
        }
        NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");

        public bool VerifyLogin(string role, string username, string password)
        {
            if ((string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) && role!="admin" )
            {
                return false;
            }

            int userId = 0;
            bool loginSuccess = false;
            if (role == "admin")
                loginSuccess = true;

            connection.Open();
            try
            {
                string query = "";
                if (role == "student")
                {
                    query = "SELECT student_id FROM students WHERE username = @username AND password = @password";
                }
                else if (role == "teacher")
                {
                    query = "SELECT identification_number FROM teachers WHERE username = @username AND password = @password";
                }

                using (NpgsqlCommand cmd = new NpgsqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        userId = Convert.ToInt32(result);
                        loginSuccess = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                connection.Close();
            }

            if (loginSuccess)
            {
                if (role == "student")
                {
                    studentUserControl1.studentIdSet(userId);
                    studentUserControl1.Visible = true;
                }
                else if (role == "teacher")
                {
                    teacherUserControl1.teacherIdSet(userId);
                    teacherUserControl1.Visible = true;
                }
                else if (role == "admin")
                {
                    adminUserControl1.Visible = true;
                }
            }

            return loginSuccess;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string role = radioButton1.Checked
        ? "student"
        : radioButton2.Checked
        ? "teacher"
        : radioButton3.Checked
        ? "admin"
        : "";


            string username = textBox1.Text;
            string password = textBox2.Text;

            if (VerifyLogin(role, username, password))
            {
                MessageBox.Show("Login successful!");

            }
            else
            {
                MessageBox.Show("Login failed. Please check your credentials.");
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
