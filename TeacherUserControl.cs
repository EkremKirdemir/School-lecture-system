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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;

namespace yazlab
{
    public partial class TeacherUserControl : UserControl
    {
        public TeacherUserControl()
        {
            InitializeComponent();
        }

        private void TeacherUserControl_Load(object sender, EventArgs e)
        {

        }
        NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        List<CourseData> courses = new List<CourseData>();
        List<Criteria> criterias = new List<Criteria>();
        List<Rank> ranks = new List<Rank>();
        int teacherId;
        StudentUserControl usercontrol1 = new StudentUserControl();
        AdminUserControl usercontroladmin = new AdminUserControl();

        void messageComboBoxUpdate()
        {
            connection.Open();
            NpgsqlDataAdapter studentDa = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", connection);
            DataTable studentDt = new DataTable();
            studentDa.Fill(studentDt);
            connection.Close();
            messageComboBox.DisplayMember = "FullName";
            messageComboBox.ValueMember = "student_id";
            messageComboBox.DataSource = studentDt;
            DataTable dataTab1 = (DataTable)messageComboBox.DataSource;
            DataRow newRow1 = dataTab1.NewRow();
            newRow1["FullName"] = "";
            newRow1["student_id"] = -1;
            dataTab1.Rows.InsertAt(newRow1, 0);
            messageComboBox.DataSource = dataTab1;
            messageComboBox.SelectedIndex = 0;
        }
        void comboBoxUpdate()
        {
            connection.Open();
            NpgsqlDataAdapter studentDa = new NpgsqlDataAdapter("SELECT student_id ||' - '||  name || ' ' || surname AS FullName, student_id FROM students", connection);
            DataTable studentDt = new DataTable();
            studentDa.Fill(studentDt);
            connection.Close();
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
            comboBoxCriteria.Items.Add("");
            foreach (int id in studentIds)
            {
                var studentCourses = FetchCourseData(id);
                if (studentCourses.Count != 0)
                {
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
            comboBoxCriteria.SelectedIndex = 0;
        }
        public List<int> GetStudentIdsWithNonNullTranscripts()
        {
            List<int> studentIds = new List<int>();

            connection.Open();
            string sqlQuery = "SELECT student_id FROM students WHERE transcript IS NOT NULL";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentIds.Add(reader.GetInt32(0));
                    }
                }
            }
            connection.Close();

            return studentIds;
        }
        public List<CourseData> FetchCourseData(int studentId)
        {
            List<CourseData> courses = new List<CourseData>();

            connection.Open();
            try
            {
                string sqlSelect = "SELECT transcript FROM students WHERE student_id=@studentId AND transcript IS NOT null";

                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, connection))
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

