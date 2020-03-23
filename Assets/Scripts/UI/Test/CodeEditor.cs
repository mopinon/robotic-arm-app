    using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface.Source.UI.Test
{
    public class CodeEditor : MonoBehaviour
    {
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField, Multiline(10)] private string codeTemplate;
        [SerializeField] private Button templateButton;

        private void Awake()
        {
            templateButton.onClick.AddListener(SetCodeTemplateIntoEditor);
        }

        /// <summary>
        /// Set gcode template in code editor
        /// </summary>
        public void SetCodeTemplateIntoEditor()
        {
            codeInputField.text = codeTemplate;
        }


        /// <summary>
        /// Clean editor view
        /// </summary>
        public void CleanEditor()
        {
            codeInputField.text = string.Empty;
        }
    }
}