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
        /**
         * Set true to avoid file saving check before closing the window
         *
         */
        private const bool DEBUG = false;

        private readonly string startupPath = Application.StartupPath;
        private readonly string currentDir = Environment.CurrentDirectory;

        /**
         * Only when initialized variable is true, will richTextBox's textChanged
         * event trigger updating of preview screen. This variable will be set true
         * after the Form is shown.
         * 
         */
        private bool initialized = false;
        // whether to show preview on startup
        private bool showPreviewOnStartup = true;
        // css style
        private string style;
        
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

            if (File.Exists(startupPath + "/style.css"))
                this.style = File.ReadAllText(startupPath + "/style.css");
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
            
            this.richTextBox1.Text = this.currentFileContent;
            this.Text = "MarkEditor - " + this.currentFilePath;
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
            string html = MarkProcessor.ToHtml(richTextBox1.Text, currentDir);
            if (style != null)
                html = "<style>\n" + style + "\n</style>\n" + html;
            html = "<html><body style=\"background: white\">\n" + html + "\n</body></html>";
            webBrowser1.DocumentText = html;
        }

        private void updateUnsavedStatus()
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

        private void save()
        {
            if (currentFileStream == null)
            {
                saveFileDialog1.Filter = "Markdown (*.md)|*.md|Markdown (*.markdown)|*.markdown"
                    + "|All files (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog1.FileName;
                    try
                    {
                        currentFileStream = FileUtils.getFileStream(currentFilePath, false);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                        currentFilePath = null;
                        return;
                    }
                    currentFileReader = new StreamReader(currentFileStream);
                    currentFileWriter = new StreamWriter(currentFileStream);

                    currentFileStream.SetLength(0);
                    currentFileWriter.Write(richTextBox1.Text);
                    currentFileWriter.Flush();
                    this.currentFileContent = richTextBox1.Text;
                    updateUnsavedStatus();

                    this.Text = "MarkEditor - " + currentFilePath;
                }
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
                string imgName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".jpg";
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
                    File.Copy(file, attachmentDir + fileName, true);
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

        private void dragDrop(string[] fileList)
        {
            foreach (string file in fileList)
            {
                if (FileUtils.isImage(file))
                {
                    string imgDir = currentDir + "/_image/";
                    string imgName = Path.GetFileName(file).Replace(" ", "");
                    if (!Directory.Exists(imgDir))
                        Directory.CreateDirectory(imgDir);

                    File.Copy(file, imgDir + imgName, true);
                    richTextBox1.SelectedText = "![](./_image/" + imgName + ")";
                }
                else
                {
                    string attachmentDir = currentDir + "/_attachment/";
                    string fileName = Path.GetFileName(file).Replace(" ", "");
                    if (!Directory.Exists(attachmentDir))
                        Directory.CreateDirectory(attachmentDir);

                    File.Copy(file, attachmentDir + fileName, true);
                    richTextBox1.SelectedText = "[" + fileName + "](./_attachment/" + fileName + ")";
                }
            }
        }

        private void preview()
        {
            splitContainer1.Panel2Collapsed = !splitContainer1.Panel2Collapsed;
        }


        /* ========================================= UI ========================================== */
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (initialized)
            {
                updatePreview();
                updateUnsavedStatus();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Size = new Size(1200, 650);
            this.Location = new Point(90, 30);
            splitContainer1.Panel2Collapsed = true;
            webBrowser1.DocumentText = "<html><body style=\"background: white\">\n\n</body></html>";
            richTextBox1.SelectionIndent = 10;
            richTextBox1.SelectionRightIndent = 2;
            richTextBox1.DragDrop += new DragEventHandler(richTextBox1_DragDrop);
            richTextBox1.AllowDrop = true;
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (!initialized)
            {
                updatePreview();
                initialized = true;
            }
            if (showPreviewOnStartup)
                splitContainer1.Panel2Collapsed = false;
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
                if (currentFileStream != null)
                    currentFileStream.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                if (currentFileStream != null)
                    currentFileStream.Close();
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
            preview();
        }

        private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                richTextBox1.SelectedText = "    ";
                e.SuppressKeyPress = true;
            }
            else if (e.Modifiers == Keys.Control && e.KeyCode == Keys.V)
            {
                paste();
            }
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            object filename = e.Data.GetData("FileDrop");
            if (filename == null) return;
            string[] fileList = filename as string[];
            if (fileList == null) return;
            this.dragDrop(fileList);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            paste();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(richTextBox1.SelectedText);
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preview();
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            int scrollHeight = webBrowser1.Document.Body.ScrollRectangle.Height;
            webBrowser1.Document.Window.ScrollTo(0, scrollHeight);
        }
    }
}
