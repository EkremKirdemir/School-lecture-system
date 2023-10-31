
namespace yazlab
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            radioButton1 = new System.Windows.Forms.RadioButton();
            radioButton2 = new System.Windows.Forms.RadioButton();
            radioButton3 = new System.Windows.Forms.RadioButton();
            label1 = new System.Windows.Forms.Label();
            textBox1 = new System.Windows.Forms.TextBox();
            textBox2 = new System.Windows.Forms.TextBox();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            studentUserControl1 = new StudentUserControl();
            teacherUserControl1 = new TeacherUserControl();
            adminUserControl1 = new AdminUserControl();
            SuspendLayout();
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new System.Drawing.Point(276, 53);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new System.Drawing.Size(66, 19);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "Student";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            radioButton2.AutoSize = true;
            radioButton2.Location = new System.Drawing.Point(348, 53);
            radioButton2.Name = "radioButton2";
            radioButton2.Size = new System.Drawing.Size(65, 19);
            radioButton2.TabIndex = 1;
            radioButton2.TabStop = true;
            radioButton2.Text = "Teacher";
            radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            radioButton3.AutoSize = true;
            radioButton3.Location = new System.Drawing.Point(419, 53);
            radioButton3.Name = "radioButton3";
            radioButton3.Size = new System.Drawing.Size(61, 19);
            radioButton3.TabIndex = 2;
            radioButton3.TabStop = true;
            radioButton3.Text = "Admin";
            radioButton3.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(348, 22);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(42, 15);
            label1.TabIndex = 3;
            label1.Text = "LOGIN";
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(302, 88);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(100, 23);
            textBox1.TabIndex = 4;
            // 
            // textBox2
            // 
            textBox2.Location = new System.Drawing.Point(302, 117);
            textBox2.Name = "textBox2";
            textBox2.Size = new System.Drawing.Size(100, 23);
            textBox2.TabIndex = 5;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(229, 96);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(60, 15);
            label2.TabIndex = 6;
            label2.Text = "Username";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(229, 125);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(57, 15);
            label3.TabIndex = 7;
            label3.Text = "Password";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(416, 88);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(90, 52);
            button1.TabIndex = 8;
            button1.Text = "Login";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // studentUserControl1
            // 
            studentUserControl1.AutoSize = true;
            studentUserControl1.Location = new System.Drawing.Point(0, 0);
            studentUserControl1.Name = "studentUserControl1";
            studentUserControl1.Size = new System.Drawing.Size(519, 404);
            studentUserControl1.TabIndex = 9;
            studentUserControl1.Visible = false;
            // 
            // teacherUserControl1
            // 
            teacherUserControl1.Location = new System.Drawing.Point(0, 0);
            teacherUserControl1.Name = "teacherUserControl1";
            teacherUserControl1.Size = new System.Drawing.Size(561, 337);
            teacherUserControl1.TabIndex = 10;
            teacherUserControl1.Visible = false;
            // 
            // adminUserControl1
            // 
            adminUserControl1.Location = new System.Drawing.Point(0, 0);
            adminUserControl1.Name = "adminUserControl1";
            adminUserControl1.Size = new System.Drawing.Size(822, 471);
            adminUserControl1.TabIndex = 11;
            adminUserControl1.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(adminUserControl1);
            Controls.Add(teacherUserControl1);
            Controls.Add(studentUserControl1);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Controls.Add(radioButton3);
            Controls.Add(radioButton2);
            Controls.Add(radioButton1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private StudentUserControl studentUserControl1;
        private TeacherUserControl teacherUserControl1;
        private AdminUserControl adminUserControl1;
    }
}

