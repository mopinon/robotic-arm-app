using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlidersContentManager : MonoBehaviour
{
    [SerializeField] private Button switchSliders;
    [SerializeField] private GameObject[] panels;
    [SerializeField] private TMP_Text buttonTitle;
    private string[] titles = new[] {"Углы", "Координаты"};
    private int index = 0;
    

    private void Start()
    {
        switchSliders.onClick.AddListener(ClickSwitch);
    }

    private void ClickSwitch()
    {
        index = (index + 1) % 2;
        foreach (var panel in panels)
            panel.SetActive(false);
        panels[index].SetActive(true);
        buttonTitle.text = titles[index];
    }
}