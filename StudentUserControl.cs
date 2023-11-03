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
        NpgsqlConnection baglanti = new NpgsqlConnection("Server=localhost; Port=5432; Database=yazlab; User Id=postgres; Password=14441903;");
        List<double> gpaList = new List<double>();
        int studentid;
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

            foreach (PdfPageBase pdfPage in pdfDocument.Pages)
            {
                string pageText = pdfPage.ExtractText();
                List<CourseData> courseData = SplitTranscript(pageText);
                baglanti.Open();
                double gpa = gpaList[gpaList.Count - 1];
                string gpaUpdate = "UPDATE students SET gpa = @gpa WHERE student_id=49";
                using (NpgsqlCommand updateCommand = new NpgsqlCommand(gpaUpdate, baglanti))
                {
                    updateCommand.Parameters.AddWithValue("@gpa", gpa);
                    updateCommand.ExecuteNonQuery();
                }
                baglanti.Close();
                foreach (CourseData course in courseData)
                {
                    listBox1.Items.Add(course.Code + "   " + course.Name + "   " + course.Credit);
                    var jsonCourse = new
                    {
                        Code = course.Code,
                        Name = course.Name,
                        Credit = course.Credit
                    };
                    baglanti.Open();

                    string sqlSelect = "SELECT transcript FROM students WHERE student_id=49";

                    using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelect, baglanti))
                    {
                        string existingJsonData = selectCommand.ExecuteScalar() as string;

                        if (existingJsonData == null)
                        {
                            existingJsonData = "[]";
                        }

                        var existingData = JsonSerializer.Deserialize<List<dynamic>>(existingJsonData);

                        existingData.Add(jsonCourse);

                        string updatedJsonStr = JsonSerializer.Serialize(existingData);

                        string sqlUpdate = "UPDATE students SET transcript = @updated_json WHERE student_id=49";

                        using (NpgsqlCommand updateCommand = new NpgsqlCommand(sqlUpdate, baglanti))
                        {
                            updateCommand.Parameters.Add(new NpgsqlParameter("@updated_json", NpgsqlDbType.Jsonb));
                            updateCommand.Parameters["@updated_json"].Value = updatedJsonStr;
                            updateCommand.ExecuteNonQuery();
                        }
                        baglanti.Close();
                    }
                }
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

            baglanti.Open();
            string sqlQuery = "SELECT identification_number FROM teachers";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sqlQuery, baglanti))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TeacherIds.Add(reader.GetInt32(0));
                    }
                }
            }
            baglanti.Close();

            return TeacherIds;
        }

        private void demandCourse_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
            checkedListBox1.Items.Clear();
            List<int> teacherIds = GetAllTeacherİds();
            foreach (int teacherId in teacherIds)
            {
                baglanti.Open();
                string lecturesJson = GetExistingLecturesJson(teacherId, baglanti);

                if (!string.IsNullOrEmpty(lecturesJson))
                {
                    List<Lecture> lectures = DeserializeLectures(lecturesJson);
                    var lecturesToAdd = lectures.Where(l => l.status == "1").ToList();

                    foreach (var lecture in lecturesToAdd)
                    {
                        string sqlSelectName = $"SELECT name, surname FROM teachers WHERE identification_number = @teacherId";
                        using (NpgsqlCommand command = new NpgsqlCommand(sqlSelectName, baglanti))
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

                baglanti.Close();
            }
        }

        private void sdCourses_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            // This should now return a list of Demand objects
            List<Demand> demands = GetDemandsForStudent(studentID);
            if (demands != null && demands.Count > 0)
            {
                foreach (var demand in demands)
                {
                    if (demand.DemandStatus == "Demanded")
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
            baglanti.Open();
            NpgsqlDataAdapter data = new NpgsqlDataAdapter("SELECT identification_number ||' - '||  name || ' ' || surname AS FullName, identification_number FROM teachers", baglanti);
            DataTable dataTable = new DataTable();
            data.Fill(dataTable);
            baglanti.Close();
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

                baglanti.Open();
                using (NpgsqlCommand selectCommand = new NpgsqlCommand(sqlSelectMessages, baglanti))
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
                baglanti.Close();
            }
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
        public void studentIdSet(int id)
        {
            studentid = id;
            messageComboboxUpdate();

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
                string[] parts = itemText.Split(new char[] { ' ' }, 4); // Splits into code, name, teacher name, teacher surname
                if (parts.Length < 4) continue; // Skip if the split did not work as expected

                string courseCode = parts[0];
                string courseName = parts[1];
                string teacherName = parts[2];
                string teacherSurname = parts[3];

                int teacherID = GetTeacherIdByName(teacherName, teacherSurname);
                if (teacherID == -1) continue; // Skip if teacher ID not found

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
            baglanti.Open();
            int teacherId = -1;
            string sql = "SELECT identification_number FROM teachers WHERE name = @name AND surname = @surname";
            using (NpgsqlCommand cmd = new NpgsqlCommand(sql, baglanti))
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
            baglanti.Close();
            return teacherId;
        }
        private void InsertDemand(string jsonDemand, int studentId)
        {
            baglanti.Open();
            // Retrieve the current JSON data
            string existingJson = "";
            string fetchSql = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            using (var fetchCmd = new NpgsqlCommand(fetchSql, baglanti))
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

            // Deserialize it into a List<Demand>, or create a new one if null or empty
            List<Demand> demands = new List<Demand>();

            try
            {
                demands = JsonSerializer.Deserialize<List<Demand>>(existingJson);
            }
            catch (JsonException)
            {
                // If deserialization to a list fails, try deserializing as a single Demand object
                try
                {
                    Demand singleDemand = JsonSerializer.Deserialize<Demand>(existingJson);
                    demands = new List<Demand> { singleDemand };
                }
                catch (JsonException)
                {
                }
            }

            // Deserialize the new demand
            Demand newDemand = JsonSerializer.Deserialize<Demand>(jsonDemand);

            // Check if the demand already exists
            if (newDemand != null && !DemandExists(demands, newDemand))
            {
                demands.Add(newDemand);
            }

            // Serialize the updated list back to JSON
            string updatedJson = JsonSerializer.Serialize(demands);

            // Update the agreement_status column with this new JSON string
            string updateSql = "UPDATE students SET agreement_status = @updatedJson WHERE student_id = @studentId";
            using (var updateCmd = new NpgsqlCommand(updateSql, baglanti))
            {
                updateCmd.Parameters.Add(new NpgsqlParameter("updatedJson", NpgsqlTypes.NpgsqlDbType.Jsonb) { Value = updatedJson });
                updateCmd.Parameters.AddWithValue("studentId", studentId);

                updateCmd.ExecuteNonQuery();
            }

            baglanti.Close();
        }

        private bool DemandExists(List<Demand> demands, Demand newDemand)
        {
            return demands.Any(d => d.DemandedCourseCode == newDemand.DemandedCourseCode &&
                                    d.DemandedCourseName == newDemand.DemandedCourseName &&
                                    d.TeacherID == newDemand.TeacherID &&
                                    d.DemandStatus == newDemand.DemandStatus);
        }
        private string GetTeacherNameSurnameById(int teacherId)
        {
            baglanti.Open();
            string sql = "SELECT name, surname FROM teachers WHERE identification_number = @teacherId";
            using (var cmd = new NpgsqlCommand(sql, baglanti))
            {
                cmd.Parameters.AddWithValue("teacherId", teacherId);
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string surname = reader.GetString(reader.GetOrdinal("surname"));
                        baglanti.Close();
                        return $"{name} {surname}";
                    }
                }
            }
            baglanti.Close();
            return "";
        }
        private List<Demand> GetDemandsForStudent(int studentId)
        {
            baglanti.Open();
            string sql = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            List<Demand> demandsList = null;

            using (var cmd = new NpgsqlCommand(sql, baglanti))
            {
                cmd.Parameters.AddWithValue("studentId", studentId);
                var result = cmd.ExecuteScalar();
                if (result != null && !(result is DBNull))
                {
                    string jsonDemand = (string)result;
                    // Check if the JSON is an array or a single object
                    if (jsonDemand.TrimStart().StartsWith("["))
                    {
                        // It's an array, deserialize to List<Demand>
                        demandsList = JsonSerializer.Deserialize<List<Demand>>(jsonDemand);
                    }
                    else
                    {
                        // It's a single object, deserialize to Demand and create a list
                        Demand singleDemand = JsonSerializer.Deserialize<Demand>(jsonDemand);
                        demandsList = new List<Demand> { singleDemand };
                    }
                }
            }
            baglanti.Close();
            return demandsList; // This could be null if there was no data or if deserialization failed
        }

        private bool RemoveDemandFromJson(int studentid, string courseCode, string courseName, int teacherId)
        {
            string existingJson = "";
            // Step 1: Retrieve the JSON data
            baglanti.Open();
            string sqlFetch = "SELECT agreement_status FROM students WHERE student_id = @studentId";
            using (var cmdSelect = new NpgsqlCommand(sqlFetch, baglanti))
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
            baglanti.Close();

            if (string.IsNullOrEmpty(existingJson))
            {
                return false; // No data found
            }

            List<Demand> demands;
            // Step 2: Attempt to Deserialize the JSON data into a list of Demand objects
            try
            {
                demands = JsonSerializer.Deserialize<List<Demand>>(existingJson);
            }
            catch (JsonException)
            {
                // If deserialization to a list fails, try deserializing as a single Demand object
                try
                {
                    Demand singleDemand = JsonSerializer.Deserialize<Demand>(existingJson);
                    demands = new List<Demand> { singleDemand };
                }
                catch (JsonException)
                {
                    // If it fails again, it means JSON is neither a list nor a valid single object
                    return false;
                }
            }

            // Step 3: Find and remove the matching Demand object
            var demandToRemove = demands.FirstOrDefault(d => d.DemandedCourseCode == courseCode && d.TeacherID == teacherId && d.DemandedCourseName == courseName && d.DemandStatus == "Demanded");
            if (demandToRemove == null)
            {
                return false; // No matching demand found
            }
            demands.Remove(demandToRemove);

            // Step 4: Serialize the list back into JSON
            string updatedJson = JsonSerializer.Serialize(demands);

            // Handle the case where the list is empty by setting updatedJson to an empty array or null as required by your database schema
            if (demands.Count == 0)
            {
                updatedJson = "[]"; // or updatedJson = "null"; if that's what the database expects
            }

            // Step 5: Update the agreement_status column
            baglanti.Open();
            string sqlUpdate = "UPDATE students SET agreement_status = @updatedJson::jsonb WHERE student_id = @studentId";
            using (var cmdUpdate = new NpgsqlCommand(sqlUpdate, baglanti))
            {
                // Explicitly state the parameter type for the jsonb column
                var param = new NpgsqlParameter("updatedJson", NpgsqlTypes.NpgsqlDbType.Jsonb)
                {
                    Value = updatedJson == "[]" ? (object)DBNull.Value : updatedJson
                };
                cmdUpdate.Parameters.Add(param);
                cmdUpdate.Parameters.AddWithValue("studentId", studentid);
                cmdUpdate.ExecuteNonQuery();
            }
            baglanti.Close();

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

                    // Remove from database
                    if (RemoveDemandFromJson(studentid, courseCode, courseName, teacherId))
                    {
                        // Remove from CheckedListBox
                        checkedListBox1.Items.Remove(checkedListBox1.SelectedItem);
                    }
                    else
                    {
                        // Handle the case where the demand could not be removed, if necessary
                    }
                }
            }
        }

        private void buttonApproved_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            // Now this will be a list of demands
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
            checkedListBox1.Visible = true;
            checkedListBox1.Items.Clear();
            checkedListBox1.Visible = true;
            int studentID = studentid;
            checkedListBox1.Items.Clear();

            // Now this will be a list of demands
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
