using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionLimits : MonoBehaviour
{
    [SerializeField] private Vector3 minBox;
    [SerializeField] private Vector3 maxBox;
    [SerializeField] private Transform man;
    private void LateUpdate()
    {
        UpdatePosition();
    }

    void UpdatePosition()
    {
        var center = (minBox + maxBox) / 2f;
        var bounds = new Bounds(center, maxBox - minBox);
        transform.position = bounds.ClosestPoint(transform.position);
    }

    void Rotartion()
    {
        //var rotation = transform.localRotation;
        transform.LookAt(man);
        
    }
        
}
