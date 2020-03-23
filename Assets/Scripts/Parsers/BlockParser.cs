using System.Collections.Generic;
using System.Linq;
using GCode.Commands;
using Interfaces;
using Omega.Routines;
using UnityEngine;

namespace Parsers
{
    public class BlockParser : MonoBehaviour, IParser
    {
        [SerializeField] public Transform blockParent;

        public List<int> lineIndex = new List<int>();
        private Coroutine _routine;
        private List<ICodeBlock> unpack = new List<ICodeBlock>();

        private List<ICodeBlock> UnpackCycles(List<ICodeBlock> commands)
        {
            var stack = new Stack<int>();
            var k = 0;
            //Проверка на корректность логики циклов
            foreach (var command in commands)
            {
                lineIndex.Add(k);
                k++;
                if (command.GetCommandID() == 3)
                    stack.Push(1);
                if (command.GetCommandID() == 4)
                {
                    if (stack.Count > 0)
                        stack.Pop();
                    else
                        return new List<ICodeBlock>();
                }
            }

            if (stack.Count > 0)
                return new List<ICodeBlock>();

            stack = new Stack<int>();

            for (var i = 0; i < commands.Count; i++)
            {
                //Поиск начала цикла
                if (commands[i].GetCommandID() == 3)
                    stack.Push(i);

                //Поиск конца цикла
                else if (commands[i].GetCommandID() == 4)
                {
                    var substring = new List<ICodeBlock>();
                    var subIndex = new List<int>();
                    var indexLastStartCycle = stack.Pop();

                    var repeat = commands[indexLastStartCycle].GetParameters()[0];

                    //Формимрование списка строк из цикла
                    for (var j = indexLastStartCycle + 1; j < i; j++)
                    {
                        subIndex.Add(lineIndex[j]);
                        substring.Add(commands[j]);
                    }

                    var prefix = new List<ICodeBlock>();
                    var prefixIndex = new List<int>();

                    //Формирование списка строк до цикла
                    for (var j = 0; j < indexLastStartCycle; j++)
                    {
                        prefixIndex.Add(lineIndex[j]);
                        prefix.Add(commands[j]);
                    }

                    var postfix = new List<ICodeBlock>();
                    var postfixIndex = new List<int>();

                    //Формирование списка строк после цикла
                    for (var j = i + 1; j < commands.Count; j++)
                    {
                        postfixIndex.Add(lineIndex[j]);
                        postfix.Add(commands[j]);
                    }

                    //Формирование нового списка строк полсе открытия цикла
                    commands = prefix;
                    lineIndex = prefixIndex;
                    for (var j = 0; j < repeat; j++)
                    {
                        lineIndex.AddRange(subIndex);
                        commands.AddRange(substring);
                    }

                    lineIndex.AddRange(postfixIndex);
                    commands.AddRange(postfix);
                }
            }

            return commands;
        }

        public List<IGCommand> GetCommands()
        {
            var unpackCycles = UnpackCycles(blockParent.GetComponentsInChildren<ICodeBlock>().ToList());
            unpack = unpackCycles;
            var commands = unpackCycles;
            return commands.Select(command => command.GetCommand()).ToList();
        }

        public List<ManipulatorScript.State> GetStateForComPort()
        {
            var unpackCycles = unpack;
            var commands = unpackCycles;

            var stateList = new List<ManipulatorScript.State>();
            var context = FindObjectOfType<ManipulatorScript>();

            var stateStart = context.ManipulatorState;
            stateStart.Time = 0;
            stateStart.CommandID = 0;
            // Debug.Log(
            //     $"ID: {stateStart.CommandID} | Ang: {stateStart.Angles[0]} " +
            //     $"{stateStart.Angles[1]} {stateStart.Angles[2]} {stateStart.Angles[3]} " +
            //     $"{stateStart.Angles[4]} | Claw: {stateStart.ClawState} | Time: {stateStart.Time}");
            foreach (var command in commands)
            {
                var igCommand = command.GetCommand();
                
                var time = igCommand.GetTime();
                igCommand.SetTime(0);
                var enumerator = igCommand.Execute();
                Routine.ByEnumerator(enumerator).Complete();
                var currentState = context.ManipulatorState;
                currentState.Time = time;
                currentState.CommandID = igCommand.GetCommandID();
                stateList.Add(currentState);
            }

            // stateList.ForEach(x =>
            //     Debug.Log(
            //         $"ID: {x.CommandID} | Ang: {x.Angles[0]} {x.Angles[1]} {x.Angles[2]} {x.Angles[3]} {x.Angles[4]} | Claw: {x.ClawState} | Time: {x.Time}"));

            Routine.ByEnumerator(context.RotateAll(stateStart.Angles.ToArray(), 0, ModeType.Absolute)).Complete();
            var operation = new ClawOpenPercent();
            operation.Percent = 0;
            operation.Time = 0;
            Routine.ByEnumerator(operation.Execute()).Complete();

            return stateList;
        }
        private struct ArrayDescription
        {
            public Description[] description;
        }

        public string GetContentForSave()
        {
            var bloks = blockParent.GetComponentsInChildren<ICodeBlock>().ToArray();
            var listDescr = bloks.Select(block => block.GetBlock()).ToList();
            var wrapper = new ArrayDescription {description = listDescr.ToArray()};
            var json = JsonUtility.ToJson(wrapper, true);
            return json;
        }

        public List<int> GetLineNumbers() => lineIndex;
    }
}