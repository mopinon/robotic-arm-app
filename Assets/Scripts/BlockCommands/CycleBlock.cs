using System;
using System.Collections.Generic;
using GCode.Commands;
using Interfaces;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class CycleBlock : CodeBlock, ICodeBlock
    {
        [SerializeField] private TMP_InputField times;
        public Description GetBlock()
        {
            var description = new Description(GetCommandID(), GetParameters());
            return description;
        }

        public byte GetCommandID() => 3;

        public List<int> GetParameters()
        {
            var parameters = new List<int>();
            try
            {
                parameters.Add(Convert.ToInt32(times.text));
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
            times.text = parameters[0].ToString();
        }

        public IGCommand GetCommand() => new Cycle();
    }
}