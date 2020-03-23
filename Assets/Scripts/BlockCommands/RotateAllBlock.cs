using System;
using System.Collections.Generic;
using System.Linq;
using Classes;
using GCode.Commands;
using Interfaces;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace BlockCommands
{
    public class RotateAllBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 6;

        [SerializeField] private TMP_Dropdown _rotationTypeInput;
        [SerializeField] private TMP_InputField[] _anglesInput;
        [SerializeField] private TMP_InputField _speedInput;

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
                parameters.AddRange(_anglesInput.Select(angle => Convert.ToInt32(angle.text)));
                parameters.Add(Convert.ToInt32(_speedInput.text));
            }
            catch (Exception e)
            {
                Debug.LogError("Неверный формат параметров");
                parameters.Add(0);
                parameters.AddRange(_anglesInput.Select(angle => 0));
                parameters.Add(0);
            }


            return parameters;
        }

        public void SetParameters(List<int> parameters)
        {
            _rotationTypeInput.value = parameters[0];
            for (var i = 0; i < _anglesInput.Length; i++)
                _anglesInput[i].text = parameters[i + 1].ToString();
            _speedInput.text = parameters.Last().ToString();
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();

            if (parameters[0] == 1)
            {
                var @params = new RotateAllRelative.Params(
                    new Transform[] {null, null, null, null, null},
                    new[] {0f, 0f, 0f, 0f, 0f});

                @params.Angle = new[] {0f, 0f, 0f, 0f, 0f};
                for (var i = 0; i < 5; i++)
                    @params.Angle[i] = parameters[i + 1];

                @params.Time = parameters[6];

                for (var i = 0; i < 5; i++)
                    @params.Target[i] = FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(i).transform;

                return new RotateAllRelative(@params);
            }

            if (parameters[0] == 0)
            {
                var @params = new RotateAllAbsolute.Params(
                    new Transform[] {null, null, null, null, null},
                    new[] {0f, 0f, 0f, 0f, 0f});

                for (var i = 0; i < 5; i++)
                    @params.Angle[i] = parameters[i + 1];

                @params.Time = parameters[6];

                for (var i = 0; i < 5; i++)
                    @params.Target[i] = FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(i).transform;

                return new RotateAllAbsolute(@params);
            }

            return new UnknownCommand();
        }
    }
}