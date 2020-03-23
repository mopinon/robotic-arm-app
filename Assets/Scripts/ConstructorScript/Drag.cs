using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform returnParent;

    private GameObject placeholder;
    public void OnBeginDrag(PointerEventData eventData)
    {
        returnParent = transform.parent;
        placeholder = new GameObject();
        placeholder.transform.SetParent(returnParent);
        
        placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex());
        
        transform.SetParent(transform.parent.parent);
		
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        
        /*if (GetComponent<Drag>().cheak == true)
        {
            print("true");
            Transform placeholderParent = placeholder.transform.parent;
            int newSiblingIndex = placeholderParent.childCount;
            for (int i = 0; i < placeholderParent.childCount; i++)
                if (transform.position.y < placeholderParent.GetChild(i).position.y)
                {
                    newSiblingIndex = i;
                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;
                    break;
                }

            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }*/
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        transform.SetParent(returnParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        Destroy(placeholder);
    }
}
