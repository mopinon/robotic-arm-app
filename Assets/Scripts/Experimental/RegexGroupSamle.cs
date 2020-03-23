using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UserInterface.Source.Experimental
{
    public class RegexGroupSamle : MonoBehaviour
    {
        [Multiline(10)]
        public string text;
        public string regex;
        // Start is called before the first frame update
        void Start()
        {
            var text = "One car red car blue car";
            var pat = @"(\w+)\s+(car)";

            // Instantiate the regular expression object.
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            // Match the regular expression pattern against a text string.
            Match m = r.Match(text);
            var matchCount = 0;
            while (m.Success)
            {
                Debug.Log("Match" + (++matchCount));
                for (var i = 1; i <= 2; i++)
                {
                    Group g = m.Groups[i];
                    Debug.Log("Group" + i + "='" + g + "'");
                    CaptureCollection cc = g.Captures;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        Capture c = cc[j];
                        Debug.Log("Capture" + j + "='" + c + "', Position=" + c.Index);
                    }
                }

                m = m.NextMatch();
            }
        }

    }
}