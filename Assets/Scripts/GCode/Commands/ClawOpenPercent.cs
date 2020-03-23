using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GCode.Commands
{
    public class ClawOpenPercent : IGCommand
    {
        public int Time, Percent;
        public void SetTime(int time)
        {
            Time = time;
        }

        public IEnumerator Execute()
        {
            var man = Object.FindObjectOfType<Manipulator.Manipulator>();
            var claw = man.Claw.GetComponent<IClaw>();
            if (man.Target != null && Percent > 5)
            {
                // claw.GetTarget().GetComponent<Collider>().enabled = false;
                var rb = man.Target.GetComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = true;
                man.Target.transform.SetParent(null);
                man.Target = null;
            }
            else if (man.Target == null && Percent > 5)
            {
                claw.GetTarget().GetComponent<Collider>().enabled = false;
            }else
                claw.GetTarget().GetComponent<Collider>().enabled = true;

            if (claw != null) 
                return claw.OpenToPercent(Percent, Time);
            return null;
        }

        public byte GetCommandID()
        {
            return 41;
        }

        public int GetTime()
        {
            return Time;
        }
    }
}