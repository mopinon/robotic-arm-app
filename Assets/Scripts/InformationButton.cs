using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InformationButton : MonoBehaviour
{
    [SerializeField] private GameObject info;
    public void Hider()
    {
        if (info.activeSelf)
            info.SetActive(false);
        else
            info.SetActive(true);
    }
}
