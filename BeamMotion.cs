using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamMotion : MonoBehaviour
{
    public float beamSpeed;
    public float distance;
    public GameObject Impact;
    private Vector3 movementVector;
    public skeletonController enemyScript;
    public Patrol guardScript;
    public woodenDoor doorScript;

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
        Ray left = new Ray(transform.position, -transform.right);
        Ray right = new Ray(transform.position, transform.right);
        Ray up = new Ray(transform.position, transform.up);
        Ray down = new Ray(transform.position, -transform.up);

        RaycastHit hitMetal;
           

        if (Physics.Raycast(ray, out hitMetal, 1f) ||
            Physics.Raycast(left, out hitMetal, 1f) ||
            Physics.Raycast(right, out hitMetal, 1f) ||
            Physics.Raycast(up, out hitMetal, 1f) ||
            Physics.Raycast(down, out hitMetal, 1f))
        {
            Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);

            if (hitMetal.rigidbody)
            {
                if (hitMetal.rigidbody.tag != "Metal")
                {

                    hitMetal.rigidbody.AddForceAtPosition(transform.forward * 10f, hitMetal.point);
                    //hitMetal.rigidbody.AddRelativeForce(transform.forward * 150f);
                    //hitMetal.rigidbody.AddExplosionForce(500f, hitMetal.point, 5f);
                    Destroy(gameObject, 0.5f);
                }


            }
            if (hitMetal.collider.tag == "Enemy")
            {
                enemyScript = hitMetal.transform.gameObject.GetComponent<skeletonController>();
                enemyScript.damage = 5;
                enemyScript.SetDamage();
                Destroy(gameObject, 0.5f);

            }
            if (hitMetal.collider.tag == "Guard")
            {
                guardScript = hitMetal.transform.gameObject.GetComponent<Patrol>();
                guardScript.damage = 5;
                guardScript.SetDamage();
                Destroy(gameObject, 0.5f);

            }
            if (hitMetal.collider.tag == "door")
            {
                doorScript = hitMetal.transform.gameObject.GetComponent<woodenDoor>();
                doorScript.damage = 10f;
                doorScript.SetDamage();
                Destroy(gameObject, 0.5f);
            }
        }
    }
}
