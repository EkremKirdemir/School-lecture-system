using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using NpgsqlTypes;
using Spire.Pdf;
using Spire.Pdf.Exporting.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Globalization;
using Spire.Pdf.Grid;
using Newtonsoft.Json.Linq;

namespace yazlab
{
    public partial class StudentUserControl : UserControl
    {
        public StudentUserControl()
        {
            InitializeComponent();

        }

        private void StudentUserControl_Load(object sender, EventArgs e)
        {

        }
        NpgsqlConnection connection = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        List<double> gpaList = new List<double>();
        int studentid;
        AdminUserControl usercontroladmin = new AdminUserControl();
        public TeacherUserControl teacher;
        List<CourseData> SplitTranscript(string text)
        {
            List<CourseData> courseData = new List<CourseData>();

            string[] splittedLines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            for (int i = 0; i < splittedLines.Length; i++)
            {
                if (splittedLines[i].Trim().StartsWith("DNO"))
                {
                    MatchCollection matches = Regex.Matches(splittedLines[i], @"GNO:(\d+(\.\d+)?)");

                    if (matches.Count > 0)
                    {
                        gpaList.Add(Double.Parse(matches[matches.Count - 1].ToString().Substring(4), CultureInfo.InvariantCulture));
                    }

                }
                if (splittedLines[i].EndsWith("(Comment)"))
                {
                    int x = i + 1;
                    while (x < splittedLines.Length && !splittedLines[x].Trim().StartsWith("DNO"))
                    {
                        string[] a = System.Text.RegularExpressions.Regex.Split(splittedLines[x], @"\s+");

                        List<string> course = new List<string>();

                        if (a.Length > 1)
                        {
                            course.Add(a[1]);
                            string Name = "";
                            for (int j = 2; j < a.Length; j++)
                            {
                                if (a[j] != "Z" && a[j] != "S")
                                {
                                    Name += " " + a[j];
                                }
                                else
                                {
                                    break;
                                }
                            }
                            course.Add(Name.Trim());
                            if (a.Length >= 3)
                            {
                                course.Add(a[a.Length - 3]);
                            }
                            courseData.Add(new CourseData
                            {
                                Code = course[0],
                                Name = course[1],
                                Credit = course.Count >= 3 ? course[2] : ""
                            });
                        }
                        x += 2;
                    }
                }
            }
            return courseData;
        }

        private void OCR(string pdfFilePath)
        {
            PdfDocument pdfDocument = new PdfDocument();
            pdfDocument.LoadFromFile(pdfFilePath);
            List<dynamic> allCourseData = new List<dynamic>();
            foreach (PdfPageBase pdfPage in pdfDocument.Pages)
            {
                string pageText = pdfPage.ExtractText();
                List<CourseData> courseData = SplitTranscript(pageText);
                allCourseData.AddRange(courseData.Select(course => new
                {
                    Code = course.Code,
                    Name = course.Name,
                    Credit = course.Credit
                }));
            }

            double gpa = gpaList.Last();

            connection.Open();

            string gpaUpdate = "UPDATE students SET gpa = @gpa WHERE student_id=@studentid";
            using (NpgsqlCommand updateCommand = new NpgsqlCommand(gpaUpdate, connection))
            {
                updateCommand.Parameters.AddWithValue("@gpa", gpa);
                updateCommand.Parameters.AddWithValue("@studentid", studentid);
                updateCommand.ExecuteNonQuery();
            }

            string updatedJsonStr = JsonSerializer.Serialize(allCourseData);

            string sqlUpdate = "UPDATE students SET transcript = @updated_json WHERE student_id=@studentid";
            using (NpgsqlCommand updateCommand = new NpgsqlCommand(sqlUpdate, connection))
            {
                updateCommand.Parameters.Add(new NpgsqlParameter("@updated_json", NpgsqlDbType.Jsonb) { Value = updatedJsonStr });
                updateCommand.Parameters.AddWithValue("@studentid", studentid);
                updateCommand.ExecuteNonQuery();
            }

            connection.Close();

            foreach (var course in allCourseData)
            {
                listBox1.Items.Add(course.Code + "   " + course.Name + "   " + course.Credit);
            }
        }

        private void usTranscript_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Open File";
                openFileDialog.Filter = "PDF (*.pdf*)|*.pdf*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {

