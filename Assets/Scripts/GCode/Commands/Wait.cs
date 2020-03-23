using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class Wait : IGCommand
    {
        private Params _parameters;

        public Wait(Params @params)
        {
            _parameters = @params;
        }

        public struct Params
        {
            /// <summary>
            /// Waiting time in milliseconds
            /// </summary>
            public int Time;

            internal int GetMilliseconds() => Time;
            internal float GetSeconds() => Time / 1000f;
        }

        public void SetTime(int time)
        {
            _parameters.Time = time;
        }

        public IEnumerator Execute()
        {
            yield return new WaitForSeconds(_parameters.GetSeconds());
        }

        public byte GetCommandID()
        {
            return 222;
        }

        public int GetTime()
        {
            return _parameters.Time;
        }
    }
}