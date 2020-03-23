using System;
using System.Collections;
using Classes;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class RotateAllAbsolute : IGCommand
    {
        public struct Params
        {
            public Params(Transform[] target, float[] angle) : this()
            {
                Target = target;
                Angle = angle;
            }

            public Transform[] Target;
            public int Time;
            public float[] Angle;
        }

        public RotateAllAbsolute(Params @params)
        {
            _parameters = @params;
        }

        private Params _parameters;

        public void SetTime(int time)
            => _parameters.Time = time;

        public IEnumerator Execute()
        {
            var sequence = DOTween.Sequence();

            var from = _parameters.Target[0].localEulerAngles;
            var finalAngle = Unit.GetAbsoluteLimitedAngle
                (from.z + 90, _parameters.Angle[0], 0);
            
            var quat = Quaternion.Euler(from.x, from.y, finalAngle + from.z);
            
            Tween quatRotation = _parameters.Target[0]
                .DOLocalRotateQuaternion(quat, _parameters.Time)
                .SetRelative(false);
            sequence.Join(quatRotation);

            for (var i = 1; i < 5; i++)
            {
                from = _parameters.Target[i].localEulerAngles;
                finalAngle = Unit.GetAbsoluteLimitedAngle
                    (from.z + 90, _parameters.Angle[i], i);
                quat = Quaternion.Euler(from.x, from.y, finalAngle + from.z);
                
                quatRotation = _parameters.Target[i]
                    .DOLocalRotateQuaternion(quat, _parameters.Time)
                    .SetRelative(false);
                sequence.Join(quatRotation); 
            }

            sequence.Play();

            if (Math.Abs(_parameters.Time) < 0.0001f) sequence.Complete();
            else yield return sequence.WaitForCompletion();

        }

        public byte GetCommandID()
        {
            return 42;
        }

        public int GetTime()
        {
            return _parameters.Time;
        }
    }
}