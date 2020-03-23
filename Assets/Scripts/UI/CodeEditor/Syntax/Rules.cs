using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace UserInterface.Source.UI.CodeEditor.Syntax
{
    /// <summary>
    ///     Syntax Rule
    /// </summary>
    [Serializable]
    public sealed class Rule
    {
        /// <summary>
        ///     The Regex Options
        /// </summary>
        public RegexOptions options;

        /// <summary>
        ///     Regex To Highlight
        /// </summary>
        public string regex;

        /// <summary>
        ///     The Style of the Rule eg. -> CommentStyle
        /// </summary>
        internal Style type;
        
        
        /// <summary>
        ///     The Style of the Rule eg. -> CommentStyle
        ///      TODO: will be replaced by style
        /// </summary>
        public Color color;
    }
}
