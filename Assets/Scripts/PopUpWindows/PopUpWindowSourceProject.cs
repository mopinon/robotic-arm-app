using System;
using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUpWindows
{
    public class PopUpWindowSourceProject : MonoBehaviour, IPopUpWindow
    {
        [SerializeField] public Button closeWindowButton;
        [SerializeField] public Button newProjectButton;
        [SerializeField] public Button loadProjectButton;
        [SerializeField] public TextMeshProUGUI windowTitleText;
        [SerializeField] public TextMeshProUGUI windowContentText;
        [SerializeField] public PopUpWindowTypeProject window;

        public void Show() => transform.gameObject.SetActive(true);

        public void Close() => transform.gameObject.SetActive(false);


        private void OpenProject(SourceProject sourceProject)
        {
            window.sourceProject = sourceProject;
            window.Show();
            Close();
        }

        private void Start()
        {
            closeWindowButton.onClick.AddListener(Close);
            newProjectButton.onClick.AddListener(() => OpenProject(SourceProject.New));
            loadProjectButton.onClick.AddListener(() => OpenProject(SourceProject.Load));
        }
    }
}