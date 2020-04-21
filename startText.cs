using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startText : MonoBehaviour
{
    public GameObject StartText;
    void Start()
    {
        
    }


    void Update()
    {
       if (Input.GetKeyDown(KeyCode.Tab))
        {
            StartText.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Tab))
        {
            StartText.SetActive(false);
        }



    }
}
