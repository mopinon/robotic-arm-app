using System.Collections.Generic;
using GCode.Commands;
using Interfaces;
using UnityEngine;

namespace BlockCommands
{
    public class EndCycleBlock : CodeBlock, ICodeBlock
    {
        public Description GetBlock()
        {
            var description = new Description(GetCommandID(), GetParameters());
            return description;
        }

        public byte GetCommandID() => 4;

        public List<int> GetParameters() => new List<int>();
        public void SetParameters(List<int> parameters)
        {
        }

        public IGCommand GetCommand() => new EndCycle();
    }
}