using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mark.Processor;

namespace mark
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            String html = MarkProcessor.ToHtml(richTextBox1.Text);
            webBrowser1.DocumentText = html;
        }
    }
}
