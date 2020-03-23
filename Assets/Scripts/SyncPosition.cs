using System;
using System.Collections;
using System.Collections.Generic;
using GCode.Commands;
using Omega.Routines;
using SerialPortDirectory;
using UnityEngine;
using UnityEngine.UI;

public class SyncPosition : MonoBehaviour
{
    [SerializeField] public Button sync;
    [SerializeField] public Transform[] target;
    [SerializeField] public Manipulator.Manipulator manipulator;
    [SerializeField] public WriteSerial serial;
    [SerializeField] public Switch isWorkWithSerial;

    private void Start()
    {
        sync.onClick.AddListener(() => StartCoroutine(SyncPositionWithPhysicalManipulator()));
    }

    public IEnumerator StartSyncPosition()
    {
        yield return SyncPositionWithPhysicalManipulator();
    }

    private IEnumerator SyncPositionWithPhysicalManipulator()
    {
        Preferences.Refresh();
        if (!isWorkWithSerial.isOn || !serial.IsOpen) 
            yield break;
       
        yield return serial.Request().Result(out var receivingResult);
        yield return Routine.Delay(1f);
        
        var parameters = new RotateAllAbsolute.Params();
        parameters.Angle = new float[] {0, 0, 0, 0, 0};
        parameters.Target = new Transform[] {null, null, null, null, null};
        for (var i = 0; i < 5; i++)
        {
            if (receivingResult.Result != null)
            {
                parameters.Angle[i] = Mathf.Clamp(receivingResult.Result[i] - Preferences.Offsets[i],
                    Preferences.MinAngles[i], Preferences.MaxAngles[i]);
                // Debug.Log($"<color=green>{parameters.Angle[i]}</color>");
            }
            else
            {
                parameters.Angle = new float[] {0, 0, 0, 0, 0};
                Debug.Log("SyncPosition script error: ReceivingInformation Result is null");
            }

            parameters.Target[i] = manipulator.GetUnitAt(i).transform;
        }

        parameters.Time = 1;

        var rotateAllAbsolute = new RotateAllAbsolute(parameters);
        yield return rotateAllAbsolute.Execute();
    }
}