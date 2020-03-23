using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UserInterface.Source.UI
{
    public class AutohidePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform targetPanel;

        private IEnumerator hideCoroutine;

        public void Start() => Hide();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hideCoroutine != null) StopCoroutine(hideCoroutine);
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            hideCoroutine = Countdown(2f, Hide);
            StartCoroutine(hideCoroutine);
        }


        private void Show() => targetPanel.DOLocalMoveX(0f, 0.2f).SetRelative(false).SetEase(Ease.InOutQuint);
        private void Hide() => targetPanel.DOAnchorPosX(targetPanel.sizeDelta.x - 10f, 0.2f).SetRelative(false).SetEase(Ease.InOutQuint);

        private static IEnumerator Countdown(float duration, Action callback)
        {
            float normalizedTime = 0;
            while (normalizedTime <= 1f)
            {
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }

            callback.Invoke();
        }
    }
}