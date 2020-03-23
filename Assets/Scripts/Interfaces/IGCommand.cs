using System.Collections;

namespace Interfaces
{
    public interface IGCommand
    {
        void SetTime(int time);
        IEnumerator Execute();
        byte GetCommandID();
        int GetTime();
    }
}