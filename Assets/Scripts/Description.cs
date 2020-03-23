using System;
using System.Collections.Generic;
[Serializable]
public struct Description
{
    public byte id;
    public List<int> parameters;

    public Description(byte _id, List<int> _parameters)
    {
        id = _id;
        parameters = _parameters;
    }
}