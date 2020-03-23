using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeCodeEditor : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,
    IPointerExitHandler, IDragHandler
{
    [SerializeField] public Texture2D texture;
    public Vector2 minSize;
    public Vector2 maxSize;
    [SerializeField] public RectTransform codeWindow;
    private Vector2 currentPointerPosition;
    private Vector2 previousPointerPosition;

    //пересечение
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(texture, new Vector2(16, 0), CursorMode.ForceSoftware);
    }

    //выход
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    
    public void OnPointerDown (PointerEventData data) {
        codeWindow.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle (codeWindow, data.position, data.pressEventCamera, out previousPointerPosition);
    }

    public void OnDrag (PointerEventData data) {
        // if (codeWindow == null)
        //     return;

        var sizeDelta = codeWindow.sizeDelta.x;
        
       // Debug.Log($"SizeDelta.x {sizeDelta}");
       
        RectTransformUtility.ScreenPointToLocalPointInRectangle (codeWindow, data.position, data.pressEventCamera, out currentPointerPosition);
        
        var resizeValue = currentPointerPosition - previousPointerPosition;
        
      //  Debug.Log($"resizeValue {resizeValue}");
        
        sizeDelta += resizeValue.x;
        
       // Debug.Log($"SizeDelta.x {sizeDelta} 2");
     //  Debug.Log(Mathf.Clamp(sizeDelta, minSize.x, maxSize.x));
        var reSizeDelta = new Vector2(
            Mathf.Clamp(sizeDelta, minSize.x, maxSize.x),
           0);

        codeWindow.sizeDelta = reSizeDelta;

        previousPointerPosition = currentPointerPosition;
    }
}