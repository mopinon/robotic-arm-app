using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ControllerScripts
{
    public class ToolBarController : MonoBehaviour
    {
        [SerializeField] public List<Transform> Shoulders;
        [SerializeField] public Transform Tool;
        [SerializeField] public TMP_InputField Angles;
        [SerializeField] public TMP_InputField TargetPosition;

        private int []Coordinates;
        private Vector3 xyz;
        private void Start()
        {
            Coordinates = new int[5];
        }

        private void LateUpdate()
        {
            for (var i = 0; i < 5; i++)
            {
                Coordinates[i] = (int) (Shoulders[i].localEulerAngles.z);
                if (Coordinates[i] > 90)
                    Coordinates[i] -= 360;
                Coordinates[i] += 90;
            }

            Angles.text = $"{Coordinates[0]}; {Coordinates[1]}; {Coordinates[2]}; {Coordinates[3]}; {Coordinates[4]}";

            xyz = Tool.position;
            xyz.x *= 1000;
            xyz.y *= 1000;
            xyz.z *= 1000;
        
            TargetPosition.text = $"X: {(int) (xyz.x)} | Y: {(int) (xyz.y)} | Z: {(int) (xyz.z)}";
        }
    }
}
