using Windows10LokkIn.Models;

namespace Windows10LokkIn.Interfaces
{
    /// <summary>
    /// Is called at startup. Allows to modify the programs configuration 
    /// </summary>
    public interface ISetup
    {
        void Initialize(Configuration configuration);
    }
}
