using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mark
{
    public partial class AboutForm : Form
    {
        private static AboutForm instance;
        
        private AboutForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        public static AboutForm GetInstance()
        {
            if (instance == null)
                instance = new AboutForm();
            return instance;
        }

        private void AboutForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance = null;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/intfrog/mark");
        }
    }
}
