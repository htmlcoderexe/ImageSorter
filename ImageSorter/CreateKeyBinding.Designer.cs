namespace ImageSorter
{
    partial class CreateKeyBinding
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
            this.KeyCharLabel = new System.Windows.Forms.Label();
            this.folderInput = new System.Windows.Forms.TextBox();
            this.OkButt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // KeyCharLabel
            // 
            this.KeyCharLabel.AutoSize = true;
            this.KeyCharLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyCharLabel.Location = new System.Drawing.Point(121, 45);
            this.KeyCharLabel.Name = "KeyCharLabel";
            this.KeyCharLabel.Size = new System.Drawing.Size(208, 108);
            this.KeyCharLabel.TabIndex = 0;
            this.KeyCharLabel.Text = "N/A";
            // 
            // folderInput
            // 
            this.folderInput.Location = new System.Drawing.Point(114, 253);
            this.folderInput.MaxLength = 32;
            this.folderInput.Name = "folderInput";
            this.folderInput.Size = new System.Drawing.Size(228, 20);
            this.folderInput.TabIndex = 1;
            this.folderInput.TextChanged += new System.EventHandler(this.folderInput_TextChanged);
            // 
            // OkButt
            // 
            this.OkButt.Enabled = false;
            this.OkButt.Location = new System.Drawing.Point(186, 333);
            this.OkButt.Name = "OkButt";
            this.OkButt.Size = new System.Drawing.Size(75, 23);
            this.OkButt.TabIndex = 2;
            this.OkButt.Text = "OK";
            this.OkButt.UseVisualStyleBackColor = true;
            this.OkButt.Click += new System.EventHandler(this.OkButt_Click);
            // 
            // CreateKeyBinding
            // 
            this.AcceptButton = this.OkButt;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 401);
            this.Controls.Add(this.OkButt);
            this.Controls.Add(this.folderInput);
            this.Controls.Add(this.KeyCharLabel);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateKeyBinding";
            this.Text = "Create New Folder";
            this.Load += new System.EventHandler(this.CreateKeyBinding_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label KeyCharLabel;
        private System.Windows.Forms.TextBox folderInput;
        private System.Windows.Forms.Button OkButt;
    }
}