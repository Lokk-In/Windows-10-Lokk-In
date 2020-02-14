using Windows10LokkIn.Models;

namespace Windows10LokkIn.Interfaces
{
    public interface ILanguage
    {
        string Identifier { get; }

        void Apply(Language language);
    }
}
