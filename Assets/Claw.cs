using UnityEngine;
using System.Collections;
public class Claw : MonoBehaviour
{
  
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
    }
}

