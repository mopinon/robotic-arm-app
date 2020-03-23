using System;
using System.Collections;
using Classes;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace GCode.Commands
{
    public class RotateAllRelative : IGCommand
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

        public RotateAllRelative(Params @params)
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
            var finalAngle = Unit.GetRelativeLimitedAngle
                (from.z + 90, _parameters.Angle[0], 0);
            var finalRotation = Vector3.zero;
            finalRotation.z = finalAngle;


            Tween rotationTween = _parameters.Target[0]
                .DOLocalRotate(finalRotation, _parameters.Time)
                .SetRelative(true);
            
            sequence.Join(rotationTween);

            for (var i = 1; i < 5; i++)
            {
                from = _parameters.Target[i].localEulerAngles;
                finalAngle = Unit.GetRelativeLimitedAngle
                    (from.z + 90, _parameters.Angle[i], i);
                finalRotation = Vector3.zero;
                finalRotation.z = finalAngle;

                rotationTween = _parameters.Target[i]
                    .DOLocalRotate(finalRotation, _parameters.Time)
                    .SetRelative(true);
                
                sequence.Join(rotationTween);
            }

            sequence.Play();

            if (Math.Abs(_parameters.Time) < 0.0001f) sequence.Complete();
            else yield return sequence.WaitForCompletion();
        }

        public byte GetCommandID()
        {
            return 49;
        }

        public int GetTime()
        {
            return _parameters.Time;
        }
    }
}