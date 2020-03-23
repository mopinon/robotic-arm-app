using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ToggleSwitch : MonoBehaviour
    {
        public RectTransform handle;
        public Toggle leftToggle, rightToggle;
        public RectTransform leftToggleBG, rightToggleBG;
        public TextMeshProUGUI leftLabel, rightLabel;
        public SVGImage leftIcon, rightIcon;
        public Color activeColor, inactiveColor;


        public Ease easeType;
        public float animationDuration;

        private void Start()
        {
            leftToggle.onValueChanged.AddListener(LeftToggleHandler);
            rightToggle.onValueChanged.AddListener(RightToggleHandler);
        }

        public void Toggle()
        {
        }

        public void LeftToggleHandler(bool isOn)
        {
            if (isOn) TurnLeftToggle();
        }

        public void RightToggleHandler(bool isOn)
        {
            if (isOn) TurnRightToggle();
        }

        private Tween toggleTween;

        public void TurnLeftToggle()
        {
            toggleTween.Pause();
            toggleTween = handle
                .DOAnchorPosX(leftToggleBG.anchoredPosition.x, animationDuration)
                .SetEase(easeType)
                .Play();

            TurnColor(true);
        }

        public void TurnRightToggle()
        {
            toggleTween.Pause();
            toggleTween = handle
                .DOAnchorPosX(rightToggleBG.anchoredPosition.x, animationDuration)
                .SetEase(easeType)
                .Play();

            TurnColor(false);
        }

        private void TurnColor(bool value)
        {
            if (value)
            {
                leftIcon.color = activeColor;
                leftLabel.color = activeColor;
                rightIcon.color = inactiveColor;
                rightLabel.color = inactiveColor;
            }
            else
            {
                rightIcon.color = activeColor;
                rightLabel.color = activeColor;
                leftIcon.color = inactiveColor;
                leftLabel.color = inactiveColor;
            }
        }
    }
}