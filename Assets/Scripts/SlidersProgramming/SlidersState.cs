using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SlidersProgramming
{
    public class SlidersState : MonoBehaviour
    {
        [SerializeField] public Slider[] Sliders;
        [SerializeField] public Button button;
        [SerializeField] public GameObject template;

        public List<int> GetParameters()
            => Sliders.Select(slider => (int) slider.value).ToList();
    }
}