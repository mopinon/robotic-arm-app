using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    { 
        var d = eventData.pointerDrag.GetComponent<Drag>();
        if (d != null)
        {
            var com = Instantiate(Resources.Load($"Prefabs/{d.name}"), transform.GetChild(0).GetChild(0).transform) as GameObject;
            com.AddComponent<CanvasGroup>();
        }
    }
}
