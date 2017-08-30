using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace mark.Utils
{
    class FileUtils
    {
        public static StreamWriter getStreamWriter(string filePath)
        {
            if (File.Exists(filePath))
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);
                return new StreamWriter(fs);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show(
                    "File does not exist. Would you like to create this file?",
                    "MarkEditor", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    return new StreamWriter(fs);
                }
            }
            return null;
        }
    }
}
