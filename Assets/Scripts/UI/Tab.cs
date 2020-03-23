using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class Tab : MonoBehaviour, ITabControlElement
    {
        private string title;
        private Sprite icon;

        [SerializeField] public Image tabIconImage;
        [SerializeField] private TextMeshProUGUI tabTitleText;
        [SerializeField] private TogglePlus tabToggle;
        [SerializeField] private Button tabCloseButton;

        [SerializeField] private TabControl tabControl;

        public UnityEvent onTabAdd = new UnityEvent();
        public UnityEvent onTabClosing = new UnityEvent();
        public UnityEvent onTabSelected = new UnityEvent();
        public UnityEvent onTabDeselected = new UnityEvent();


        public Sprite Icon
        {
            get => icon;
            set
            {
                icon = value;
                tabIconImage.sprite = icon;
            }
        }

        public string Title
        {
            get => title;
            set
            {
                title = value;
                tabTitleText.text = title;
                name = title;
            }
        }

        private void Start() => InitTab();

        public Tab Initialize(ToggleGroup toggleGroup, TabControl tabControl, string tabTitle = default, Sprite sprite = default)
        {
            this.tabToggle.group = toggleGroup;
            this.Title = tabTitle;
            this.Icon = sprite;
            this.tabControl = tabControl;
        
            return this;
        }

        private void InitTab()
        {
            onTabAdd.Invoke();
            tabCloseButton.onClick.AddListener(onTabClosing.Invoke);
            tabToggle.onToggleClick.AddListener(() => { Select(); });
            
            tabToggle.onValueChanged.AddListener(x =>
            {
                if (!x)
                {
                    Deselect();
                }
            });
        }

        public Tab Select()
        {
            tabToggle.isOn = true;
            tabControl.SelectedTab = this;
            onTabSelected.Invoke();
            return this;
        }

        public Tab Deselect()
        {
            tabToggle.isOn = false;
            tabControl.SelectedTab = null;
            onTabDeselected.Invoke();
            return this;
        }

        public Tab Show()
        {
            gameObject.SetActive(true);
            return this;
        }

        public Tab Hide()
        {
            gameObject.SetActive(false);
            return this;
        }
    }
}