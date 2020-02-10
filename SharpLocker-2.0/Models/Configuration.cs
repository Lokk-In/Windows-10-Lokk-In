using System;

namespace SharpLocker_2._0.Models
{
    public class Configuration
    {
        private string placeholderText = "Password";
        private int minPasswordErrors = 1;
        private int maxPasswordErrors = 2;
        private int blur = 10;

        /// <summary>
        /// Enables or disables the debug mode
        /// </summary>
        public bool DebugMode { get; set; } = false;

        /// <summary>
        /// Changes amount of baclground blur
        /// </summary>
        public int BlurIntensity
        {
            get
            {
                return blur;
            }
            set
            {
                if (value <= 0) throw new Exception("Blur intensity must be 1 or higher");

                blur = value;
            }

        }

        /// <summary>
        /// Sets how a bright wallpaper will be dimmed
        /// </summary>
        public int DarknessIntensity { get; set; } = -80;

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
