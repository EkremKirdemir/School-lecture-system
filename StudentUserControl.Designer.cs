
namespace yazlab
{
    partial class StudentUserControl
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
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            usTranscript = new System.Windows.Forms.Button();
            listBox1 = new System.Windows.Forms.ListBox();
            demandCourse = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            sdCourses = new System.Windows.Forms.Button();
            add = new System.Windows.Forms.Button();
            Delete = new System.Windows.Forms.Button();
            sendButton = new System.Windows.Forms.Button();
            messageTextBox = new System.Windows.Forms.TextBox();
            messagesListBox = new System.Windows.Forms.ListBox();
            messagesComboBox = new System.Windows.Forms.ComboBox();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // usTranscript
            // 
            usTranscript.Location = new System.Drawing.Point(24, 19);
            usTranscript.Name = "usTranscript";
            usTranscript.Size = new System.Drawing.Size(113, 41);
            usTranscript.TabIndex = 0;
            usTranscript.Text = "Upload/Show Transcript";
            usTranscript.UseVisualStyleBackColor = true;
            usTranscript.Click += usTranscript_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new System.Drawing.Point(24, 109);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(324, 169);
            listBox1.TabIndex = 1;
            listBox1.Visible = false;
            // 
            // demandCourse
            // 
            demandCourse.Location = new System.Drawing.Point(143, 20);
            demandCourse.Name = "demandCourse";
            demandCourse.Size = new System.Drawing.Size(105, 41);
            demandCourse.TabIndex = 2;
            demandCourse.Text = "Demand Course";
            demandCourse.UseVisualStyleBackColor = true;
            demandCourse.Click += demandCourse_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(24, 71);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(0, 15);
            label1.TabIndex = 3;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(24, 109);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(324, 166);
            checkedListBox1.TabIndex = 4;
            checkedListBox1.Visible = false;
            // 
            // sdCourses
            // 
            sdCourses.Location = new System.Drawing.Point(255, 20);
            sdCourses.Name = "sdCourses";
            sdCourses.Size = new System.Drawing.Size(106, 40);
            sdCourses.TabIndex = 5;
            sdCourses.Text = "Show Demanded Courses";
            sdCourses.UseVisualStyleBackColor = true;
            sdCourses.Click += sdCourses_Click;
            // 
            // add
            // 
            add.Location = new System.Drawing.Point(142, 67);
            add.Name = "add";
            add.Size = new System.Drawing.Size(106, 23);
            add.TabIndex = 6;
            add.Text = "Add";
            add.UseVisualStyleBackColor = true;
            add.Click += add_Click;
            // 
            // Delete
            // 
            Delete.Location = new System.Drawing.Point(256, 67);
            Delete.Name = "Delete";
            Delete.Size = new System.Drawing.Size(105, 23);
            Delete.TabIndex = 7;
            Delete.Text = "Delete";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // sendButton
            // 
            sendButton.Location = new System.Drawing.Point(685, 255);
            sendButton.Name = "sendButton";
            sendButton.Size = new System.Drawing.Size(75, 23);
            sendButton.TabIndex = 8;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            // 
            // messageTextBox
            // 
            messageTextBox.Location = new System.Drawing.Point(552, 255);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.Size = new System.Drawing.Size(127, 23);
            messageTextBox.TabIndex = 9;
            // 
            // messagesListBox
            // 
            messagesListBox.FormattingEnabled = true;
            messagesListBox.ItemHeight = 15;
            messagesListBox.Location = new System.Drawing.Point(552, 60);
            messagesListBox.Name = "messagesListBox";
            messagesListBox.Size = new System.Drawing.Size(208, 184);
            messagesListBox.TabIndex = 10;
            // 
            // messagesComboBox
            // 
            messagesComboBox.FormattingEnabled = true;
            messagesComboBox.Location = new System.Drawing.Point(552, 20);
            messagesComboBox.Name = "messagesComboBox";
            messagesComboBox.Size = new System.Drawing.Size(208, 23);
            messagesComboBox.TabIndex = 11;
            // 
            // StudentUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(messagesComboBox);
            Controls.Add(messagesListBox);
            Controls.Add(messageTextBox);
            Controls.Add(sendButton);
            Controls.Add(Delete);
            Controls.Add(add);
            Controls.Add(sdCourses);
            Controls.Add(checkedListBox1);
            Controls.Add(label1);
            Controls.Add(demandCourse);
            Controls.Add(listBox1);
            Controls.Add(usTranscript);
            Name = "StudentUserControl";
            Size = new System.Drawing.Size(825, 332);
            Load += StudentUserControl_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button usTranscript;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button demandCourse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button sdCourses;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button Delete;
        private System.Windows.Forms.Button sendButton;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.ListBox messagesListBox;
        private System.Windows.Forms.ComboBox messagesComboBox;
    }
}
