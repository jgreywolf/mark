using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using mark.Processor;
using mark.Utils;

namespace mark
{
    public partial class MainForm : Form
    {
        private readonly string currentDir = Environment.CurrentDirectory;

        private string currentFilePath;
        private FileStream currentFileStream;
        private StreamReader currentFileReader;
        private StreamWriter currentFileWriter;
        private string currentFileContent;

        public MainForm(string[] args)
        {
            InitializeComponent();
            if (args.Length == 0)
                init();
            else
                init(args[0]);
        }

        private void init(string filePath)
        {
            if (!Path.IsPathRooted(filePath))
                filePath = Path.GetFullPath(Path.Combine(currentDir, filePath));
            try
            {
                currentFileStream = FileUtils.getFileStream(filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the file");
                this.init();
                return;
            }
            if (currentFileStream == null)
            {
                this.init();
                return;
            }
            this.currentFilePath = filePath;
            this.currentFileReader = new StreamReader(currentFileStream);
            this.currentFileWriter = new StreamWriter(currentFileStream);
            this.currentFileContent = this.currentFileReader.ReadToEnd();

            this.Text = "MarkEditor - " + this.currentFilePath;
            this.richTextBox1.Text = this.currentFileContent;
            this.updatePreview();
        }

        private void init()
        {
            this.currentFilePath = null;
            this.currentFileStream = null;
            this.currentFileReader = null;
            this.currentFileWriter = null;
            this.currentFileContent = null;
            this.Text = "MarkEditor - " + "Untitled";
        }

        public void updatePreview()
        {
            String html = MarkProcessor.ToHtml(richTextBox1.Text);
            webBrowser1.DocumentText = html;
        }

        public void updateUnsavedStatus()
        {
            if (!richTextBox1.Text.Equals(currentFileContent))
            {
                if (!this.Text.EndsWith("*"))
                    this.Text += "*";
            }
            else
            {
                if (this.Text.EndsWith("*"))
                    this.Text = this.Text.TrimEnd('*');
            }
        }

        public void save()
        {
            if (currentFileStream == null)
            {
                MessageBox.Show("TODO: Show save file dialog");
            }
            else
            {
                currentFileStream.SetLength(0);
                currentFileWriter.Write(richTextBox1.Text);
                currentFileWriter.Flush();
                this.currentFileContent = richTextBox1.Text;
                updateUnsavedStatus();
            }
        }


        /* ================================ UI ============================== */
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            updatePreview();
            updateUnsavedStatus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }
    }
}
