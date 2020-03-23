using System.Collections.Generic;
using System.Linq;
using Classes;
using GCode.Commands;
using Interfaces;
using Omega.Routines;
using TMPro;
using UI.CodeEditor.Highlighting;
using UnityEngine;

namespace Parsers
{
    public class GCodeParser : MonoBehaviour, IParser
    {
        [SerializeField] public TMP_InputField inputField;
        public List<int> lineIndex = new List<int>();
        private List<string> unpack;
        public List<IGCommand> GetCommands()
        {
            var igcommands = new List<IGCommand>();
            lineIndex = new List<int>();
            var text = inputField.text;
            text = NormalizationCommandLines(text);
            var commandLines = UnpackCycles(text);
            unpack = commandLines;
            for (var i = 0; i < commandLines.Count; i++)
            {
                if (commandLines[i] != "\n")
                {
                    var s = commandLines[i];
                    igcommands.Add(Resolve(s));
                }
            }

            return igcommands;
        }

        private string NormalizationCommandLines(string text)
        {
            var commandLines = text;
            while (commandLines.Contains("  "))
                commandLines = commandLines.Replace("  ", " ");
            // while (commandLines.Contains("\n\n"))
            //     commandLines = commandLines.Replace("\n\n", "\n");
            while (commandLines.Contains("\n "))
                commandLines = commandLines.Replace("\n ", "\n");
            commandLines = commandLines.Trim(' ');
            return commandLines;
        }

        private string NextCommand(ref string commandLine)
        {
            var firstSpaceIndex = commandLine.IndexOf(' ');
            if (firstSpaceIndex != -1)
            {
                var commandArgument = commandLine.Substring(0, firstSpaceIndex);
                commandLine = commandLine.Substring(firstSpaceIndex + 1);
                return commandArgument;
            }
            else
            {
                return commandLine;
            }
        }

