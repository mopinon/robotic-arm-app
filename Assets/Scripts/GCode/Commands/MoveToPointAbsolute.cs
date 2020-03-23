using System;
using System.Collections;
using DG.Tweening;
using Interfaces;
using RootMotion.FinalIK;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GCode.Commands
{
    public class MoveToPointAbsolute : IGCommand
    {
        private Params _parameters;

        public struct Params
        {
            public Transform Target;
            public Transform Point;
            public Transform Tool;
            public Vector3 FinalPosition;
            public int Time;
        }

        public MoveToPointAbsolute(Params @params)
        {
            _parameters = @params;
        }

        public void SetTime(int time)
        {
            _parameters.Time = time;
        }

        public IEnumerator Execute()
        {
            var man = Object.FindObjectOfType<Manipulator.Manipulator>();
            for (var i = 0; i < 5; i++)
            {
                var shoulder = man.GetUnitAt(i).transform;
                var limit = shoulder.GetComponent<RotationLimitHinge>();
                limit.enabled = true;
            }
            var to = _parameters.FinalPosition / 1000f;
            var ccdik = _parameters.Target.GetComponent<CCDIK>();

            ccdik.enabled = true;
            _parameters.Point.position = _parameters.Tool.position;
            ccdik.solver.target = _parameters.Point;
            Tween positionTween = _parameters.Point.DOMove(to, _parameters.Time)
                .SetRelative(false);
            if (Math.Abs(_parameters.Time) < 0.0001f)
                positionTween.Complete();
            else yield return positionTween.WaitForCompletion();

            ccdik.enabled = false;
            ccdik.solver.target = null;
            ccdik.solver.Update();
            
            for (var i = 0; i < 5; i++)
            {
                var shoulder = man.GetUnitAt(i).transform;
                var limit = shoulder.GetComponent<RotationLimitHinge>();
                limit.enabled = false;
            }
        }

        public byte GetCommandID()
        {
            return 255;
        }

        public int GetTime()
        {
            return _parameters.Time;
        }
    }
}