using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FirePillarUr : MonoBehaviour
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
        if (other.gameObject.CompareTag("ur"))
        {
            Destroy(other);
            Flame.SetActive(true);
            playerScript.urFlames = playerScript.urFlames + 1;
        }
    }
}
