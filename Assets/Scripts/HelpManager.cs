using System;
using System.Collections;
using System.Collections.Generic;
using Plugins.Simple_Tooltip.Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class HelpManager : MonoBehaviour
{
    [SerializeField] private SimpleTooltip[] uiObjects;
    [SerializeField] private Button help;
    [SerializeField] private Toggle isHelpOn;
    private bool isHelpTime = true;
    private void Start()
    {
        help.onClick.AddListener(ClickHelp);
    }

    private void ClickHelp()
    {
        isHelpOn.isOn = !isHelpOn.isOn;
        isHelpTime = !isHelpTime;
        foreach (var uiObject in uiObjects)
            uiObject.enabled = isHelpTime;
    }
}
