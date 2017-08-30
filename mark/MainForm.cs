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
        private StreamWriter currentFileWriter;

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
                currentFileWriter = FileUtils.getStreamWriter(filePath);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to open the file");
                this.init();
                return;
            }
            if (currentFileWriter == null)
            {
                this.init();
                return;
            }
            this.currentFilePath = filePath;
            this.Text = "MarkEditor - " + this.currentFilePath;
        }

        private void init()
        {
            this.currentFilePath = null;
            this.currentFileWriter = null;
            this.Text = "MarkEditor - " + "Untitled";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            String html = MarkProcessor.ToHtml(richTextBox1.Text);
            webBrowser1.DocumentText = html;
        }
    }
}
