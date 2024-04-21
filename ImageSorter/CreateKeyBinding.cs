using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageSorter
{
    public partial class CreateKeyBinding : Form
    {
        public char Key { get; set; }
        public string Folder { get; set; }
        public string RootFolder { get; set; }
        public CreateKeyBinding()
        {
            InitializeComponent();
            Key = ' ';
        }

        
        private void CreateKeyBinding_Load(object sender, EventArgs e)
        {
            // show the keychar to assign a folder to
            if (Key != ' ')
                KeyCharLabel.Text = "<" + Key.ToString() + ">";
        }
        // try creating the folder and return the result on success
        private void OkButt_Click(object sender, EventArgs e)
        {
            // create the complete path to try using
            string fullpath = Path.Combine(RootFolder, folderInput.Text);
            try
            {
                Directory.CreateDirectory(fullpath);
            }
            // on failure, display error and stop, allowing the user to try a different directory
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create the directory specified: \r\n"+ex.Message, "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // set return values and close the box
            this.Folder = folderInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        // update the state of the OK button as the folder name changes
        private void folderInput_TextChanged(object sender, EventArgs e)
        {
            // only enable the button if the name is not emtpy and doesn't contain invalid characters
            if (folderInput.Text != "" || !ImageSorterSingleton.IsNameInvalid(folderInput.Text))
                OkButt.Enabled = true;
            else
                OkButt.Enabled = false;

        }
    }
}
