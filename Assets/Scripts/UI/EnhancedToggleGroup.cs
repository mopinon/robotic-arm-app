using System;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UserInterface.Source.UI
{
    public class EnhancedToggleGroup : ToggleGroup
    {
        public ToggleEvent onActiveToggleChanged = new ToggleEvent();

        protected override void Start()
        {
            foreach (Transform transformToggle in gameObject.transform)
            {
                var toggle = transformToggle.gameObject.GetComponent<Toggle>();
                if(toggle==null) return;
                toggle.onValueChanged.AddListener((isSelected) =>
                {
                    if (!isSelected)
                    {
                        return;
                    }

                    var activeToggle = Active();
                    DoOnChange(activeToggle);
                });
            }
        }

        private Toggle Active()
        {
            return ActiveToggles().FirstOrDefault();
        }

        protected virtual void DoOnChange(Toggle newActiveToggle)
        {
            onActiveToggleChanged?.Invoke(newActiveToggle);
        }
    }

    [Serializable]
    public class ToggleEvent : UnityEvent<Toggle>
    {
    }
}