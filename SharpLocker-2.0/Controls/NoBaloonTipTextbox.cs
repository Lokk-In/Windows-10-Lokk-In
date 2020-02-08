using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpLocker_2._0.Controls
{
    public class NoBaloonTipTextbox: TextBox
    {
        protected override void WndProc(ref Message m)
        {
            // supress baloon tip
            if (m.Msg != 0x1503) //EM_SHOWBALOONTIP
                base.WndProc(ref m);
        }
    }
}
