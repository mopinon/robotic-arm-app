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
    public class MoveToPointBlock : CodeBlock, ICodeBlock
    {
        private byte commandID = 5;

        [SerializeField] private TMP_Dropdown _rotationTypeInput;
        [SerializeField] private TMP_InputField _xInput, _yInput, _zInput, _speedInput;

        private void Start()
        {
            var commands = transform.GetComponentsInChildren<IGCommand>();
        }

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

        //TODO: Добавить проверку на то, существуют ли значения в интерфейсе
        public List<int> GetParameters()
        {
            var parameters = new List<int>();

            try
            {
                parameters.Add(_rotationTypeInput.value);
                parameters.Add(Convert.ToInt32(_xInput.text));
                parameters.Add(Convert.ToInt32(_yInput.text));
                parameters.Add(Convert.ToInt32(_zInput.text));
                parameters.Add(Convert.ToInt32(_speedInput.text));
            }
            catch (Exception e)
            {
                Debug.LogError("Неверный формат параметров");
                parameters.Add(0);
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
            _xInput.text = parameters[1].ToString();
            _yInput.text = parameters[2].ToString();
            _zInput.text = parameters[3].ToString();
            _speedInput.text = parameters[4].ToString();
        }

        public IGCommand GetCommand()
        {
            var parameters = GetParameters();

            if (parameters[0] == 1)
            {
                var @params = new MoveToPointRelative.Params();

                @params.FinalPosition.x = parameters[1];
                @params.FinalPosition.y = parameters[2];
                @params.FinalPosition.z = parameters[3];
                @params.Time = parameters[4];
                var man = FindObjectOfType<Manipulator.Manipulator>();
                @params.Target = man.GetUnitAt(0).transform;
                @params.Tool = man.GetUnitAt(8).transform;
                @params.Point = man.GetUnitAt(9).transform;

                return new MoveToPointRelative(@params);
            }

            if (parameters[0] == 0)
            {
                var @params = new MoveToPointAbsolute.Params();

                @params.FinalPosition.x = parameters[1];
                @params.FinalPosition.y = parameters[2];
                @params.FinalPosition.z = parameters[3];
                @params.Time = parameters[4];
                var man = FindObjectOfType<Manipulator.Manipulator>();
                @params.Target = man.GetUnitAt(0).transform;
                @params.Tool = man.GetUnitAt(8).transform;
                @params.Point = man.GetUnitAt(9).transform;

                return new MoveToPointAbsolute(@params);
            }

            return new UnknownCommand();
        }
    }
}