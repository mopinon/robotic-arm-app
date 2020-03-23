using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Classes;
using Omega.Routines;
using TMPro;
using UnityEngine;

namespace SerialPortDirectory
{
    public class WriteSerial : MonoBehaviour
    {
        // Перменные состояния данных для UART и COM-портов
        [Header("StatusUART")] private SerialPort _serialPort;
        public int BaudRateUART = 9600;
        private string[] allport;
        [SerializeField] public SerialManager serialManager;
        public bool IsOpen => _serialPort != null && _serialPort.IsOpen;

        public int indexActiveComPort = -1;

        [Header("UI_elements")] public TMP_Dropdown u_dropdown;
        public TMP_InputField u_BaundRate;
        [SerializeField] public TMP_InputField ConsoleDebug;
        [SerializeField] private Switch isWorkWithSerial;
        [SerializeField] private RobotStatesManager stateManager;

        public void CloseSerialPort()
        {
            indexActiveComPort = -1;
            Debug.Log("СОМ-порт закрыт");
            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort.Dispose();
            }

            isWorkWithSerial.OffSwitch();
        }

        public void ChoicePortName()
        {
            var i = u_dropdown.value;
            var com = u_dropdown.options[i];
            // Debug.Log(com.text + " " + BaudRateUART);
            if (indexActiveComPort != i)
            {
                OpenSerialPort(com.text);
                indexActiveComPort = i;
                Debug.Log("Attempt to open Serial");
            }
            else
                Debug.LogError("Этот СОМ-порт уже открыт");
        }

        // Открытие COM-порта
        void OpenSerialPort(string portName)
        {
            BaudRateUART = int.Parse(u_BaundRate.text);
            _serialPort = new SerialPort(portName, BaudRateUART);

            try
            {
                _serialPort.Open();
                Debug.Log("СОМ-порт октрыт успешно");
            }
            catch (Exception e)
            {
                Debug.LogError($"Can not Open Serial: {e.Message}");
            }
        }

        //--------------------------------------------- Отправка данных -------------------------------------------------- 

        public IEnumerator GetValuesToPort(float[] angles, int[] c)
        {
            for (var i = 0; i < angles.Length; i++)
            {
                angles[i] = angles[i] + Preferences.Offsets[i];
                angles[i] = Mathf.Clamp(angles[i], Preferences.MinAngles[i], Preferences.MaxAngles[i]);
            }

            var angle1 = BitConverter.GetBytes((int) angles[0] * 60);
            var angle2 = BitConverter.GetBytes((int) angles[1] * 60);
            var angle3 = BitConverter.GetBytes((int) angles[2] * 60);
            var angle4 = BitConverter.GetBytes((int) angles[3] * 60);
            var angle5 = BitConverter.GetBytes((int) angles[4] * 60);
            var angle6 = BitConverter.GetBytes((int) angles[5] * 60);

            var clow1 = BitConverter.GetBytes(c[0] * 60);
            var clow2 = BitConverter.GetBytes(c[1] * 60);
            var str = "Byte sent to com port ";


            var angles_clowes = new[] {angle1, angle2, angle3, angle4, angle5, angle6, clow1, clow2};
            // var reversed = angles_clowes.Select(e => e.Reverse().ToArray()).ToArray();
            var aggregated = angles_clowes.Aggregate((lhs, rhs) => lhs.Concat(rhs).ToArray());

            var started = new byte[] {0xFF, 0xFE, 0x02, 0x20};
            var requestBody = started.Concat(aggregated).Append((byte) 0).ToArray();
            requestBody[requestBody.Length - 1] = (byte) requestBody.Select(e => (int) e).Sum();

            str += requestBody.Select(e => e.ToString("X")).Aggregate((a, b) => $"{a} {b}");
            Debug.Log(str);

            // Debug.Log(requestBody.Select(e => e.ToString("X")).Aggregate((a, b) => $"{a}, {b}")); 

            yield return Routine.Task(() => { _serialPort.Write(requestBody, 0, requestBody.Length); })
                .GetRoutine(out var routine);
            if (routine.IsError)
            {
                Debug.LogWarning("Check Serial");
                CloseSerialPort();
            }


            Debug.Log("Data sent to UART");
        }

        public IEnumerator UpdateState(byte state)
        {
           yield return SetState(state);
        }

