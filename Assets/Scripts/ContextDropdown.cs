using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class ContextDropdown : MonoBehaviour
{
    [SerializeField] public TabControl tabControl;
    [SerializeField] public TMP_Dropdown dropdown;
    [SerializeField] public FileManager fileManager;

    private void Start()
    {
        tabControl.onTabAdd.AddListener(RefreshContextList);
        tabControl.onTabClosing.AddListener(RefreshContextList);
        fileManager.onProgrammeSave.AddListener((RefreshContextList));
    }

    private void RefreshContextList()
    {
        dropdown.ClearOptions();
        var titles = tabControl.tabs.Select(tab => tab.Title).ToList();
        dropdown.AddOptions(titles);
    }
}