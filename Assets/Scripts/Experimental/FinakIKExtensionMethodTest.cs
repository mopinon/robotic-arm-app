using System;
using System.Linq;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace UserInterface.Source.Experimental
{
    public class FinakIKExtensionMethodTest : MonoBehaviour
    {
        public CCDIK ccdik;
        public Transform target;
        public RotationLimit[] limits;

        [Button]
        public void Test()
        {
            var solver = ccdik.GetIKSolver() as IKSolverCCD;

            var angles = solver.GetFinalAngles(target.position);

            //angles.ForEach(x => Debug.LogWarning(x.eulerAngles));

            for (var i = angles.Length-1; i > -1; i--)
            {
               // angles[i] = limits[i].GetLimitedLocalRotation(angles[i], out _);
                //solver.bones[i].transform.rotation = angles[i];
            }

            var eulerAngles = angles.Select(angle => angle.eulerAngles);

           // eulerAngles.ForEach(x => Debug.Log(x));
        }

        [Button]
        public void TestLimitedLocalRotation()
        {
            Quaternion before = Quaternion.Euler(0f, 0f, 0f);
            Quaternion after = limits[1].GetLimitedLocalRotation(before, out _);

            //Debug.LogWarning($"Before {before.eulerAngles}");
            //Debug.LogWarning($"After {after.eulerAngles}");
        }
        
    }
}