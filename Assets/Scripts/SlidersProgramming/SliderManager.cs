using System.Linq;
using Omega.Routines;
using RootMotion.FinalIK;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlidersProgramming
{
    public class SliderManager : MonoBehaviour
    {
        [SerializeField] private GameObject rotateAllTemplate;
        [SerializeField] public GameObject Parent;
        [SerializeField] public Button button;
        [SerializeField] private Button moveToPointButton;
        [SerializeField] public GameObject moveToPointTemplate;
        [SerializeField] public GameObject target;
        [SerializeField] public GameObject manipulator;
       
        private Coroutine _timeoutCoroutine;

        private void Start()
        {
            button.onClick.AddListener(OnPlusButtonClick);
            moveToPointButton.onClick.AddListener(AddMoveToPoint);
        }

        public void OnPlusButtonClick()
        {
            var com = Instantiate(rotateAllTemplate, Parent.transform);
            com.SetActive(true);
            com.gameObject.GetComponentInChildren<TMP_Dropdown>().value = 1;
            var index = com.transform.GetSiblingIndex();
            com.transform.SetSiblingIndex(index);
        }

        private void AddMoveToPoint()
        {
            var com = Instantiate(moveToPointTemplate, Parent.transform);
            com.SetActive(true);
            com.gameObject.GetComponentInChildren<TMP_Dropdown>().value = 1;
        }

        public void CoordinateSliders()
        {
            StopCoroutine(_timeoutCoroutine);
            var ccdik = manipulator.GetComponent<CCDIK>();
            var claw = ccdik.solver.bones.Last().transform;
            target.transform.position = claw.transform.position;
            ccdik.solver.target = target.transform;

            ccdik.solver.Update();
            _timeoutCoroutine = StartCoroutine(Routine.Delay(2).Callback(() =>
            {
                
            }));
        }
    }
}