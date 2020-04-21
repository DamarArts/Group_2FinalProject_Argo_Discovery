using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float scrollSpeed = 10f;
    public Transform playerBody, target, leftRayDet, rightRayDet, CameraControls, MagnetStart;
    float xRotation = 0f;
    private PlayerMovement playerScript;
    private float displacementSpeed = 7f;
    private float nWhy = 0f;
    public float camFollowPlayerDistance;
    public Vector3 originalPosition,startPosition;
    public bool ignoreCollision;
    [Range(1, 30)]
    [Tooltip("The minimum distance the camera can be relative to the player.")]
    public float minDistance = 7.0f;
    [Range(1, 200)]
    [Tooltip("The maximum distance the camera can be relative to the player.")]
    public float maxDistance = 40.0f;
    public float startY,startX;
    private Vector3 zoomedIn;
    public float zoomX;
    [Range(-2.3f, -8f)]
    public float scroll = -7f;


    void Start()
    {
        scroll = -5f;
        startY = transform.position.y;
        startX = transform.position.x;
        startPosition = transform.position;
        playerScript = GetComponent<PlayerMovement>();

        //Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        

        //_scrollInputMSACC = Input.GetAxis(inputMouseScrollWheel);
    }

    void Update()
    {

        //originalPosition = new Vector3 (transform.position.x, transform.position.x , startX);
        originalPosition = transform.position;
        

        scroll = scroll + Input.GetAxis("Mouse ScrollWheel");
        scroll = Mathf.Clamp(scroll,-8f, -2.3f);
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        //
        nWhy = -mouseY;
        //nWhy = Mathf.Clamp(nWhy, -5f, 10f);
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        
      
        CameraControls.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        transform.LookAt(target);

        playerBody.Rotate(Vector3.up * mouseX);
      


        Vector3 normalPos = new Vector3(target.localPosition.x, target.localPosition.y, scroll);  
        Vector3 zoomedIn = new Vector3(target.localPosition.x, target.localPosition.y, Mathf.Lerp(transform.localPosition.z,-2.3f, Time.deltaTime * 60f)).normalized;
        Vector3 zoomedOut = new Vector3(0, 1.8f, Mathf.Lerp(transform.localPosition.z, -4f, Time.deltaTime * 60f)).normalized;


        camFollowPlayerDistance = Vector3.Distance(transform.position, target.position);
        float camLerpSpeed = Time.deltaTime * displacementSpeed * (camFollowPlayerDistance * 0.1f);

        camFollowPlayerDistance = Mathf.Clamp(camFollowPlayerDistance, minDistance, maxDistance);

        Vector3 direction = (target.position - originalPosition).normalized;
        Vector3 finalPos = target.position - direction * camFollowPlayerDistance;

        Vector3 leftRayDirection = (target.position - leftRayDet.transform.position).normalized;
        Vector3 leftRayFinalPos = target.position - leftRayDirection * camFollowPlayerDistance;

        Vector3 rightRayDirection = (target.position - rightRayDet.transform.position).normalized;
        Vector3 rightRayFinalpos = target.position - rightRayDirection * camFollowPlayerDistance;



        //
            float camTargetDistance = Vector3.Distance(transform.localPosition, target.localPosition);
            if (!Physics.Linecast(target.position, finalPos))
            {
            transform.localPosition = Vector3.Lerp(transform.localPosition, normalPos, Time.deltaTime);
            }

            else if (Physics.Linecast(target.position, finalPos))
            {
            if (camTargetDistance > 1.5f)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, zoomedIn, camLerpSpeed);
            }
 
            }

/*
            else if (!Physics.Linecast(originalPosition, target.position))
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, normalPos, Time.deltaTime * displacementSpeed);
            }
            else if (Physics.Linecast(originalPosition, target.position))
            {
                if (ignoreCollision)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime *displacementSpeed);
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, normalPos, Time.deltaTime * displacementSpeed);
                }

        }

 */           

    }
}
