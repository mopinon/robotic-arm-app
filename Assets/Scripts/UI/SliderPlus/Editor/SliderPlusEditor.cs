using UnityEditor;
using UnityEditor.UI;

namespace UserInterface.Source.UI.Editor
{
    
    [CustomEditor(typeof(SliderPlus), true)]
    [CanEditMultipleObjects]
    public class SliderPlusEditor : SliderEditor
    {
        SerializedProperty m_InputField;

        protected override void OnEnable()
        {
            base.OnEnable();
            m_InputField = serializedObject.FindProperty("inputField");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_InputField);
            serializedObject.ApplyModifiedProperties();
        }
    }
}