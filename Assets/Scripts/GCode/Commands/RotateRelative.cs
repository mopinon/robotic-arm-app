using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class RotateRelative : IGCommand
    {
        private Params _parameters;

        public RotateRelative(Params @params)
        {
            _parameters = @params;
        }

        public struct Params
        {
            public Transform Target;
            public int Time;
            public float Angle;
            public int UnitNumber;
        }

        public void SetTime(int time)
        {
            _parameters.Time = time;
        }

        public IEnumerator Execute()
        {
            var from = _parameters.Target.localEulerAngles;
            
            var finalAngle = Unit.GetRelativeLimitedAngle
                (from.z + 90, _parameters.Angle, _parameters.UnitNumber);
            
            var finalRotation = Vector3.zero;
            finalRotation.z = finalAngle;
            
            Tween rotationTween = _parameters.Target
                .DOLocalRotate(finalRotation, _parameters.Time)
                .SetRelative(true);

            if(Math.Abs(_parameters.Time) < 0.0001f)
                rotationTween.Complete();
            else yield return rotationTween.WaitForCompletion();
        }

        public byte GetCommandID()
        {
            return 111;
        }

        public int GetTime()
        {
            return _parameters.Time;
        }


    #region Trash

        public List<float> GetFloatTargetAngle()
        {
            return default;
        }

        public List<int> GetIntParameters()
        {
            return default;
        }

        public List<int> GetFinalAngles(List<int> prev)
        {
            return default;
        }

    #endregion
    }
}