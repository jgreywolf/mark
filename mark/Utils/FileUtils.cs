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
        public static FileStream getFileStream(string filePath, bool showDialogIfNotExists=true)
        {
            if (File.Exists(filePath))
                return new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);

            if (!showDialogIfNotExists)
                return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            DialogResult dialogResult = MessageBox.Show(
                "File does not exist. Would you like to create this file?",
                "MarkEditor", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
                return new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            return null;
        }
    }
}
