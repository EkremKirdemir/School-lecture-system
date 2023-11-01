using Npgsql;
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
                    // Check if the course already exists in the Courses list based on Code (or any other criteria you prefer)
                    if (!courses.Any(c => c.Code == course.Code))
                    {
                        courses.Add(course);
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
    }
}
