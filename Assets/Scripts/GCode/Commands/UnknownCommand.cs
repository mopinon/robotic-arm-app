using System.Collections;
using Interfaces;

namespace GCode.Commands
{
    public class UnknownCommand : IGCommand
    {
        private int Time;

        public void SetTime(int time)
            => Time = time;

        public IEnumerator Execute()
        {
            // var inputField = GameObject.Find("InputFieldGcode").GetComponent<TMP_InputField>();
            // if (!inputField.text.Contains("Unknown command"))
            //     inputField.text = "Unknown command" + '\n' + inputField.text;
            yield return null;
        }

        public byte GetCommandID()
        {
            return 0;
        }

        public int GetTime()
            => Time;
    }
}