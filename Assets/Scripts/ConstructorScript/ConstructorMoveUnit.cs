/*
using Classes;
using UnityEngine;
using UnityEngine.UI;

namespace ConstructorScript
{
    public class ConstructorMoveUnit : MonoBehaviour
    {
        [SerializeField] public GameObject TextConstructor;
        [SerializeField] public InputField Parametrs;
        private float Angle;
        private int NumberUnit;
        private float Speed;
        public void AddCommandToText()
        {
            var text = Parametrs.GetComponent<InputField>().text;
            var textConstructor = TextConstructor.GetComponent<Text>().text;
            var gCoder = new GCodeParser();
        
            text = gCoder.NormalizationCommandLines(text);
        
            Angle = float.Parse(gCoder.NextCommand(ref text));
            NumberUnit = int.Parse(gCoder.NextCommand(ref text));
            Speed = float.Parse(gCoder.NextCommand(ref text));
        
            if (textConstructor == "Gcode here")
                TextConstructor.GetComponent<Text>().text = "";

            TextConstructor.GetComponent<Text>().text += Parametrs.GetComponent<InputField>().text + '\n';
        }
    }
}
*/
