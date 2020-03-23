using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace UserInterface.Source.UI
{
    public class MultipanelController : MonoBehaviour
    {
        public static int activePanelIndex;
        [SerializeField] public RectTransform[] panels;
        [SerializeField] private int startPanelIndex;

        public void AddPanel(RectTransform panel)
        {
            var list = panels.ToList();
            list.Add(panel);
            panels = list.ToArray();
        }

        public void DeletePanel(int index)
        {
            Debug.Log(index);
            var list = panels.ToList();
            list.RemoveAt(index);
            panels = list.ToArray();
            if (index == activePanelIndex)
            {
                if (index > panels.Length - 1)
                {
                    activePanelIndex--;
                    SetActivePanel(index - 1);
                }
                // else if (index == 2 && panels.Length == 2)
                //     SetActivePanel(0);
                // else
                //     SetActivePanel(index);
            }
        }

        private void Start()
        {
            SetPanelsActive(false);
            PutPanelsTogether();
            SetActivePanel(startPanelIndex);
        }


        private void PutPanelsTogether()
        {
            foreach (var panel in panels)
            {
                ResetOffset(panel);
            }
        }

        public void ResetOffset(RectTransform rect)
        {
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        private void SetPanelsActive(bool value)
        {
            foreach (var panel in panels)
                panel.gameObject.SetActive(value);
        }

        [Button]
        private void Next()
        {
            if (activePanelIndex < panels.Length - 1)
            {
                SetActivePanel(activePanelIndex + 1);
            }
        }

        [Button]
        private void Previous()
        {
            if (activePanelIndex > 0)
            {
                SetActivePanel(activePanelIndex - 1);
            }
        }

        public void SetActivePanel(int value)
        {
            if (value < 0 || value > panels.Length - 1) return;

            panels[activePanelIndex].gameObject.SetActive(false);
            activePanelIndex = value;
            panels[activePanelIndex].gameObject.SetActive(true);
        }
    }
}