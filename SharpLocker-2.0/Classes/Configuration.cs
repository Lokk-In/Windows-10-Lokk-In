namespace SharpLocker_2._0.Classes
{
    public class Configuration
    {
        private string placeholderText = "Password";

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

    }
}
