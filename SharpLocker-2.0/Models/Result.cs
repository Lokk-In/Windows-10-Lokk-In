using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10LokkIn.Models
{
    public class Result
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string DomainName { get; set; }
        public string Password { get; set; }
        public List<string> WrongPasswords { get; set; } = new List<string>();
    }
}
