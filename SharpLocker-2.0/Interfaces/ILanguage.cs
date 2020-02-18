using Windows10LokkIn.Models;

namespace Windows10LokkIn.Interfaces
{
    /// <summary>
    /// Is called after the program starts and allows to add custom languages to the program
    /// </summary>
    public interface ILanguage
    {
        /// <summary>
        /// Three digit windows language key.
        /// E.g.: ENG, DEU
        /// </summary>
        string Identifier { get; }

        /// <summary>
        /// Allows to change the text of the current language
        /// </summary>
        /// <param name="language"></param>
        void Apply(Language language);
    }
}
