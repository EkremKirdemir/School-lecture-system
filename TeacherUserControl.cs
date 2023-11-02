using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace yazlab
{
    public partial class TeacherUserControl : UserControl
    {
        public TeacherUserControl()
        {
            InitializeComponent();
            comboBoxUpdate();
        }

        private void TeacherUserControl_Load(object sender, EventArgs e)
        {

        }
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        List<CourseData> courses = new List<CourseData>();
        List<Criteria> criterias = new List<Criteria>();
        void comboBoxUpdate()
        {
            baglanti.Open();
            NpgsqlDataAdapter studentDa = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", baglanti);
            DataTable studentDt = new DataTable();
            studentDa.Fill(studentDt);
            baglanti.Close();
            studentsComboBox.DisplayMember = "FullName";
            studentsComboBox.ValueMember = "student_id";
            studentsComboBox.DataSource = studentDt;
            DataTable dataTab1 = (DataTable)studentsComboBox.DataSource;
            DataRow newRow1 = dataTab1.NewRow();
            newRow1["FullName"] = "";
            newRow1["student_id"] = -1;
            dataTab1.Rows.InsertAt(newRow1, 0);
            studentsComboBox.DataSource = dataTab1;
            studentsComboBox.SelectedIndex = 0;
            List<int> studentIds = GetStudentIdsWithNonNullTranscripts();
            foreach (int id in studentIds)
            {
                var studentCourses = FetchCourseData(id);
                foreach (var course in studentCourses)
                {     
                    if (!courses.Any(c => c.Code == course.Code))
                    {
                        courses.Add(course);
                        comboBoxCriteria.Items.Add(course.Name);
                    }
                }
            }


        }
        public List<int> GetStudentIdsWithNonNullTranscripts()
        {
            List<int> studentIds = new List<int>();

            baglanti.Open();
            string sqlQuery = "SELECT student_id FROM students WHERE transcript IS NOT NULL";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, baglanti))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentIds.Add(reader.GetInt32(0));
                    }
                }
            }
            baglanti.Close();

            return studentIds;
        }
        public List<CourseData> FetchCourseData(int studentId)
        {
            List<CourseData> courses = new List<CourseData>();


            baglanti.Open();
            try
            {
                string sqlSelect = "SELECT transcript FROM students WHERE student_id=@studentId AND transcript IS NOT null";

                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, baglanti))
                {
                    selectCommand.Parameters.AddWithValue("@studentId", studentId);

                    using (var reader = selectCommand.ExecuteReader())
                    {
                        if (reader.Read() && !reader.IsDBNull(0))
                        {
                            string jsonData = reader.GetString(0);
                            courses = JsonSerializer.Deserialize<List<CourseData>>(jsonData);
                        }
                    }
                }
            }
            catch
            {
                transcripListBox.Items.Clear();
                transcripListBox.Items.Add("transcript not available");
            }

            baglanti.Close();


            return courses;
        }
        private void studentsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (studentsComboBox.SelectedIndex != 0)
            {
                transcripListBox.Items.Clear();
                transcripListBox.Visible = true;
                List<CourseData> Course = FetchCourseData((int)studentsComboBox.SelectedValue);
                if (Course.Count > 0)
                {
                    foreach (CourseData course in Course)
                    {
                        transcripListBox.Items.Add(course.Code + " " + course.Name + " " + course.Credit);
                    }
                }
            }
        }

        private void buttonAddCriteria_Click(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(textBoxCriteria.Text, out value) && comboBoxCriteria.SelectedItem != null)
            {
                Criteria criteria = new Criteria();
                criteria.Name = comboBoxCriteria.SelectedItem.ToString();
                criteria.Value = value;
                criterias.Add(criteria);
                listBoxCriteria.Items.Add(criteria.Name + " X " + criteria.Value.ToString());
            }
            else if (textBoxCriteria.Text == "" && comboBoxCriteria.SelectedItem != null)
            {
                Criteria criteria = new Criteria();
                criteria.Name = comboBoxCriteria.SelectedItem.ToString();
                criteria.Value = 1;
                criterias.Add(criteria);
                listBoxCriteria.Items.Add(criteria.Name + " X " + criteria.Value.ToString());
            }

        }

        private void buttonDeleteCriteria_Click(object sender, EventArgs e)
        {
            if (listBoxCriteria.SelectedItem != null)
            {
                string selectedItem = comboBoxCriteria.SelectedItem.ToString().Split(" X ")[0];
                criterias.RemoveAll(criteria => criteria.Name == selectedItem);
                listBoxCriteria.Items.Remove(listBoxCriteria.SelectedItem);
            }
        }

        private void buttonInterest_Click(object sender, EventArgs e)
        {
            if (textBoxInterest.Text != "")
            {
                var newInterest = new Interest
                {
                    interest_area = textBoxInterest.Text
                };

                baglanti.Open();
                NpgsqlCommand selectCommand = new NpgsqlCommand("SELECT interest_areas FROM teachers WHERE identification_number = 1", baglanti);
                string existingJsonData = selectCommand.ExecuteScalar() as string;
                baglanti.Close();

                List<Interest> existingInterests;

                if (!string.IsNullOrEmpty(existingJsonData))
                {
                    existingInterests = JsonSerializer.Deserialize<List<Interest>>(existingJsonData);
                }
                else
                {
                    existingInterests = new List<Interest>();
                }

                existingInterests.Add(newInterest);

                string updatedJsonStr = JsonSerializer.Serialize(existingInterests);
                NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET interest_areas = @p1 WHERE identification_number = 1", baglanti);
                komut1.Parameters.Add(new NpgsqlParameter("@p1", NpgsqlDbType.Jsonb) { Value = updatedJsonStr });
                baglanti.Open();
                komut1.ExecuteNonQuery();
                baglanti.Close();



            }
        }
    }
    public class Criteria
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    public class Interest
    {
        public string interest_area { get; set; }
    }
}
