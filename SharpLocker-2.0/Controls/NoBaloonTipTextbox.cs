using System.Windows.Forms;

namespace Windows10LokkIn.Controls
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
