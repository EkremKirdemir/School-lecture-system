
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.usTranscript = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.demandCourse = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.sdCourses = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.Delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // usTranscript
            // 
            this.usTranscript.Location = new System.Drawing.Point(24, 19);
            this.usTranscript.Name = "usTranscript";
            this.usTranscript.Size = new System.Drawing.Size(113, 41);
            this.usTranscript.TabIndex = 0;
            this.usTranscript.Text = "Upload/Show Transcript";
            this.usTranscript.UseVisualStyleBackColor = true;
            this.usTranscript.Click += new System.EventHandler(this.usTranscript_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(24, 109);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(324, 169);
            this.listBox1.TabIndex = 1;
            this.listBox1.Visible = false;
            // 
            // demandCourse
            // 
            this.demandCourse.Location = new System.Drawing.Point(143, 20);
            this.demandCourse.Name = "demandCourse";
            this.demandCourse.Size = new System.Drawing.Size(105, 41);
            this.demandCourse.TabIndex = 2;
            this.demandCourse.Text = "Demand Course";
            this.demandCourse.UseVisualStyleBackColor = true;
            this.demandCourse.Click += new System.EventHandler(this.demandCourse_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 15);
            this.label1.TabIndex = 3;
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(24, 109);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(324, 166);
            this.checkedListBox1.TabIndex = 4;
            this.checkedListBox1.Visible = false;
            // 
            // sdCourses
            // 
            this.sdCourses.Location = new System.Drawing.Point(255, 20);
            this.sdCourses.Name = "sdCourses";
            this.sdCourses.Size = new System.Drawing.Size(106, 40);
            this.sdCourses.TabIndex = 5;
            this.sdCourses.Text = "Show Demanded Courses";
            this.sdCourses.UseVisualStyleBackColor = true;
            this.sdCourses.Click += new System.EventHandler(this.sdCourses_Click);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(142, 67);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(106, 23);
            this.add.TabIndex = 6;
            this.add.Text = "Add";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // Delete
            // 
            this.Delete.Location = new System.Drawing.Point(256, 67);
            this.Delete.Name = "Delete";
            this.Delete.Size = new System.Drawing.Size(105, 23);
            this.Delete.TabIndex = 7;
            this.Delete.Text = "Delete";
            this.Delete.UseVisualStyleBackColor = true;
            this.Delete.Click += new System.EventHandler(this.Delete_Click);
            // 
            // StudentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.Controls.Add(this.Delete);
            this.Controls.Add(this.add);
            this.Controls.Add(this.sdCourses);
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.demandCourse);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.usTranscript);
            this.Name = "StudentUserControl";
            this.Size = new System.Drawing.Size(497, 332);
            this.Load += new System.EventHandler(this.StudentUserControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
