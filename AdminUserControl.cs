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
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int studentIdToDelete = (int)comboBox3.SelectedValue; // silmek istediğiniz öğretmenin kimliğini belirtmelisiniz.

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

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            int teacherIdToDelete = (int)comboBox2.SelectedValue; // Silmek istediğiniz öğretmenin kimliğini belirtmelisiniz.

            NpgsqlCommand komut1 = new NpgsqlCommand("DELETE FROM teachers WHERE identification_number = @teacherId", baglanti);
            komut1.Parameters.AddWithValue("@teacherId", teacherIdToDelete);
            komut1.ExecuteNonQuery();
            baglanti.Close();

            comboBoxUpdate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            comboBoxUpdate();
            
        }
        void comboBoxUpdate()
        {
            baglanti.Open();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            comboBox1.DisplayMember = "FullName"; // Görüntülenen değer
            comboBox1.ValueMember = "identification_number"; // Geri alınacak değer
            comboBox1.DataSource = dt;

            NpgsqlDataAdapter data = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", baglanti);
            DataTable dataTable = new DataTable();
            data.Fill(dataTable);
            comboBox2.DisplayMember = "FullName";
            comboBox2.ValueMember = "identification_number";
            comboBox2.DataSource = dataTable;

            da = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", baglanti);
            dt = new DataTable();
            da.Fill(dt);
            comboBox3.DisplayMember = "FullName"; // Görüntülenen değer
            comboBox3.ValueMember = "student_id"; // Geri alınacak değer
            comboBox3.DataSource = dt;

            baglanti.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            NpgsqlCommand komut2 = new NpgsqlCommand("insert into  students(name,surname,username,password) values (@p1,@p2,@p3,@p4)", baglanti);
            komut2.Parameters.AddWithValue("@p1", textBox7.Text);
            komut2.Parameters.AddWithValue("@p2", textBox8.Text);
            komut2.Parameters.AddWithValue("@p3", textBox6.Text);
            komut2.Parameters.AddWithValue("@p3", textBox6.Text);
            komut2.Parameters.AddWithValue("@p4", textBox9.Text);
            komut2.ExecuteNonQuery();
            baglanti.Close();

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
        public string GetExistingLecturesJson(int teacherId, NpgsqlConnection connection)
        {
            string existingLecturesJson = null;
            using (NpgsqlCommand command = new NpgsqlCommand("SELECT lectures FROM teachers WHERE identification_number = @teacherid", connection))
            {
                command.Parameters.AddWithValue("teacherid", teacherId);
                //           connection.Open();
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        existingLecturesJson = reader["lectures"].ToString();
                    }
                }
                //     connection.Close();
            }
            return existingLecturesJson;
        }


        private void button10_Click(object sender, EventArgs e)
        {

            baglanti.Open();

            int selectedValue = (int)comboBox1.SelectedValue; // ComboBox'tan seçilen öğenin değerini al

            // Öncelikle mevcut JSONB verileri çekin
            string existingLecturesJson = GetExistingLecturesJson(selectedValue, baglanti);

            if (string.IsNullOrEmpty(existingLecturesJson))
            {
                // Mevcut JSONB verisi null veya boşsa, yeni bir JSONB dizisi oluştur
                existingLecturesJson = $@"[{{""Code"": ""{textBox11.Text}"", ""Name"": ""{textBox10.Text}""}}]";
            }
            else
            {
                // Mevcut JSONB verisi null değilse, mevcut verileri koruyarak yeni dersi eklemek için birleştir
                existingLecturesJson = existingLecturesJson.Remove(existingLecturesJson.Length - 1) + ","; // Son "]" karakterini kaldır
                existingLecturesJson += $@"{{""Code"": ""{textBox11.Text}"", ""Name"": ""{textBox10.Text}""}}]";
            }

            // Güncellenmiş JSONB verisini kullanarak güncelleme işlemi
            using (NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET lectures = @jsonlectures WHERE identification_number = @teacherid", baglanti))
            {
                komut1.Parameters.AddWithValue("jsonlectures", NpgsqlTypes.NpgsqlDbType.Jsonb, existingLecturesJson);
                komut1.Parameters.AddWithValue("teacherid", selectedValue); // Öğretmenin kimliği burada belirtilmelidir
                komut1.ExecuteNonQuery();
            }

            baglanti.Close();


        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {


            baglanti.Open();
            int teacherIdToUpdate = (int)comboBox2.SelectedValue; // Güncellemek istediğiniz öğretmenin kimliğini belirtmelisiniz.

            NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET name = @p1, surname = @p2, username = @p3, password = @p4, quota = @p5 WHERE identification_number = @teacherid", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox2.Text);
            komut1.Parameters.AddWithValue("@p2", textBox3.Text);
            komut1.Parameters.AddWithValue("@p3", textBox1.Text);
            komut1.Parameters.AddWithValue("@p4", textBox5.Text);
            komut1.Parameters.AddWithValue("@p5", int.Parse(textBox4.Text));
            komut1.Parameters.AddWithValue("@teacherid", teacherIdToUpdate); // Güncellemek istediğiniz öğretmenin kimliği
            komut1.ExecuteNonQuery();

            baglanti.Close();

            comboBoxUpdate();

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            int studentIdToUpdate = (int)comboBox3.SelectedValue; // Güncellemek istediğiniz öğretmenin kimliğini belirtmelisiniz.

            NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE students SET name = @p1, surname = @p2, username = @p3, password = @p4 WHERE student_id = @studentId", baglanti);
            komut1.Parameters.AddWithValue("@p1", textBox7.Text);
            komut1.Parameters.AddWithValue("@p2", textBox11.Text);
            komut1.Parameters.AddWithValue("@p3", textBox6.Text);
            komut1.Parameters.AddWithValue("@p4", textBox8.Text);
            komut1.Parameters.AddWithValue("@studentId", studentIdToUpdate); // Güncellemek istediğiniz öğretmenin kimliği
            komut1.ExecuteNonQuery();

            baglanti.Close();

            comboBoxUpdate();
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void AdminUserControl_Load(object sender, EventArgs e)
        {

        }
    }
}
