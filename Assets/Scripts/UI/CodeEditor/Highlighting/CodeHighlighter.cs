/*
 * Simple code highlighting test w/ tmp + rich text + regexp
 */

using System.Collections;
using System.Text.RegularExpressions;
using Omega.Routines;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UserInterface.Source.UI.CodeEditor;

namespace UI.CodeEditor.Highlighting
{
    public class CodeHighlighter : SerializedMonoBehaviour
    {
        [SerializeField] private TMP_InputField codeInputField;
        [SerializeField] private TextMeshProUGUI codeOutputField;
        [SerializeField] private SyntaxRule syntaxRules;

        private Routine<string> _highlightRoutine;

        private void Start()
        {
            codeInputField.onValueChanged.AddListener(_ => OnEdit());
        }

        public void OnEdit()
        {
            IEnumerator Enumerator(RoutineControl<string> @this)
            {
                Debug.Log("here");
                var input = codeInputField.text;
                yield return Routine.Task(() => CreateHighlight(input)).Result(out var result);
                @this.SetResult(result);
            }

            if (_highlightRoutine is null || _highlightRoutine.IsComplete)
            {
                _highlightRoutine = Routine.ByEnumerator<string>(Enumerator)
                    .Callback(output => codeOutputField.text = output)
                    .InBackground(ExecutionOrder.LateUpdate);
            }
            else
            {
                if (_highlightRoutine.IsProcessing)
                { 
                    _highlightRoutine.Cancel();
                    _highlightRoutine = null;
                }
            }
        }

        public string CreateHighlight(string input)
        {
            foreach (var rule in syntaxRules.GetEnumerator().Values)
                input = ReplaceStringWithColor(input, new Regex(rule.regex, rule.options), rule.color);

            return input;
        }

        private string ReplaceStringWithColor(string @string, Regex regex, Color color)
        {
            foreach (Match itemMatch in regex.Matches(@string))
            {
                @string = @string.Replace(itemMatch.Value, GetLineWithColor(itemMatch.Value, color));
            }

            return @string;
        }

        private static string GetLineWithColor(string @string, Color color)
        {
            return @string
                .Insert(@string.Length, "</color>")
                .Insert(0, $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>");
        }
    }
}