using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = System.Random;

namespace UI
{
    public class TabControl : MonoBehaviour, ITabControl
    {
        [SerializeField] private GameObject tabTemplate, pageTemplate;
        [SerializeField] private Transform pageParent;
        [SerializeField] public List<Tab> tabs = new List<Tab>();
        [SerializeField] public List<Page> pages = new List<Page>();
        [SerializeField] private Button newTabButton;
        [SerializeField] private ToggleGroup tabsToggleGroup;

        public UnityEvent onTabAdd = new UnityEvent();
        public UnityEvent onTabClosing = new UnityEvent();
        
        public Tab selectedTab;
        public Page selectedPage;

        public Tab SelectedTab
        {
            get => selectedTab;
            set => selectedTab = value;
        }
        public Page SelectedPage
        {
            get => selectedPage;
            set => selectedPage = value;
        }

        public int SelectedTabIndex => tabs.IndexOf(SelectedTab);

        private GameObject TabTemplate
        {
            get => tabTemplate;
            set => tabTemplate = value;
        }

        private GameObject PageTemplate
        {
            get => pageTemplate;
            set => pageTemplate = value;
        }

        private void Start()
        {
            TabTemplate.SetActive(false);
            PageTemplate.SetActive(false);
        }

        public void BindPageToTap(Tab tab, Page page)
        {
            tab.onTabDeselected.AddListener(page.Hide);
            tab.onTabSelected.AddListener(page.Show);
            tab.onTabAdd.AddListener(onTabAdd.Invoke);
            tab.onTabClosing.AddListener(() =>
            {
                pages.Remove(page);
                page.Remove();
                CloseTab(tab);
                onTabClosing.Invoke();
            });
        }

        /// <summary>
        /// Create new tab in scene
        /// </summary>
        /// <returns>New tab instance</returns>
        public Tab AddTab()
        {
            var tab = Instantiate(TabTemplate, transform).GetComponent<Tab>();
            tabs.Add(tab);
            tab.Initialize(tabsToggleGroup, this);
            return tab;
        }

        public Page AddPage()
        {
            var page = Instantiate(PageTemplate, pageParent).GetComponent<Page>();
            pages.Add(page);

            return page;
        }

        /// <summary>
        /// Move tab to specific position
        /// </summary>
        /// <param name="tab"></param>
        /// <param name="position"></param>
        public void MoveTab(Tab tab, int position)
        {
            tabs.Remove(tab);
            tabs.Insert(position, tab);
        }

        /// <summary>
        /// Move tab transform to specific position
        /// </summary>
        /// <param name="tab">Tab</param>
        /// <param name="index">Target position</param>
        public void MoveTabTransform(Tab tab, int index)
        {
            tab.transform.SetSiblingIndex(index);
        }

        /// <summary>
        /// Move tab to the end
        /// </summary>
        /// <param name="tab">Tab</param>
        private void BringToEnd(Tab tab)
        {
            MoveTab(tab, tabs.Count - 1);
            MoveTabTransform(tab, tabs.Count);
        }

        /// <summary>
        /// Remove specific tab
        /// </summary>
        /// <param name="tab"></param>
        public void CloseTab(Tab tab)
        {
            Destroy(tab.gameObject);
            tabs.Remove(tab);
        }

        public void CloseTab(int index)
        {
            CloseTab(transform.GetChilds()[index].GetComponent<Tab>());
        }

        public void SelectTab(int index)
        {
            SelectTab(tabs[index]);
        }

        public void SelectTab(Tab tab)
        {
            tab.Select();
        }

        public void DeselectTab(int index) => DeselectTab(tabs[index]);
        public void DeselectTab(Tab tab) => tab.Deselect();


        private static class Utils
        {
            private static Random random = new System.Random();

            public static string RandomString(int length)
            {
                const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            public static Color RandomColor()
            {
                return UnityEngine.Random.ColorHSV();
            }
        }
    }
}