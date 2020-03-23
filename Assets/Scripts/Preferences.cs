using Classes;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class Preferences : ScriptableObject
{
    #region INI Settings

    private const string Path = "config.ini";

    private const string SectionNamePrefix = "ACTUATOR_YAW_";
    private const string MinVar = "MIN";
    private const string MaxVar = "MAX";
    private const string OffsetVar = "OFFSET";

    #endregion

    private const int ShoulderCount = 6;

    private static IniFile _ini;

    private static int _gripperMinAngle;
    private static int _gripperMaxAngle;
    private static int _gripperOffset;
    public static int[] MinAngles { get; } = new int[ShoulderCount];

    public static int[] MaxAngles { get; } = new int[ShoulderCount];

    public static int[] Offsets { get; } = new int[ShoulderCount];

    [Button]
    public static void Read()
    {
        _ini = new IniFile();
        _ini.Load(Path);
    }

    private void InitData()
    {
    }

    public static void Write(string section, string var, string value) => _ini[section][section] = value;

    [Button]
    public static void Refresh()
    {
        Read();

        for (var i = 0; i < ShoulderCount; i++)
        {
            MinAngles[i] = _ini[SectionNamePrefix + (i + 1)][MinVar].ToInt();
            MaxAngles[i] = _ini[SectionNamePrefix + (i + 1)][MaxVar].ToInt();
            Offsets[i] = _ini[SectionNamePrefix + (i + 1)][OffsetVar].ToInt();
            Unit.MinAngle[i] = MinAngles[i];
            Unit.MaxAngle[i] = MaxAngles[i];


            // Debug.Log(MinAngles[i]);
            //                                   Debug.Log(MaxAngles[i]);
            //                                   Debug.Log(Offsets[i]);
        }

        var man = FindObjectOfType<Manipulator.Manipulator>();
        for (int i = 0; i < 5; i++)
        {
            var shoulder = man.GetUnitAt(i).transform;
            var limit = shoulder.GetComponent<RotationLimitHinge>();
            limit.max = MaxAngles[i] - 90;
            limit.min = MinAngles[i] - 90;
            // limit.enabled = true;
        }

        _gripperMinAngle = _ini[SectionNamePrefix + "CLAW"][MinVar].ToInt();
        _gripperMaxAngle = _ini[SectionNamePrefix + "CLAW"][MaxVar].ToInt();
        _gripperOffset = _ini[SectionNamePrefix + "CLAW"][OffsetVar].ToInt();
        //
        // Debug.Log(_gripperMinAngle);
        // Debug.Log(_gripperMaxAngle);
        // Debug.Log(_gripperOffset);
    }
}