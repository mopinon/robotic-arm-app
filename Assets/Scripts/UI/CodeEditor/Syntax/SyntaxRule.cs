using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UserInterface.Source.UI.CodeEditor.Syntax;

namespace UserInterface.Source.UI.CodeEditor
{
    [CreateAssetMenu]
    public class SyntaxRule : SerializedScriptableObject, IEnumerable
    {
        [SerializeField, DictionaryDrawerSettings(DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)] 
        public Dictionary<string, Rule> rules = new Dictionary<string, Rule>();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (IEnumerator) GetEnumerator();
        }

        public Dictionary<string, Rule> GetEnumerator()
        {
            return new Dictionary<string, Rule>(rules);
        }
    }
}