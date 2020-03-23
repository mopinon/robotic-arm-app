using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sirenix.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hilighter : MonoBehaviour
{
    [SerializeField] private TMP_Text output;
    [SerializeField] private TMP_InputField input;

    private void Start() => OnEdit();

    public void OnEdit()
    {
        output.text = string.Empty;
        input.textComponent.alpha = 0.5f;
        output.GetComponent<TMP_Text>().alpha = 1f;
        var str = input.text.Split('\n', '\r');
        List<string> NewStr = new List<string>();
        var regex = new Regex(@"(^[A-Za-z]+)(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s-\d+[,.]?\d*|\s\d+[,.]?\d*)?(\s\(.*\))?");
        foreach (var s in str)
        {
            var match = regex.Match(s);
            string st = $"<color=#4F8BFF>{match.Groups[1]}</color>" +
                        $"<color=#C586C0>{match.Groups[2]}</color>" +
                        $"<color=#C586C0>{match.Groups[3]}</color>" +
                        $"<color=#C586C0>{match.Groups[4]}</color>" + 
                        $"<color=#C586C0>{match.Groups[5]}</color>" +
                        $"<color=#C586C0>{match.Groups[6]}</color>" +
                        $"<color=#C586C0>{match.Groups[7]}</color>" + 
                        $"<color=#00CA59>{match.Groups[8]}</color>{Environment.NewLine}";
            NewStr.Add(st);
        }
        Output(NewStr);
    }

    private void Output(List<string> s)
    {
        foreach (var st in s)
        {
            output.text += st;
        } 
    }
}