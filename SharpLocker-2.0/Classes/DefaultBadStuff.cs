using System.Collections.Generic;

namespace SharpLocker_2._0.Classes
{
    public class DefaultBadStuff : Interfaces.IDoBadStuff
    {
        public void Now(string password, string username, string domainName, List<string> wrongPasswords)
        {
            // Do your malicious (or non malicious) activity here
        }
    }
}
