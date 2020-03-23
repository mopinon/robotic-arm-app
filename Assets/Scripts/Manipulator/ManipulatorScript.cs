using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DG.Tweening;
using GCode.Commands;
using Interfaces;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

public class ManipulatorScript : MonoBehaviour, IManipulatorController
{
    [SerializeField] public List<Transform> Shoulders;
    private List<RotationLimitHinge> limits;
    public int StateClaw = 0;

    [SerializeField] private Transform leftGripper, rightGripper;
    
    public IEnumerator SetGripperPosition(float percent, float time)
   {
       percent = Mathf.Clamp(percent, 0, 100);
       var percentSwap = percent;
       percent = percent - StateClaw;
       StateClaw = (int) percentSwap;
       var finalQleft = Quaternion.Euler(new Vector3(percent/2, 0, 0));
       var finalQright = Quaternion.Euler(new Vector3(-percent/2, 0,0));
        
       var sequence = DOTween.Sequence();
       sequence
           .Join(leftGripper.DOLocalRotateQuaternion(finalQleft, 1)).SetRelative(true)
           .Join(rightGripper.DOLocalRotateQuaternion(finalQright, 1)).SetRelative(true);

       if (Math.Abs(time) < 0.0001f)
           sequence.Complete();
       else yield return sequence.WaitForCompletion();
   }
    /// <summary>
    /// Получить список нормализованных углов Z для всех колен
    /// </summary>
    /// <returns>нормализованные углы</returns>
    public List<int> GetShouldersAngles()
    {
        return Shoulders.Select(shoulder => (int) GetNormalizedEulerAngle(shoulder.localEulerAngles.z)).ToList();
    }

    /// <summary>
    /// Нормализует углы эйлера (делает отрицательными)
    /// </summary>
    /// <param name="angle">не нормализованный угол</param>
    /// <returns>нормализованный</returns>
    public float GetNormalizedEulerAngle(float angle)
    {
        return angle > 180 ? angle - 360 : angle;
    }


    public State ManipulatorState
    {
        get
        {
            var manipulatorState = new State();

            manipulatorState.Angles = GetShouldersAngles();
            manipulatorState.ClawState = FindObjectOfType<Manipulator.Manipulator>()
                .GetUnitAt(5)
                .GetComponent<IClaw>()
                .GetState();
            manipulatorState.Time = 0;

            return manipulatorState;
        }
    }


    public struct State
    {
        public byte CommandID;
        public int Time;
        public int ClawState;
        public List<int> Angles;

        public void SetTime(int value)
        {
            Time = value;
        }


        public int GetTime() => Time;
    }

    public IEnumerator Rotate(int angle, int shoulder, int time, ModeType modeType)
    {
        var unit = Shoulders[shoulder];
        var currentAngle = unit.localEulerAngles;

        var targetAngle = modeType == ModeType.Relative
            ? new Vector3(currentAngle.x, currentAngle.y, currentAngle.z + angle)
            : new Vector3(currentAngle.x, currentAngle.y, angle);
        var timeLeft = 0f;
        var delta = modeType == ModeType.Relative ? angle : targetAngle.z - currentAngle.z;
        while (timeLeft < time)
        {
            unit.Rotate(Vector3.forward, delta * Time.deltaTime / time, Space.Self);
            timeLeft += Time.deltaTime;
            yield return null;
        }

        unit.localEulerAngles = targetAngle;
    }

    public IEnumerator RotateAll(int[] angle, int time, ModeType modeType)
    {
        var CorutineList = new List<IEnumerator>();
        
        var i = 0;
        foreach (var shoulder in Shoulders)
        {
            CorutineList.Add(Rotate(angle[i], Shoulders.IndexOf(shoulder), time, modeType));
            i++;
        }


        while (true)
        {
            var result = false;
            foreach (var corutine in CorutineList)
                result |= corutine.MoveNext();
            if (result)
                yield return null;
            else
                break;
        }
    }

   
    public void MoveToPoint(Vector3 pos, ModeType modeType)
    {
    }


    public IEnumerator OpenGripper(float time)
    {
        yield return SetGripperPosition(100, time);
    }

    public IEnumerator CloseGripper(float time)
    {
        yield return SetGripperPosition(0, time);
    }

    //
    // public void SetLimits(RotationLimitHinge shoulder, int min, int max)
    // {
    //     shoulder.min = min;
    //     shoulder.max = max;
    // }
    //
    // public void SetLimits(int i, int min, int max)
    // {
    //     var shoulder = Shoulders[i].GetComponent<RotationLimitHinge>();
    //     SetLimits(shoulder, min, max);
    // }
    //
    // public void SetAllLimits(int min, int max)
    // {
    //     foreach (var hinge in limits) SetLimits(hinge, min, max);
    //
    //     //limits.ForEach(x=> SetLimits(x, min,max));
    // }
    // public RotationLimitHinge GetRotationLimit(int shoulder)
    // {
    //     var limit = Shoulders[shoulder].GetComponent<RotationLimitHinge>();
    //     return limit;
    // }
    //
    // public List<RotationLimitHinge> GetAllRotationLimits()
    // {
    //     List<RotationLimitHinge> limits = new List<RotationLimitHinge>();
    //     foreach (var shoulder in Shoulders)
    //     {
    //         limits.Add(GetRotationLimit(shoulder.GetSiblingIndex()));
    //     }
    //
    //     return limits;
    // }
    //
}