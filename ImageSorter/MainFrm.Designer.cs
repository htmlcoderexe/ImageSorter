namespace ImageSorter
{
    partial class MainFrm
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
            this.FolderProgress = new System.Windows.Forms.ProgressBar();
            this.MM = new System.Windows.Forms.MenuStrip();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameOnMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageContainer = new System.Windows.Forms.PictureBox();
            this.KeyBindList = new System.Windows.Forms.ListBox();
            this.progresslabel = new System.Windows.Forms.Label();
            this.MainControls = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.MM.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).BeginInit();
            this.MainControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // FolderProgress
            // 
            this.FolderProgress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FolderProgress.Location = new System.Drawing.Point(0, 471);
            this.FolderProgress.MarqueeAnimationSpeed = 1;
            this.FolderProgress.Name = "FolderProgress";
            this.FolderProgress.Size = new System.Drawing.Size(1032, 23);
            this.FolderProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.FolderProgress.TabIndex = 0;
            // 
            // MM
            // 
            this.MM.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderToolStripMenuItem,
            this.lockKeysToolStripMenuItem,
            this.undoToolStripMenuItem,
            this.renameOnMoveToolStripMenuItem,
            this.refreshToolStripMenuItem});
            this.MM.Location = new System.Drawing.Point(0, 0);
            this.MM.Name = "MM";
            this.MM.Size = new System.Drawing.Size(1032, 24);
            this.MM.TabIndex = 1;
            this.MM.Text = "menuStrip1";
            // 
            // openFolderToolStripMenuItem
            // 
            this.openFolderToolStripMenuItem.Name = "openFolderToolStripMenuItem";
            this.openFolderToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openFolderToolStripMenuItem.Size = new System.Drawing.Size(84, 20);
            this.openFolderToolStripMenuItem.Text = "Open Folder";
            this.openFolderToolStripMenuItem.Click += new System.EventHandler(this.openFolderToolStripMenuItem_Click);
            // 
            // lockKeysToolStripMenuItem
            // 
            this.lockKeysToolStripMenuItem.CheckOnClick = true;
            this.lockKeysToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lockKeysToolStripMenuItem.Name = "lockKeysToolStripMenuItem";
            this.lockKeysToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.lockKeysToolStripMenuItem.Size = new System.Drawing.Size(94, 20);
            this.lockKeysToolStripMenuItem.Text = "Lock keybinds";
            this.lockKeysToolStripMenuItem.Click += new System.EventHandler(this.lockKeysToolStripMenuItem_Click);
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // renameOnMoveToolStripMenuItem
            // 
            this.renameOnMoveToolStripMenuItem.Name = "renameOnMoveToolStripMenuItem";
            this.renameOnMoveToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F2;
            this.renameOnMoveToolStripMenuItem.Size = new System.Drawing.Size(112, 20);
            this.renameOnMoveToolStripMenuItem.Text = "Rename on move";
            this.renameOnMoveToolStripMenuItem.Click += new System.EventHandler(this.renameOnMoveToolStripMenuItem_Click);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // ImageContainer
            // 
            this.ImageContainer.BackColor = System.Drawing.Color.Black;
            this.ImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageContainer.Location = new System.Drawing.Point(0, 24);
            this.ImageContainer.Name = "ImageContainer";
            this.ImageContainer.Size = new System.Drawing.Size(912, 447);
            this.ImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageContainer.TabIndex = 2;
            this.ImageContainer.TabStop = false;
            // 
            // KeyBindList
            // 
            this.KeyBindList.Dock = System.Windows.Forms.DockStyle.Right;
            this.KeyBindList.FormattingEnabled = true;
            this.KeyBindList.Location = new System.Drawing.Point(912, 24);
            this.KeyBindList.Name = "KeyBindList";
            this.KeyBindList.Size = new System.Drawing.Size(120, 447);
            this.KeyBindList.TabIndex = 3;
            // 
            // progresslabel
            // 
            this.progresslabel.BackColor = System.Drawing.Color.Transparent;
            this.progresslabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progresslabel.Location = new System.Drawing.Point(445, 474);
            this.progresslabel.Name = "progresslabel";
            this.progresslabel.Size = new System.Drawing.Size(78, 23);
            this.progresslabel.TabIndex = 4;
            this.progresslabel.Text = "---/---";
            this.progresslabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MainControls
            // 
            this.MainControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.MainControls.Location = new System.Drawing.Point(0, 24);
            this.MainControls.Name = "MainControls";
            this.MainControls.Size = new System.Drawing.Size(912, 135);
            this.MainControls.TabIndex = 5;
            this.MainControls.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ImageSorter.Properties.Resources.IconOpenFile;
            this.toolStripButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(132, 132);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 494);
            this.Controls.Add(this.MainControls);
            this.Controls.Add(this.progresslabel);
            this.Controls.Add(this.ImageContainer);
            this.Controls.Add(this.KeyBindList);
            this.Controls.Add(this.FolderProgress);
            this.Controls.Add(this.MM);
            this.KeyPreview = true;
            this.MainMenuStrip = this.MM;
            this.Name = "MainFrm";
            this.Text = "Image Sorter";
            this.SizeChanged += new System.EventHandler(this.MainFrm_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainFrm_KeyPress);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).EndInit();
            this.MainControls.ResumeLayout(false);
            this.MainControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar FolderProgress;
        private System.Windows.Forms.MenuStrip MM;
        private System.Windows.Forms.ToolStripMenuItem openFolderToolStripMenuItem;
        private System.Windows.Forms.PictureBox ImageContainer;
        private System.Windows.Forms.ListBox KeyBindList;
        private System.Windows.Forms.ToolStripMenuItem lockKeysToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.Label progresslabel;
        private System.Windows.Forms.ToolStripMenuItem renameOnMoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStrip MainControls;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}

