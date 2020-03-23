using System.Collections;
using UnityEngine;

namespace Interfaces
{
    public interface IClaw
    {
        IEnumerator OpenToPercent(int percent, int time);
        Transform GetTarget();
        int GetState();
    }
}