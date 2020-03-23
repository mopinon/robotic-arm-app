using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UserInterface.Source.UI
{
    /// <summary>
    /// Enhanced slider with binding 
    /// </summary>
    [AddComponentMenu("UIPlus/Slider Plus", 31)]
    [RequireComponent(typeof(RectTransform))]
    public class SliderPlus : Slider
    {
        [SerializeField] private TMP_InputField inputField;

        private TMP_InputField InputField
        {
            get => inputField;
            set
            {
                if (inputField != null)
                    inputField = value;
            }
        }

        protected override void Start()
        {
            base.Start();
            this.BindInputField(InputField);
            this.BindSlider(this);
        }

        private void BindInputField(TMP_InputField field)
        {
            if (field == null) return;

            if (wholeNumbers)
            {
                field.contentType = TMP_InputField.ContentType.IntegerNumber;
                field.onValueChanged.AddListener(s => this.value = int.Parse(s, CultureInfo.InvariantCulture));
            }
            else
            {
                field.contentType = TMP_InputField.ContentType.DecimalNumber;
                field.onValueChanged.AddListener(s => this.value = float.Parse(s, CultureInfo.InvariantCulture));
            }
        }

        private void BindSlider(Slider slider)
        {
            if (slider != null && InputField != null)
                slider.onValueChanged.AddListener(s =>
                    InputField.text = slider.value.ToString(CultureInfo.InvariantCulture));
        }

        public new void Set(float input, bool sendCallback = true)
        {
            base.Set(input, sendCallback);
        }
    }
}