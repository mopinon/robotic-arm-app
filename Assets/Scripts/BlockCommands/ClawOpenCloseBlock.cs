using System;
using System.Collections.Generic;
using GCode.Commands;
using Interfaces;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class ClawOpenCloseBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 1;
        [SerializeField] private TMP_Dropdown isClose;

        public Description GetBlock()
        {
            var description = new Description(GetCommandID(), GetParameters());
            return description;
        }

        public byte GetCommandID()
        {
            return commandID;
        }

        public List<int> GetParameters()
        {
            var parameters = new List<int>();
            parameters.Add(Convert.ToInt32(isClose.value));

            return parameters;
        }

        public void SetParameters(List<int> parameters)
        {
            isClose.value = parameters[0];
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();
            var command = new ClawOpenPercent();
            if (parameters[0] == 0)
                command.Percent = 100;
            else
                command.Percent = 0;
            command.Time = 1;

            return command;
        }
    }
}