using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class SlidersProgrammPlayer : MonoBehaviour
{
    [SerializeField] public Button button;
    [SerializeField] public Transform blockParent;
    
    private void Start()
    {
        button.onClick.AddListener(StartClick);
    }

    private Coroutine cor;
    public void StartClick()
    {
        button.interactable = false;
        cor = StartCoroutine(StartClickEnumerator());
    }

    [Button]
    private void StopCommandQueue()
    {
        StopCoroutine(cor);
    }

    private IEnumerator StartClickEnumerator()
    {
        var commands = blockParent.GetComponentsInChildren<ICodeBlock>();
        foreach (var command in commands)
        {
            var enumerator = command.GetCommand().Execute();
            yield return enumerator;
        }

        button.interactable = true;
    }
}
