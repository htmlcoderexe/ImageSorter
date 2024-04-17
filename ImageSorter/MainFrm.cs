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
        // the program will try to look for this file for its settings
        const string ConfigFileName = "ImageSorter.cfg";
        // This character will separate keys and assigned folders 
        // chosen because it is unlikely to occur as a keypress,
        // looks like ":=" asignment operator and is actually
        // a mathematical symbol (one of!) for assigning a variable.
        const char Separator = '≔';
        // progress display string (Folder name: completed/total)
        const string ProgressBarLabelString = "{0}: {1}/{2}";
        // stores the file currently operated on
        string CurrentPath;
        // used for rename functionality, stores new filename that will be used in the target dir
        string NewFileName = "";
        // used to store the current directory
        DirectoryInfo CurrentDir;
        // queue to progress through the folder
        LinkedList<string> Todo;
        // this is the dictionary that will hold all the keybindings
        Dictionary<char, string> SubFolders;
        // this will hold the Undo stack
        Stack<Tuple<string, string>> UndoStack;
        // hardcoding the image extensions here for now.
        string[] extensions = { ".png",".jpg",".gif",".jpeg",".webp" };

        // stores the state of the keybinding lock
        bool KeyLockState = false;

        /// <summary>
        /// Check if filename contains any invalid characters.
        /// </summary>
        /// <param name="s">Filename to check</param>
        /// <returns></returns>
        public static bool IsNameInvalid(string s)
        {
            if (s.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) >= 0)
                return true;
            return false;
        }

        /// <summary>
        /// Tries to find a filename that would not collide with an existing one.
        /// </summary>
        /// <param name="filename">The colliding file name.</param>
        /// <param name="folder">Target folder</param>
        /// <returns></returns>
        string GetFreeFileName(string filename, string folder)
        {
            string ext = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);
            
            int counter = 2;
            string candidatename = name + "_" + counter.ToString()+ext;

            while (File.Exists(Path.Combine(folder, candidatename)))
            {
                counter++;
                candidatename = name + "_" + counter.ToString() + ext;
            }

            return candidatename;
        }

        /// <summary>
        /// Loads a list of all filenames with image-like extensions from a given folder.
        /// </summary>
        /// <param name="path">Folder path to search</param>
        /// <returns>List of filenames</returns>
        List<string> GetImageFiles(string path)
        {
            List<string> result = new List<string>();

            foreach(string ext in extensions)
            {
                result.AddRange(Directory.EnumerateFiles(path, "*" + ext, SearchOption.TopDirectoryOnly));
            }
            return result;
        }

        /// <summary>
        /// Refreshes the right-hand pane showing currently available keybinds
        /// </summary>
        void UpdateKeyBindList()
        {
            KeyBindList.Items.Clear();
            foreach(KeyValuePair<char, string> kvp in SubFolders)
            {
                KeyBindList.Items.Add("<" + kvp.Key.ToString() + "> = " + kvp.Value);
            }
        }
        

        /// <summary>
        /// Dequeues the next image item to process.
        /// </summary>
        void Advance()
        {
            //exit if there's no queue
            if(Todo==null)
            {
                return;
            }
            //if the queue is empty, the folder is complete
            if(Todo.Count<1)
            {
                // clear the image preview
                ImageContainer.Image = null;
                // tell the user
                MessageBox.Show("This folder has no more images.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FolderProgress.Value = FolderProgress.Maximum;
                // update the label
                progresslabel.Text = $"{CurrentDir.Name}: Complete!";
                Todo = null;
                return;
            }
            // fetch the next item and update current and progress bar
            string FilePath = Todo.First();
            Todo.RemoveFirst();
            CurrentPath = FilePath;
            // reset rename function
            NewFileName = "";
            RenamePreview.Text = "";
            // update progress bar
            FolderProgress.Value = FolderProgress.Maximum-(Todo.Count() + 1);
            // update progress bar label
            progresslabel.Text = string.Format(ProgressBarLabelString, CurrentDir.Name, FolderProgress.Value.ToString(), FolderProgress.Maximum.ToString());
            // useful to display current path, maybe change it to a display box somewhere
            this.Text = CurrentPath;
            // try displaying the image
            try
            {
                //creating a temporary image from the file and then cloning it 
                //releases the lock on the original file
                Image tmp = new Bitmap(CurrentPath);
                ImageContainer.Image = new Bitmap(tmp);
                tmp.Dispose();
            }
            catch(Exception e)
            {
                //show an error, it is still possible to operate on the file.
                MessageBox.Show("Failed to load image.\r\n"+e.Message, "Image error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public MainFrm()
        {
            InitializeComponent();
            SubFolders = new Dictionary<char, string>();
            UpdateKeyBindList();
            UndoStack = new Stack<Tuple<string, string>>();
        }


        // align the progress label to the progress bar on resize
        private void MainFrm_SizeChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Try parsing a configuration line.
        /// </summary>
        /// <param name="line">Line to be parsed.</param>
        /// <returns>true on success, false on faliure</returns>
        KeyValuePair<char, string>? ParseConfigLine(string line)
        {
            //bail early if empty
            if (line == "" || line == null)
                return null;
            //split the line into the keychar and the folder name
            string[] parts = line.Split(new char[]{ Separator}, 2);
            //sanity check, if there are not two parts, fail
            if (parts.Length != 2)
                return null;
            if (parts[0].Length > 1)
                return null;
            //the first(and only) char of the first piece is the keychar
            char keychar = parts[0][0];
            //the second piece is the foldername
            string folder = parts[1];
            //check if foldername is valid for usage
            if (IsNameInvalid(folder))
                return null;
            return new KeyValuePair<char, string>(keychar, folder);
        }

        /// <summary>
        /// Try parsing a configuration file.
        /// </summary>
        /// <param name="lines">Array of configuration lines</param>
        /// <returns>List of binding KVPs or empty list on failure</returns>
        List<KeyValuePair<char, string>> ParseConfigFile(string[] lines)
        {
            List<KeyValuePair<char, string>> configs = new List<KeyValuePair<char, string>>();
            //go over each line
            for(int i =0;i< lines.Length;i++)
            {
                //try getting a valid config line
                KeyValuePair<char, string>? kvp = ParseConfigLine(lines[i]);
                //if line is valid, add to the config list
                if(kvp.HasValue)
                {
                    configs.Add(kvp.Value);
                }
                else
                {
                    //last line is allowed to be invalid (most likely empty)
                    //anywhere else, return empty list/fail
                    if (i != lines.Length - 1)
                    {
                        return new List<KeyValuePair<char, string>>();
                    }
                }
            }
            //if not failed yet, return what is collected so far
            return configs;
        }

        /// <summary>
        /// Try loading a configuration file from a folder
        /// </summary>
        /// <param name="folderpath">Folder containing the file</param>
        /// <returns>List of binding KVPs or null on fail</returns>
        List<KeyValuePair<char,string>> LoadConfigFile(string folderpath)
        {
            List<KeyValuePair<char, string>> configs = new List<KeyValuePair<char, string>>();
            //get the proper filename and try loading
            string filepath = Path.Combine(folderpath, ConfigFileName);
            //if file's missing, fail
            if (!File.Exists(filepath))
                return null;
            string[] filecontents;
            try
            {
                //try reading the file as lines and parsing those
                filecontents = File.ReadAllLines(filepath);
                configs = ParseConfigFile(filecontents);
                //if non-empty list is returned, return it
                if (configs.Count > 0)
                    return configs;
                //else fail
                return null;
            }
            //couldn't load, don't care too much why right now, fail
            catch(Exception ex)
            {
                return null;
            }
        }

        bool SaveConfigFile(string folderpath)
        {
            //if there are no bindings, don't bother and fail
            if (SubFolders.Count < 1)
                return false;
            string[] lines = new string[SubFolders.Count];
            //squish the dictionary
            KeyValuePair<char,string>[] bindings =SubFolders.ToArray();
            //go slot per slot
            for(int i=0;i<bindings.Length;i++)
            {
                //each line is the keychar, then ≔, then the subfolder
                lines[i] = bindings[i].Key.ToString() + Separator.ToString() + bindings[i].Value;
            }
            //attempt to write the config to disk
            try
            {
                File.WriteAllLines(Path.Combine(folderpath, ConfigFileName), lines);
            }
            //if write fails, so does this function
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load a folder for processing.
        /// </summary>
        /// <param name="path">Absolute path to the folder.</param>
        void OpenFolder(string path)
        {
            //Set current working dir
            CurrentDir = new DirectoryInfo(path);
            //fetch a list of all images
            List<string> FileNames = GetImageFiles(path);
            //create and populate a queue to process one item at a time
            Todo = new LinkedList<string>();
            foreach(string filepath in FileNames)
            {
                Todo.AddLast(filepath);
            }
            //set the progress bar
            FolderProgress.Maximum = FileNames.Count();
            FolderProgress.Value = 0;
            //refresh the bind list
            SubFolders.Clear();
            //try loading a configuration file from the target folder
            List<KeyValuePair<char, string>> configs = LoadConfigFile(path);
            //if successful, load all keybinds from it
            if(configs!=null)
            {
                foreach(KeyValuePair<char,string> kvp in configs)
                {
                    LoadKeyBind(path, kvp.Key, kvp.Value);
                }
            }
            //else try default config (should be in the application's own folder)
            //for now there's no way to adjust it except for copying one created for a specific folder
            else
            {
                configs = LoadConfigFile(Path.GetDirectoryName(Application.ExecutablePath));
                //if successful, load all keybinds from it
                if (configs != null)
                {
                    foreach (KeyValuePair<char, string> kvp in configs)
                    {
                        LoadKeyBind(path, kvp.Key, kvp.Value);
                    }
                }
                //if that fails, load this default hardcoded preset
                else
                {
                    bool errorcheck = LoadKeyBind(path, ' ', "nsfw");
                    //if for some reason that fails, inform the user and offer to close the program.
                    if (!errorcheck)
                    {
                        DialogResult exitrequest = MessageBox.Show("Unable to create default keybind - check if you are able to make changes to the target directory. Would you like to exit the program?", "Folder write error (probably)", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if(exitrequest == DialogResult.Yes)
                        {
                            this.Close();
                        }
                    }
                }
            }
            //reset undo stack
            UndoStack.Clear();
            //refresh the pane that shows the bindings 
            UpdateKeyBindList();
        }

        bool LoadKeyBind(string workingdir, char key, string target)
        {
            //create the corresponding folder
            string folderpath = Path.Combine(workingdir, target);
            try
            {
                Directory.CreateDirectory(folderpath);
            }
            //usually this only fails if there is a file named like the target exactly, in which case, try a random name
            catch (Exception ex)
            {
                
                string newrandomstring = Path.GetRandomFileName();
                
                try
                {
                    Directory.CreateDirectory(Path.Combine(workingdir, target+"_" + newrandomstring));

                }
                //either something else went wrong, or we're astronomically unlucky - display error and give the option to exit
                catch (Exception ex2)
                {
                    MessageBox.Show("Could not create \""+ target + "_" + newrandomstring + "\" folder for <"+key.ToString()+"> keybind. This keybind will be unavailable. The below information may clarify the cause of the issue:\r\n" + ex2.Message, "We tried", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                //tell the user the folder had to be renamed...
                MessageBox.Show("Could not create \"" + target + "\" folder for <" + key.ToString() + "> keybind.\r\nThe program was able to use a fallback name: \"" + target + "_" + newrandomstring + "\"\r\n See if the below information helps clarify the cause of the issue:\r\n" + ex.Message, "We tried", MessageBoxButtons.OK, MessageBoxIcon.Warning);


            }
            SubFolders.Add(key, target);
            return true;
        }


        /// <summary>
        /// Moves a file located at a specified path, triggered by a keypress.
        /// </summary>
        /// <param name="key">Triggering keypress</param>
        /// <param name="path">Path of the file to be moved</param>
        /// <returns>true if the system may advance to the  next file</returns>
        bool MoveByKey(char key,string path)
        {
            // get the parent dir
            string foldername = Path.GetDirectoryName(path);
            // get the filename, if rename exists, use that instead
            string filename = NewFileName == "" ? Path.GetFileName(path) : NewFileName;
            // get the correct target subfolder based on the keypress
            // check if there's a folder defined for the key and display an error if not
            if(!SubFolders.ContainsKey(key))
            {
                MessageBox.Show("Missing keybinding for <" + key.ToString() + ">", "Keybinding error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // do not advance as the user may want to enter a correct key or create a new one
                return false;
            }
            string targetname = SubFolders[key];
            // create the target path and try moving the file
            string completepath = Path.Combine(foldername, targetname, filename);
            try
            {
                // if there's a file with the same name in the target folder (for example due to the rename function).
                // try to create an automatic rename and offer it to the user
                if(File.Exists(completepath))
                {
                    string replacementname = GetFreeFileName(filename, Path.GetDirectoryName(completepath));
                    DialogResult replacefile = MessageBox.Show("File \""+filename+"\" already exists in folder. Do you want to use this name instead?\r\n"+replacementname, "File error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    // if user accepts, proceed with the move as normal
                    if (replacefile== DialogResult.Yes)
                    {
                        //build the new path
                        completepath = Path.Combine(foldername, targetname, replacementname);
                        try
                        {

                            File.Move(path, completepath);
                        }
                        //show an error message in case of failure
                        catch (Exception ex)
                        {
                            MessageBox.Show("Failed to move file.\r\n" + ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            //allow the system to advance in case of broken files
                            return true;
                        }
                    }
                    //else cancel the action
                    else
                    {

                        return false;
                    }
                }
                //if there is no file conflict, simply try to move the file
                else
                {
                    File.Move(path, completepath);
                }
            }
            //show an error message in case of failure
            catch(Exception ex)
            {
                MessageBox.Show("Failed to move file.\r\n"+ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //allow the system to advance in case of broken files
                return true;
            }
            //if nothing went wrong and the file was moved,
            //add an undo entry
            UndoStack.Push(new Tuple<string,string>(path, completepath));
            //return success - proceed to next image
            return true;
        }

        /// <summary>
        /// Create a new keybinding/folder combination with user prompt.
        /// </summary>
        /// <param name="keypress">The target keybinding.</param>
        bool CreateNewKey(char keypress)
        {
            //initialise the form
            CreateKeyBinding prompt = new CreateKeyBinding
            {
                Key = keypress,
                RootFolder = CurrentDir.FullName
            };
            //show the form
            prompt.ShowDialog();
            //if folder was selected successfully, add the binding
            if(prompt.DialogResult== DialogResult.OK)
            {
                SubFolders.Add(keypress, prompt.Folder);
            }
            //user cancelled the operation somehow
            else
            {
                return false;
            }
            //refresh the keybind display
            UpdateKeyBindList();
            //write the config
            SaveConfigFile(CurrentDir.FullName);
            return true;
        }
       
        private void MainFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if there's no queue initialised or
            if (Todo == null)
                return;
            // if keypress is not in the bindings list, check the lock setting
            if (!SubFolders.ContainsKey(e.KeyChar))
            {
                //if locked, display a message informing the user about the lock and cancel
                if(KeyLockState)
                {
                    MessageBox.Show("There is no folder registered for <" + e.KeyChar.ToString() + ">. New keybindings may only be created when the keys are unlocked, press Ctrl+L to toggle.", "Unrecognised key", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                //else try to create a new binding
                else
                {
                    //if key creation is cancelled, do nothing and return.
                    if(!CreateNewKey(e.KeyChar))
                    {
                        return;
                    }
                    //otherwise proceed further and attempt the move
                }
            }
            //move the file according to the key pressed
            if(MoveByKey(e.KeyChar, CurrentPath))
            {
                //get to the next item
                Advance();
            }
        }

        #region Menu Actions

        // Select a folder to process
        void OpenFolderCommand()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                OpenFolder(dlg.SelectedPath);
                Advance();
            }
        }

        /// <summary>
        /// Undoes a single file operation.
        /// </summary>
        void Undo()
        {
            // do nothing if there are no undos available
            if (UndoStack == null || UndoStack.Count == 0)
                return;

            // get the most recent operation
            Tuple<string, string> UndoEntry = UndoStack.Pop();
            // attempt to reverse the move
            try
            {
                File.Move(UndoEntry.Item2, UndoEntry.Item1);
            }
            // show an error message in case of failure
            catch (Exception ex)
            {
                MessageBox.Show("Undo failed!\r\n" + ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // no point in doing anything else
                return;
            }
            FolderProgress.Value += 1;
            // put the current file back into the queue
            Todo.AddFirst(CurrentPath);
            // put the recently undone file back into the queue and advance to it
            Todo.AddFirst(UndoEntry.Item1);
            Advance();
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
            // hold my Undo history...
            Stack<Tuple<string, string>> undoBackup = new Stack<Tuple<string, string>>();
            // shove it in there
            while (UndoStack.Count > 0)
            {
                undoBackup.Push(UndoStack.Pop());
            }
            //load the folder again
            OpenFolder(CurrentDir.FullName);
            //the load clears undo history, good thing we backed that up
            while (undoBackup.Count > 0)
            {
                UndoStack.Push(undoBackup.Pop());
            }
            //Extend the progress bar by undo history
            FolderProgress.Maximum += UndoStack.Count();
            Advance();

        }

        void RenameFile()
        {
            RenameFile prompt = new RenameFile();
            //get extension of current file
            prompt.FileExtension = Path.GetExtension(CurrentPath);
            DialogResult result = prompt.ShowDialog();
            if (result == DialogResult.OK)
            {
                NewFileName = prompt.FileName;
                RenamePreview.Text = NewFileName;
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

    }
}
