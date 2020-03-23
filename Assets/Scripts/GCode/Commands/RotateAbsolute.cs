using System;
using System.Collections;
using System.Collections.Generic;
using Classes;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class RotateAbsolute : IGCommand
    {
        private Params _parameters;

        public RotateAbsolute(Params @params)
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

            var finalAngle = Unit.GetAbsoluteLimitedAngle
                (from.z + 90, _parameters.Angle, _parameters.UnitNumber);

            var finalRotation = from + new Vector3(0, 0, finalAngle);


            Tween rotationTween = _parameters.Target
                .DOLocalRotate(finalRotation, _parameters.Time, RotateMode.FastBeyond360)
                .SetRelative(false);

            if (Math.Abs(_parameters.Time) < 0.0001f)
                rotationTween.Complete();
            else yield return rotationTween.WaitForCompletion();
        }

        public byte GetCommandID()
        {
            return 44;
        }

        public int GetTime() => _parameters.Time;
    }
}