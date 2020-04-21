using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBeam : MonoBehaviour
{
    private AudioSource gunAudio;
    Light lLight;
    public GameObject Impact;
    public GameObject Beam;
    public Transform Turbin, Revolver;
    public float fireRate;
    private float nextFire;
    public float RotationSpeed, incrementalIncrease;
    public PlayerMovement playerScript;


    void Start()
    {
        gunAudio = GetComponent<AudioSource>();
        lLight = gameObject.GetComponent<Light>();
        incrementalIncrease = 1f;
        lLight.enabled = false;
        RotationSpeed = 0f;
        RotationSpeed = Mathf.Clamp(RotationSpeed, 0f, 150f);
    }


    void FixedUpdate()
    {
        bool _isFiring = Input.GetMouseButton(0);
        Turbin.Rotate(new Vector3(0, 0, -90) * Time.deltaTime);
        Revolver.Rotate(new Vector3(0, 0, 90) * Time.deltaTime);
        // Turbin.localRotation = Quaternion.Euler(0f, 0f, 25f);
        if (_isFiring)
        {
            
            //gunAudio.Pause();
            playerScript.SetHeating();
            if (Input.GetMouseButton(0))
            {
                
                RotationSpeed = RotationSpeed + incrementalIncrease;
                RotationSpeed = Mathf.Clamp(RotationSpeed, 0f, 25f);
                //Turbin.Rotate(Vector3.back * RotationSpeed);
                Turbin.localRotation = Quaternion.Euler(Vector3.back - new Vector3(0,0,RotationSpeed * RotationSpeed) * Time.time);
                Revolver.localRotation = Quaternion.Euler(Vector3.back + new Vector3(0, 0, RotationSpeed * RotationSpeed) * Time.time);
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(Beam, transform.position, transform.rotation);
                    gunAudio.Play();
                }

            }


        }
        else
        {
            playerScript.SetCooling();

        }
        
    }

}
