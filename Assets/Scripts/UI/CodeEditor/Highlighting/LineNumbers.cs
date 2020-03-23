using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UserInterface.Source.UI;

namespace UI.CodeEditor.Highlighting
{
    public class LineNumbers : MonoBehaviour, ILineHighlighter
    {
        [SerializeField] private TextMeshProUGUI lineText;
        [SerializeField] private TextMeshProUGUI codeText;
        [SerializeField] private Color markedLineColor;
        private void Start() => UpdateNumberLine();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return)) UpdateNumberLine();
        }

        [Button]
        public void UpdateNumberLine()
        {
            UpdateLine(GetNumberOfLines(codeText.text));
        }


        private static int GetNumberOfLines(string inputField)
        {
            return inputField.Split('\n').Length;
        }

        private void UpdateLine(int value)
        {
            lineText.text = "";
            for (var i = 1; i <= value; i++)
            {
                lineText.text += i + "\n";
            }
        }
        

        [Button]
        private void UpdateLinesWithMark(int lineCount, int markedLine, Color markColor)
        {
            lineText.text = "";
            for (var i = 1; i <= lineCount; i++)
            {
                if (i == markedLine)
                {
                    lineText.text += GetMarkedString(i.ToString(), markColor) + "\n";
                }
                else
                {
                    lineText.text += i + "\n";
                }
            }
        }


        [Button]
        public void SetLineColor(int lineIndex)
        {
          UpdateLinesWithMark(GetNumberOfLines(codeText.text), lineIndex, markedLineColor);
        }

        public void ResetColorLines()
        {
            UpdateNumberLine();
        }

        public void Refresh()
        {
            UpdateNumberLine();
        }

        private static string GetMarkedString(string @string, Color color)
        {
            return @string
                .Insert(@string.Length, "</mark>")
                .Insert(0, $"<mark=#{ColorUtility.ToHtmlStringRGBA(color)}>");
        }
    }
}