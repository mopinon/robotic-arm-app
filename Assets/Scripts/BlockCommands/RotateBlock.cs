using System;
using System.Collections.Generic;
using Classes;
using GCode.Commands;
using Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class RotateBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 7;

        [SerializeField] private TMP_Dropdown _rotationTypeInput;
        [SerializeField] private TMP_InputField _unitNumberInput, _angleInput, _speedInput;


        public Description GetBlock()
        {
            var description = new Description(GetCommandID(), GetParameters());
            return description;
        }

        public byte GetCommandID() => commandID;

        [Button]
        public void Test()
        {
            Debug.Log($"Params: {GetParameters().ToArray()}");
            Debug.Log($"Command: {GetCommand()}");
        }

        public List<int> GetParameters()
        {
            var parameters = new List<int>();
            try
            {
                parameters.Add(_rotationTypeInput.value);
                parameters.Add(Convert.ToInt32(_unitNumberInput.text));
                parameters.Add(Convert.ToInt32(_angleInput.text));
                parameters.Add(Convert.ToInt32(_speedInput.text));
            }
            catch (Exception e)
            {
                Debug.LogError("Неверный формат параметров");
                parameters.Add(0);
                parameters.Add(0);
                parameters.Add(0);
                parameters.Add(0);
            }
            return parameters;
        }

        public void SetParameters(List<int> parameters)
        {
            _rotationTypeInput.value = parameters[0];
            _unitNumberInput.text = parameters[1].ToString();
            _angleInput.text = parameters[2].ToString();
            _speedInput.text = parameters[3].ToString();
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();

            if (parameters[0] == 1)
            {
                var @params = new RotateRelative.Params();

                @params.UnitNumber = parameters[1];
                @params.Angle = parameters[2];
                @params.Time = parameters[3];
                @params.Target = FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(@params.UnitNumber).transform;

                return new RotateRelative(@params);
            }

            if (parameters[0] == 0)
            {
                var @params = new RotateAbsolute.Params();

                @params.UnitNumber = parameters[1];
                @params.Angle = parameters[2];
                @params.Time = parameters[3];
                @params.Target = FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(@params.UnitNumber).transform;
                return new RotateAbsolute(@params);
            }

            return new UnknownCommand();
        }
    }
}