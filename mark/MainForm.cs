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
using System.Drawing.Imaging;
using System.Collections.Specialized;

namespace mark
{
    public partial class MainForm : Form
    {
        private const bool DEBUG = true;
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
            this.currentFilePath = "Untitled";
            this.currentFileStream = null;
            this.currentFileReader = null;
            this.currentFileWriter = null;
            this.currentFileContent = null;
            this.Text = "MarkEditor - " + this.currentFilePath;
        }

        public void updatePreview()
        {
            String html = MarkProcessor.ToHtml(richTextBox1.Text, currentDir);
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

        private void paste()
        {
            /*MessageBox.Show("Image: " + Clipboard.ContainsImage()
                + "\nFileDropList: " + Clipboard.ContainsFileDropList()
                + "\nText: " + Clipboard.ContainsText()
                + "\nAudio: " + Clipboard.ContainsAudio());*/
            if (Clipboard.ContainsImage())
            {
                Image img = Clipboard.GetImage();
                string imgDir = currentDir + "/_image/";
                string imgName = "a.jpg";
                if (!Directory.Exists(imgDir))
                    Directory.CreateDirectory(imgDir);

                img.Save(imgDir + imgName, ImageFormat.Jpeg);
                Clipboard.SetText("![](./_image/"+imgName+")");
                richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
                Clipboard.SetImage(img);
            }
            else if (Clipboard.ContainsFileDropList())
            {
                StringCollection files = Clipboard.GetFileDropList();
                string attachmentDir = currentDir + "/_attachment/";
                if (!Directory.Exists(attachmentDir))
                    Directory.CreateDirectory(attachmentDir);

                foreach (string file in files)
                {
                    string fileName = Path.GetFileName(file).Replace(" ", "");
                    File.Copy(file, attachmentDir + fileName);
                    Clipboard.SetText("["+fileName+"](./_attachment/"+fileName+")");
                    richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
                }
                Clipboard.SetFileDropList(files);
            }
            else
            {
                richTextBox1.Paste(DataFormats.GetFormat(DataFormats.UnicodeText));
            }
        }


        /* ========================================= UI ========================================== */
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            updatePreview();
            updateUnsavedStatus();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1200, 650);
            this.Location = new Point(90, 30);
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            MarkProcessor.Initialize();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DEBUG) return;
            if (!this.Text.EndsWith("*")) return;
            DialogResult dialogResult = MessageBox.Show(
                "Save changes to\"" + currentFilePath + "\"?",
                "MarkEditor",
                MessageBoxButtons.YesNoCancel);
            if (dialogResult == DialogResult.Yes)
            {
                save();
            }
            else if (dialogResult == DialogResult.No)
            {
                // do nothing
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                paste();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paste();
        }
    }
}
