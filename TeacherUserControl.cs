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
            messageComboBoxUpdate();
        }

        private void TeacherUserControl_Load(object sender, EventArgs e)
        {

        }
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        List<CourseData> courses = new List<CourseData>();
        List<Criteria> criterias = new List<Criteria>();
        List<Rank> ranks = new List<Rank>();
        void messageComboBoxUpdate()
        {
            baglanti.Open();
            NpgsqlDataAdapter studentDa = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", baglanti);
            DataTable studentDt = new DataTable();
            studentDa.Fill(studentDt);
            baglanti.Close();
            messageComboBox.DisplayMember = "FullName";
            messageComboBox.ValueMember = "student_id";
            messageComboBox.DataSource = studentDt;
        }
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

        private void buttonAcceptCriterias_Click(object sender, EventArgs e)
        {
            transcripListBox.Items.Clear();
            if (criterias.Count == 0)
            {
                MessageBox.Show("No criteria have been added.");
                return;
            }

            // Get student ids with non-null transcripts
            List<int> studentIds = GetStudentIdsWithNonNullTranscripts();
            ranks.Clear(); // Clear the previous ranks, if you want to start fresh each time this is run

            // Loop through each student ID
            foreach (int id in studentIds)
            {
                double sumOfMultipliedMatches = 0;
                bool allCriteriaMatched = true;

                // Fetch course data for the student
                List<CourseData> studentCourses = FetchCourseData(id);

                // Loop through each criteria
                foreach (var criteria in criterias)
                {
                    // Find if there is a matching course for the criteria
                    CourseData matchingCourse = studentCourses.FirstOrDefault(course => course.Name == criteria.Name);

                    if (matchingCourse != null)
                    {
                        double grade;
                        switch (matchingCourse.Credit)
                        {
                            case "AA":
                                grade = 4.0;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "BA":
                                grade = 3.5;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "BB":
                                grade = 3.0;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "CB":
                                grade = 2.5;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "CC":
                                grade = 2.0;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "DC":
                                grade = 1.5;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "DD":
                                grade = 1.0;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "FD":
                                grade = 0.5;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                            case "FF":
                                grade = 0.0;
                                sumOfMultipliedMatches += criteria.Value * grade;
                                break;
                        }
                    }
                    else
                    {
                        // If any criteria do not match, mark it and break out of the loop
                        allCriteriaMatched = false;
                        break;
                    }
                }

                // If all criteria matched for the student, create a Rank object and add it to the ranks list
                if (allCriteriaMatched)
                {
                    Rank rank = new Rank
                    {
                        id = id,
                        Value = sumOfMultipliedMatches
                    };
                    ranks.Add(rank);
                }
            }

            // If you need to do something with the ranks list (like updating the UI), do it here

            MessageBox.Show("Ranking complete.");
            baglanti.Open();
            if (ranks.Count > 1)
            {
                for (int i = 0; i < ranks.Count - 1; i++)
                {
                    for (int j = 1; j < ranks.Count; j++)
                    {
                        if (ranks[i].Value < ranks[j].Value)
                        {
                            List<Rank> temp = new List<Rank>();
                            temp.Add(ranks[i]);
                            ranks[i] = ranks[j];
                            ranks[j] = temp[0];
                            temp.Clear();
                        }
                    }
                }
                for (int i = 0; i < ranks.Count; i++)
                {
                    transcripListBox.Visible = true;
                    string sqlSelect = "SELECT name,surname FROM students WHERE student_id=@i";
                    using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, baglanti))
                    {
                        selectCommand.Parameters.AddWithValue("@i", ranks[i].id);

                        using (NpgsqlDataReader reader = selectCommand.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                transcripListBox.Items.Add(reader["name"].ToString() + " " + reader["surname"].ToString() + "       " + ranks[i].Value.ToString());
                            }
                        }
                    }
                }

            }
            else if (ranks.Count == 1)
            {
                string sqlSelect = "SELECT name,surname FROM students WHERE student_id=@studentId";
                int studentID = ranks[0].id;

                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, baglanti))
                {
                    selectCommand.Parameters.AddWithValue("@studentId", ranks[0].id);

                    using (NpgsqlDataReader reader = selectCommand.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            transcripListBox.Items.Add(reader["name"].ToString() + " " + reader["surname"].ToString() + "       " + ranks[0].Value.ToString());
                            transcripListBox.Visible = true;
                        }
                    }
                }
            }
            baglanti.Close();
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
        public void messagesListBoxUpdate()
        {
            int targetStudentId = (int)messageComboBox.SelectedValue;
            int identificationNumber = 3; // Student ID to filter messages

            string sqlSelectMessages = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

            baglanti.Open();
            using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelectMessages, baglanti))
            {
                selectCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);

                var dbResult = selectCommand.ExecuteScalar();
                var existingJsonData = dbResult != DBNull.Value ? (string)dbResult : "[]";

                var messages = JsonSerializer.Deserialize<List<Content>>(existingJsonData);

                // Clearing the ListBox for the new set of items
                messageListBox.Items.Clear();

                foreach (var message in messages)
                {
                    if (message.StudentId == targetStudentId)
                    {
                        string senderName = "";
                        if (message.Sent == 0)
                        {
                            // Message is from a student
                            senderName = GetNameSurname("students", "student_id", message.StudentId);
                        }
                        else
                        {
                            // Message is from a teacher
                            senderName = GetNameSurname("teachers", "identification_number", identificationNumber);
                        }

                        string displayText = $"{senderName}: {message.Message}";
                        messageListBox.Items.Add(displayText);
                    }
                }
            }
            baglanti.Close();
        }
        private string GetNameSurname(string tableName, string idColumn, int idValue)
        {
            string sqlSelectName = $"SELECT name, surname FROM {tableName} WHERE {idColumn} = @idValue";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlSelectName, baglanti))
            {
                command.Parameters.AddWithValue("@idValue", idValue);
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["name"].ToString() + " " + reader["surname"].ToString();
                    }
                }
            }
            return "Unknown";
        }
        private void messageComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            messagesListBoxUpdate();
        }

        private void messageSendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(messagesTextBox.Text))
            {
                MessageBox.Show("Please enter a message to send.");
                return;
            }

            int identificationNumber = 3;
            string sqlSelect = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

            var newMessage = new
            {
                StudentId = 48, // You might want to replace this with an actual dynamic value
                Message = messagesTextBox.Text.Trim(),
                Sent = 1 // I assume you want to store the date and time when the message was sent
            };

            baglanti.Open();
            using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, baglanti))
            {
                selectCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);

                var dbResult = selectCommand.ExecuteScalar();
                var existingJsonData = dbResult != DBNull.Value ? (string)dbResult : "[]";

                var existingMessages = JsonSerializer.Deserialize<List<dynamic>>(existingJsonData);

                existingMessages.Add(newMessage);

                string updatedJsonStr = JsonSerializer.Serialize(existingMessages);

                string sqlUpdate = "UPDATE teachers SET sent_messages = @updated_json WHERE identification_number = @identificationNumber";

                using (NpgsqlCommand updateCommand = new NpgsqlCommand(sqlUpdate, baglanti))
                {
                    updateCommand.Parameters.AddWithValue("@updated_json", NpgsqlDbType.Jsonb, updatedJsonStr);
                    updateCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);
                    updateCommand.ExecuteNonQuery();
                }
            }
            baglanti.Close();
            messagesListBoxUpdate();
        }
    
    }
    public class Rank
    {
        public int id { get; set; }
        public double Value { get; set; }
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
