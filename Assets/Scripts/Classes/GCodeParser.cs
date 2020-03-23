// using GCode.Commands;
// using Interfaces;
// using UnityEngine;
// using Object = UnityEngine.Object;
//
// namespace Classes
// {
//     public class CoParser
//     {
//         public string NormalizationCommandLines(string text)
//         {
//             var commandLines = text;
//             while (commandLines.Contains("  "))
//                 commandLines = commandLines.Replace("  ", " ");
//             // while (commandLines.Contains("\n\n"))
//             //     commandLines = commandLines.Replace("\n\n", "\n");
//             while (commandLines.Contains("\n "))
//                 commandLines = commandLines.Replace("\n ", "\n");
//             commandLines = commandLines.Trim(' ');
//             return commandLines;
//         }
//
//         public string NextCommand(ref string commandLine)
//         {
//             var firstSpaceIndex = commandLine.IndexOf(' ');
//             if (firstSpaceIndex != -1)
//             {
//                 var commandArgument = commandLine.Substring(0, firstSpaceIndex);
//                 commandLine = commandLine.Substring(firstSpaceIndex + 1);
//                 return commandArgument;
//             }
//             else
//             {
//                 return commandLine;
//             }
//         }
//
//         public IGCommand Resolve(string commandLine)
//         {
//             var commandKey = NextCommand(ref commandLine);
//
//             switch (commandKey)
//             {
//                 case "ClawOpenPercent":
//                 {
//                     var operation = new ClawOpenPercent();
//                     var command = NextCommand(ref commandLine);
//                     var isPercent = int.TryParse(command, out operation.Percent);
//                     operation.Time = 1;
//                     if (isPercent)
//                         return operation;
//                     
//                     return new UnknownCommand();
//                 }
//                 case "ClawOpen":
//                 {
//                     var operation = new ClawOpen {Time = 1};
//                     return operation;
//                 }
//                 case "ClawClose":
//                 {
//                     var operation = new ClawClose {Time = 1};
//                     return operation;
//                 }
//                 case "RotateAllAbsolute":
//                 {
//                     var parameters = new RotateAllAbsolute.Params();
//
//                     string command;
//                     var isAngle = true;
//                     parameters.Angle = new float[] {0, 0, 0, 0, 0};
//                     parameters.Target = new Transform[] {null, null, null, null, null};
//                     for (var i = 0; i < 5; i++)
//                     {
//                         command = NextCommand(ref commandLine);
//                         isAngle &= float.TryParse(command, out parameters.Angle[i]);
//                     }
//
//                     command = NextCommand(ref commandLine);
//                     var isTime = int.TryParse(command, out parameters.Time);
//                     var man = Object.FindObjectOfType<Manipulator>();
//                     for (var i = 0; i < 5; i++)
//                         parameters.Target[i] = man.GetUnitAt(i).transform;
//
//                     var operation = new RotateAllAbsolute(parameters);
//                     if (isAngle && isTime)
//                         return operation;
//
//                     return new UnknownCommand();
//                 }
//                 case "RotateAllRelative":
//                 {
//                     var parameters = new RotateAllRelative.Params();
//
//                     string command;
//                     var isAngle = true;
//                     parameters.Angle = new float[] {0, 0, 0, 0, 0};
//                     parameters.Target = new Transform[] {null, null, null, null, null};
//                     for (var i = 0; i < 5; i++)
//                     {
//                         command = NextCommand(ref commandLine);
//                         isAngle &= float.TryParse(command, out parameters.Angle[i]);
//                     }
//
//                     command = NextCommand(ref commandLine);
//                     var isTime = int.TryParse(command, out parameters.Time);
//                     var man = Object.FindObjectOfType<Manipulator>();
//                     for (var i = 0; i < 5; i++)
//                         parameters.Target[i] = man.GetUnitAt(i).transform;
//
//                     var operation = new RotateAllRelative(parameters);
//                     if (isAngle && isTime)
//                         return operation;
//
//                     return new UnknownCommand();
//                 }
//                 case "MoveToPointAbsolute":
//                 {
//                     var parameters = new MoveToPointAbsolute.Params();
//
//                     var command = NextCommand(ref commandLine);
//
//                     var isX = float.TryParse(command, out parameters.FinalPosition.x);
//                     command = NextCommand(ref commandLine);
//                     var isY = float.TryParse(command, out parameters.FinalPosition.y);
//                     command = NextCommand(ref commandLine);
//                     var isZ = float.TryParse(command, out parameters.FinalPosition.z);
//                     command = NextCommand(ref commandLine);
//                     var isSpeed = int.TryParse(command, out parameters.Time);
//                     var man = Object.FindObjectOfType<Manipulator>();
//                     parameters.Target = man.GetUnitAt(0).transform;
//                     parameters.Tool = man.GetUnitAt(8).transform;
//                     parameters.Point = man.GetUnitAt(9).transform;
//
//                     var operation = new MoveToPointAbsolute(parameters);
//
//                     if (isSpeed && isX && isY && isZ)
//                         return operation;
//                     return new UnknownCommand();
//                 }
//                 case "MoveToPointRelative":
//                 {
//                     var parameters = new MoveToPointRelative.Params();
//
//                     var command = NextCommand(ref commandLine);
//
//                     var isX = float.TryParse(command, out parameters.FinalPosition.x);
//                     command = NextCommand(ref commandLine);
//                     var isY = float.TryParse(command, out parameters.FinalPosition.y);
//                     command = NextCommand(ref commandLine);
//                     var isZ = float.TryParse(command, out parameters.FinalPosition.z);
//                     command = NextCommand(ref commandLine);
//                     var isSpeed = float.TryParse(command, out parameters.Time);
//                     var man = Object.FindObjectOfType<Manipulator>();
//                     parameters.Target = man.GetUnitAt(0).transform;
//                     parameters.Tool = man.GetUnitAt(8).transform;
//                     parameters.Point = man.GetUnitAt(9).transform;
//
//                     var operation = new MoveToPointRelative(parameters);
//
//                     if (isSpeed && isX && isY && isZ)
//                         return operation;
//                     return new UnknownCommand();
//                 }
//
//                 case "RotateRelative":
//                 {
//                     var parameters = new RotateRelative.Params();
//
//                     var command = NextCommand(ref commandLine);
//                     var isNumberUnit = int.TryParse(command, out parameters.UnitNumber);
//
//                     command = NextCommand(ref commandLine);
//                     var isAngle = float.TryParse(command, out parameters.Angle);
//
//                     command = NextCommand(ref commandLine);
//                     var isSpeed = int.TryParse(command, out parameters.Time);
//
//                     parameters.Target = Object.FindObjectOfType<Manipulator>().GetUnitAt(parameters.UnitNumber)
//                         .transform;
//
//                     var operation = new RotateRelative(parameters);
//
//                     if (isAngle && isSpeed && isNumberUnit)
//                         return operation;
//
//                     return new UnknownCommand();
//                 }
//
//                 case "RotateAbsolute":
//                 {
//                     var parameters = new RotateAbsolute.Params();
//
//                     var command = NextCommand(ref commandLine);
//                     var isNumberUnit = int.TryParse(command, out parameters.UnitNumber);
//
//                     command = NextCommand(ref commandLine);
//                     var isAngle = float.TryParse(command, out parameters.Angle);
//
//                     command = NextCommand(ref commandLine);
//                     var isSpeed = int.TryParse(command, out parameters.Time);
//
//                     parameters.Target = Object.FindObjectOfType<Manipulator>().GetUnitAt(parameters.UnitNumber)
//                         .transform;
//
//                     var operation = new RotateAbsolute(parameters);
//
//                     if (isAngle && isSpeed && isNumberUnit)
//                         return operation;
//
//                     return new UnknownCommand();
//                 }
//
//                 case "Wait":
//                 {
//                     var parameters = new Wait.Params();
//
//                     var command = NextCommand(ref commandLine);
//                     var isDelay = int.TryParse(command, out parameters.Time);
//
//                     var operation = new Wait(parameters);
//
//                     if (isDelay)
//                         return operation;
//                     return new UnknownCommand();
//                 }
//
//                 default:
//                 {
//                     var operation = new UnknownCommand();
//                     return operation;
//                 }
//             }
//         }
//     }
// }