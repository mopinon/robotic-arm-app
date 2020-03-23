using System.Collections;
using System.Collections.Generic;
using BlockCommands;
using Interfaces;
using UnityEngine;

public interface ICodeBlock
{
    Description GetBlock();
    byte GetCommandID();
    List<int> GetParameters();
    void SetParameters(List<int> parameters);
    IGCommand GetCommand();
}
