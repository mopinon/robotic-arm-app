using Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PopUpWindows
{
    public class PopUpWindowTypeProject : MonoBehaviour, IPopUpWindow
    {
        [SerializeField] public Button closeWindowButton;
        [SerializeField] public Button gcodeButton;
        [SerializeField] public Button blockButton;
        [SerializeField] public TextMeshProUGUI windowTitleText;
        [SerializeField] public TextMeshProUGUI windowContentText;
        [SerializeField] public GameObject gcodeProject;
        [SerializeField] public GameObject blockProject;

        [SerializeField] public GameObject projectSpace;

        public SourceProject sourceProject;
        
        private string[] content = new[]
        {
            "Какой тип проекта вы хотите создать?",
            "Как тип проекта вы хотите открыть?"
        };

        private string[] titlle = new[]
        {
            "Создание проекта",
            "Открытие проекта"
        };

        private GameObject[] projects;

        public PopUpWindowTypeProject()
        {
            projects = new[]
            {
                gcodeProject,
                blockProject
            };
        }

        public void Show() => transform.gameObject.SetActive(true);

        public void Close() => transform.gameObject.SetActive(false);

        private void OpenProject(TypeProject typeProject)
        {
            var project = Instantiate(projects[(int) typeProject], projectSpace.transform);

        }

        private void Start()
        {
            closeWindowButton.onClick.AddListener(Close);
            gcodeButton.onClick.AddListener(() => OpenProject(TypeProject.Gcode));
            blockButton.onClick.AddListener(() => OpenProject(TypeProject.Block));
        }
    }
}