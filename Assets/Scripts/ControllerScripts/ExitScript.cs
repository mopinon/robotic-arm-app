using UnityEngine;
using UnityEngine.UI;

namespace ControllerScripts
{
    public class ExitScript : MonoBehaviour
    {
        [SerializeField] public Button yesButton;
        [SerializeField] public Button noButton;

        private void Start()
        {
            noButton.onClick.AddListener(Application.Quit);
        }
    }
}
