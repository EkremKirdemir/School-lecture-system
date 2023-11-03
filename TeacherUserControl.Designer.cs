
namespace yazlab
{
    partial class TeacherUserControl
    {
        /// <summary> 
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Bileşen Tasarımcısı üretimi kod

        /// <summary> 
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            messageComboBox = new System.Windows.Forms.ComboBox();
            messageListBox = new System.Windows.Forms.ListBox();
            messageSendButton = new System.Windows.Forms.Button();
            messagesTextBox = new System.Windows.Forms.TextBox();
            studentsComboBox = new System.Windows.Forms.ComboBox();
            transcripListBox = new System.Windows.Forms.ListBox();
            checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            buttonDemandedFromYou = new System.Windows.Forms.Button();
            buttonShowOther = new System.Windows.Forms.Button();
            buttonCourseOptions = new System.Windows.Forms.Button();
            buttonDemandCourse = new System.Windows.Forms.Button();
            buttonShowTranscript = new System.Windows.Forms.Button();
            buttonDemandSmall = new System.Windows.Forms.Button();
            buttonApproveCourse = new System.Windows.Forms.Button();
            buttonAccept = new System.Windows.Forms.Button();
            groupBox = new System.Windows.Forms.GroupBox();
            buttonDeleteCriteria = new System.Windows.Forms.Button();
            buttonAcceptCriterias = new System.Windows.Forms.Button();
            listBoxCriteria = new System.Windows.Forms.ListBox();
            label1 = new System.Windows.Forms.Label();
            buttonAddCriteria = new System.Windows.Forms.Button();
            comboBoxCriteria = new System.Windows.Forms.ComboBox();
            textBoxCriteria = new System.Windows.Forms.TextBox();
            textBoxInterest = new System.Windows.Forms.TextBox();
            buttonInterest = new System.Windows.Forms.Button();
            buttonback = new System.Windows.Forms.Button();
            groupBox.SuspendLayout();
            SuspendLayout();
            // 
            // messageComboBox
            // 
            messageComboBox.FormattingEnabled = true;
            messageComboBox.Location = new System.Drawing.Point(794, 8);
            messageComboBox.Name = "messageComboBox";
            messageComboBox.Size = new System.Drawing.Size(238, 23);
            messageComboBox.TabIndex = 0;
            messageComboBox.SelectedIndexChanged += messageComboBox_SelectedIndexChanged;
            // 
            // messageListBox
            // 
            messageListBox.FormattingEnabled = true;
            messageListBox.ItemHeight = 15;
            messageListBox.Location = new System.Drawing.Point(794, 37);
            messageListBox.Name = "messageListBox";
            messageListBox.Size = new System.Drawing.Size(238, 199);
            messageListBox.TabIndex = 1;
            // 
            // messageSendButton
            // 
            messageSendButton.Location = new System.Drawing.Point(983, 242);
            messageSendButton.Name = "messageSendButton";
            messageSendButton.Size = new System.Drawing.Size(49, 23);
            messageSendButton.TabIndex = 2;
            messageSendButton.Text = "Send";
            messageSendButton.UseVisualStyleBackColor = true;
            messageSendButton.Click += messageSendButton_Click;
            // 
            // messagesTextBox
            // 
            messagesTextBox.Location = new System.Drawing.Point(794, 242);
            messagesTextBox.Name = "messagesTextBox";
            messagesTextBox.Size = new System.Drawing.Size(183, 23);
            messagesTextBox.TabIndex = 3;
            // 
            // studentsComboBox
            // 
            studentsComboBox.FormattingEnabled = true;
            studentsComboBox.Location = new System.Drawing.Point(3, 94);
            studentsComboBox.Name = "studentsComboBox";
            studentsComboBox.Size = new System.Drawing.Size(166, 23);
            studentsComboBox.TabIndex = 4;
            studentsComboBox.SelectedIndexChanged += studentsComboBox_SelectedIndexChanged;
            // 
            // transcripListBox
            // 
            transcripListBox.FormattingEnabled = true;
            transcripListBox.ItemHeight = 15;
            transcripListBox.Location = new System.Drawing.Point(3, 123);
            transcripListBox.Name = "transcripListBox";
            transcripListBox.Size = new System.Drawing.Size(281, 229);
            transcripListBox.TabIndex = 5;
            transcripListBox.Visible = false;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(3, 123);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(278, 220);
            checkedListBox1.TabIndex = 6;
            checkedListBox1.Visible = false;
            // 
            // buttonDemandedFromYou
            // 
            buttonDemandedFromYou.Location = new System.Drawing.Point(3, 39);
            buttonDemandedFromYou.Name = "buttonDemandedFromYou";
            buttonDemandedFromYou.Size = new System.Drawing.Size(114, 49);
            buttonDemandedFromYou.TabIndex = 7;
            buttonDemandedFromYou.Text = "Show Demanded Courses From You";
            buttonDemandedFromYou.UseVisualStyleBackColor = true;
            // 
            // buttonShowOther
            // 
            buttonShowOther.Location = new System.Drawing.Point(123, 39);
            buttonShowOther.Name = "buttonShowOther";
            buttonShowOther.Size = new System.Drawing.Size(122, 49);
            buttonShowOther.TabIndex = 8;
            buttonShowOther.Text = "Show Other Demands";
            buttonShowOther.UseVisualStyleBackColor = true;
            // 
            // buttonCourseOptions
            // 
            buttonCourseOptions.Location = new System.Drawing.Point(251, 39);
            buttonCourseOptions.Name = "buttonCourseOptions";
            buttonCourseOptions.Size = new System.Drawing.Size(114, 49);
            buttonCourseOptions.TabIndex = 7;
            buttonCourseOptions.Text = "Show My Course Options";
            buttonCourseOptions.UseVisualStyleBackColor = true;
            buttonCourseOptions.Click += buttonCourseOptions_Click;
            // 
            // buttonDemandCourse
            // 
            buttonDemandCourse.Location = new System.Drawing.Point(371, 39);
            buttonDemandCourse.Name = "buttonDemandCourse";
            buttonDemandCourse.Size = new System.Drawing.Size(114, 49);
            buttonDemandCourse.TabIndex = 7;
            buttonDemandCourse.Text = "Demand Courses";
            buttonDemandCourse.UseVisualStyleBackColor = true;
            // 
            // buttonShowTranscript
            // 
            buttonShowTranscript.Location = new System.Drawing.Point(175, 94);
            buttonShowTranscript.Name = "buttonShowTranscript";
            buttonShowTranscript.Size = new System.Drawing.Size(106, 23);
            buttonShowTranscript.TabIndex = 9;
            buttonShowTranscript.Text = "Show Transcript";
            buttonShowTranscript.UseVisualStyleBackColor = true;
            // 
            // buttonDemandSmall
            // 
            buttonDemandSmall.Location = new System.Drawing.Point(290, 134);
            buttonDemandSmall.Name = "buttonDemandSmall";
            buttonDemandSmall.Size = new System.Drawing.Size(90, 34);
            buttonDemandSmall.TabIndex = 10;
            buttonDemandSmall.Text = "Demand";
            buttonDemandSmall.UseVisualStyleBackColor = true;
            // 
            // buttonApproveCourse
            // 
            buttonApproveCourse.Location = new System.Drawing.Point(290, 174);
            buttonApproveCourse.Name = "buttonApproveCourse";
            buttonApproveCourse.Size = new System.Drawing.Size(90, 34);
            buttonApproveCourse.TabIndex = 10;
            buttonApproveCourse.Text = "Approve";
            buttonApproveCourse.UseVisualStyleBackColor = true;
            // 
            // buttonAccept
            // 
            buttonAccept.Location = new System.Drawing.Point(290, 214);
            buttonAccept.Name = "buttonAccept";
            buttonAccept.Size = new System.Drawing.Size(90, 34);
            buttonAccept.TabIndex = 10;
            buttonAccept.Text = "Accept";
            buttonAccept.UseVisualStyleBackColor = true;
            buttonAccept.Click += buttonAccept_Click;
            // 
            // groupBox
            // 
            groupBox.Controls.Add(buttonDeleteCriteria);
            groupBox.Controls.Add(buttonAcceptCriterias);
            groupBox.Controls.Add(listBoxCriteria);
            groupBox.Controls.Add(label1);
            groupBox.Controls.Add(buttonAddCriteria);
            groupBox.Controls.Add(comboBoxCriteria);
            groupBox.Controls.Add(textBoxCriteria);
            groupBox.Location = new System.Drawing.Point(403, 113);
            groupBox.Name = "groupBox";
            groupBox.Size = new System.Drawing.Size(350, 241);
            groupBox.TabIndex = 11;
            groupBox.TabStop = false;
            groupBox.Text = "Course Criterias";
            // 
            // buttonDeleteCriteria
            // 
            buttonDeleteCriteria.Location = new System.Drawing.Point(244, 61);
            buttonDeleteCriteria.Name = "buttonDeleteCriteria";
            buttonDeleteCriteria.Size = new System.Drawing.Size(85, 57);
            buttonDeleteCriteria.TabIndex = 5;
            buttonDeleteCriteria.Text = "DeleteCriterias";
            buttonDeleteCriteria.UseVisualStyleBackColor = true;
            buttonDeleteCriteria.Click += buttonDeleteCriteria_Click;
            // 
            // buttonAcceptCriterias
            // 
            buttonAcceptCriterias.Location = new System.Drawing.Point(244, 135);
            buttonAcceptCriterias.Name = "buttonAcceptCriterias";
            buttonAcceptCriterias.Size = new System.Drawing.Size(85, 57);
            buttonAcceptCriterias.TabIndex = 5;
            buttonAcceptCriterias.Text = "Accept Criterias";
            buttonAcceptCriterias.UseVisualStyleBackColor = true;
            buttonAcceptCriterias.Click += buttonAcceptCriterias_Click;
            // 
            // listBoxCriteria
            // 
            listBoxCriteria.FormattingEnabled = true;
            listBoxCriteria.ItemHeight = 15;
            listBoxCriteria.Location = new System.Drawing.Point(6, 53);
            listBoxCriteria.Name = "listBoxCriteria";
            listBoxCriteria.Size = new System.Drawing.Size(217, 139);
            listBoxCriteria.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(148, 25);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(14, 15);
            label1.TabIndex = 2;
            label1.Text = "X";
            // 
            // buttonAddCriteria
            // 
            buttonAddCriteria.Location = new System.Drawing.Point(244, 22);
            buttonAddCriteria.Name = "buttonAddCriteria";
            buttonAddCriteria.Size = new System.Drawing.Size(85, 25);
            buttonAddCriteria.TabIndex = 1;
            buttonAddCriteria.Text = "Add Criteria";
            buttonAddCriteria.UseVisualStyleBackColor = true;
            buttonAddCriteria.Click += buttonAddCriteria_Click;
            // 
            // comboBoxCriteria
            // 
            comboBoxCriteria.FormattingEnabled = true;
            comboBoxCriteria.Location = new System.Drawing.Point(6, 22);
            comboBoxCriteria.Name = "comboBoxCriteria";
            comboBoxCriteria.Size = new System.Drawing.Size(136, 23);
            comboBoxCriteria.TabIndex = 0;
            // 
            // textBoxCriteria
            // 
            textBoxCriteria.Location = new System.Drawing.Point(168, 22);
            textBoxCriteria.Name = "textBoxCriteria";
            textBoxCriteria.Size = new System.Drawing.Size(55, 23);
            textBoxCriteria.TabIndex = 3;
            // 
            // textBoxInterest
            // 
            textBoxInterest.Location = new System.Drawing.Point(526, 44);
            textBoxInterest.Name = "textBoxInterest";
            textBoxInterest.Size = new System.Drawing.Size(100, 23);
            textBoxInterest.TabIndex = 12;
            // 
            // buttonInterest
            // 
            buttonInterest.Location = new System.Drawing.Point(647, 43);
            buttonInterest.Name = "buttonInterest";
            buttonInterest.Size = new System.Drawing.Size(85, 23);
            buttonInterest.TabIndex = 13;
            buttonInterest.Text = "Add Interest";
            buttonInterest.UseVisualStyleBackColor = true;
            buttonInterest.Click += buttonInterest_Click;
            // 
            // buttonback
            // 
            buttonback.Location = new System.Drawing.Point(3, 8);
            buttonback.Name = "buttonback";
            buttonback.Size = new System.Drawing.Size(75, 23);
            buttonback.TabIndex = 14;
            buttonback.Text = "Back";
            buttonback.UseVisualStyleBackColor = true;
            buttonback.Click += buttonback_Click;
            // 
            // TeacherUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(buttonback);
            Controls.Add(buttonInterest);
            Controls.Add(textBoxInterest);
            Controls.Add(groupBox);
            Controls.Add(buttonAccept);
            Controls.Add(buttonApproveCourse);
            Controls.Add(buttonDemandSmall);
            Controls.Add(buttonShowTranscript);
            Controls.Add(buttonShowOther);
            Controls.Add(buttonDemandCourse);
            Controls.Add(checkedListBox1);
            Controls.Add(buttonCourseOptions);
            Controls.Add(buttonDemandedFromYou);
            Controls.Add(transcripListBox);
            Controls.Add(studentsComboBox);
            Controls.Add(messagesTextBox);
            Controls.Add(messageSendButton);
            Controls.Add(messageListBox);
            Controls.Add(messageComboBox);
            Name = "TeacherUserControl";
            Size = new System.Drawing.Size(1035, 410);
            Load += TeacherUserControl_Load;
            VisibleChanged += TeacherUserControl_VisibleChanged;
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ComboBox messageComboBox;
        private System.Windows.Forms.ListBox messageListBox;
        private System.Windows.Forms.Button messageSendButton;
        private System.Windows.Forms.TextBox messagesTextBox;
        private System.Windows.Forms.ComboBox studentsComboBox;
        private System.Windows.Forms.ListBox transcripListBox;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button buttonDemandedFromYou;
        private System.Windows.Forms.Button buttonShowOther;
        private System.Windows.Forms.Button buttonCourseOptions;
        private System.Windows.Forms.Button buttonDemandCourse;
        private System.Windows.Forms.Button buttonShowTranscript;
        private System.Windows.Forms.Button buttonDemandSmall;
        private System.Windows.Forms.Button buttonApproveCourse;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonAddCriteria;
        private System.Windows.Forms.ComboBox comboBoxCriteria;
        private System.Windows.Forms.Button buttonDeleteCriteria;
        private System.Windows.Forms.Button buttonAcceptCriterias;
        private System.Windows.Forms.ListBox listBoxCriteria;
        private System.Windows.Forms.TextBox textBoxCriteria;
        private System.Windows.Forms.TextBox textBoxInterest;
        private System.Windows.Forms.Button buttonInterest;
        private System.Windows.Forms.Button buttonback;
    }
}
