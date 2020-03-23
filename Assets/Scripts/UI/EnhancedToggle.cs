//
// Класс, дополняющий стандартный компонент Toggle. 
// TODO: Унаследовать его от Toggle и использовать только расширенную версию.

//

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CognitiveReality.Source.UI
{
    public class EnhancedToggle : MonoBehaviour
    {
        private readonly LogType _logType = default;

        private readonly float _activeIconAlpha = 1f;
        private readonly float _inactiveIconAlpha = 0.3f;


        private readonly float _activeBackgroundAlpha = 0.3f;
        private readonly float _inactiveBackgroundAlpha = 0f;
        private readonly float _crossfadeDuration = 0.2f;

        private Toggle _toggle;
        [SerializeField] public UnityEvent onToggleClick;
        [SerializeField] private Graphic _graphicIcon;
        [SerializeField] private Graphic _graphicBackground;

        private void Init()
        {
            _toggle = GetComponent<Toggle>();
            _graphicBackground = transform.GetChild(0).GetComponent<Graphic>();
            _graphicIcon = transform.GetChild(0).GetChild(0).GetComponent<Graphic>();
        }

        // !!! TODO:  Reset and OnValidate methods not call in build. They calls only in Editor
        private void Reset() => Init();
        private void OnValidate() => Init();

        private void Start()
        {
        
            if(!Application.isEditor)
                Init();
            _toggle = GetComponent<Toggle>();
            //Debug.Log(_toggle);
            _toggle.onValueChanged.AddListener(OnToggleValueChangedHandler);
            OnToggleValueChangedHandler(_toggle.isOn);
        }

        private void OnToggleValueChangedHandler(bool value)
        {
            if (_logType.Equals(LogType.Log)) Debug.Log($"Toggle {name} change state to {value}");
            if (value) onToggleClick?.Invoke();
            
            _graphicIcon.CrossFadeAlpha(value ? _activeIconAlpha : _inactiveIconAlpha, _crossfadeDuration, false);
            _graphicBackground.CrossFadeAlpha(value ? _activeBackgroundAlpha : _inactiveBackgroundAlpha, _crossfadeDuration, false);
        }
    }
}