        private Routine SetState(byte state)
        {
            return Routine.Task(() =>
            {
                var started = new byte[] {0xFF, 0xFE, 0x03, 0x01, state, 0x0};
                started[started.Length - 1] = (byte) started.Sum(e => (int) e);
                var str = "Byte sent to com port ";
                foreach (var s in started)
                    str += s.ToString("X") + " ";
                Debug.Log(str);
                _serialPort.Write(started, 0, started.Length);
            });
        }

        //--------------------------------------------- Запрос данных с манипулятора  -------------------------------------------------- 

        //Запрашиваем данные с COM-port, отправка стартового запроса

        public Routine<int[]> Request()
        {
            IEnumerator RoutineUpdate(RoutineControl<int[]> @this)
            {
                var started = new byte[] {0xFF, 0xFE, 0x03, 0x01, 0x00, 0x01};
                // started[started.Length - 1] = (byte) started.Sum(e => (int) e);
                yield return Routine.Task(() => { _serialPort.Write(started, 0, started.Length); })
                    .GetRoutine(out var routine);
                yield return Routine.Delay(0.1f);
                yield return ReceivingInformationAsync().Result(out var receivingResult);
                // if (routine.IsError)
                //     Console.text += routine.Exception.Message + "\n";
                //
                // Console.text += "Data sent to UART";
                if (receivingResult.Result == null)
                {
                    Debug.LogWarning("Warning: Check Serial");
                    CloseSerialPort();
                    yield break;
                }

                @this.SetResult(receivingResult.Result);
            }

            return Routine.ByEnumerator<int[]>(RoutineUpdate);
        }


        // получение данных с COM-port
        public void ReceivingInformation()
        {
            StartCoroutine(Request());
        }

        private Routine<int[]> ReceivingInformationAsync()
        {
            IEnumerator RoutineUpdate(RoutineControl<int[]> @this)
            {
                yield return Routine.Task(() =>
                {
                    var temp = new byte[38];
                    _serialPort.Read(temp, 0, temp.Length);
                    return temp;
                }).Result(out var result);

                if (result.Routine.IsError)
                {
                    Debug.LogWarning("Check Serial");
                    CloseSerialPort();
                    yield break;
                }

                var buffer = result.Result;
                // Debug.Log(buffer.Length);
                var str = "Byte sent from com port ";
                foreach (var b in buffer)
                    str = str + (b.ToString("X") + " ");
                Debug.Log(str);
                var started = buffer.Take(4).ToArray();
                var hashSum = buffer.Last();
                var state = buffer[buffer.Length - 2];

                var lastState = stateManager.stateManipulator;
                if (state != lastState)
                {
                    stateManager.stateManipulator = state;
                    if (stateManager.stateManipulator == 0)
                        Debug.Log("Robot is locked: code = 0");
                    if (stateManager.stateManipulator == 1)
                        Debug.Log("Robot is ready: code = 1");
                    if (stateManager.stateManipulator == 2)
                        Debug.Log("Robot is neutral");
                    if (stateManager.stateManipulator == 3)
                        Debug.LogWarning("Robot broke!");
                }

                var anglesAndClowesRaw =
                    buffer.Skip(4).Take(8 * 4).ToArray(); // 8 * 4 = (6 angles + 2 clowes) * 4 bytes (aka Int32)

                var angle1 = BitConverter.ToInt32(anglesAndClowesRaw, 0);
                var angle2 = BitConverter.ToInt32(anglesAndClowesRaw, 4);
                var angle3 = BitConverter.ToInt32(anglesAndClowesRaw, 8);
                var angle4 = BitConverter.ToInt32(anglesAndClowesRaw, 12);
                var angle5 = BitConverter.ToInt32(anglesAndClowesRaw, 16);
                var angle6 = BitConverter.ToInt32(anglesAndClowesRaw, 20);
                var clow1 = BitConverter.ToInt32(anglesAndClowesRaw, 24);
                var clow2 = BitConverter.ToInt32(anglesAndClowesRaw, 28);

                var listParam = new List<int>();
                listParam.Add(angle1);
                listParam.Add(angle2);
                listParam.Add(angle3);
                listParam.Add(angle4);
                listParam.Add(angle5);
                listParam.Add(angle6);
                listParam.Add(clow1);
                listParam.Add(clow2);
                for (var index = 0; index < listParam.Count; index++)
                    listParam[index] /= 60;


                var array = listParam.ToArray();

                @this.SetResult(array);

                // Debug.Log($"{angle1}, {angle2}, {angle3}, {angle4}, {angle5}, {angle6}, {clow1}, {clow2}");
            }

            return Routine.ByEnumerator<int[]>(RoutineUpdate);
        }
    }
}