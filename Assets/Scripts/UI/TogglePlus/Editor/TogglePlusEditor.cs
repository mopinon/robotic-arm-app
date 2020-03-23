using UnityEditor;
using UnityEditor.UI;

namespace UI.Editor
{
    [CustomEditor(typeof(TogglePlus), true)]
    [CanEditMultipleObjects]
    public class TogglePlusEditor : ToggleEditor
    {
        private SerializedProperty m_OnToggleClickProperty;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_OnToggleClickProperty = serializedObject.FindProperty("onToggleClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_OnToggleClickProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
