using System;
using System.Collections.Generic;
using GCode.Commands;
using Interfaces;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class ClawOpenPercentBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 2;
        [SerializeField] private TMP_InputField _percent;

        public Description GetBlock()
        {
            var description = new Description(GetCommandID(), GetParameters());
            return description;
        }

        public byte GetCommandID() => commandID;

        public List<int> GetParameters()
        {
            var parameters = new List<int>();
            try
            {
                parameters.Add(Convert.ToInt32(_percent.text));
            }
            catch (Exception e)
            {
                parameters.Add(0);
                Debug.LogError("Неверный формат параметров");
            }


            return parameters;
        }

        public void SetParameters(List<int> parameters)
        {
            _percent.text = parameters[0].ToString();
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();
            var command = new ClawOpenPercent {Percent = parameters[0], Time = 1};

            return command;
        }
    }
}