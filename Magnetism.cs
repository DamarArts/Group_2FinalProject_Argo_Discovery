using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{

    Light lLight;
    public Transform spawnPos;
    private Rigidbody rb;
    public GameObject Impact, Player, GateText, BarrierText, BottleText, pillarText;
    public PlayerMovement playerScript;
    public GameObject gravitationalBeam, kaBottle, urBottle, raBottle;
    public Transform Turbin, Revolver, pickUpPosition, backPack, KaBottles;
    public float fireRate;
    private float nextFire;
    private Vector3 movementVector;
    public float RotationSpeed, incrementalIncrease;
    public float speed;
    public int kaCount, raCount, urCount;
    public GameObject ka1, ra1, ur1;

    void Start()
    {
        kaCount = 0;
        raCount = 0;
        urCount = 0;
        speed = 5f;
        lLight = gameObject.GetComponent<Light>();
        lLight.enabled = false;
        RotationSpeed = 0f;
        RotationSpeed = Mathf.Clamp(RotationSpeed, 0f, 150f);
        incrementalIncrease = 1f;
    }


    void Update()
    {
        if (kaCount > 0)
        {

            ka1.SetActive(true);
        }
        if (raCount > 0)
        {
            ra1.SetActive(true);
        }
        if (urCount > 0)
        {
            ur1.SetActive(true);
        }
        GameObject[] objects = GameObject.FindGameObjectsWithTag("rigidobject");
        Rigidbody[] rigidobjects = new Rigidbody[objects.Length];
        GameObject[] fireobjects = GameObject.FindGameObjectsWithTag("fireobject");
        Rigidbody[] rigidobjectsfire = new Rigidbody[fireobjects.Length];
        BoxCollider[] box = new BoxCollider[fireobjects.Length];
        // Find the rigidbodies restore gravity
        for (var i = 0; i < objects.Length; i++)
        {
            rigidobjects[i] = objects[i].GetComponent<Rigidbody>();
            rigidobjects[i].useGravity = true;
        }
        for (var i = 0; i < fireobjects.Length; i++)
        {
            rigidobjectsfire[i] = fireobjects[i].GetComponent<Rigidbody>();
            rigidobjectsfire[i].useGravity = true;
        }
        var position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 35f);
        position = Camera.main.ScreenToWorldPoint(position);
        transform.LookAt(position);
        movementVector = (position - transform.position).normalized;

        Turbin.Rotate(new Vector3(0, 0, -90) * Time.deltaTime);
        Revolver.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
        Ray ray = new Ray(transform.position, movementVector);
        RaycastHit hitMetal;

        GateText.SetActive(false);
        BottleText.SetActive(false);
        BarrierText.SetActive(false);
        pillarText.SetActive(false);


        if (Physics.Raycast(ray, out hitMetal, 10f))
        {
            if (hitMetal.rigidbody)
            {
                if (hitMetal.collider.CompareTag("Metal"))
                {
                    GateText.SetActive(true);

                }

                if (hitMetal.collider.CompareTag("ka"))
                {

                    BottleText.SetActive(true);


                }
                if (hitMetal.collider.CompareTag("ra"))
                {
                    BottleText.SetActive(true);

                }
                if (hitMetal.collider.CompareTag("ur"))
                {

                    BottleText.SetActive(true);
                }



            }
            if (hitMetal.collider)
            {
                if (hitMetal.collider.CompareTag("pillar"))
                {

                    pillarText.SetActive(true);
                }
                if (hitMetal.collider.CompareTag("barrier"))
                {

                    BarrierText.SetActive(true);
                }
            }
        }











        bool _isFiring = Input.GetKey(KeyCode.E);
         if (_isFiring)
        {
            if (Input.GetKey(KeyCode.E))
            {
                RotationSpeed = RotationSpeed + incrementalIncrease;
                RotationSpeed = Mathf.Clamp(RotationSpeed, 0f, 25f);
                //Turbin.Rotate(Vector3.back * RotationSpeed);
                Turbin.localRotation = Quaternion.Euler(Vector3.back - new Vector3(0, 0, RotationSpeed * RotationSpeed) * Time.time);
                Revolver.localRotation = Quaternion.Euler(Vector3.back + new Vector3(0, 0, RotationSpeed * RotationSpeed) * Time.time);



                lLight.enabled = true;
                    //if (Physics.SphereCast(transform.position, 10f, movementVector, out hitMetal, 30f))
                    if (Physics.Raycast(ray, out hitMetal, 30f))
                    {
                        if (hitMetal.rigidbody)
                        {
                            if (hitMetal.collider.CompareTag("Metal"))
                        {

                                hitMetal.rigidbody.AddForceAtPosition(Physics.gravity * -2f, hitMetal.point);
                                //hitMetal.rigidbody.AddForce(Physics.gravity * -1f);
                                //Destroy(gameObject, 0.5f);
                                Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);
                            }
                            if (hitMetal.rigidbody.tag == "rigidobject")
                            {
                            Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);
                            hitMetal.rigidbody.useGravity = false;
                            hitMetal.rigidbody.position = Vector3.Lerp(hitMetal.rigidbody.position, pickUpPosition.position, speed * Time.deltaTime);
                            }
                            if (hitMetal.rigidbody.tag == "fireobject")
                            {
                                BoxCollider fbox =  hitMetal.rigidbody.gameObject.GetComponent<BoxCollider>();
                                fbox.enabled = true;
                                Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);
                                hitMetal.rigidbody.useGravity = false;
                                hitMetal.rigidbody.position = Vector3.Lerp(hitMetal.transform.position, pickUpPosition.position, speed * Time.deltaTime);
                            }
                            if (hitMetal.collider.CompareTag("ka"))
                            {

                                hitMetal.rigidbody.gameObject.SetActive(false);
                                kaCount = kaCount + 1;


                        }
                            if (hitMetal.collider.CompareTag("ra"))
                            {
                            hitMetal.rigidbody.gameObject.SetActive(false);
                            raCount = raCount + 1;

                            }
                            if (hitMetal.collider.CompareTag("ur"))
                            {

                            hitMetal.rigidbody.gameObject.SetActive(false);
                            urCount = urCount + 1;

                            }
                    }
                    }
                    if (Time.time > nextFire)
                    {
                        nextFire = Time.time + fireRate;
                        Instantiate(gravitationalBeam, transform.position, transform.rotation);
                    }
                
                lLight.enabled = false;
            }

        }
        //-------------------------------------------------
        if (kaCount > 0)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {

                Instantiate(kaBottle, spawnPos.position, spawnPos.rotation);
                kaCount = kaCount - 1;

            }
        }
        else if (kaCount == 0)
        {
            playerScript.hasFlameofKa = false;
            ka1.SetActive(false);
        }
        //-------------------------------------------------
        if (raCount > 0)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {

                Instantiate(raBottle, spawnPos.position, spawnPos.rotation);
                raCount = raCount - 1;

            }
        }
        else if (raCount == 0)
        {
            playerScript.hasFlameofRa = false;
            ra1.SetActive(false);
        }
        //-------------------------------------------------
        if (urCount > 0)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {

                Instantiate(urBottle, spawnPos.position, spawnPos.rotation);
                urCount = urCount - 1;

            }
        }
        else if (urCount == 0)
        {
            playerScript.hasFlameofUr = false;
            ur1.SetActive(false);
        }
        //-------------------------------------------------

    }
}
