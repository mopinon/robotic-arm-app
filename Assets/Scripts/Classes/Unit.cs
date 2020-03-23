using System;
using UnityEngine;

namespace Classes
{
    public static class Unit
    {
        public static float[] MinAngle = {0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f, 0f};
        public static float[] MaxAngle = {180, 180, 180f, 180f, 180f, 180f, 0f, 0f, 0f};

        public static float GetRelativeLimitedAngle(float from, float angle, int unitNumber)
        {
            float limitedAngle;

            from = from > 180 ? from - 360 : from;
            limitedAngle = angle > 180 ? angle - 360 : angle;

            if (from + angle > MaxAngle[unitNumber]) limitedAngle = MaxAngle[unitNumber] - from;
            else if (from + angle < MinAngle[unitNumber]) limitedAngle = MinAngle[unitNumber] - from;

            return limitedAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from">начальный угол поворота</param>
        /// <param name="finalAngle">угол поворота</param>
        /// <param name="unitNumber">номер кости</param>
        /// <returns>Конечный угол поворота</returns>
        public static float GetAbsoluteLimitedAngle(float from, float finalAngle, int unitNumber)
        {
            return GetAbsoluteLimitedAngle(from, finalAngle, MinAngle[unitNumber], MaxAngle[unitNumber]);
        }

        public static float GetAbsoluteLimitedAngle(float from, float finalAngle, float min, float max)
        {
            float limitedAngle;
            from = from > 180 ? from - 360 : from;
            limitedAngle = finalAngle;

            if (finalAngle > max)
            {
                limitedAngle = max;
            }
            else if (finalAngle < min)
            {
                limitedAngle = min;
            }

            var delta = limitedAngle - from;

            
            return delta;
        }
    }
}