        private IGCommand Resolve(string commandLine)
        {
            var commandKey = NextCommand(ref commandLine);

            switch (commandKey)
            {
                case "GripperOpenPercent":
                {
                    var operation = new ClawOpenPercent();
                    var command = NextCommand(ref commandLine);
                    var isPercent = int.TryParse(command, out operation.Percent);
                    operation.Time = 1;
                    if (isPercent)
                        return operation;

                    return new UnknownCommand();
                }
                case "GripperOpen":
                {
                    var operation = new ClawOpenPercent {Percent = 100, Time = 1};
                    return operation;
                }
                case "GripperClose":
                {
                    var operation = new ClawOpenPercent {Percent = 0, Time = 1};
                    return operation;
                }
                case "RotateAllAbsolute":
                {
                    var parameters = new RotateAllAbsolute.Params();

                    string command;
                    var isAngle = true;
                    parameters.Angle = new float[] {0, 0, 0, 0, 0};
                    parameters.Target = new Transform[] {null, null, null, null, null};
                    for (var i = 0; i < 5; i++)
                    {
                        command = NextCommand(ref commandLine);
                        isAngle &= float.TryParse(command, out parameters.Angle[i]);
                    }

                    command = NextCommand(ref commandLine);
                    var isTime = int.TryParse(command, out parameters.Time);
                    var man = FindObjectOfType<Manipulator.Manipulator>();
                    for (var i = 0; i < 5; i++)
                        parameters.Target[i] = man.GetUnitAt(i).transform;

                    var operation = new RotateAllAbsolute(parameters);
                    if (isAngle && isTime)
                        return operation;

                    return new UnknownCommand();
                }
                case "RotateAllRelative":
                {
                    var parameters = new RotateAllRelative.Params();

                    string command;
                    var isAngle = true;
                    parameters.Angle = new float[] {0, 0, 0, 0, 0};
                    parameters.Target = new Transform[] {null, null, null, null, null};
                    for (var i = 0; i < 5; i++)
                    {
                        command = NextCommand(ref commandLine);
                        isAngle &= float.TryParse(command, out parameters.Angle[i]);
                    }

                    command = NextCommand(ref commandLine);
                    var isTime = int.TryParse(command, out parameters.Time);
                    var man = Object.FindObjectOfType<Manipulator.Manipulator>();
                    for (var i = 0; i < 5; i++)
                        parameters.Target[i] = man.GetUnitAt(i).transform;

                    var operation = new RotateAllRelative(parameters);
                    if (isAngle && isTime)
                        return operation;

                    return new UnknownCommand();
                }
                case "MoveToPointAbsolute":
                {
                    var parameters = new MoveToPointAbsolute.Params();

                    var command = NextCommand(ref commandLine);

                    var isX = float.TryParse(command, out parameters.FinalPosition.x);
                    command = NextCommand(ref commandLine);
                    var isY = float.TryParse(command, out parameters.FinalPosition.y);
                    command = NextCommand(ref commandLine);
                    var isZ = float.TryParse(command, out parameters.FinalPosition.z);
                    command = NextCommand(ref commandLine);
                    var isSpeed = int.TryParse(command, out parameters.Time);
                    var man = Object.FindObjectOfType<Manipulator.Manipulator>();
                    parameters.Target = man.GetUnitAt(0).transform;
                    parameters.Tool = man.GetUnitAt(8).transform;
                    parameters.Point = man.GetUnitAt(9).transform;

                    var operation = new MoveToPointAbsolute(parameters);

                    if (isSpeed && isX && isY && isZ)
                        return operation;
                    return new UnknownCommand();
                }
                case "MoveToPointRelative":
                {
                    var parameters = new MoveToPointRelative.Params();

                    var command = NextCommand(ref commandLine);

                    var isX = float.TryParse(command, out parameters.FinalPosition.x);
                    command = NextCommand(ref commandLine);
                    var isY = float.TryParse(command, out parameters.FinalPosition.y);
                    command = NextCommand(ref commandLine);
                    var isZ = float.TryParse(command, out parameters.FinalPosition.z);
                    command = NextCommand(ref commandLine);
                    var isSpeed = float.TryParse(command, out parameters.Time);
                    var man = Object.FindObjectOfType<Manipulator.Manipulator>();
                    parameters.Target = man.GetUnitAt(0).transform;
                    parameters.Tool = man.GetUnitAt(8).transform;
                    parameters.Point = man.GetUnitAt(9).transform;

                    var operation = new MoveToPointRelative(parameters);

                    if (isSpeed && isX && isY && isZ)
                        return operation;
                    return new UnknownCommand();
                }

                case "RotateRelative":
                {
                    var parameters = new RotateRelative.Params();

                    var command = NextCommand(ref commandLine);
                    var isNumberUnit = int.TryParse(command, out parameters.UnitNumber);

                    command = NextCommand(ref commandLine);
                    var isAngle = float.TryParse(command, out parameters.Angle);

                    command = NextCommand(ref commandLine);
                    var isSpeed = int.TryParse(command, out parameters.Time);

                    parameters.Target = Object.FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(parameters.UnitNumber)
                        .transform;

                    var operation = new RotateRelative(parameters);

                    if (isAngle && isSpeed && isNumberUnit)
                        return operation;

                    return new UnknownCommand();
                }

                case "RotateAbsolute":
                {
                    var parameters = new RotateAbsolute.Params();

                    var command = NextCommand(ref commandLine);
                    var isNumberUnit = int.TryParse(command, out parameters.UnitNumber);

                    command = NextCommand(ref commandLine);
                    var isAngle = float.TryParse(command, out parameters.Angle);

                    command = NextCommand(ref commandLine);
                    var isSpeed = int.TryParse(command, out parameters.Time);

                    parameters.Target = Object.FindObjectOfType<Manipulator.Manipulator>().GetUnitAt(parameters.UnitNumber)
                        .transform;

                    var operation = new RotateAbsolute(parameters);

                    if (isAngle && isSpeed && isNumberUnit)
                        return operation;

                    return new UnknownCommand();
                }

                case "Wait":
                {
                    var parameters = new Wait.Params();

                    var command = NextCommand(ref commandLine);
                    var isDelay = int.TryParse(command, out parameters.Time);

                    var operation = new Wait(parameters);

                    if (isDelay)
                        return operation;
                    return new UnknownCommand();
                }

                default:
                {
                    var operation = new UnknownCommand();
                    return operation;
                }
            }
        }

