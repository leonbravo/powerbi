namespace PowerBIAPI
{
    partial class Form1
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
            this.btnCreateDataset = new System.Windows.Forms.Button();
            this.btnAddNewRecord = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.btnclearAndAdd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateDataset
            // 
            this.btnCreateDataset.Location = new System.Drawing.Point(55, 54);
            this.btnCreateDataset.Name = "btnCreateDataset";
            this.btnCreateDataset.Size = new System.Drawing.Size(119, 23);
            this.btnCreateDataset.TabIndex = 0;
            this.btnCreateDataset.Text = "Create Dataset";
            this.btnCreateDataset.UseVisualStyleBackColor = true;
            this.btnCreateDataset.Click += new System.EventHandler(this.btnCreateDataset_Click);
            // 
            // btnAddNewRecord
            // 
            this.btnAddNewRecord.Location = new System.Drawing.Point(55, 99);
            this.btnAddNewRecord.Name = "btnAddNewRecord";
            this.btnAddNewRecord.Size = new System.Drawing.Size(119, 23);
            this.btnAddNewRecord.TabIndex = 1;
            this.btnAddNewRecord.Text = "Add New Record";
            this.btnAddNewRecord.UseVisualStyleBackColor = true;
            this.btnAddNewRecord.Click += new System.EventHandler(this.btnAddNewRecord_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(333, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(815, 237);
            this.textBox1.TabIndex = 2;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(55, 12);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 20);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "1";
            // 
            // btnclearAndAdd
            // 
            this.btnclearAndAdd.Location = new System.Drawing.Point(55, 154);
            this.btnclearAndAdd.Name = "btnclearAndAdd";
            this.btnclearAndAdd.Size = new System.Drawing.Size(119, 23);
            this.btnclearAndAdd.TabIndex = 4;
            this.btnclearAndAdd.Text = "Clear and add";
            this.btnclearAndAdd.UseVisualStyleBackColor = true;
            this.btnclearAndAdd.Click += new System.EventHandler(this.btnclearAndAdd_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1180, 261);
            this.Controls.Add(this.btnclearAndAdd);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btnAddNewRecord);
            this.Controls.Add(this.btnCreateDataset);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCreateDataset;
        private System.Windows.Forms.Button btnAddNewRecord;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button btnclearAndAdd;
    }
}

