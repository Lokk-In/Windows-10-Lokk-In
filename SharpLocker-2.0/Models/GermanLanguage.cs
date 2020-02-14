using Windows10LokkIn.Interfaces;
using System;

namespace Windows10LokkIn.Models
{
    public class GermanLanguage : ILanguage
    {
        public string Identifier => "DEU";

        public void Apply(Language language)
        {
            language.CapsLockText = "FESTSTELLTASTE ist aktiviert";
            language.OtherUserText = "Anderer Benutzer";
            language.PlaceholderText = "Kennwort";
            language.WrongPasswordText = @"Das Kennwort ist falsch. Stellen Sie sicher, dass Sie das Kennwort für Ihr Microsoft-Konto verwenden. Sie können das Kennwort jederzeit unter 
account.live.com/password/reset zurücksetzen.";
            language.Keyboard = "Tastatur";
        }
    }
}
