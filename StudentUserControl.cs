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
                string gpaUpdate = "UPDATE students SET gpa = @gpa WHERE student_id=48";
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
                    
                    string sqlSelect = "SELECT transcript FROM students WHERE student_id=48";

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

                        string sqlUpdate = "UPDATE students SET transcript = @updated_json WHERE student_id=48";

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

        private void demandCourse_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;
        }

        private void sdCourses_Click(object sender, EventArgs e)
        {
            checkedListBox1.Visible = true;

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
    }

    public class CourseData
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Credit { get; set; }
    }
}