            connection.Close();
            return courses;
        }
        private void studentsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            checkedListBox1.Visible = false;
            if (studentsComboBox.SelectedIndex != 0 && studentsComboBox.SelectedValue != null)
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
            if (int.TryParse(textBoxCriteria.Text, out value) && comboBoxCriteria.SelectedItem != null && comboBoxCriteria.SelectedIndex != 0)
            {
                Criteria criteria = new Criteria();
                criteria.Name = comboBoxCriteria.SelectedItem.ToString();
                criteria.Value = value;
                criterias.Add(criteria);
                listBoxCriteria.Items.Add(criteria.Name + " X " + criteria.Value.ToString());
            }
            else if (textBoxCriteria.Text == "" && comboBoxCriteria.SelectedItem != null && comboBoxCriteria.SelectedIndex != 0)
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

            List<int> studentIds = GetStudentIdsWithNonNullTranscripts();
            ranks.Clear();

            foreach (int id in studentIds)
            {
                double sumOfMultipliedMatches = 0;
                bool allCriteriaMatched = true;

                List<CourseData> studentCourses = FetchCourseData(id);

                foreach (var criteria in criterias)
                {
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
                        allCriteriaMatched = false;
                        break;
                    }
                }

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
            MessageBox.Show("Ranking complete.");
            connection.Open();
            if (ranks.Count > 1)
            {
                for (int i = 0; i < ranks.Count - 1; i++)
                {
                    for (int j = i + 1; j < ranks.Count; j++)
                    {
                        if (ranks[i].Value < ranks[j].Value)
                        {
                            Rank temp = ranks[i];
                            ranks[i] = ranks[j];
                            ranks[j] = temp;
                        }
                    }
                }

                for (int i = 0; i < ranks.Count; i++)
                {
                    transcripListBox.Visible = true;
                    string sqlSelect = "SELECT name,surname FROM students WHERE student_id=@i";
                    using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, connection))
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

                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, connection))
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
            connection.Close();
        }
        private void buttonInterest_Click(object sender, EventArgs e)
        {
            if (textBoxInterest.Text != "")
            {
                var newInterest = new Interest
                {
                    interest_area = textBoxInterest.Text
                };

                connection.Open();
                NpgsqlCommand selectCommand = new NpgsqlCommand("SELECT interest_areas FROM teachers WHERE identification_number = 1", connection);
                string existingJsonData = selectCommand.ExecuteScalar() as string;
                connection.Close();

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
                NpgsqlCommand komut1 = new NpgsqlCommand("UPDATE teachers SET interest_areas = @p1 WHERE identification_number = 1", connection);
                komut1.Parameters.Add(new NpgsqlParameter("@p1", NpgsqlDbType.Jsonb) { Value = updatedJsonStr });
                connection.Open();
                komut1.ExecuteNonQuery();
                connection.Close();
            }
        }
        private List<Lecture> DeserializeLectures(string jsonData)
        {
            return JsonSerializer.Deserialize<List<Lecture>>(jsonData);
        }
        public string GetExistingLecturesJson(int teacherId)
        {
            
               connection.Open();
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
            connection.Close();
            return existingLecturesJson;
        }
        public void messagesListBoxUpdate()
        {
            int targetStudentId = (int)messageComboBox.SelectedValue;
            int identificationNumber = teacherId;

            string sqlSelectMessages = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

            connection.Open();
            using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelectMessages, connection))
            {
                selectCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);

                var dbResult = selectCommand.ExecuteScalar();
                var existingJsonData = dbResult != DBNull.Value ? (string)dbResult : "[]";

                var messages = JsonSerializer.Deserialize<List<Content>>(existingJsonData);

                messageListBox.Items.Clear();

                foreach (var message in messages)
                {
                    if (message.StudentId == targetStudentId)
                    {
                        string senderName = "";
                        if (message.Sent == 0)
                        {
                            senderName = GetNameSurname("students", "student_id", message.StudentId);
                        }
                        else
                        {
                            senderName = GetNameSurname("teachers", "identification_number", identificationNumber);
                        }

                        string displayText = $"{senderName}: {message.Message}";
                        messageListBox.Items.Add(displayText);
                    }
                }
            }
            connection.Close();
        }
        private string GetNameSurname(string tableName, string idColumn, int idValue)
        {
            string sqlSelectName = $"SELECT name, surname FROM {tableName} WHERE {idColumn} = @idValue";
            using (NpgsqlCommand command = new NpgsqlCommand(sqlSelectName, connection))
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
            if (messageComboBox.SelectedValue != null)
                messagesListBoxUpdate();
        }

        private void messageSendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(messagesTextBox.Text))
            {
                MessageBox.Show("Please enter a message to send.");
                return;
            }

            int identificationNumber = teacherId;
            string sqlSelect = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

            var newMessage = new
            {
                StudentId = (int)messageComboBox.SelectedValue,
            Message = messagesTextBox.Text.Trim(),
                Sent = 1
            };

            connection.Open();
            using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, connection))
            {
                selectCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);

                var dbResult = selectCommand.ExecuteScalar();
                var existingJsonData = dbResult != DBNull.Value ? (string)dbResult : "[]";

                var existingMessages = JsonSerializer.Deserialize<List<dynamic>>(existingJsonData);

                existingMessages.Add(newMessage);

                string updatedJsonStr = JsonSerializer.Serialize(existingMessages);

                string sqlUpdate = "UPDATE teachers SET sent_messages = @updated_json WHERE identification_number = @identificationNumber";

                using (NpgsqlCommand updateCommand = new NpgsqlCommand(sqlUpdate, connection))
                {
                    updateCommand.Parameters.AddWithValue("@updated_json", NpgsqlDbType.Jsonb, updatedJsonStr);
                    updateCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);
                    updateCommand.ExecuteNonQuery();
                }
            }
            connection.Close();
            messagesListBoxUpdate();
        }

        private void buttonback_Click(object sender, EventArgs e)
        {
            transcripListBox.Items.Clear();
            checkedListBox1.Items.Clear();
            listBoxCriteria.Items.Clear();
            comboBoxCriteria.Items.Clear();
            studentsComboBox.DataSource = null;
            messageComboBox.DataSource = null;
            comboBoxCriteria.DataSource = null;
            courses.Clear();
            criterias.Clear();
            ranks.Clear();
            textBoxCriteria.Clear();
            textBoxInterest.Clear();
            messagesTextBox.Clear();
            this.Visible = false;
        }

        private void TeacherUserControl_VisibleChanged(object sender, EventArgs e)
        {

        }
        public void teacherIdSet(int id)
        {
            teacherId = id;
            comboBoxUpdate();
            messageComboBoxUpdate();
            messagesTextBox.MaxLength = usercontroladmin.characterLimit();
        }

        private void buttonCourseOptions_Click(object sender, EventArgs e)
        {
            buttonApproveCourse.Enabled = false;
            buttonAccept.Enabled = true;
            buttonDemandSmall.Enabled = false;
            buttonDemandMid.Enabled = false;
            checkedListBox1.Visible = true;
            transcripListBox.Visible = false;

            string lecturesJson = GetExistingLecturesJson(teacherId);

            if (!string.IsNullOrEmpty(lecturesJson))
            {
                List<Lecture> lectures = DeserializeLectures(lecturesJson);
                var lecturesToAdd = lectures.Where(l => l.status == "0").ToList();

                foreach (var lecture in lecturesToAdd)
                {
                    checkedListBox1.Items.Add(lecture.Name);
                }
            }


        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            string currentJson = GetExistingLecturesJson(teacherId);
            connection.Open();
            List<Lecture> lectures = DeserializeLectures(currentJson);

            List<int> checkedIndices = new List<int>();

            try
            {
                foreach (int index in checkedListBox1.CheckedIndices)
                {
                    string checkedLectureCode = (string)checkedListBox1.Items[index];

                    var lectureToUpdate = lectures.Find(l => l.Name == checkedLectureCode);
                    if (lectureToUpdate != null)
                    {
                        lectureToUpdate.status = "1";
                        checkedIndices.Add(index);
                    }
                }

                string updatedJson = JsonSerializer.Serialize(lectures);
                using (NpgsqlCommand cmd = new NpgsqlCommand("UPDATE teachers SET lectures = @lecturesJson WHERE identification_number = @teacherId", connection))
                {
                    cmd.Parameters.AddWithValue("@lecturesJson", NpgsqlTypes.NpgsqlDbType.Jsonb, updatedJson);
                    cmd.Parameters.AddWithValue("@teacherId", teacherId);
                    cmd.ExecuteNonQuery();
                }
            }
            finally
            {
                connection.Close();
            }

            for (int i = checkedIndices.Count - 1; i >= 0; i--)
            {
                checkedListBox1.Items.RemoveAt(checkedIndices[i]);
            }
        }

        private void buttonDemandSmall_Click(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();
            foreach (object item in checkedListBox1.CheckedItems)
            {
                string itemText = item.ToString();
                string[] parts = itemText.Split(new char[] { ' ' }, 7);
                if (parts.Length < 4) continue;
                string studentid = parts[2];
                string courseCode = parts[4];
                string courseName = parts[5];


                if (teacherId == -1) continue;

                UpdateDemanderStatus(teacherId, courseCode, courseName, Int32.Parse(studentid), "teacher", "Demanded");
                itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove)
            {
                checkedListBox1.Items.Remove(item);
            }
        }
        public void UpdateDemanderStatus(int teacherId, string demandedCourseCode, string demandedCourseName, int studentid, string demander, string demandstatus)
        {

            connection.Open();

            string sqlSelect = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, connection))
            {
                selectCommand.Parameters.AddWithValue("@studentId", studentid);

                var agreementStatusJson = (string)selectCommand.ExecuteScalar();
                if (!string.IsNullOrEmpty(agreementStatusJson))
                {
                    var agreementStatus = JsonSerializer.Deserialize<List<Demand>>(agreementStatusJson);

                    var indexToUpdate = agreementStatus.FindIndex(d =>
                        d.Demander == demander &&
                        d.TeacherID == teacherId &&
                        d.DemandedCourseCode == demandedCourseCode &&
                        d.DemandedCourseName == demandedCourseName &&
                        d.DemandStatus == "Demanded");

                    if (indexToUpdate != -1)
                    {
                        agreementStatus[indexToUpdate].DemandStatus = demandstatus;

                        var updatedJsonStr = JsonSerializer.Serialize(agreementStatus);

                        string sqlUpdate = "UPDATE students SET agreement_status = @updatedJson WHERE student_id = @studentId";
                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(sqlUpdate, connection))
                        {
                            var param = new NpgsqlParameter("@updatedJson", NpgsqlDbType.Jsonb)
                            {
                                Value = updatedJsonStr
                            };
                            updateCommand.Parameters.Add(param);
                            updateCommand.Parameters.AddWithValue("@studentId", studentid);

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            connection.Close();

        }


        private void buttonDemandCourse_Click(object sender, EventArgs e)
        {
            transcripListBox.Items.Clear();
            checkedListBox1.Items.Clear();
            transcripListBox.Visible = true;
            checkedListBox1.Visible = false;
            List<int> studentIds = new List<int>();
            List<string> studentnames = new List<string>();
            List<string> studentSurnames = new List<string>();

            connection.Open();
            string sqlQuery = "SELECT student_id, name, surname FROM students";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentIds.Add(reader.GetInt32(0));
                   
                        if (!reader.IsDBNull(1))
                        {
                            studentnames.Add(reader.GetString(1));
                        }
                        else
                        {
                            studentnames.Add(string.Empty);
                        }

                        if (!reader.IsDBNull(2))
                        {
                            studentSurnames.Add(reader.GetString(2));
                        }
                        else
                        {
                            studentSurnames.Add(string.Empty);
                        }
                    }
                }
            }
            connection.Close();
            for (int i = 0; i < studentIds.Count; i++)
            {
                transcripListBox.Items.Add(studentIds[i].ToString() + " " + studentnames[i] + " " + studentSurnames[i]);
            }

            buttonApproveCourse.Enabled = false;
            buttonAccept.Enabled = false;
            buttonDemandSmall.Enabled = false;
            buttonDemandMid.Enabled = true;
            comboBoxLectures.Visible = true;
            connection.Close();
            string lecturesJson = GetExistingLecturesJson(teacherId);

            if (!string.IsNullOrEmpty(lecturesJson))
            {
                List<Lecture> lectures = DeserializeLectures(lecturesJson);
                var lecturesToAdd = lectures.Where(l => l.status == "1").ToList();

                foreach (var lecture in lecturesToAdd)
                {
                    comboBoxLectures.Items.Add(lecture.Code + " " + lecture.Name);
                }
            }
        }

        private void buttonDemandedFromYou_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
            checkedListBox1.Items.Clear();
            transcripListBox.Visible = false;
            List<int> studentIds = new List<int>();

            connection.Open();
            string sqlQuery = "SELECT student_id FROM students";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentIds.Add(reader.GetInt32(0));
                    }
                }
            }
            connection.Close();

            foreach (int studentID in studentIds)
            {
                List<Demand> demands = usercontrol1.GetDemandsForStudent(studentID);
                if (demands != null && demands.Count > 0)
                {
                    foreach (var demand in demands)
                    {
                        if (demand.DemandStatus == "Demanded" && demand.Demander == "student" && demand.TeacherID == teacherId)
                        {
                            string displayText = $"{demand.DemandedCourseCode} {demand.DemandedCourseName}";
                            displayText = $"Student ID: {studentID} - " + displayText;
                            checkedListBox1.Items.Add(displayText);
                        }
                    }
                }
            }
            buttonApproveCourse.Enabled = true;
            buttonAccept.Enabled = false;
            buttonDemandSmall.Enabled = false;
            buttonDemandMid.Enabled = false;
        }

        public bool sameExists()
        {
            return true;
        }
        private void buttonShowOther_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Clear();
            checkedListBox1.Visible = true;
            transcripListBox.Visible = false;
            List<int> studentIds = new List<int>();

            connection.Open();
            string sqlQuery = "SELECT student_id FROM students";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentIds.Add(reader.GetInt32(0));
                    }
                }
            }
            connection.Close();

            foreach (int studentID in studentIds)
            {
                List<Demand> demands = usercontrol1.GetDemandsForStudent(studentID);
                if (demands != null && demands.Count > 0)
                {
                    foreach (var demand in demands)
                    {
                        if (demand.DemandStatus == "Demanded" && demand.Demander != "teacher")
                        {
                            string displayText = $"{demand.DemandedCourseCode} {demand.DemandedCourseName}";
                            displayText = $"Student ID: {studentID} - " + displayText;
                            checkedListBox1.Items.Add(displayText);
                        }
                    }
                }
            }
            buttonApproveCourse.Enabled = false;
            buttonAccept.Enabled = false;
            buttonDemandSmall.Enabled = true;
            buttonDemandMid.Enabled = false;
        }

        private void buttonApproveCourse_Click(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();
            foreach (object item in checkedListBox1.CheckedItems)
            {
                string itemText = item.ToString();
                string[] parts = itemText.Split(new char[] { ' ' }, 7);
                if (parts.Length < 4) continue;
                string studentid = parts[2];
                string courseCode = parts[4];
                string courseName = parts[5];


                if (teacherId == -1) continue;

                UpdateDemanderStatus(teacherId, courseCode, courseName, Int32.Parse(studentid), "student", "Approved");
                itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove)
            {
                checkedListBox1.Items.Remove(item);
            }
        }

        private void buttonDemandMid_Click(object sender, EventArgs e)
        {
           
            if (comboBoxLectures.SelectedItem == null)
                MessageBox.Show("Select Lecture");
            else if(transcripListBox.SelectedItem == null)
                MessageBox.Show("Select Student");
            else
            {
                
                string[] parts = comboBoxLectures.SelectedItem.ToString().Split(new char[] { ' ' }, 2); 
                string courseCode = parts[0];
                string courseName = parts[1];
                string[] parts1 = transcripListBox.SelectedItem.ToString().Split(new char[] { ' ' }, 2);
                string studentid = parts1[0];
            Demand demand = new Demand
            {
                Demander = "teacher",
                DemandedCourseCode = courseCode,
                DemandedCourseName = courseName,
                DemandStatus = "Demanded",
                TeacherID = teacherId
            };
            string jsonDemand = JsonSerializer.Serialize(demand);
            usercontrol1.InsertDemand(jsonDemand, Int32.Parse(studentid));
            }

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
    public class Lecture
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string status { get; set; }
    }
}
