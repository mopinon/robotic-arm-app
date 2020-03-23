using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class TogglePlus : Toggle
    {
        [SerializeField] public UnityEvent onToggleClick;

        protected override void Start()
        {
            base.Start();
            onValueChanged.AddListener(OnToggleValueChangedHandler);
        }

        private void OnToggleValueChangedHandler(bool value)
        {
            if (value) onToggleClick?.Invoke();
        }
    }
}