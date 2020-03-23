using System.Collections;
using Interfaces;
using UnityEngine;

namespace Claws
{
    public class MagneticClaw : MonoBehaviour, IClaw
    {
        [SerializeField] private GameObject magnet;
        [SerializeField] public Transform target;
        private float maxForce = 20f;
        private float minForce = 0f;

        public Transform GetTarget() => target;
        public int GetState()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator OpenToPercent(int percent, int time)
        {
            yield return null;
        }
    }
}