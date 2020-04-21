using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 35f);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.LookAt(position);
    }
}
