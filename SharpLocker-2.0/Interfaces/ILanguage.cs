using SharpLocker_2._0.Models;

namespace SharpLocker_2._0.Interfaces
{
    interface ILanguage
    {
        string Identifier { get; }

        void Apply(Language language);
    }
}
