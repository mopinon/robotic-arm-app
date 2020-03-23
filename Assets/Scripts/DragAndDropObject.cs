using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDropObject : MonoBehaviour
{
    public EventTrigger[] buttons;
    public GameObject[] target;
    private bool _state = false;
    public LayerMask rayCastMask;
    private Camera _mainCamera;
    private GameObject _obj;
    Collider collider;
    private float Y;

    private void Start()
    {
        _mainCamera = Camera.main;

        for (var index = 0; index < buttons.Length; index++)
        {
            var dragEntry = new EventTrigger.Entry {eventID = EventTriggerType.Drag};
            var endDragEntry = new EventTrigger.Entry {eventID = EventTriggerType.EndDrag};

            var index1 = index;

            dragEntry.callback.AddListener(_ => { MoveObject(index1); });
            endDragEntry.callback.AddListener(_ =>
            {
                _obj.layer = 8;
                _state = false;
                _obj.GetComponent<Collider>().enabled = true;
                _obj.AddComponent<Rigidbody>();
            });


            AddEvent(buttons[index], dragEntry);
            AddEvent(buttons[index], endDragEntry);
        }
    }

    private static void AddEvent(EventTrigger @eventTrigger, EventTrigger.Entry @entry) =>
        @eventTrigger.triggers.Add(@entry);

    private void MoveObject(int index)
    {
        var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        var isRayToTable = Physics.Raycast(ray.origin, ray.direction, out var hit, Mathf.Infinity, rayCastMask);
        if (hit.collider == null) return;

        if (isRayToTable)
        {
            if (!_state)
            {
                _obj = Instantiate(target[index]);
                _state = true;
                _obj.SetActive(true);
                collider = _obj.GetComponent<Collider>();
                Y = collider.bounds.size.y;
                collider.enabled = false;
            }
            else
            {
                var position = hit.point + new Vector3(0, Y / 2, 0);
                _obj.transform.position = position;
            }
        }
    }
}