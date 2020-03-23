using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Omega.Routines;
using RootMotion.FinalIK;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UserInterface.Source.UI;

/// <summary>
/// TODO: Сделать ивент, котрый при вращении манипулятора будет обновлять слайдеры
/// </summary>
[Serializable]
public class RotationEvent : UnityEvent<List<Vector3>>
{
}

public class ManualController : SerializedMonoBehaviour
{
    [SerializeField] private Dictionary<Transform, Slider> sliderRoutes = new Dictionary<Transform, Slider>();

    [SerializeField] private List<Slider> IKTargetSliders = new List<Slider>();
    [SerializeField] private Transform IKTarget;
    [SerializeField] private Transform manipulator;


    [BoxGroup] [SerializeField] private TMP_InputField[] rotateAllFields;
    [BoxGroup] [SerializeField] private TMP_InputField[] targetFields;
    private Coroutine _timeoutCoroutine;

    private void Start()
    {
        this.Bind();
        SetSliderLimits(sliderRoutes.Values.ToArray(), new List<(float min, float max)>
        {
            (0f, 180f),
            (0f, 180f),
            (0f, 180f),
            (0f, 180f),
            (0f, 180f)
        });

        SetSliderLimits(IKTargetSliders.ToArray(), new List<(float min, float max)>
        {
            (-1000, 1000),
            (-1000, 1000),
            (-1000, 1000)
        });

        SetSlidersToTargetPosition();
    }

    private static void SetSlidersValues(Slider slider, float value) => slider.SetValueWithoutNotify(value);

    private void SetSlidersToTargetPosition()
    {
        for (var index = 0; index < IKTargetSliders.Count; index++)
        {
            SetSlidersValues(IKTargetSliders[index], IKTarget.position[index]);
        }
    }

    /// <summary>
    /// Подписывает вращение колен на изменение значения слайдера
    /// </summary>
    private void Bind()
    {
        foreach (var slider in sliderRoutes)
        {
            slider.Value.onValueChanged.AddListener(x => { RotateOnZAxis(slider.Key, slider.Value.value); });
        }

        for (var i = 0; i < 5; i++)
        {
            var i1 = i;
            sliderRoutes.ElementAt(i).Value.onValueChanged.AddListener(x =>
            {
                rotateAllFields[i1].text = x.ToString(CultureInfo.InvariantCulture);
            });
        }

        for (var i = 0; i < 3; i++)
        {
            var i1 = i;
            IKTargetSliders.ElementAt(i).onValueChanged.AddListener(x =>
            {
                targetFields[i1].text = x.ToString(CultureInfo.InvariantCulture);
            });
        }


        foreach (var slider in IKTargetSliders)
        {
            slider.onValueChanged.AddListener(x =>
            {
                SetTargetPosition(new Vector3(IKTargetSliders[0].value, IKTargetSliders[1].value,
                    IKTargetSliders[2].value));
            });
        }
    }

    private void SetTargetPosition(Vector3 position)
    {
        if (_timeoutCoroutine != null)
            StopCoroutine(_timeoutCoroutine);

        IKTarget.gameObject.SetActive(true);
        var ccdik = manipulator.GetComponent<CCDIK>();
        var clawPosition = ccdik.solver.bones.Last().transform.position;
        var IKposition = IKTarget.position;
        IKposition = clawPosition;
        IKposition = position / 1000f;
        IKTarget.position = IKposition;
        ccdik.solver.target = IKTarget;
        ccdik.enabled = true;
        ccdik.solver.Update();
        ccdik.enabled = false;

        _timeoutCoroutine = StartCoroutine(Routine.Delay(2).Callback(() =>
        {
            IKTarget.gameObject.SetActive(false);
        }));
    }

    /// TODO
    /// <summary>
    /// Устанавливает вращение по оси Z в 
    /// Тестовый метод. Позже заменить на реализацию IManipulator
    /// </summary>
    /// <param name="t">Вращаемый трансформ</param>
    /// <param name="value">Слайдер, у которого стащить значение</param>
    private static void RotateOnZAxis(Transform t, float value)
    {
        var newlocalEulerAngles = t.localEulerAngles;
        t.localEulerAngles = new Vector3(newlocalEulerAngles.x, newlocalEulerAngles.y, value - 90);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sliders"></param>
    /// <param name="limits"></param>
    protected static void SetSliderLimits(Slider[] sliders, List<(float min, float max)> limits)
    {
        for (var i = 0; i < sliders.Length; i++)
        {
            sliders[i].minValue = limits[i].min;
            sliders[i].maxValue = limits[i].max;
        }
    }

    /// <summary>
    /// Обновить все слайдеры в соответствии с углами поворота манипулятора
    /// </summary>
    /// <param name="sliders"></param>
    /// <param name="values"></param>
    private static void UpdateSliders(Slider[] sliders, float[] values)
    {
        for (var i = 0; i < sliders.Length; i++)
        {
            (sliders[i] as SliderPlus)?.Set(values[i], false);
        }
    }
}