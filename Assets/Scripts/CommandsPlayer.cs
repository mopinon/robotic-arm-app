// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UIElements;
//
// public class CommandsPlayer : MonoBehaviour
// {
//     
//     private Manipulator.Manipulator manipulator;
//     private static IGCommand[] _commands;
//
//     private static int commandListVersion = 0;
//     public static event Action onCommandListChange; 
//     public static IGCommand[] CommandList
//     {
//         get => _commands;
//         set
//         {
//             commandListVersion++;
// //            Debug.Log($"Current version: {commandListVersion}");
//             onCommandListChange?.Invoke();
//             _commands = value;
//         }
//     }
//
//     private bool isPlaying;
//
//     public List<int> currentManipulatorAngles = new List<int> {0,0,0,0,0};
//     
// }