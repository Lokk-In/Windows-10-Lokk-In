using Windows10LokkIn.Models;

namespace Windows10LokkIn.Interfaces
{
    /// <summary>
    /// Is called after the login button as been successfuly pressed 
    /// </summary>
    public interface IDoBadStuff
    {
        void Now(Result result);
    }
}
