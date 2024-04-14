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
        //stores the file currently operated on
        string CurrentPath;
        //used to store the current directory
        DirectoryInfo CurrentDir;
        //queue to progress through the folder
        Queue<string> Todo;
        //this is the dictionary that will hold all the keybindings
        Dictionary<char, string> SubFolders;
        //hardcoding the image extensions here for now.
        string[] extensions = { ".png",".jpg",".gif",".jpeg",".webp" };

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
                //clear the image preview
                ImageContainer.Image = null;
                //tell the user
                MessageBox.Show("This folder has no more images.", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            //fetch the next item and update current and progress bar
            string FilePath = Todo.Dequeue();
            CurrentPath = FilePath;
            FolderProgress.Value = Todo.Count() + 1;
            //useful to display current path, maybe change it to a display box somewhere
            this.Text = CurrentPath;
            //try displaying the image
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
            SubFolders.Add(' ', "nsfw");
            UpdateKeyBindList();
        }
        //Select a folder to process
        private void openFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            DialogResult result = dlg.ShowDialog();
            if(result== DialogResult.OK)
            {
                OpenFolder(dlg.SelectedPath);
                Advance();
            }


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
            Todo = new Queue<string>();
            foreach(string filepath in FileNames)
            {
                Todo.Enqueue(filepath);
            }
            //set the progress bar
            FolderProgress.Maximum = FileNames.Count();
            //create the default NSFW folder
            string nsfwfolderpath = Path.Combine(path, "nsfw");
            try
            {
                Directory.CreateDirectory(nsfwfolderpath);
            }
            //usually this only fails if there is a file named "nsfw" exactly, in which case, try a random name
            catch(Exception ex)
            {
                string newrandomstring = Path.GetRandomFileName();

                try
                {
                    Directory.CreateDirectory(Path.Combine(path, "nsfw_"+newrandomstring));
                }
                //either something else went wrong, or we're astronomically unlucky - display error and exit
                catch(Exception ex2)
                {
                    MessageBox.Show("Could not create NSFW folder. The program will now exit. See if the below information helps clarify the cause of the issue:\r\n" + ex2.Message, "We tried", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }

            }

            UpdateKeyBindList();
        }
        /// <summary>
        /// Moves a file located at a specified path, triggered by a keypress.
        /// </summary>
        /// <param name="key">Triggering keypress</param>
        /// <param name="path">Path of the file to be moved</param>
        /// <returns>true if the file was moved, false otherwise</returns>
        bool MoveByKey(char key,string path)
        {
            //get the parent dir
            string foldername = Path.GetDirectoryName(path);
            //get the filename
            string filename = Path.GetFileName(path);
            //get the correct target subfolder based on the keypress
            //check if there's a folder defined for the key and display an error if not
            if(!SubFolders.ContainsKey(key))
            {
                MessageBox.Show("Missing keybinding for <"+key.ToString()+">", "Keybinding error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            string targetname = SubFolders[key];
            //create the target path and try moving the file
            string completepath = Path.Combine(foldername, targetname, filename);
            try
            {
                File.Move(path, completepath);
            }
            //show an error message in case of failure
            catch(Exception ex)
            {
                MessageBox.Show("Failed to move file.\r\n"+ex.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //if nothing went wrong and the file was moved, confirm success
            return true;
        }
        /// <summary>
        /// Create a new keybinding/folder combination with user prompt.
        /// </summary>
        /// <param name="keypress">The target keybinding.</param>
        void CreateNewKey(char keypress)
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
                SubFolders.Add(keypress, prompt.Folder);
            UpdateKeyBindList();
        }
       
        private void MainFrm_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if there's no queue initialised or
            if (Todo == null)
                return;
            //if there's nothing in the queue, do nothing
            if (Todo.Count < 1)
                return;
            //if keypress is not in the bindings list and the Lock setting is not on, create a new binding
            if (!SubFolders.ContainsKey(e.KeyChar) && !lockKeysToolStripMenuItem.Checked)
            {
                CreateNewKey(e.KeyChar);
            }
            //move the file according to the key pressed
            MoveByKey(e.KeyChar, CurrentPath);
            //get to the next item
            Advance();
        }

        private void lockKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //basically a toggle
            lockKeysToolStripMenuItem.Checked = !lockKeysToolStripMenuItem.Checked;
        }
    }
}
