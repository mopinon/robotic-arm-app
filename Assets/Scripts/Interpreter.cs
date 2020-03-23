using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Omega.Routines;
using SerialPortDirectory;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class Interpreter : MonoBehaviour
{
    [SerializeField] public Button play;
    [SerializeField] public Button stop;
    [SerializeField] public TabControl tabControl;
    [SerializeField] public WriteSerial serial;
    [SerializeField] public TMP_InputField Console;
    [SerializeField] public TMP_Dropdown dropdownContext;
    [SerializeField] public SyncPosition syncPosition;
    [SerializeField] public SerialManager serialManager;
    [SerializeField] public Switch isWorkWithSerial;

    private Coroutine _playCoroutine;
    private Coroutine _timeoutCoroutine;

    // Ограничение по времени на ожидание действий физического манипулятора
    private int timeOut = 10;

    private void Start()
    {
        play.onClick.AddListener(StartClick);
        stop.onClick.AddListener(StopPlay);
        serialManager.onSerialLost.AddListener(StopPlay);
    }

    public void StopPlay()
    {
        if (_playCoroutine != null)
            StopCoroutine(_playCoroutine);

        play.interactable = true;
        if (dropdownContext.options.Count != 0)
        {
            var programme = tabControl.pages[dropdownContext.value];
            var highLighter = programme.GetComponentInChildren<ILineHighlighter>();
            highLighter.Refresh();
        }

        Debug.Log("Программа была остановлена");
    }

    private void StartClick()
    {
        if (!isWorkWithSerial.isOn)
        {
            _playCoroutine = StartCoroutine(Play());
            return;
        }

        if (serial.IsOpen)
        {
            _playCoroutine = StartCoroutine(Play());
        }
        else if (isWorkWithSerial.isOn)
            Debug.LogError("Нет соединения с COM-портом");
    }

    private IEnumerator Play()
    {
        play.interactable = false;

        yield return syncPosition.StartSyncPosition();

        if (tabControl.pages.Count < 1)
        {
            play.interactable = true;
            yield break;
        }

        var programme = tabControl.pages[dropdownContext.value];
        var parser = programme.GetComponentInChildren<IParser>();
        var highLighter = programme.GetComponentInChildren<ILineHighlighter>();
        highLighter.Refresh();

        var commands = parser.GetCommands();
        var states = new List<ManipulatorScript.State>();
        if (isWorkWithSerial.isOn)
            states = parser.GetStateForComPort();
        var lineIndex = parser.GetLineNumbers();

        for (var j = 0; j < commands.Count; j++)
        {
            if (isWorkWithSerial.isOn)
                StartPort(states[j]);
            highLighter.SetLineColor(lineIndex[j]);
            var command = commands[j];
            yield return command.Execute();
            highLighter.ResetColorLines();
            if (isWorkWithSerial.isOn)
            {
                var state = states[j];
                var virtualManipulatorAngles = state.Angles.ToArray();
                while (isWorkWithSerial.isOn)
                {
                    _timeoutCoroutine = StartCoroutine(Routine.Delay(timeOut).Callback(() =>
                    {
                        Debug.LogWarning("Time out");
                        if (serial.IsOpen)
                            serial.CloseSerialPort();
                        StopCoroutine(_playCoroutine);
                        play.interactable = true;
                    }));
                    yield return serial.Request().Result(out var receivingResult);
                    StopCoroutine(_timeoutCoroutine);

                    yield return Routine.Delay(0.5f);
                    var physicalManipulatorAngles = receivingResult.Result;
                    if (physicalManipulatorAngles == null)
                    {
                        StopCoroutine(_playCoroutine);
                        yield break;
                    }
                    var count = 5;
                    var hasInvalid = false;
                    for (var i = 0; i < count; i++)
                    {
                        var vma = virtualManipulatorAngles[i];
                        var pma = physicalManipulatorAngles[i];
                        var delta = Mathf.Abs(vma - pma);
                        if (delta > 2)
                        {
                            hasInvalid = true;
                            break;
                        }
                    }
                    if (hasInvalid)
                    {
                        Debug.Log("Angles are not equal");
                        yield return null;
                    }
                    else
                    {
                        Debug.Log("Angles are equal");
                        break;
                    }
                }
            }
        }

        play.interactable = true;
        Debug.Log("Выполнение программы закончилось");
    }

    private void StartPort(ManipulatorScript.State state)
    {
        StartCoroutine(PushStates(state));
    }

    private IEnumerator PushStates(ManipulatorScript.State _state)
    {
        var state = _state;
        state.Angles.Add(0);
        var str = "";
        str = "Angles sent to Com port: ";
        for (var i = 0; i < state.Angles.Count; i++)
        {
            state.Angles[i] += 90;
            str += state.Angles[i] + " ";
        }

        // Debug.Log(
        //     $"a1: {state.Angles[0]}, a2: {state.Angles[1]}, a3: {state.Angles[2]}, a4: {state.Angles[3]}, a5: {state.Angles[4]}, a6: {state.Angles[5]}");
        int percent = 100 - state.ClawState;
        percent = (int) (percent * 45f / 100f);
        str += percent + " " + percent;
        Debug.Log(str);
        var c = new int[] {percent, percent};
        

        yield return serial.GetValuesToPort(state.Angles.Select(angle => (float) angle).ToArray(), c);
    }
}