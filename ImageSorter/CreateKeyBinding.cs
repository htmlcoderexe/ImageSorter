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
            if (Key != ' ')
                KeyCharLabel.Text = Key.ToString();
        }

        private void OkButt_Click(object sender, EventArgs e)
        {
            string fullpath = Path.Combine(RootFolder, folderInput.Text);
            try
            {
                Directory.CreateDirectory(fullpath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to create the directory specified", "Directory error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Folder = folderInput.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void folderInput_TextChanged(object sender, EventArgs e)
        {
            if (folderInput.Text != "" ||!MainFrm.IsNameInvalid(folderInput.Text))
                OkButt.Enabled = true;
            else
                OkButt.Enabled = false;

        }
    }
}
