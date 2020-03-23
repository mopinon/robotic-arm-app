using System;
using System.IO.Ports;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace SerialPortDirectory
{
    public class SerialManager : MonoBehaviour
    {
        [SerializeField] public WriteSerial writeSerial;
        [Header("UI_elements")] public TMP_Dropdown u_dropdown;
        private string[] allport;
        
        public UnityEvent onSerialConnected = new UnityEvent();
        public UnityEvent onSerialLost = new UnityEvent();
        private int numberOfPorts = -1;
        
        private void Update()
        {
            allport = SerialPort.GetPortNames();
            
            if (numberOfPorts < allport.Length)
            {
                numberOfPorts = allport.Length;
                u_dropdown.options = allport.Select(e => new TMP_Dropdown.OptionData(e)).ToList();
                onSerialConnected.Invoke();
            }

            if (numberOfPorts > allport.Length)
            {
                numberOfPorts = allport.Length;
                u_dropdown.options = allport.Select(e => new TMP_Dropdown.OptionData(e)).ToList();
                onSerialLost.Invoke();
            }
        }
    }
}