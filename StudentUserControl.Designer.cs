﻿
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
            buttonBack = new System.Windows.Forms.Button();
            buttonDemand = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            buttonApproved = new System.Windows.Forms.Button();
            buttonTeacherDemands = new System.Windows.Forms.Button();
            buttonAcceptDemand = new System.Windows.Forms.Button();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // usTranscript
            // 
            usTranscript.Location = new System.Drawing.Point(17, 52);
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
            listBox1.Location = new System.Drawing.Point(17, 142);
            listBox1.Name = "listBox1";
            listBox1.Size = new System.Drawing.Size(324, 169);
            listBox1.TabIndex = 1;
            listBox1.Visible = false;
            // 
            // demandCourse
            // 
            demandCourse.Location = new System.Drawing.Point(136, 53);
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
            label1.Location = new System.Drawing.Point(17, 104);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(0, 15);
            label1.TabIndex = 3;
            // 
            // checkedListBox1
            // 
            checkedListBox1.FormattingEnabled = true;
            checkedListBox1.Location = new System.Drawing.Point(17, 142);
            checkedListBox1.Name = "checkedListBox1";
            checkedListBox1.Size = new System.Drawing.Size(324, 166);
            checkedListBox1.TabIndex = 4;
            checkedListBox1.Visible = false;
            // 
            // sdCourses
            // 
            sdCourses.Location = new System.Drawing.Point(248, 53);
            sdCourses.Name = "sdCourses";
            sdCourses.Size = new System.Drawing.Size(106, 40);
            sdCourses.TabIndex = 5;
            sdCourses.Text = "Show Demanded Courses";
            sdCourses.UseVisualStyleBackColor = true;
            sdCourses.Click += sdCourses_Click;
            // 
            // add
            // 
            add.Location = new System.Drawing.Point(135, 100);
            add.Name = "add";
            add.Size = new System.Drawing.Size(106, 23);
            add.TabIndex = 6;
            add.Text = "Add";
            add.UseVisualStyleBackColor = true;
            add.Click += add_Click;
            // 
            // Delete
            // 
            Delete.Location = new System.Drawing.Point(249, 100);
            Delete.Name = "Delete";
            Delete.Size = new System.Drawing.Size(105, 23);
            Delete.TabIndex = 7;
            Delete.Text = "Delete";
            Delete.UseVisualStyleBackColor = true;
            Delete.Click += Delete_Click;
            // 
            // sendButton
            // 
            sendButton.Location = new System.Drawing.Point(678, 288);
            sendButton.Name = "sendButton";
            sendButton.Size = new System.Drawing.Size(75, 23);
            sendButton.TabIndex = 8;
            sendButton.Text = "Send";
            sendButton.UseVisualStyleBackColor = true;
            sendButton.Click += sendButton_Click;
            // 
            // messageTextBox
            // 
            messageTextBox.Location = new System.Drawing.Point(545, 288);
            messageTextBox.Name = "messageTextBox";
            messageTextBox.Size = new System.Drawing.Size(127, 23);
            messageTextBox.TabIndex = 9;
            // 
            // messagesListBox
            // 
            messagesListBox.FormattingEnabled = true;
            messagesListBox.ItemHeight = 15;
            messagesListBox.Location = new System.Drawing.Point(545, 93);
            messagesListBox.Name = "messagesListBox";
            messagesListBox.Size = new System.Drawing.Size(208, 184);
            messagesListBox.TabIndex = 10;
            // 
            // messagesComboBox
            // 
            messagesComboBox.FormattingEnabled = true;
            messagesComboBox.Location = new System.Drawing.Point(545, 53);
            messagesComboBox.Name = "messagesComboBox";
            messagesComboBox.Size = new System.Drawing.Size(208, 23);
            messagesComboBox.TabIndex = 11;
            messagesComboBox.SelectedIndexChanged += messagesComboBox_SelectedIndexChanged;
            // 
            // buttonBack
            // 
            buttonBack.Location = new System.Drawing.Point(17, 13);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new System.Drawing.Size(75, 23);
            buttonBack.TabIndex = 12;
            buttonBack.Text = "Back";
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += buttonBack_Click;
            // 
            // buttonDemand
            // 
            buttonDemand.Location = new System.Drawing.Point(349, 145);
            buttonDemand.Name = "buttonDemand";
            buttonDemand.Size = new System.Drawing.Size(75, 23);
            buttonDemand.TabIndex = 13;
            buttonDemand.Text = "Demand";
            buttonDemand.UseVisualStyleBackColor = true;
            buttonDemand.Click += buttonDemand_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new System.Drawing.Point(349, 174);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Cancel Demand";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonApproved
            // 
            buttonApproved.Location = new System.Drawing.Point(360, 53);
            buttonApproved.Name = "buttonApproved";
            buttonApproved.Size = new System.Drawing.Size(104, 40);
            buttonApproved.TabIndex = 15;
            buttonApproved.Text = "Show Approved Courses";
            buttonApproved.UseVisualStyleBackColor = true;
            buttonApproved.Click += buttonApproved_Click;
            // 
            // buttonTeacherDemands
            // 
            buttonTeacherDemands.Location = new System.Drawing.Point(365, 100);
            buttonTeacherDemands.Name = "buttonTeacherDemands";
            buttonTeacherDemands.Size = new System.Drawing.Size(99, 39);
            buttonTeacherDemands.TabIndex = 16;
            buttonTeacherDemands.Text = "Demands From Teachers";
            buttonTeacherDemands.UseVisualStyleBackColor = true;
            buttonTeacherDemands.Click += buttonTeacherDemands_Click;
            // 
            // buttonAcceptDemand
            // 
            buttonAcceptDemand.Location = new System.Drawing.Point(349, 221);
            buttonAcceptDemand.Name = "buttonAcceptDemand";
            buttonAcceptDemand.Size = new System.Drawing.Size(75, 23);
            buttonAcceptDemand.TabIndex = 17;
            buttonAcceptDemand.Text = "Accept";
            buttonAcceptDemand.UseVisualStyleBackColor = true;
            buttonAcceptDemand.Click += buttonAcceptDemand_Click;
            // 
            // StudentUserControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            AutoSize = true;
            Controls.Add(buttonAcceptDemand);
            Controls.Add(buttonTeacherDemands);
            Controls.Add(buttonApproved);
            Controls.Add(buttonCancel);
            Controls.Add(buttonDemand);
            Controls.Add(buttonBack);
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
        private System.Windows.Forms.Button buttonBack;
        private System.Windows.Forms.Button buttonDemand;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonApproved;
        private System.Windows.Forms.Button buttonTeacherDemands;
        private System.Windows.Forms.Button buttonAcceptDemand;
    }
}
