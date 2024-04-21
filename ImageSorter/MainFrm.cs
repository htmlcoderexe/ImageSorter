using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace ImageSorter
{
    public partial class MainFrm : Form
    {

        // stores the state of the keybinding lock
        bool KeyLockState = false;

        // progress display string (Folder name: completed/total)
        const string ProgressBarLabelString = "{0}: {1}/{2}";





        /// <summary>
        /// Refreshes the right-hand pane showing currently available keybinds
        /// </summary>
        void UpdateKeyBindList()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            KeyBindList.Items.Clear();
            foreach(KeyValuePair<char, string> kvp in FileOp.GetBindings())
            {
                KeyBindList.Items.Add("<" + kvp.Key.ToString() + "> = " + kvp.Value);
            }
        }
        
        void UpdateUserInterface()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();

            // set the progress bar
            FolderProgress.Maximum = FileOp.FilesTotal;
            FolderProgress.Value = FileOp.FilesDone + 1;

            // reset rename text
            RenamePreview.Text = "";
            // update progress bar
            FolderProgress.Value = FileOp.FilesDone;
            // update progress bar label, add 1 to the counter as it indicates the current file
            progresslabel.Text = string.Format(ProgressBarLabelString, FileOp.CurrentDir.Name, FileOp.FilesDone + 1, FileOp.FilesTotal);
            // useful to display current path, maybe change it to a display box somewhere
            this.Text = "ImageSorter - " + FileOp.CurrentFile;
            // try displaying the image
            try
            {
                // creating a temporary image from the file and then cloning it 
                // releases the lock on the original file
                Image tmp = new Bitmap(FileOp.CurrentFile);
                ImageContainer.Image = new Bitmap(tmp);
                tmp.Dispose();
            }
            catch(Exception e)
            {
                // clear the image preview
                ImageContainer.Image = null;
                // show an error, it is still possible to operate on the file.
                MessageBox.Show("Failed to load image.\r\n" + e.Message, "Image error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            UpdateKeyBindList();
        }


        /// <summary>
        /// Dequeues the next image item to process.
        /// </summary>
        void Advance()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            // exit if there's nothing to process
            if(!FileOp.IsActive)
            {
                return;
            }

            // fetch the next item and update current and progress bar
            bool advanced = FileOp.Advance();
            // if no next file, clear the image and inform user
            if(!advanced)
            {
                // clear the image preview
                ImageContainer.Image = null;
                // tell the user
                MessageBox.Show("This folder has no more images.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FolderProgress.Value = FolderProgress.Maximum;
                // update the label
                progresslabel.Text = $"{FileOp.CurrentDir.Name}: Complete!";
                return;
            }
            // Update the GUI
            UpdateUserInterface();
        }
        
        public MainFrm()
        {
            InitializeComponent();
        }


        // align the progress label to the progress bar on resize
        private void MainFrm_SizeChanged(object sender, EventArgs e)
        {

        }

 






        /// <summary>
        /// Moves a file using the specified keypress.
        /// </summary>
        /// <param name="Key">Triggering keypress</param>
        /// <returns>true if the system may advance to the  next file</returns>
        bool MoveByKey(char Key)
        {
            string[] ErrorReasons = new string[] {
                "No error. Everything is fine. Why are you even seeing this message?",
                "Generic I/O error. I know, right? I am as confused as you are.",
                "It looks like this user does not have access to the folder. Try running this program as an elevated user, if you dare.",
                "The resulting filename will be way too long. That is not allowed. Try a shorter filename, or even the folder path.",
                "Somehow, while you were sorting images, the folder went missing so we can't put your file there. Try refreshing, sometimes it helps.",
                "Totally stumped here. Really. No clue. Solar radiation? Moon phases? Weather on Mars? Beats me."
            };

            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();

            ImageSorterSingleton.FileProcessResult result = FileOp.ProcessCurrentFile(Key);

            switch (result)
            {
                // if keypress is not in the bindings list, check the lock setting
                case ImageSorterSingleton.FileProcessResult.NoKeyBinding:
                {
                    // if locked, display a message informing the user about the lock and cancel
                    if (KeyLockState)
                    {
                        MessageBox.Show($"There is no folder registered for <{Key}>. New keybindings may only be created when the keys are unlocked, press Ctrl+L to toggle.", "Unrecognised key", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return false;
                    }
                    // else try to create a new binding
                    else
                    {
                        // if the binding was created, try again
                        if (CreateNewKey(Key))
                        {
                            return MoveByKey(Key); ;
                        }
                        else
                        {
                            return false;
                        }
                        //otherwise proceed further and attempt the move
                    }
                }

                // somehow, the file about to be moved went missing
                case ImageSorterSingleton.FileProcessResult.FileIsMissing:
                {
                    MessageBox.Show("Failed to move file - the source file is missing.", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // advance anyway as this won't resolve itself by mashing a different
                    // button in most cases
                    return true;
                }

                // yay!
                case ImageSorterSingleton.FileProcessResult.Success:
                {
                    return true;
                }

                // who knows what went wrong here
                case ImageSorterSingleton.FileProcessResult.UnknownError:
                {
                    MessageBox.Show("An unknown problem occurred moving the file. That's all we know.", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // advance anyway as this won't resolve itself by mashing a different
                    // button in most cases
                    return true;
                }

                // tried to write to an already existing file - offer a rename
                case ImageSorterSingleton.FileProcessResult.TargetExists:
                {
                    // request an available filename and present the option
                    string FileName = FileOp.GetFreeFileName();
                    DialogResult replacefile = MessageBox.Show($"File \"{FileOp.CurrentTargetFileName}\" already exists in folder. Do you want to use this name instead?\r\n{FileName}", "File error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    // if accepted, retry
                    if(replacefile == DialogResult.Yes)
                    {
                        // "rename" the file and retry move
                        FileOp.NewFileName = FileName;
                        return MoveByKey(Key);
                    }
                    // user cancels and wants to do something else, I guess
                    else
                    {
                        return false;
                    }
                }

                // writing the file failed, the problem is specifically in the target directory
                case ImageSorterSingleton.FileProcessResult.WriteError:
                {
                    // be more informative by giving details why their move failed
                    MessageBox.Show("Failed to move file. \r\n" + ErrorReasons[(int)FileOp.LastWriteError], "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // can fix some of those while the image is still up
                    // for grabs, but probably easier to just skip it 
                    return true;
                }

            }

            return false;
        }

        /// <summary>
        /// Create a new keybinding/folder combination with user prompt.
        /// </summary>
        /// <param name="keypress">The target keybinding.</param>
        bool CreateNewKey(char keypress)
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            // initialise the form
            CreateKeyBinding prompt = new CreateKeyBinding
            {
                Key = keypress,
                RootFolder = FileOp.CurrentDir.FullName
            };
            // show the form
            prompt.ShowDialog();
            // if folder was selected successfully, add the binding
            if(prompt.DialogResult == DialogResult.OK)
            {
                FileOp.LoadKeyBind(FileOp.CurrentDir.FullName, keypress, prompt.Folder);
            }
            // user cancelled the operation somehow
            else
            {
                return false;
            }
            // refresh the display
            UpdateUserInterface();
            // save config
            FileOp.SaveCurrentConfig();
            return true;
        }
       
        private void MainFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            // check if there's an ongoing sort operation
            if (!FileOp.IsActive)
                return; 
            // Try to move the file
            if(MoveByKey(e.KeyChar))
            {
                // get to the next item on success
                Advance();
            }
        }

        #region Menu Actions

        // Select a folder to process
        void OpenFolderCommand()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult dlgresult = dlg.ShowDialog();
            if(dlgresult != DialogResult.OK)
                return;
            ImageSorterSingleton.OpenFolderResult result = FileOp.OpenFolder(dlg.SelectedPath);

            // check if load was successful, exit if not
            switch(result)
            {
                // no images in the folder, bail
                case ImageSorterSingleton.OpenFolderResult.NoImages:
                {
                    MessageBox.Show($"The folder \"{dlg.SelectedPath}\" did not contain any images. Please try another folder.", "No images found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                // couldn't setup any of the folders, bail
                case ImageSorterSingleton.OpenFolderResult.AllKeybindsFailed:
                {
                    MessageBox.Show($"Unable to setup keybinds for \"{dlg.SelectedPath}\". The folder may be protected, missing or otherwise inaccessible.", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // somehow, the folder went missing meantime, bail
                case ImageSorterSingleton.OpenFolderResult.NotFound:
                {
                    MessageBox.Show($"The folder \"{dlg.SelectedPath}\" was not found.", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // no access rights to folder, bail
                case ImageSorterSingleton.OpenFolderResult.AccessDenied:
                {
                    MessageBox.Show($"You do not have access to \"{dlg.SelectedPath}\".", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // system path length restriction reached, bail
                case ImageSorterSingleton.OpenFolderResult.PathTooLong:
                {
                    MessageBox.Show($"The path \"{dlg.SelectedPath}\" was too long, at {dlg.SelectedPath.Length} characters long.", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // couldn't create some of the folders, keep going but inform user
                case ImageSorterSingleton.OpenFolderResult.KeybindFailed:
                {
                    MessageBox.Show("The folder was loaded successfully. However, some assigned folders were unable to be created.", "Missing keybindings", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    break;
                }
                // everything went ok, keep going
                case ImageSorterSingleton.OpenFolderResult.Success:
                {
                    break;
                }
                // some unknown failure, bail
                case ImageSorterSingleton.OpenFolderResult.Other:
                {
                    MessageBox.Show($"The folder \"{dlg.SelectedPath}\" was not loaded due to an unknown error.", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // RO folder, offer to clear flag 
                case ImageSorterSingleton.OpenFolderResult.ReadOnly:
                {

                    MessageBox.Show($"Unable to use \"{dlg.SelectedPath}\". The folder may be protected, missing or otherwise inaccessible.", "Setup failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                    // apparently readonly forlders are wacky, forget this for now.
                    // DialogResult removeRO = MessageBox.Show($"The folder \"{dlg.SelectedPath}\" is read-only. Would you like to remove the read-only flag and continue?", "Read-only folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    // // user didn't confirm to remove RO, bail
                    // if(removeRO != DialogResult.Yes)
                    //     return;
                    // DirectoryInfo targetdir = new DirectoryInfo(dlg.SelectedPath);
                    // try
                    // {
                    //     targetdir.Attributes.
                    // }
                    // return;
                }
            }

            // if still here, set up and load

            Advance();

        }

        /// <summary>
        /// Undoes a single file operation.
        /// </summary>
        void Undo()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            if(!FileOp.Undo())
                MessageBox.Show("Undo failed!\r\n", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Toggle the keybinding lock and update the state of the menu items
        void ToggleKeyLock()
        {
            // toggle the thing
            KeyLockState = !KeyLockState;
            // update the text on a menu that's literally invisible and only there for the accel key
            lockKeysToolStripMenuItem.Text = KeyLockState ? "Unlock keybinds" : "Lock keybinds";
            // update the button's look at the same time
            KeybindLockToggle.Checked = KeyLockState;
            KeybindLockToggle.Image = KeyLockState ? ImageSorter.Properties.Resources.IconLocked : Properties.Resources.IconUnlocked;
            KeybindLockToggle.Text = KeyLockState ? "Unlock keybinds" : "Lock keybinds";
        }



        // reload current folder
        private void ReloadFolder()
        {

            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();

            ImageSorterSingleton.OpenFolderResult result = FileOp.ReloadFolder();

            if(result == ImageSorterSingleton.OpenFolderResult.KeybindFailed || result == ImageSorterSingleton.OpenFolderResult.Success)
                FileOp.Advance();
            else
                MessageBox.Show("Reloading failed!", "Reload error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            UpdateUserInterface();
        }

        void RenameFile()
        {
            ImageSorterSingleton FileOp = ImageSorterSingleton.GetInstance();
            RenameFile prompt = new RenameFile();
            //get extension of current file
            prompt.FileExtension = Path.GetExtension(FileOp.CurrentFile);
            DialogResult result = prompt.ShowDialog();
            if (result == DialogResult.OK)
            {
                FileOp.NewFileName = prompt.FileName;
                RenamePreview.Text = FileOp.NewFileName;
            }
            
        }
#endregion


        #region menu event handlers
        private void lockKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleKeyLock();
        }

        private void KeybindLockToggle_Click(object sender, EventArgs e)
        {
            ToggleKeyLock();
        }




        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }


        private void UndoButton_Click(object sender, EventArgs e)
        {
            Undo();
        }

        // show rename dialog and set the rename target 
        private void renameOnMoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameFile();
        }

        private void RenameButton_Click(object sender, EventArgs e)
        {
            RenameFile();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReloadFolder();
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            ReloadFolder();
        }
        private void OpenButton_Click(object sender, EventArgs e)
        {
            OpenFolderCommand();
        }
        
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFolderCommand();
        }
        #endregion


        private void KeyBindList_DoubleClick(object sender, EventArgs e)
        {

        }

        private void MainFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
