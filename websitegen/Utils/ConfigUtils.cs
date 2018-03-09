using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace websitegen.Utils
{
    class ConfigUtils
    {
        private string fileName;
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public ConfigUtils(string fileName)
        {
            this.fileName = fileName;
            foreach (var row in File.ReadAllLines(fileName))
                dict.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));
        }

        public string get(string key)
        {
            return dict[key];
        }
    }
}
