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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace yazlab
{
    public partial class AdminUserControl : UserControl
    {
        public AdminUserControl()
        {
            InitializeComponent();
            comboBoxUpdate();

        }
        private void button4_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            int studentIdToDelete = (int)comboBox3.SelectedValue;

            NpgsqlCommand komut1 = new NpgsqlCommand("DELETE FROM students WHERE student_id = @studentId", baglanti);
            komut1.Parameters.AddWithValue("@studentId", studentIdToDelete);
            komut1.ExecuteNonQuery();
            baglanti.Close();
            comboBoxUpdate();

        }
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        //NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab1; User Id=postgres; Password=1822;");
        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut1 = new NpgsqlCommand("insert into  teachers(name,surname,username,password,quota) values (@p1,@p2,@p3,@p4,@p5)", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox2.Text);
            komut1.Parameters.AddWithValue("@p2", textBox3.Text);
            komut1.Parameters.AddWithValue("@p3", textBox1.Text);
            komut1.Parameters.AddWithValue("@p4", textBox5.Text);
            komut1.Parameters.AddWithValue("@p5", int.Parse(textBox4.Text));
            komut1.ExecuteNonQuery();
            baglanti.Close();

            comboBoxUpdate();

        }
        private void button6_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            int teacherIdToDelete = (int)comboBox2.SelectedValue;

            NpgsqlCommand komut1 = new NpgsqlCommand("DELETE FROM teachers WHERE identification_number = @teacherId", baglanti);
            komut1.Parameters.AddWithValue("@teacherId", teacherIdToDelete);
            komut1.ExecuteNonQuery();
            baglanti.Close();

            comboBoxUpdate();
        }
        void comboBoxUpdate()
        {
            baglanti.Open();

            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            baglanti.Close();
            comboBox1.DisplayMember = "FullName";
            comboBox1.ValueMember = "identification_number";
            comboBox1.DataSource = dt;
            baglanti.Open();
            NpgsqlDataAdapter data = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", baglanti);
            DataTable dataTable = new DataTable();
            data.Fill(dataTable);
            baglanti.Close();
            comboBox2.DisplayMember = "FullName";
            comboBox2.ValueMember = "identification_number";
            comboBox2.DataSource = dataTable;

            baglanti.Open();
            NpgsqlDataAdapter studentDa = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", baglanti);
            DataTable studentDt = new DataTable();
            studentDa.Fill(studentDt);
            baglanti.Close();
            comboBox3.DisplayMember = "FullName";
            comboBox3.ValueMember = "student_id";
            comboBox3.DataSource = studentDt;

            DataTable dataTab = (DataTable)comboBox2.DataSource;
            DataRow newRow = dataTab.NewRow();
            newRow["FullName"] = "New Teacher";
            newRow["identification_number"] = -1;
            dataTab.Rows.InsertAt(newRow, 0);
            comboBox2.DataSource = dataTab;
            DataTable dataTab1 = (DataTable)comboBox3.DataSource;
            DataRow newRow1 = dataTab1.NewRow();
            newRow1["FullName"] = "New Student";
            newRow1["student_id"] = -1;
            
            dataTab1.Rows.InsertAt(newRow1, 0);
            comboBox3.DataSource = dataTab1;

        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }


        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut2 = new NpgsqlCommand("insert into  students(name,surname,username,password) values (@p1,@p2,@p3,@p4)", baglanti);
            komut2.Parameters.AddWithValue("@p1", textBox7.Text);
            komut2.Parameters.AddWithValue("@p2", textBox8.Text);
            komut2.Parameters.AddWithValue("@p3", textBox6.Text);
            komut2.Parameters.AddWithValue("@p4", textBox9.Text);
            komut2.ExecuteNonQuery();
            baglanti.Close();

            comboBoxUpdate();

        }
        public string GetExistingLecturesJson(int teacherId, NpgsqlConnection connection)
        {
            string existingLecturesJson = null;
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT lectures FROM teachers WHERE identification_number = @teacherid", connection))
            {
                command.Parameters.AddWithValue("teacherid", teacherId);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        existingLecturesJson = reader["lectures"].ToString();
                    }
                }
            }
            return existingLecturesJson;
        }


        private void button10_Click(object sender, EventArgs e)
        {

            baglanti.Open();

            int selectedValue = (int)comboBox1.SelectedValue;

            string existingLecturesJson = GetExistingLecturesJson(selectedValue, baglanti);

            if (string.IsNullOrEmpty(existingLecturesJson))
            {
                existingLecturesJson = $@"[{{""Code"": ""{textBox11.Text}"", ""Name"": ""{textBox10.Text}"",""status"": ""0""}}]";
            }
            else
            {
                existingLecturesJson = existingLecturesJson.Remove(existingLecturesJson.Length - 1) + ",";
                existingLecturesJson += $@"{{""Code"": ""{textBox11.Text}"", ""Name"": ""{textBox10.Text}"",""status"": ""0""}}]";
            }

            using (NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET lectures = @jsonlectures WHERE identification_number = @teacherid", baglanti))
            {
                komut1.Parameters.AddWithValue("jsonlectures", NpgsqlTypes.NpgsqlDbType.Jsonb, existingLecturesJson);
                komut1.Parameters.AddWithValue("teacherid", selectedValue);
                komut1.ExecuteNonQuery();
            }

            baglanti.Close();

            comboBoxUpdate();


        }
        private void button5_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int teacherIdToUpdate = (int)comboBox2.SelectedValue; 

            NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET name = @p1, surname = @p2, username = @p3, password = @p4, quota = @p5 WHERE identification_number = @teacherid", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox2.Text);
            komut1.Parameters.AddWithValue("@p2", textBox3.Text);
            komut1.Parameters.AddWithValue("@p3", textBox1.Text);
            komut1.Parameters.AddWithValue("@p4", textBox5.Text);
            komut1.Parameters.AddWithValue("@p5", int.Parse(textBox4.Text));
            komut1.Parameters.AddWithValue("@teacherid", teacherIdToUpdate);
            komut1.ExecuteNonQuery();

            baglanti.Close();

            comboBoxUpdate();

        }

        private void button7_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int studentIdToUpdate = (int)comboBox3.SelectedValue;

            NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE students SET name = @p1, surname = @p2, username = @p3, password = @p4 WHERE student_id = @studentId", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox7.Text);
            komut1.Parameters.AddWithValue("@p2", textBox8.Text);
            komut1.Parameters.AddWithValue("@p3", textBox6.Text);
            komut1.Parameters.AddWithValue("@p4", textBox9.Text);
            komut1.Parameters.AddWithValue("@studentId", studentIdToUpdate);
            komut1.ExecuteNonQuery();

            baglanti.Close();

            comboBoxUpdate();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            comboBoxUpdate();

        }

        private void AdminUserControl_Load(object sender, EventArgs e)
        {

        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox3.SelectedIndex == 0)
            {
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
            }
            else
            {

                int studentId = (int)comboBox3.SelectedValue;
                baglanti.Open();
                using (NpgsqlCommand command = new NpgsqlCommand("SELECT username, name, surname,  password FROM students WHERE student_id = @studentId", baglanti))
                {

                    command.Parameters.AddWithValue("@studentId", studentId);
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox6.Text = reader["username"].ToString();
                            textBox7.Text = reader["name"].ToString();
                            textBox8.Text = reader["surname"].ToString();
                            textBox9.Text = reader["password"].ToString();
                        }
                        else
                        {
                            
                        }
                    }
                }
                baglanti.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex == 0)
            {
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
            }
            else
            {

                int identificationNumber = (int)comboBox2.SelectedValue;

                using (NpgsqlCommand command = new NpgsqlCommand("SELECT username, name, surname, quota, password FROM teachers WHERE identification_number = @identificationNumber", baglanti))
                {

                    command.Parameters.AddWithValue("@identificationNumber", identificationNumber);
                    
                    baglanti.Open();
                    using (NpgsqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            textBox1.Text = reader["username"].ToString();
                            textBox2.Text = reader["name"].ToString();
                            textBox3.Text = reader["surname"].ToString();
                            textBox4.Text = reader["quota"].ToString();
                            textBox5.Text = reader["password"].ToString();
                        }
                        else
                        {
                            
                        }
                    }
                    baglanti.Close();
                }
            }
        }
    }
}
