using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirePillarRa : MonoBehaviour
{
    public GameObject Instructions, Info;
    public GameObject Player, Flame;
    private PlayerMovement playerScript;
    //public int kaFlames;
    void Start()
    {
        playerScript = Player.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ra"))
        {
            Destroy(other);
            Flame.SetActive(true);
            playerScript.raFlames = playerScript.raFlames + 1;
        }
    }
}
