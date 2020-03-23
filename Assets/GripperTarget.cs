using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GripperTarget: MonoBehaviour
{
    [SerializeField] private Transform target;
    [Range(0, 10)][SerializeField] private float reflectionCof = 1;

    private Vector3? _previous;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!_previous.HasValue)
        {
            _previous = transform.localPosition;
            return;
        }

        var delta = transform.localPosition - _previous.Value;

        var reflectedDelta = delta * reflectionCof;
        target.localPosition += reflectedDelta;

        _previous = transform.localPosition;
    }
}
