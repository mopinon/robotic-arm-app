using System;
using UnityEngine;

public class LogPosition : MonoBehaviour
{
    [SerializeField] public Manipulator.Manipulator man;
    private void OnTriggerEnter(Collider other)
    {
        other.enabled = false;
        man.Target = transform.gameObject;
        var claw = man.Claw.transform;
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        transform.SetParent(claw);
        Debug.Log(other.name);
        // Debug.Log(transform.parent);
    }

    

    private void OnMouseEnter()
    {
        var pos = transform.position;
        Debug.Log($"{(int)(pos.x * 1000)}, {(int)(pos.y * 1000)}, {(int)(pos.z * 1000)}");
    }
}
