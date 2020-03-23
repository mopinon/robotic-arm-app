using System.Collections.Generic;

namespace Interfaces
{
    public interface IParser
    {
        List<IGCommand> GetCommands();
        List<ManipulatorScript.State> GetStateForComPort();
        string GetContentForSave();
        List<int> GetLineNumbers();
    }
}