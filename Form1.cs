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
        }
        public bool VerifyLogin(string role, string username, string password)
        {
            if (username == "" && password == "")
            {
                if (role == "student")
                {
                    studentUserControl1.studentIdSet(49);
                    studentUserControl1.Visible = true;
                }
                else if (role == "teacher")
                {
                    teacherUserControl1.teacherIdSet(4);
                    teacherUserControl1.Visible = true;              
                }
                else if (role == "admin")
                {
                    adminUserControl1.Visible = true;
                }
                return true;
            }
            else
                return false;
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
