// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using Classes;
// using Omega.Routines;
// using TMPro;
// using UI.CodeEditor.Highlighting;
// using UnityEngine;
// using UnityEngine.UI;
//
//
// public class GCodeScript : MonoBehaviour
// {
//     [SerializeField] public TMP_InputField inputField;
//     [SerializeField] public Button startButton;
//     [SerializeField] public Button stopButton;
//     [SerializeField] public Button startButtonBlock;
//     [SerializeField] public LineNumbers lineHighliter;
//
//     public static bool isPlayCommand;
//     private List<int> lineIndex;
//
//     private void Start()
//     {
//         //  lineHighliter.
//         startButton.onClick.AddListener(StartClick);
//         stopButton.onClick.AddListener(StopCommandQueue);
//     }
//
//     private Coroutine cor;
//
//     private void StartClick()
//     {
//         isPlayCommand = true;
//         startButtonBlock.interactable = false;
//         startButton.interactable = false;
//         cor = StartCoroutine(StartClickEnumerator());
//     }
//
//     private void StopCommandQueue()
//     {
//         if (cor != null)
//         {
//             isPlayCommand = false;
//             startButton.interactable = true;
//             startButtonBlock.interactable = true;
//             StopCoroutine(cor);
//         }
//     }
//
//     private List<string> UnpackCycles(string text)
//     {
//         var unpackText = text.Split('\n').ToList();
//         var stack = new Stack<int>();
//
//         //Проверка на корректность логики циклов
//         for (var i = 0; i < unpackText.Count; i++)
//         {
//             lineIndex.Add(i);
//             var str = unpackText[i];
//             if (str.StartsWith("Do "))
//                 stack.Push(1);
//             if (str.StartsWith("End do"))
//             {
//                 if (stack.Count > 0)
//                     stack.Pop();
//                 else
//                     return new List<string>();
//             }
//         }
//
//         if (stack.Count > 0)
//             return new List<string>();
//
//
//         stack = new Stack<int>();
//
//         for (var i = 0; i < unpackText.Count; i++)
//         {
//             //Поиск начала цикла
//             if (unpackText[i].StartsWith("Do "))
//                 stack.Push(i);
//
//             //Поиск конца цикла
//             else if (unpackText[i].StartsWith("End do"))
//             {
//                 var indexLastStartCycle = stack.Pop();
//                 var words = unpackText[indexLastStartCycle].Split(' ').ToList();
//                 //Проверка на синтаксис цикла
//                 if (!(words.Count == 2 && int.TryParse(words[1], out var repeat)))
//                     return words;
//
//                 var substring = new List<string>();
//                 var subindex = new List<int>();
//                 //Формимрование списка строк из цикла
//                 for (var j = indexLastStartCycle + 1; j < i; j++)
//                 {
//                     subindex.Add(lineIndex[j]);
//                     substring.Add(unpackText[j]);
//                 }
//
//                 var prefix = new List<string>();
//                 var prefixIndex = new List<int>();
//
//                 //Формирование списка строк до цикла
//                 for (var j = 0; j < indexLastStartCycle; j++)
//                 {
//                     prefixIndex.Add(lineIndex[j]);
//                     prefix.Add(unpackText[j]);
//                 }
//
//                 var postfix = new List<string>();
//                 var postfixIndex = new List<int>();
//
//                 //Формирование списка строк после цикла
//                 for (var j = i + 1; j < unpackText.Count; j++)
//                 {
//                     postfixIndex.Add(lineIndex[j]);
//                     postfix.Add(unpackText[j]);
//                 }
//
//                 //Формирование нового списка строк полсе открытия цикла
//                 unpackText = prefix;
//                 lineIndex = prefixIndex;
//
//                 for (var j = 0; j < repeat; j++)
//                 {
//                     lineIndex.AddRange(subindex);
//                     unpackText.AddRange(substring);
//                 }
//
//                 lineIndex.AddRange(postfixIndex);
//                 unpackText.AddRange(postfix);
//             }
//         }
//
//
//         return unpackText;
//     }
//
//     private IEnumerator StartClickEnumerator()
//     {
//         lineIndex = new List<int>();
//         var numberLinesForHighilithing = new List<int>();
//
//         var gCoder = new GCodeParser();
//         var text = inputField.text;
//         text = gCoder.NormalizationCommandLines(text);
//         var commandLines = UnpackCycles(text);
//         foreach (var s in lineIndex)
//             Debug.Log(s);
//         int time;
//         var stateList = new List<ManipulatorScript.State>();
//
//
//         var context = FindObjectOfType<ManipulatorScript>();
//         var stateStart = context.ManipulatorState;
//         stateStart.Time = 0;
//         stateStart.CommandID = 0;
//         // Debug.Log(
//         //     $"ID: {stateStart.CommandID} | Ang: {stateStart.Angles[0]} " +
//         //     $"{stateStart.Angles[1]} {stateStart.Angles[2]} {stateStart.Angles[3]} " +
//         //     $"{stateStart.Angles[4]} | Claw: {stateStart.ClawState} | Time: {stateStart.Time}");
//
//         foreach (var a in from s in commandLines where s != "\n" select gCoder.Resolve(s))
//         {
//             time = a.GetTime();
//             a.SetTime(0);
//             var enumerator = a.Execute();
//             Routine.ByEnumerator(enumerator).Complete();
//             var currentState = context.ManipulatorState;
//             currentState.Time = time;
//             currentState.CommandID = a.GetCommandID();
//             stateList.Add(currentState);
//         }
//
//         //stateList.ForEach(x =>
// //            Debug.Log(
//         //              $"ID: {x.CommandID} | Ang: {x.Angles[0]} {x.Angles[1]} {x.Angles[2]} {x.Angles[3]} {x.Angles[4]} | Claw: {x.ClawState} | Time: {x.Time}"));
//
//         Routine.ByEnumerator(context.RotateAll(stateStart.Angles.ToArray(), 0, ModeType.Absolute)).Complete();
//         Routine.ByEnumerator(context.CloseGripper(0)).Complete();
//         context.StateClaw = 0;
//
//         for (var i = 0; i < commandLines.Count; i++)
//         {
//             if (commandLines[i] != "\n")
//             {
//                 lineHighliter.SetLineColor(lineIndex[i] + 1);
//                 var s = commandLines[i];
//                 var a = gCoder.Resolve(s);
//                 var enumerator = a.Execute();
//
//                 yield return enumerator;
//                 lineHighliter.ResetColorLines();
//             }
//         }
//
//         startButtonBlock.interactable = true;
//         startButton.interactable = true;
//         isPlayCommand = false;
//
//         yield return null;
//     }
// }