using System.Collections;
using Interfaces;
using Microsoft.Win32;

namespace GCode.Commands
{
    public class EndCycle : IGCommand
    {
        private int Time;
        public void SetTime(int time) => Time = time;

        public IEnumerator Execute()
        {
            yield return null;
        }

        public byte GetCommandID() => 144;

        public int GetTime() => Time;
    }
}