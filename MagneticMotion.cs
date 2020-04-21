using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagneticMotion : MonoBehaviour
{
    public float beamSpeed;
    public float distance;
    public GameObject Impact;
    private Vector3 movementVector;
    

    void Start()
    {

        Destroy(gameObject, 1);
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.LookAt(position);
        movementVector = (position - transform.position).normalized * beamSpeed;
    }


    void FixedUpdate()
    {
        transform.position += movementVector * Time.deltaTime;

        Ray ray = new Ray(transform.position, transform.forward);
        
        
        RaycastHit hitMetal;


        if (Physics.Raycast(ray, out hitMetal, 0.5f))
       
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green);
            

            if (hitMetal.rigidbody)
            {
                if (hitMetal.rigidbody.tag == "Metal")
                {

                   // hitMetal.rigidbody.AddForceAtPosition(Physics.gravity * -10f * Time.deltaTime, hitMetal.point);
                    //hitMetal.rigidbody.AddForce(Physics.gravity * -1f);
                    Destroy(gameObject, 0.5f);
                    //Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);
                }
            }

            else
            {
                Destroy(gameObject, 0.5f);

            }
        }
    }
}
