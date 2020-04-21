using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeUpCam : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    float xRotation = 0f;
    private float nWhy = 0f;
    private PlayerMovement playerScript;



    void Start()
    {
        playerScript = GetComponent<PlayerMovement>();

        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    void Update()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        nWhy = -mouseY;
        nWhy = Mathf.Clamp(nWhy, -90f, 35f);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
        //target.localRotation = Quaternion.Euler(xRotation, 0f, 0f);



    }
}


