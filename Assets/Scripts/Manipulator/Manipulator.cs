using System;
using Interfaces;
using UnityEngine;

namespace Manipulator
{
    public class Manipulator : MonoBehaviour
    {
        [SerializeField] private GameObject Base;
        [SerializeField] private GameObject Shoulder1;
        [SerializeField] private GameObject Shoulder2;
        [SerializeField] private GameObject Shoulder3;
        [SerializeField] private GameObject Shoulder4;
        [SerializeField] public GameObject Claw;
        [SerializeField] private GameObject ClawRight;
        [SerializeField] private GameObject ClawLeft;
        [SerializeField] private GameObject Tool;
        [SerializeField] private GameObject Test;
        private GameObject target;
        public GameObject Target
        {
            get => target;
            set => target = value;
        }
        private GameObject[] Units;
        private GameObject ToPosition;
        private GameObject tool;
        private float[] MaxAngle;
        private float[] MinAngle;
        private float[] MaxSpeed;
        private float[] MinSpeed;

        private float[] CurrentAngle;
        private Vector3[] SavedPosition;

        public GameObject GetUnitAt(int number)
        {
            switch (number)
            {
                case 0:
                    return Base;
                case 1:
                    return Shoulder1;
                case 2:
                    return Shoulder2;
                case 3:
                    return Shoulder3;
                case 4:
                    return Shoulder4;
                case 5:
                    return Claw;
                case 6:
                    return ClawRight;
                case 7:
                    return ClawLeft;
                case 8:
                    return Tool;
                case 9:
                    return Test;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}