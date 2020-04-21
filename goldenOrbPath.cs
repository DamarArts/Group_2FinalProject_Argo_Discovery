using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldenOrbPath : MonoBehaviour
{
    private GameObject winOrb;
    private float _distance;
    public float speed;
    private Vector3 winOrbPosition;
    private Vector3 movementVector1;
    //public float time;
    // Start is called before the first frame update
    void Start()
    {
        winOrb = GameObject.FindGameObjectWithTag("Object2");
        if (winOrb == null)
        {
            Debug.Log("no orb found");
        }

        //Invoke("SetPath", 0);
        
    }

    void Update()
    {
        Invoke("SetPath", 1.5f);
        transform.Rotate(new Vector3(20f, 60f, 20f) * Time.deltaTime);

    }
    void SetPath()
    {
        if (winOrb != null)
        {
            _distance = Vector3.Distance(transform.position, winOrb.transform.position);
            winOrbPosition = new Vector3(winOrb.transform.position.x, transform.position.y, winOrb.transform.position.z);
            movementVector1 = (winOrbPosition - transform.position).normalized * speed;
            transform.position += movementVector1 * Time.deltaTime;
        }
        else
        {
            transform.position = transform.position;
        }

    }
}
