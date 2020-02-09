using System;

namespace SharpLocker_2._0.Classes
{
    public class Configuration
    {
        private string placeholderText = "Password";
        private int minPasswordErrors = 1;
        private int maxPasswordErrors = 2;

        /// <summary>
        /// Enables or disables the debug mode
        /// </summary>
        public bool DebugMode { get; set; } = false;

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
        /// Value between >= 0 and <= MaxPasswordErrors.
        /// Min amount of entered passwords before logging in.
        /// Amount of needed password entries is randomly picked between min and max password erros.
        /// </summary>
        public int MinPasswordErrors
        {
            get
            {
                return minPasswordErrors;
            }
            set
            {
                if (value > maxPasswordErrors) throw new Exception("Min password errors can not be higher then max password errors.");
                if (value < 0) throw new Exception("Min password errors can not be lower then zero.");

                minPasswordErrors = value;
            }
        }

        /// <summary>
        /// Value between >= 0 and >= MinPasswordErrors.
        /// Max amount of entered passwords before logging in.
        /// Amount of needed password entries is randomly picked between min and max password erros.
        /// </summary>
        public int MaxPasswordErrors
        {
            get
            {
                return maxPasswordErrors;
            }
            set
            {
                if (value < minPasswordErrors) throw new Exception("Max password errors can not be lower then min password errors.");
                if (value < 0) throw new Exception("Max password errors can not be lower then zero.");

                maxPasswordErrors = value;
            }
        }

    }
}
