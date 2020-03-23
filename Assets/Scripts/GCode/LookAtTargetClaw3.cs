using UnityEngine;

namespace GCode
{
    public class LookAtTargetClaw3 : MonoBehaviour
    {
        [SerializeField] public Transform target;

        void Update()
        {
            var vecUp = transform.up;
            Quaternion rotation = new Quaternion();
            rotation.SetLookRotation(target.position - transform.position, vecUp);
            rotation *= Quaternion.Euler(0, -90, 0);
            transform.rotation = rotation;
        }
    }
}