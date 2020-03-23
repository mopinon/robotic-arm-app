using System;
using System.Collections;
using System.Collections.Generic;
using Omega.Routines;
using SerialPortDirectory;
using UnityEngine;
using UnityEngine.UI;

public class RobotStatesManager : MonoBehaviour
{
    [SerializeField] private Button brokenButton;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button neutralButton;
    [SerializeField] private Button stopButton;
    [SerializeField] private Interpreter interpreter;
    [SerializeField] private WriteSerial writeSerial;

    public int stateManipulator = -1;

    private void Start()
    {
        brokenButton.onClick.AddListener(Broken);
        resetButton.onClick.AddListener(ResetRobot);
        neutralButton.onClick.AddListener(Neutral);
        stopButton.onClick.AddListener(StopRobot);
    }

    private void Broken()
    {
        if (writeSerial.IsOpen)
        {
            Debug.LogError("Crash!!!");
            stateManipulator = 3;
            interpreter.StopPlay();
            StartCoroutine(writeSerial.UpdateState((byte) 2));
        }
    }


    private void ResetRobot()
    {
        if (stateManipulator == 3 && writeSerial.IsOpen)
        {
            Debug.Log("Reset!");
            stateManipulator = 1;
            StartCoroutine(writeSerial.UpdateState((byte) 3));
        }
    }

    private void Neutral()
    {
        if (stateManipulator == 1 && writeSerial.IsOpen)
        {
            Debug.Log("Passive!");
            stateManipulator = 2;
            StartCoroutine(writeSerial.UpdateState((byte) 5));
        }
    }

    private void StopRobot()
    {
        if (stateManipulator != 3 && writeSerial.IsOpen)
        {
            Debug.Log("Stop!");
            interpreter.StopPlay();
            stateManipulator = 1;
            StartCoroutine(writeSerial.UpdateState((byte) 4));
        }
    }
}