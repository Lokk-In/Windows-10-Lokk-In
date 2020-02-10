using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpLocker_2._0.Models
{
    public class Language
    {
        private string placeholderText = "Password";

        /// <summary>
        /// The text that is displayed as placeholder.
        /// If null or empty the default value is used
        /// </summary>
        public string PlaceholderText
        {
            get
            {
                return placeholderText;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    placeholderText = "Password";
                }
                else
                {
                    placeholderText = value;
                }
            }
        }

        /// <summary>
        /// The text that is displayed if a wrong password is entered
        /// </summary>
        public string WrongPasswordText { get; set; }
        
        /// <summary>
        /// The text of the other user button
        /// </summary>
        public string OtherUserText { get; set; }

        /// <summary>
        /// The text that is displayed when caps lock is activated
        /// </summary>
        public string CapsLockText { get; set; }
    }
}
