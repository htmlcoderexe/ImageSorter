namespace ImageSorter
{
    partial class RenameFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenameFile));
            this.OKButt = new System.Windows.Forms.Button();
            this.FileInput = new System.Windows.Forms.TextBox();
            this.CancelButt = new System.Windows.Forms.Button();
            this.helplabel = new System.Windows.Forms.Label();
            this.clearbutt = new System.Windows.Forms.Button();
            this.ExtBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // OKButt
            // 
            this.OKButt.Enabled = false;
            this.OKButt.Location = new System.Drawing.Point(107, 172);
            this.OKButt.Name = "OKButt";
            this.OKButt.Size = new System.Drawing.Size(75, 23);
            this.OKButt.TabIndex = 0;
            this.OKButt.Text = "Rename";
            this.OKButt.UseVisualStyleBackColor = true;
            this.OKButt.Click += new System.EventHandler(this.OKButt_Click);
            // 
            // FileInput
            // 
            this.FileInput.Location = new System.Drawing.Point(29, 146);
            this.FileInput.MaxLength = 32;
            this.FileInput.Name = "FileInput";
            this.FileInput.Size = new System.Drawing.Size(303, 20);
            this.FileInput.TabIndex = 1;
            this.FileInput.TextChanged += new System.EventHandler(this.FileInput_TextChanged);
            // 
            // CancelButt
            // 
            this.CancelButt.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelButt.Location = new System.Drawing.Point(188, 172);
            this.CancelButt.Name = "CancelButt";
            this.CancelButt.Size = new System.Drawing.Size(75, 23);
            this.CancelButt.TabIndex = 2;
            this.CancelButt.Text = "Cancel";
            this.CancelButt.UseVisualStyleBackColor = true;
            // 
            // helplabel
            // 
            this.helplabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helplabel.Location = new System.Drawing.Point(25, 20);
            this.helplabel.Name = "helplabel";
            this.helplabel.Size = new System.Drawing.Size(401, 123);
            this.helplabel.TabIndex = 3;
            this.helplabel.Text = resources.GetString("helplabel.Text");
            // 
            // clearbutt
            // 
            this.clearbutt.Location = new System.Drawing.Point(305, 172);
            this.clearbutt.Name = "clearbutt";
            this.clearbutt.Size = new System.Drawing.Size(121, 23);
            this.clearbutt.TabIndex = 4;
            this.clearbutt.Text = "Clear rename";
            this.clearbutt.UseVisualStyleBackColor = true;
            this.clearbutt.Click += new System.EventHandler(this.clearbutt_Click);
            // 
            // ExtBox
            // 
            this.ExtBox.Enabled = false;
            this.ExtBox.Location = new System.Drawing.Point(338, 146);
            this.ExtBox.Name = "ExtBox";
            this.ExtBox.Size = new System.Drawing.Size(87, 20);
            this.ExtBox.TabIndex = 5;
            this.ExtBox.DoubleClick += new System.EventHandler(this.ExtBox_DoubleClick);
            // 
            // RenameFile
            // 
            this.AcceptButton = this.OKButt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CancelButt;
            this.ClientSize = new System.Drawing.Size(438, 198);
            this.Controls.Add(this.ExtBox);
            this.Controls.Add(this.clearbutt);
            this.Controls.Add(this.helplabel);
            this.Controls.Add(this.CancelButt);
            this.Controls.Add(this.FileInput);
            this.Controls.Add(this.OKButt);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenameFile";
            this.Text = "Rename File";
            this.Load += new System.EventHandler(this.RenameFile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button OKButt;
        private System.Windows.Forms.TextBox FileInput;
        private System.Windows.Forms.Button CancelButt;
        private System.Windows.Forms.Label helplabel;
        private System.Windows.Forms.Button clearbutt;
        private System.Windows.Forms.TextBox ExtBox;
    }
}