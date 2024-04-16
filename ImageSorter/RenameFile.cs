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
    public partial class RenameFile : Form
    {
        public string FileName {get;set; }
        public string FileExtension { get; set; }
        public RenameFile()
        {
            InitializeComponent();
        }

        private void OKButt_Click(object sender, EventArgs e)
        {
            this.FileName = FileInput.Text+ExtBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FileInput_TextChanged(object sender, EventArgs e)
        {
            //only enable the button if the name is not emtpy and doesn't contain invalid characters
            if (FileInput.Text != "" || !MainFrm.IsNameInvalid(FileInput.Text))
                OKButt.Enabled = true;
            else
                OKButt.Enabled = false;
        }
        //clears the filename
        private void clearbutt_Click(object sender, EventArgs e)
        {
            this.FileName = "";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        //enable editing of the extension if requested here
        private void ExtBox_DoubleClick(object sender, EventArgs e)
        {
            ExtBox.Enabled = true;
        }

        private void RenameFile_Load(object sender, EventArgs e)
        {
            //should probably also load the filename at some point or something
            ExtBox.Text = FileExtension;
        }
    }
}
