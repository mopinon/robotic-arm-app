using System;
using System.Collections;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace Claws
{
    public class ClawTwoShoulders : MonoBehaviour, IClaw
    {
        [SerializeField] private Transform leftGripper, rightGripper;
        [SerializeField] public Transform target;
        public int StateClaw = 0;

        public Transform GetTarget() => target;
        public int GetState()
        {
            return StateClaw;
        }

        public IEnumerator OpenToPercent(int percent, int time)
        {
            percent = Mathf.Clamp(percent, 0, 100);
            var percentSwap = percent;
            percent = percent - StateClaw;
            StateClaw = (int) percentSwap;
            var finalQleft = Quaternion.Euler(percent / 2, 0, 0);
            var finalQright = Quaternion.Euler(-percent / 2, 0, 0);

            var sequence = DOTween.Sequence();
            sequence
                .Join(leftGripper.DOLocalRotateQuaternion(finalQleft, 1)).SetRelative(true)
                .Join(rightGripper.DOLocalRotateQuaternion(finalQright, 1)).SetRelative(true);

            if (Math.Abs(time) < 0.0001f)
                sequence.Complete();
            else yield return sequence.WaitForCompletion();
        }
    }
}