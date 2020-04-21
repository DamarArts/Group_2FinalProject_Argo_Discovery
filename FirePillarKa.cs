using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirePillarKa : MonoBehaviour
{
    public GameObject Instructions, Info;
    public GameObject Player, Flame;
    private PlayerMovement playerScript;
    //public int kaFlames;
    void Start()
    {
        playerScript = Player.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ka"))
        {
            Destroy(other);
            Flame.SetActive(true);
            playerScript.kaFlames = playerScript.kaFlames + 1;
        }
    }
}
