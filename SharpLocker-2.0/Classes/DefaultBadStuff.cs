using Windows10LokkIn.Models;

namespace Windows10LokkIn.Classes
{
    /// <summary>
    /// Is called when the login button is pressed and now custom interfaces was loaded
    /// </summary>
    public class DefaultBadStuff : Interfaces.IDoBadStuff
    {
        public void Now(Result result)
        {
            // Do your malicious (or non malicious) activity here
        }
    }
}
