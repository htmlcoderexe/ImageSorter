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
            System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
            System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
            this.FolderProgress = new System.Windows.Forms.ProgressBar();
            this.MM = new System.Windows.Forms.MenuStrip();
            this.openFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lockKeysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameOnMoveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.KeyBindList = new System.Windows.Forms.ListBox();
            this.MainControls = new System.Windows.Forms.ToolStrip();
            this.OpenButton = new System.Windows.Forms.ToolStripButton();
            this.ReloadButton = new System.Windows.Forms.ToolStripButton();
            this.UndoButton = new System.Windows.Forms.ToolStripButton();
            this.RenameButton = new System.Windows.Forms.ToolStripButton();
            this.RenamePreview = new System.Windows.Forms.ToolStripLabel();
            this.KeybindLockToggle = new System.Windows.Forms.ToolStripButton();
            this.ImageContainer = new System.Windows.Forms.PictureBox();
            this.progresslabel = new System.Windows.Forms.ToolStripLabel();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.MM.SuspendLayout();
            this.MainControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.AutoSize = false;
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(6, 32);
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.AutoSize = false;
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(6, 32);
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
            this.MM.Visible = false;
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
            // KeyBindList
            // 
            this.KeyBindList.Dock = System.Windows.Forms.DockStyle.Right;
            this.KeyBindList.FormattingEnabled = true;
            this.KeyBindList.Location = new System.Drawing.Point(912, 39);
            this.KeyBindList.Name = "KeyBindList";
            this.KeyBindList.Size = new System.Drawing.Size(120, 432);
            this.KeyBindList.TabIndex = 3;
            // 
            // MainControls
            // 
            this.MainControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenButton,
            this.ReloadButton,
            this.UndoButton,
            toolStripSeparator2,
            this.RenameButton,
            this.RenamePreview,
            toolStripSeparator1,
            this.KeybindLockToggle,
            this.progresslabel});
            this.MainControls.Location = new System.Drawing.Point(0, 0);
            this.MainControls.Name = "MainControls";
            this.MainControls.Size = new System.Drawing.Size(1032, 39);
            this.MainControls.TabIndex = 5;
            // 
            // OpenButton
            // 
            this.OpenButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenButton.Image = global::ImageSorter.Properties.Resources.IconOpenFile;
            this.OpenButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.OpenButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(36, 36);
            this.OpenButton.Text = "Open Folder";
            this.OpenButton.Click += new System.EventHandler(this.OpenButton_Click);
            // 
            // ReloadButton
            // 
            this.ReloadButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ReloadButton.Image = global::ImageSorter.Properties.Resources.IconReload;
            this.ReloadButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ReloadButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ReloadButton.Name = "ReloadButton";
            this.ReloadButton.Size = new System.Drawing.Size(36, 36);
            this.ReloadButton.Text = "Reload Folder";
            this.ReloadButton.Click += new System.EventHandler(this.ReloadButton_Click);
            // 
            // UndoButton
            // 
            this.UndoButton.CheckOnClick = true;
            this.UndoButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.UndoButton.Image = global::ImageSorter.Properties.Resources.IconUndo;
            this.UndoButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.UndoButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.UndoButton.Name = "UndoButton";
            this.UndoButton.Size = new System.Drawing.Size(36, 36);
            this.UndoButton.Text = "Undo";
            this.UndoButton.Click += new System.EventHandler(this.UndoButton_Click);
            // 
            // RenameButton
            // 
            this.RenameButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RenameButton.Image = global::ImageSorter.Properties.Resources.IconRename;
            this.RenameButton.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.RenameButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RenameButton.Name = "RenameButton";
            this.RenameButton.Size = new System.Drawing.Size(36, 36);
            this.RenameButton.Text = "Rename";
            this.RenameButton.Click += new System.EventHandler(this.RenameButton_Click);
            // 
            // RenamePreview
            // 
            this.RenamePreview.Name = "RenamePreview";
            this.RenamePreview.Size = new System.Drawing.Size(0, 36);
            // 
            // KeybindLockToggle
            // 
            this.KeybindLockToggle.CheckOnClick = true;
            this.KeybindLockToggle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.KeybindLockToggle.Image = global::ImageSorter.Properties.Resources.IconUnlocked;
            this.KeybindLockToggle.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.KeybindLockToggle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.KeybindLockToggle.Name = "KeybindLockToggle";
            this.KeybindLockToggle.Size = new System.Drawing.Size(36, 36);
            this.KeybindLockToggle.Text = "toolStripButton3";
            this.KeybindLockToggle.Click += new System.EventHandler(this.KeybindLockToggle_Click);
            // 
            // ImageContainer
            // 
            this.ImageContainer.BackColor = System.Drawing.Color.Black;
            this.ImageContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageContainer.Location = new System.Drawing.Point(0, 39);
            this.ImageContainer.Name = "ImageContainer";
            this.ImageContainer.Size = new System.Drawing.Size(912, 432);
            this.ImageContainer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.ImageContainer.TabIndex = 2;
            this.ImageContainer.TabStop = false;
            // 
            // progresslabel
            // 
            this.progresslabel.DoubleClickEnabled = true;
            this.progresslabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.progresslabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.progresslabel.Name = "progresslabel";
            this.progresslabel.Size = new System.Drawing.Size(181, 36);
            this.progresslabel.Text = "No folder loaded";
            // 
            // MainFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1032, 494);
            this.Controls.Add(this.ImageContainer);
            this.Controls.Add(this.KeyBindList);
            this.Controls.Add(this.FolderProgress);
            this.Controls.Add(this.MM);
            this.Controls.Add(this.MainControls);
            this.KeyPreview = true;
            this.MainMenuStrip = this.MM;
            this.Name = "MainFrm";
            this.Text = "Image Sorter";
            this.SizeChanged += new System.EventHandler(this.MainFrm_SizeChanged);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MainFrm_KeyPress);
            this.MM.ResumeLayout(false);
            this.MM.PerformLayout();
            this.MainControls.ResumeLayout(false);
            this.MainControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageContainer)).EndInit();
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
        private System.Windows.Forms.ToolStripMenuItem renameOnMoveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStrip MainControls;
        private System.Windows.Forms.ToolStripButton OpenButton;
        private System.Windows.Forms.ToolStripButton UndoButton;
        private System.Windows.Forms.ToolStripButton ReloadButton;
        private System.Windows.Forms.ToolStripButton RenameButton;
        private System.Windows.Forms.ToolStripButton KeybindLockToggle;
        private System.Windows.Forms.ToolStripLabel RenamePreview;
        private System.Windows.Forms.ToolStripLabel progresslabel;
    }
}

