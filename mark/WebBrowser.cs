using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace mark
{
    class WebBrowser : System.Windows.Forms.WebBrowser
    {
        public override bool PreProcessMessage(ref Message msg)
        {
            // disable some shortcut keys
            if (msg.WParam.ToInt32() == (int)Keys.F5)
                return false;
            if ((Control.ModifierKeys & Keys.Control) == Keys.Control && msg.WParam.ToInt32() == (int)Keys.P)
                return false;
            return base.PreProcessMessage(ref msg);
        }
    }
}
