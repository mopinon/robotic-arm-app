using RootMotion.FinalIK;
using UnityEngine;

namespace UserInterface.Source.Experimental
{
    public static class FinalIKExtensionMethods
    {
        public static Quaternion[] GetFinalAngles(this IKSolverCCD solver, Vector3 targetPosition)
        {
            // Массив финальных, не ограниченных углов
            var finalAngles = new Quaternion[solver.bones.Length - 1];
            var lastDelta = 1110f;
            var g = 0;
            Vector3 lastPosition;

            while (g < 1000)
            {
                Debug.Log(lastDelta);
                if (lastDelta < 0.00001f)
                    break;
                lastPosition = solver.bones[4].transform.position;
                for (int i = solver.bones.Length - 2; i > -1; i--)
                {
                    //CCD tends to overemphasise the rotations of the bones closer to the target position. Reducing bone weight down the hierarchy will compensate for this effect.
                    float w = solver.bones[i].weight * solver.IKPositionWeight;

                    if (w > 0f)
                    {
                        Vector3 toLastBone = solver.bones[solver.bones.Length - 1].transform.position -
                                             solver.bones[i].transform.position;
                        Vector3 toTarget = targetPosition - solver.bones[i].transform.position;


                        // Get the rotation to direct the last bone to the target
                        Quaternion targetRotation = Quaternion.FromToRotation(toLastBone, toTarget) *
                                                    solver.bones[i].transform.rotation;

                        // Применение вращения к костям манипулятора
                        {
                            if (w >= 1) solver.bones[i].transform.rotation = targetRotation;
                            else
                                solver.bones[i].transform.rotation =
                                    Quaternion.LerpUnclamped(solver.bones[i].transform.rotation, targetRotation, w);
                        }

                        // prepare angle for returning
                        finalAngles[i] = targetRotation;
                    }

                    // Rotation Constraints
                    if (solver.useRotationLimits && solver.bones[i].rotationLimit != null)
                        solver.bones[i].rotationLimit.Apply();
                }

                lastDelta = (lastPosition - solver.bones[4].transform.position).sqrMagnitude;
                g++;
            }

            return finalAngles;
        }
    }
}