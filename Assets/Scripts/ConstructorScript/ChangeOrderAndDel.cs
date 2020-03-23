using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOrderAndDel : MonoBehaviour
{
    
    public void Up()
    {
        int newIndex = transform.GetSiblingIndex();
        newIndex--;
        transform.SetSiblingIndex(newIndex);
    }

    public void Down()
    {
        int newIndex = transform.GetSiblingIndex();
        newIndex++;
        transform.SetSiblingIndex(newIndex);
    }

    public void Del()
    {
        DestroyImmediate(gameObject);
    }
}
