using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class woodenDoor : MonoBehaviour
{
    public float maxHp , currentHp;
    public float damage;
    public Slider hpBar;
    private GameObject player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        maxHp = 100f;
        currentHp = maxHp;
        hpBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentHp <= 0)
        {
            hpBar.gameObject.SetActive(false);
            gameObject.tag= "rigidobject";
        }

        var distance = Vector3.Distance(transform.position, player.transform.position);
        if(currentHp > 0)
        {
            if (distance < 15f)
            {
                hpBar.value = CalculateHealth();
                hpBar.gameObject.SetActive(true);
            }
            else
            {
                hpBar.gameObject.SetActive(false);
            }
            
        }

    }
    public void SetDamage()
    {
        currentHp = currentHp - damage;
    }
    float CalculateHealth()
    {
        return currentHp / maxHp;
    }
}
