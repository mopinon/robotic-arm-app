using System.Collections;
using Interfaces;

namespace GCode.Commands
{
    
    public class Cycle : IGCommand
    {
        private int Time;
        public void SetTime(int time) => Time = time;

        public IEnumerator Execute()
        {
            yield return null;
        }

        public byte GetCommandID() => 83;

        public int GetTime() => Time;
    }
}