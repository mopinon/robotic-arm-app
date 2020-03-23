using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Interfaces
{
    public abstract class CodeBlock : MonoBehaviour
    {
        [BoxGroup("Parameters"), SerializeField]
        private Color mainBlockColor, mainParameterColor, selectionBlockColor, selectionParameterColor;

        [BoxGroup("Parameters"), SerializeField]
        private Image blockBackground;

        [BoxGroup("Parameters"), SerializeField]
        private Image[] parameterBackground;

        [ContextMenu("Init Parametes")]
        private void InitParameters()
        {
            parameterBackground = GetComponentsInChildren<Image>();
        }

        [Button]
        public void SetMainColor()
        {
            blockBackground.color = mainBlockColor;
            parameterBackground.ForEach(x => x.color = mainParameterColor);
        }

        [Button]
        public void SetSelectionColor()
        {
            blockBackground.color = selectionBlockColor;
            parameterBackground.ForEach(x => x.color = selectionParameterColor);
        }
    }
}