using System.Collections;
using UnityEngine;

public enum ModeType : int
{
    Absolute = 0,
    Relative
}
public interface IManipulatorController
{
    IEnumerator Rotate(int angle, int shoulder, int time, ModeType modeType);
    IEnumerator RotateAll(int[] angle, int time, ModeType modeType);
    void MoveToPoint(Vector3 pos, ModeType modeType);

    IEnumerator SetGripperPosition(float percent, float time);
    IEnumerator OpenGripper(float time);
    IEnumerator CloseGripper(float time);

    //GetState();
}