        private List<string> UnpackCycles(string text)
        {
            var unpackText = text.Split('\n').ToList();
            var stack = new Stack<int>();

            //Проверка на корректность логики циклов
            for (var i = 0; i < unpackText.Count; i++)
            {
                lineIndex.Add(i + 1);
                var str = unpackText[i];
                if (str.StartsWith("Do "))
                    stack.Push(1);
                if (str.StartsWith("End do"))
                {
                    if (stack.Count > 0)
                        stack.Pop();
                    else
                        return new List<string>();
                }
            }

            if (stack.Count > 0)
                return new List<string>();


            stack = new Stack<int>();

            for (var i = 0; i < unpackText.Count; i++)
            {
                //Поиск начала цикла
                if (unpackText[i].StartsWith("Do "))
                    stack.Push(i);

                //Поиск конца цикла
                else if (unpackText[i].StartsWith("End do"))
                {
                    var indexLastStartCycle = stack.Pop();
                    var words = unpackText[indexLastStartCycle].Split(' ').ToList();
                    //Проверка на синтаксис цикла
                    if (!(words.Count == 2 && int.TryParse(words[1], out var repeat)))
                        return words;

                    var substring = new List<string>();
                    var subindex = new List<int>();
                    //Формимрование списка строк из цикла
                    for (var j = indexLastStartCycle + 1; j < i; j++)
                    {
                        subindex.Add(lineIndex[j]);
                        substring.Add(unpackText[j]);
                    }

                    var prefix = new List<string>();
                    var prefixIndex = new List<int>();

                    //Формирование списка строк до цикла
                    for (var j = 0; j < indexLastStartCycle; j++)
                    {
                        prefixIndex.Add(lineIndex[j]);
                        prefix.Add(unpackText[j]);
                    }

                    var postfix = new List<string>();
                    var postfixIndex = new List<int>();

                    //Формирование списка строк после цикла
                    for (var j = i + 1; j < unpackText.Count; j++)
                    {
                        postfixIndex.Add(lineIndex[j]);
                        postfix.Add(unpackText[j]);
                    }

                    //Формирование нового списка строк полсе открытия цикла
                    unpackText = prefix;
                    lineIndex = prefixIndex;

                    for (var j = 0; j < repeat; j++)
                    {
                        lineIndex.AddRange(subindex);
                        unpackText.AddRange(substring);
                    }

                    lineIndex.AddRange(postfixIndex);
                    unpackText.AddRange(postfix);
                }
            }


            return unpackText;
        }

        public List<ManipulatorScript.State> GetStateForComPort()
        {
            var text = inputField.text;
            text = NormalizationCommandLines(text);
            var commandLines = unpack;

            var stateList = new List<ManipulatorScript.State>();

            var context = FindObjectOfType<ManipulatorScript>();
            var stateStart = context.ManipulatorState;
            stateStart.Time = 0;
            stateStart.CommandID = 0;
            // Debug.Log(
            //     $"ID: {stateStart.CommandID} | Ang: {stateStart.Angles[0]} " +
            //     $"{stateStart.Angles[1]} {stateStart.Angles[2]} {stateStart.Angles[3]} " +
            //     $"{stateStart.Angles[4]} | Claw: {stateStart.ClawState} | Time: {stateStart.Time}");

            foreach (var a in from s in commandLines where s != "\n" select Resolve(s))
            {
                var time = a.GetTime();
                a.SetTime(0);
                var enumerator = a.Execute();
                Routine.ByEnumerator(enumerator).Complete();
                var currentState = context.ManipulatorState;
                currentState.Time = time;
                currentState.CommandID = a.GetCommandID();
                stateList.Add(currentState);
            }

            foreach (var state in stateList)
            {
                // Debug.Log(
                //     $"ID: {state.CommandID} | Ang: {state.Angles[0]} " +
                //     $"{state.Angles[1]} {state.Angles[2]} {state.Angles[3]} " +
                //     $"{state.Angles[4]} | Claw: {state.ClawState} | Time: {state.Time}");
            }

            Routine.ByEnumerator(context.RotateAll(stateStart.Angles.ToArray(), 0, ModeType.Absolute)).Complete();
            Routine.ByEnumerator(context.CloseGripper(0)).Complete();

            return stateList;
        }

        public string GetContentForSave() => inputField.text;
        public List<int> GetLineNumbers() => lineIndex;
    }
}