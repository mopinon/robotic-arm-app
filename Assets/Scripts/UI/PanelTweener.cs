using System;
using DG.Tweening;
using UnityEngine;

namespace UserInterface.Source.UI
{
    public class PanelTweener : MonoBehaviour
    {
    #region Serialized fields

        [SerializeField] private GameObject target;
        [SerializeField] private Ease easeType = Ease.Flash;
        [SerializeField] private float easeDuration = 0f;
        [SerializeField] private bool openOnStart = false;

    #endregion

    #region Private fields

        private bool visibile;
        private RectTransform targetRectTransform;
        private CanvasGroup targetCanvasGroup;

    #endregion

    #region Event functions

        private void Start()
        {
            if (target == null)
                target = gameObject;

            targetRectTransform = target.GetComponent<RectTransform>();
            targetCanvasGroup = target.GetComponent<CanvasGroup>();

            if (openOnStart) 
                Show();
            else 
                Hide();
        }

    #endregion

    #region Public methods

        public void Toggle()
        {
            if (!visibile) Show(() => visibile = true);
            else
                Hide(() =>
                {
                    target.SetActive(true);
                    visibile = false;
                });
        }

    #endregion

    #region Private methods

        private void Show(Action onComplete = default)
        {
            DOTween
                .To(() => targetCanvasGroup.alpha, x => targetCanvasGroup.alpha = x, 1, easeDuration)
                .SetEase(easeType)
                .OnStart(() => { target.SetActive(true); })
                .OnComplete(() => { onComplete?.Invoke(); });
        }

        private void Hide(Action onComplete = default)
        {
            DOTween
                .To(() => targetCanvasGroup.alpha, x => targetCanvasGroup.alpha = x, 0, easeDuration)
                .SetEase(easeType)
                .OnComplete(() => { onComplete?.Invoke(); });
        }

        private void SetVisibility(bool value) => visibile = value;

    #endregion
    }
}