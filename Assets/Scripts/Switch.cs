using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Switch : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private bool _isOn;
    [SerializeField] private RectTransform Indicator;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color onColor;
    [SerializeField] private Color offColor;
    [SerializeField] private float tweenTime = 0.25f;
    private float onX;
    private float offX;
    public bool isOn => _isOn;
    
    public delegate void ValueChanched(bool v);

    public event ValueChanched valueChanged;

    private void OnEnable()
    {
        Toggle(isOn);
    }

    // Start is called before the first frame update
    void Start()
    {
        offX = Indicator.anchoredPosition.x;
        onX = -Indicator.anchoredPosition.x;
    }

    private void Toggle(bool value)
    {
        if (value != isOn)
        {
            _isOn = value;
            ToggleColor(isOn);
            MoveIndicator(isOn);
            valueChanged?.Invoke(isOn);
        }
    }

    private void ToggleColor(bool isOn)
    {
        if (isOn)
            backgroundImage.DOColor(onColor, tweenTime);
        else
            backgroundImage.DOColor(offColor, tweenTime);
    }

    private void MoveIndicator(bool isOn)
    {
        if (isOn)
            Indicator.DOAnchorPosX(onX, tweenTime);
        else
            Indicator.DOAnchorPosX(offX, tweenTime);
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!isOn);
    }

    public void OnSwitch()
    {
        Toggle(true);
    }

    public void OffSwitch()
    {
        Toggle(false);
    }
}