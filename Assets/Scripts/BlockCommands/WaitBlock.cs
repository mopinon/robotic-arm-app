using System;
using System.Collections.Generic;
using GCode.Commands;
using Interfaces;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class WaitBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 8;
        [SerializeField] private TMP_InputField _time;
        [SerializeField] private TMP_Dropdown _timeType;

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
                parameters.Add(Convert.ToInt32(_time.text));
                parameters.Add(_timeType.value);
            }
            catch (Exception e)
            {
                parameters.Add(0);
                parameters.Add(0);
                Debug.LogError("Неверный формат параметров");
            }

            return parameters;
        }

        public void SetParameters(List<int> parameters)
        {
            _time.text = parameters[0].ToString();
            _timeType.value = parameters[1];
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();
            var @params = new Wait.Params();

            if (parameters[1] == 0) @params.Time = parameters[0];
            else @params.Time = parameters[0] * 1000;


            return new Wait(@params);
        }
    }
}