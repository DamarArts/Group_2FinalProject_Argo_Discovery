using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swordController : MonoBehaviour
{
    public GameObject Impact;
    public PlayerMovement playerScript;


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            playerScript.SetAttackDamage();
            Instantiate(Impact, transform.position, transform.rotation);
        }
    }


}