                    string selectedFilePath = openFileDialog.FileName;
                    listBox1.Items.Clear();
                    listBox1.Visible = true;
                    label1.Text = "Course list";
                    OCR(selectedFilePath);
                }
            }
            usTranscript.Enabled = false;
        }
        private List<Lecture> DeserializeLectures(string jsonData)
        {
            return JsonSerializer.Deserialize<List<Lecture>>(jsonData);
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
        public List<int> GetAllTeacherİds()
        {
            List<int> TeacherIds = new List<int>();

            connection.Open();
            string sqlQuery = "SELECT identification_number FROM teachers";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TeacherIds.Add(reader.GetInt32(0));
                    }
                }
            }
            connection.Close();

            return TeacherIds;
        }

        private void demandCourse_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            buttonAcceptDemand.Enabled = false;
            buttonDemand.Enabled = true;
            checkedListBox1.Visible = true;
            checkedListBox1.Items.Clear();
            List<int> teacherIds = GetAllTeacherİds();
            foreach (int teacherId in teacherIds)
            {
                connection.Open();
                string lecturesJson = GetExistingLecturesJson(teacherId, connection);

                if (!string.IsNullOrEmpty(lecturesJson))
                {
                    List<Lecture> lectures = DeserializeLectures(lecturesJson);
                    var lecturesToAdd = lectures.Where(l => l.status == "1").ToList();

                    foreach (var lecture in lecturesToAdd)
                    {
                        string sqlSelectName = $"SELECT name, surname FROM teachers WHERE identification_number = @teacherId";
                        using (NpgsqlCommand command = new NpgsqlCommand(sqlSelectName, connection))
                        {
                            command.Parameters.AddWithValue("@teacherId", teacherId);
                            using (NpgsqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    checkedListBox1.Items.Add(lecture.Code + " " + lecture.Name + " " + reader["name"] + " " + reader["surname"]);
                                }
                            }
                        }
                    }
                }

                connection.Close();
            }
        }

        private void sdCourses_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = true;
            buttonAcceptDemand.Enabled = false;
            buttonDemand.Enabled = false;
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            List<Demand> demands = GetDemandsForStudent(studentID);
            if (demands != null && demands.Count > 0)
            {
                foreach (var demand in demands)
                {
                    if (demand.DemandStatus == "Demanded" && demand.Demander == "student")
                    {
                        string teacherDetails = GetTeacherNameSurnameById(demand.TeacherID);
                        string displayText = $"{demand.DemandedCourseCode} {demand.DemandedCourseName} {teacherDetails} {demand.DemandStatus}";
                        checkedListBox1.Items.Add(displayText);
                    }
                }
            }
            else
            {
            }
        }


        private void Delete_Click(object sender, EventArgs e)
        {
            for (int i = checkedListBox1.Items.Count - 1; i >= 0; i--)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    checkedListBox1.Items.RemoveAt(i);
                }
            }
        }

        private void add_Click(object sender, EventArgs e)
        {
            checkedListBox1.Items.Add("showing demanded");
        }
        public void messageComboboxUpdate()
        {
            connection.Open();
            NpgsqlDataAdapter data = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", connection);
            DataTable dataTable = new DataTable();
            data.Fill(dataTable);
            connection.Close();
            messagesComboBox.DisplayMember = "FullName";
            messagesComboBox.ValueMember = "identification_number";
            messagesComboBox.DataSource = dataTable;
            DataTable dataTab1 = (DataTable)messagesComboBox.DataSource;
            DataRow newRow1 = dataTab1.NewRow();
            newRow1["FullName"] = "";
            newRow1["identification_number"] = -1;
            dataTab1.Rows.InsertAt(newRow1, 0);
            messagesComboBox.DataSource = dataTab1;
            messagesComboBox.SelectedIndex = 0;
        }
        public void messagesListBoxUpdate()
        {
            if (messagesComboBox.SelectedIndex != 0)
            {
                int identificationNumber = (int)messagesComboBox.SelectedValue;
                int targetStudentId = studentid;

                string sqlSelectMessages = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

                connection.Open();
                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelectMessages, connection))
                {
                    selectCommand.Parameters.AddWithValue("@identificationNumber", identificationNumber);

                    var dbResult = selectCommand.ExecuteScalar();
                    var existingJsonData = dbResult != DBNull.Value ? (string)dbResult : "[]";

                    var messages = JsonSerializer.Deserialize<List<Content>>(existingJsonData);

                    messagesListBox.Items.Clear();

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
                            messagesListBox.Items.Add(displayText);
                        }
                    }
                }
                connection.Close();
            }
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
        private void messagesComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            messagesListBoxUpdate();
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(messageTextBox.Text))
            {
                MessageBox.Show("Please enter a message to send.");
                return;
            }

            int identificationNumber = (int)messagesComboBox.SelectedValue;
            string sqlSelect = "SELECT sent_messages FROM teachers WHERE identification_number = @identificationNumber";

            var newMessage = new
            {
                StudentId = studentid,
                Message = messageTextBox.Text.Trim(),
                Sent = 0
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

        private void buttonBack_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            checkedListBox1.Items.Clear();
            messagesListBox.Items.Clear();
            messagesComboBox.SelectedIndex = 0;
            messageTextBox.Clear();
            gpaList.Clear();
            this.Visible = false;
        }

        private void buttonDemand_Click(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();
            foreach (object item in checkedListBox1.CheckedItems)
            {
                string itemText = item.ToString();
                string[] parts = itemText.Split(new char[] { ' ' }, 4);
                if (parts.Length < 4) continue;

                string courseCode = parts[0];
                string courseName = parts[1];
                string teacherName = parts[2];
                string teacherSurname = parts[3];

                int teacherID = GetTeacherIdByName(teacherName, teacherSurname);
                if (teacherID == -1) continue;

                Demand demand = new Demand
                {
                    Demander = "student",
                    DemandedCourseCode = courseCode,
                    DemandedCourseName = courseName,
                    DemandStatus = "Demanded",
                    TeacherID = teacherID
                };

                string jsonDemand = JsonSerializer.Serialize(demand);
                InsertDemand(jsonDemand, studentid);
                itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove)
            {
                checkedListBox1.Items.Remove(item);
            }
        }
        private int GetTeacherIdByName(string name, string surname)
        {
            connection.Open();
            int teacherId = -1;
            string sql = "SELECT identification_number FROM teachers WHERE name = @name AND surname = @surname";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        teacherId = reader.GetInt32(0);
                    }
                }
            }
            connection.Close();
            return teacherId;
        }
        public void InsertDemand(string jsonDemand, int studentId)
        {
            connection.Open();
            string existingJson = "";
            string fetchSql = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            using (var fetchCmd = new NpgsqlCommand(fetchSql, connection))
            {
                fetchCmd.Parameters.AddWithValue("studentId", studentId);
                using (var reader = fetchCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        existingJson = reader["agreement_status"].ToString();
                    }
                }
            }

            List<Demand> demands = new List<Demand>();

            try
            {
                demands = JsonSerializer.Deserialize<List<Demand>>(existingJson);
            }
            catch (JsonException)
            {
                try
                {
                    Demand singleDemand = JsonSerializer.Deserialize<Demand>(existingJson);
                    demands = new List<Demand> { singleDemand };
                }
                catch (JsonException)
                {
                }
            }

            Demand newDemand = JsonSerializer.Deserialize<Demand>(jsonDemand);

            if (newDemand != null && !DemandExists(demands, newDemand))
            {
                demands.Add(newDemand);
            }

            string updatedJson = JsonSerializer.Serialize(demands);

            string updateSql = "UPDATE students SET agreement_status = @updatedJson WHERE student_id = @studentId";
            using (var updateCmd = new NpgsqlCommand(updateSql, connection))
            {
                updateCmd.Parameters.Add(new NpgsqlParameter("updatedJson", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = updatedJson });
                updateCmd.Parameters.AddWithValue("studentId", studentId);

                updateCmd.ExecuteNonQuery();
            }

            connection.Close();
        }

        private bool DemandExists(List<Demand> demands, Demand newDemand)
        {
            return demands.Any(d => d.DemandedCourseCode == newDemand.DemandedCourseCode &&
                                    d.DemandedCourseName == newDemand.DemandedCourseName &&
                                    d.TeacherID == newDemand.TeacherID &&
                                    d.DemandStatus == newDemand.DemandStatus &&
                                    d.Demander == newDemand.Demander);
        }
        public string GetTeacherNameSurnameById(int teacherId)
        {
            connection.Open();
            string sql = "SELECT name, surname FROM teachers WHERE identification_number = @teacherId";
            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("teacherId", teacherId);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string surname = reader.GetString(reader.GetOrdinal("surname"));
                        connection.Close();
                        return $"{name} {surname}";
                    }
                }
            }
            connection.Close();
            return "";
        }
        public List<Demand> GetDemandsForStudent(int studentId)
        {
            connection.Open();
            string sql = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            List<Demand> demandsList = null;

            using (var cmd = new NpgsqlCommand(sql, connection))
            {
                cmd.Parameters.AddWithValue("studentId", studentId);
                var result = cmd.ExecuteScalar();
                if (result != null && !(result is DBNull))
                {
                    string jsonDemand = (string)result;
                    if (jsonDemand.TrimStart().StartsWith("["))
                    {
                        demandsList = JsonSerializer.Deserialize<List<Demand>>(jsonDemand);
                    }
                    else
                    {
                        Demand singleDemand = JsonSerializer.Deserialize<Demand>(jsonDemand);
                        demandsList = new List<Demand> { singleDemand };
                    }
                }
            }
            connection.Close();
            return demandsList;
        }

        private bool RemoveDemandFromJson(int studentid, string courseCode, string courseName, int teacherId)
        {
            string existingJson = "";
            connection.Open();
            string sqlFetch = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            using (var cmdSelect = new NpgsqlCommand(sqlFetch, connection))
            {
                cmdSelect.Parameters.AddWithValue("studentId", studentid);
                using (var reader = cmdSelect.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        existingJson = reader["agreement_status"].ToString();
                    }
                }
            }
            connection.Close();

            if (string.IsNullOrEmpty(existingJson))
            {
                return false;
            }

            List<Demand> demands;
            try
            {
                demands = JsonSerializer.Deserialize<List<Demand>>(existingJson);
            }
            catch (JsonException)
            {
                try
                {
                    Demand singleDemand = JsonSerializer.Deserialize<Demand>(existingJson);
                    demands = new List<Demand> { singleDemand };
                }
                catch (JsonException)
                {
                    return false;
                }
            }

            var demandToRemove = demands.FirstOrDefault(d => d.DemandedCourseCode == courseCode && d.TeacherID == teacherId && d.DemandedCourseName == courseName && d.DemandStatus == "Demanded");
            if (demandToRemove == null)
            {
                return false;
            }
            demands.Remove(demandToRemove);

            string updatedJson = JsonSerializer.Serialize(demands);

            if (demands.Count == 0)
            {
                updatedJson = "[]";
            }

            connection.Open();
            string sqlUpdate = "UPDATE students SET agreement_status = @updatedJson::jsonb WHERE student_id = @studentId";
            using (var cmdUpdate = new NpgsqlCommand(sqlUpdate, connection))
            {
                var param = new NpgsqlParameter("updatedJson", NpgsqlTypes.NpgsqlDbType.Jsonb)
                {
                    Value = updatedJson == "[]" ? (object)DBNull.Value : updatedJson
                };
                cmdUpdate.Parameters.Add(param);
                cmdUpdate.Parameters.AddWithValue("studentId", studentid);
                cmdUpdate.ExecuteNonQuery();
            }
            connection.Close();

            return true;
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.SelectedItem != null)
            {
                string selectedItemText = checkedListBox1.SelectedItem.ToString();
                string[] parts = selectedItemText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    string courseCode = parts[0];
                    string courseName = parts[1];
                    string teacherName = parts[2];
                    string teacherSurname = parts[3];
                    int teacherId = GetTeacherIdByName(teacherName, teacherSurname);

                    if (RemoveDemandFromJson(studentid, courseCode, courseName, teacherId))
                    {
                        checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
                    }  
                }
            }
        }

        private void buttonApproved_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            buttonAcceptDemand.Enabled = false;
            buttonDemand.Enabled = false;
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            List<Demand> demands = GetDemandsForStudent(studentID);
            if (demands != null)
            {
                foreach (var demand in demands)
                {
                    if (demand.DemandStatus == "Approved")
                    {
                        string teacherDetails = GetTeacherNameSurnameById(demand.TeacherID);
                        string displayText = $"{demand.DemandedCourseCode} {demand.DemandedCourseName} {teacherDetails} {demand.DemandStatus}";
                        checkedListBox1.Items.Add(displayText);
                    }
                }
            }
        }

        private void buttonTeacherDemands_Click(object sender, EventArgs e)
        {
            buttonCancel.Enabled = false;
            buttonAcceptDemand.Enabled = true;
            buttonDemand.Enabled = false;
            checkedListBox1.Visible = true;
            checkedListBox1.Items.Clear();
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            List<Demand> demands = GetDemandsForStudent(studentID);
            if (demands != null)
            {
                foreach (var demand in demands)
                {
                    if (demand.DemandStatus == "Demanded" && demand.Demander == "teacher")
                    {
                        string teacherDetails = GetTeacherNameSurnameById(demand.TeacherID);
                        string displayText = $"{demand.DemandedCourseCode} {demand.DemandedCourseName} {teacherDetails} {demand.DemandStatus}";
                        checkedListBox1.Items.Add(displayText);
                    }
                }
            }

        }

        private void buttonAcceptDemand_Click(object sender, EventArgs e)
        {
            List<object> itemsToRemove = new List<object>();
            foreach (object item in checkedListBox1.CheckedItems)
            {
                string itemText = item.ToString();
                string[] parts = itemText.Split(new char[] { ' ' }, 7);
                if (parts.Length < 4) continue;
                string  name = parts[2];
                string courseCode = parts[0];
                string courseName = parts[1];
                string surname = parts[3];
                int teacherId = -1;
                connection.Open();
                string sqlQuery = "SELECT identification_number FROM teachers WHERE name = @teacher_name AND surname=@teacher_surname";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@teacher_name", name);
                    cmd.Parameters.AddWithValue("@teacher_surname", surname);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            teacherId = Int32.Parse(reader["identification_number"].ToString());
                        }
                    }
                }
                connection.Close();

                if (teacherId == -1) continue;

                teacher.UpdateDemanderStatus(teacherId, courseCode, courseName, studentid, "teacher", "Approved");
                itemsToRemove.Add(item);
            }
            foreach (var item in itemsToRemove)
            {
                checkedListBox1.Items.Remove(item);
            }
        }
        private void getinterest_area()
        {
            comboBox1.Items.Clear();
            connection.Open();

            string query = "SELECT interest_areas FROM teachers";

            using (NpgsqlCommand command = new NpgsqlCommand(query, connection))
            {
                using (NpgsqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(0))
                        {
                            string jsonbData = reader.GetString(0);

                            MatchCollection matches = Regex.Matches(jsonbData, "\"(.*?)\"");

                            foreach (Match match in matches)
                            {
                                string itemText = match.Groups[1].Value;
                                if (itemText != "interest_area")
                                    comboBox1.Items.Add(itemText);
                            }
                        }

                    }
                }
            }
            connection.Close();
        }

        void interestAreaCheck()
        {
            listBox1.Items.Clear();
            listBox1.Visible = true;
            checkedListBox1.Visible = false;

            string selectedInterestArea = comboBox1.Text;
            if (string.IsNullOrWhiteSpace(selectedInterestArea))
            {
                return;
            }

            connection.Open();
            string teacherQuery = "SELECT identification_number, name, surname, lectures FROM teachers WHERE interest_areas::text LIKE @interestAreaPattern";
            using (NpgsqlCommand teacherCommand = new NpgsqlCommand(teacherQuery, connection))
            {
                teacherCommand.Parameters.AddWithValue("@interestAreaPattern", $"%\"{selectedInterestArea}\"%");

                using (NpgsqlDataReader teacherReader = teacherCommand.ExecuteReader())
                {
                    while (teacherReader.Read())
                    {
                        string teacherId = teacherReader["identification_number"].ToString();
                        string teacherName = teacherReader["name"].ToString();
                        string teacherSurname = teacherReader["surname"].ToString();
                        if (!teacherReader.IsDBNull(3))
                        {
                            string jsonbLectures = teacherReader.GetString(3);
                            JArray jsonArray = JArray.Parse(jsonbLectures);

                            foreach (JObject course in jsonArray)
                            {
                                string courseCode = course["Code"].ToString();
                                string courseName = course["Name"].ToString();
                                string courseStatus = course["status"].ToString();

                                if (courseStatus == "0")
                                {
                                    string displayText = $"{courseCode} {courseName} {teacherName} {teacherSurname}";
                                    listBox1.Items.Add(displayText);
                                }
                            }
                        }
                    }
                }
            }

            connection.Close();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getinterest_area();
            interestAreaCheck();
        }
        public void studentIdSet(int id)
        {
            studentid = id;
            messageComboboxUpdate();
            getinterest_area();
            buttonCancel.Enabled = false;
            buttonAcceptDemand.Enabled = false;
            buttonDemand.Enabled = false;
            messageTextBox.MaxLength = usercontroladmin.characterLimit();
        }

    }

        public class CourseData
        {
            public string Code { get; set; }
            public string Name { get; set; }
            public string Credit { get; set; }
        }
        public class Content
        {
            public int StudentId { get; set; }
            public string Message { get; set; }
            public int Sent { get; set; }
        }
        public class Demand
        {
            public string Demander { get; set; }
            public string DemandedCourseCode { get; set; }
            public string DemandedCourseName { get; set; }
            public string DemandStatus { get; set; }
            public int TeacherID { get; set; }
        }

    }
