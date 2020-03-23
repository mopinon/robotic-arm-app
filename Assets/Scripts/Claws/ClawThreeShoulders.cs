using System;
using System.Collections;
using DG.Tweening;
using Interfaces;
using UnityEngine;

namespace Claws
{
    public class ClawThreeShoulders : MonoBehaviour, IClaw
    {
        private const float minZ = 0.04603f;
        private const float maxZ = 0.0708f;
        [SerializeField] private Transform target;
        [SerializeField] private Transform targetBone;
        
        public Transform GetTarget() => targetBone;
        public int GetState()
        {
            throw new NotImplementedException();
        }

        public IEnumerator OpenToPercent(int percent, int time)
        {
            var targetZ = minZ + (maxZ - minZ) / 100f * percent;
            var tween = target.DOLocalMoveZ(targetZ, 1)
                .SetRelative(false);
            if (Math.Abs(time) < 0.0001f)
                tween.Complete();
            else yield return tween.WaitForCompletion();
        }
    }
}