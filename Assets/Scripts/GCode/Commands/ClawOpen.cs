using System.Collections;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class ClawOpen : IGCommand
    {
        public int Time;
        public void SetTime(int time)
        {
            Time = time;
        }

        public IEnumerator Execute()
        {
            var claw = Object.FindObjectOfType<Manipulator.Manipulator>().Claw.GetComponent<IClaw>();
            return claw.OpenToPercent(100, Time);
        }

        public byte GetCommandID()
        {
            return 66;
        }

        public int GetTime()
        {
            return Time;
        }
    }
}