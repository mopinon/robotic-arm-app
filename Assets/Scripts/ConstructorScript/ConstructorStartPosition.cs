using UnityEngine;
using UnityEngine.UI;

namespace ConstructorScript
{
    public class ConstructorStartPosition : MonoBehaviour
    {
        [SerializeField] public GameObject TextConstructor;
        public void OnCLick()
        {
            if (TextConstructor.GetComponent<Text>().text == "Gcode here")
                TextConstructor.GetComponent<Text>().text = "";

            TextConstructor.GetComponent<Text>().text += "StartPosition" + '\n';
        }
    }
}
