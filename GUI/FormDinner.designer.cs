namespace GUI
{
    partial class FormDinner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.txtHour = new System.Windows.Forms.DomainUpDown();
            this.txtMinute = new System.Windows.Forms.DomainUpDown();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date";
            // 
            // dtDate
            // 
            this.dtDate.CustomFormat = "yyyy-MM-dd";
            this.dtDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDate.Location = new System.Drawing.Point(69, 19);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(100, 20);
            this.dtDate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(196, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(30, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Time";
            // 
            // txtHour
            // 
            this.txtHour.Items.Add("00");
            this.txtHour.Items.Add("01");
            this.txtHour.Items.Add("02");
            this.txtHour.Items.Add("03");
            this.txtHour.Items.Add("04");
            this.txtHour.Items.Add("05");
            this.txtHour.Items.Add("06");
            this.txtHour.Items.Add("07");
            this.txtHour.Items.Add("08");
            this.txtHour.Items.Add("09");
            this.txtHour.Items.Add("10");
            this.txtHour.Items.Add("11");
            this.txtHour.Items.Add("12");
            this.txtHour.Items.Add("13");
            this.txtHour.Items.Add("14");
            this.txtHour.Items.Add("15");
            this.txtHour.Items.Add("16");
            this.txtHour.Items.Add("17");
            this.txtHour.Items.Add("18");
            this.txtHour.Items.Add("19");
            this.txtHour.Items.Add("20");
            this.txtHour.Items.Add("21");
            this.txtHour.Items.Add("22");
            this.txtHour.Items.Add("23");
            this.txtHour.Location = new System.Drawing.Point(241, 19);
            this.txtHour.Name = "txtHour";
            this.txtHour.Size = new System.Drawing.Size(76, 20);
            this.txtHour.TabIndex = 7;
            // 
            // txtMinute
            // 
            this.txtMinute.Items.Add("00");
            this.txtMinute.Items.Add("01");
            this.txtMinute.Items.Add("02");
            this.txtMinute.Items.Add("03");
            this.txtMinute.Items.Add("04");
            this.txtMinute.Items.Add("05");
            this.txtMinute.Items.Add("06");
            this.txtMinute.Items.Add("07");
            this.txtMinute.Items.Add("08");
            this.txtMinute.Items.Add("09");
            this.txtMinute.Items.Add("10");
            this.txtMinute.Items.Add("11");
            this.txtMinute.Items.Add("12");
            this.txtMinute.Items.Add("13");
            this.txtMinute.Items.Add("14");
            this.txtMinute.Items.Add("15");
            this.txtMinute.Items.Add("16");
            this.txtMinute.Items.Add("17");
            this.txtMinute.Items.Add("18");
            this.txtMinute.Items.Add("19");
            this.txtMinute.Items.Add("20");
            this.txtMinute.Items.Add("21");
            this.txtMinute.Items.Add("22");
            this.txtMinute.Items.Add("23");
            this.txtMinute.Items.Add("24");
            this.txtMinute.Items.Add("25");
            this.txtMinute.Items.Add("26");
            this.txtMinute.Items.Add("27");
            this.txtMinute.Items.Add("28");
            this.txtMinute.Items.Add("29");
            this.txtMinute.Items.Add("30");
            this.txtMinute.Items.Add("31");
            this.txtMinute.Items.Add("32");
            this.txtMinute.Items.Add("33");
            this.txtMinute.Items.Add("34");
            this.txtMinute.Items.Add("35");
            this.txtMinute.Items.Add("36");
            this.txtMinute.Items.Add("37");
            this.txtMinute.Items.Add("38");
            this.txtMinute.Items.Add("39");
            this.txtMinute.Items.Add("40");
            this.txtMinute.Items.Add("41");
            this.txtMinute.Items.Add("42");
            this.txtMinute.Items.Add("43");
            this.txtMinute.Items.Add("44");
            this.txtMinute.Items.Add("45");
            this.txtMinute.Items.Add("46");
            this.txtMinute.Items.Add("47");
            this.txtMinute.Items.Add("48");
            this.txtMinute.Items.Add("49");
            this.txtMinute.Items.Add("50");
            this.txtMinute.Items.Add("51");
            this.txtMinute.Items.Add("52");
            this.txtMinute.Items.Add("53");
            this.txtMinute.Items.Add("54");
            this.txtMinute.Items.Add("55");
            this.txtMinute.Items.Add("56");
            this.txtMinute.Items.Add("57");
            this.txtMinute.Items.Add("58");
            this.txtMinute.Items.Add("59");
            this.txtMinute.Location = new System.Drawing.Point(323, 19);
            this.txtMinute.Name = "txtMinute";
            this.txtMinute.Size = new System.Drawing.Size(76, 20);
            this.txtMinute.TabIndex = 8;
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(69, 59);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(383, 20);
            this.txtLocation.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Where";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(69, 95);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(383, 20);
            this.txtDescription.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Descr";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(151, 182);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(241, 182);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // FormDinner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 217);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtDescription);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMinute);
            this.Controls.Add(this.txtHour);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.label1);
            this.Name = "FormDinner";
            this.Text = "FormDinner";
            this.Load += new System.EventHandler(this.FormDinner_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.DateTimePicker dtDate;
        public System.Windows.Forms.DomainUpDown txtHour;
        public System.Windows.Forms.DomainUpDown txtMinute;
        public System.Windows.Forms.TextBox txtLocation;
        public System.Windows.Forms.TextBox txtDescription;
    }
}