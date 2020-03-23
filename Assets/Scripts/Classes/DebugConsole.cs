using System;
using TMPro;
using UnityEngine;

namespace Classes
{
    public class DebugConsole : MonoBehaviour
    {
        private static TMP_InputField console;

        private void Start()
        {
            if (console == null)
            {
                console = GetComponent<TMP_InputField>();
                DontDestroyOnLoad(this);
            }
            else
                Destroy(this);
        }

        public static void WriteError(string error)
        {
            console.text += $"[{DateTime.Now.ToString("HH:mm:ss")}] " + "<color=#FF0000>ERROR : </color>" + error + Environment.NewLine;
            console.caretPosition = console.text.Length - 1;
        }

        public static void WriteWarning(string warning)
        {
            console.text += $"[{DateTime.Now.ToString("HH:mm:ss")}] " + "<color=#FFCC00>WARNING :</color> " + warning + Environment.NewLine;
            console.caretPosition = console.text.Length - 1;
        }

        public static void Write(string @string)
        {
            console.text += $"[{DateTime.Now.ToString("HH:mm:ss")}] " + @string + Environment.NewLine;
            console.caretPosition = console.text.Length - 1;
        }

        public static void Clear()
        {
            console.text = String.Empty;
        }
    }
}

