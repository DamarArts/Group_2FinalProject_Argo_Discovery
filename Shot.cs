
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{

    private GameObject Player;
    private float _distance;
    private Rigidbody shot;
    public GameObject shotExplosion, Impact, ImpactOther;
    public GameObject ProjectilSound;
    public float speed;
    private Vector3 PlayerPosition;
    private Vector3 CurrentShotPosition;
    private Vector3 movementVector1;
    private Vector3 movementVector2;
    public float _minDist;
    private Vector3 lastPosition;
    private Shot shotScript;
    public PlayerMovement playerScript;

    public float lifetime;

    public bool FollowPlayer, keepGoing, IsTriggered, isMelee;

    private void Start()
    {
        shotScript = GetComponent<Shot>();
        Instantiate(ProjectilSound, transform.position, transform.rotation);
        shot = GetComponent<Rigidbody>();
        Player = GameObject.FindGameObjectWithTag("Player");
        playerScript = Player.GetComponent<PlayerMovement>();
        transform.LookAt(Player.transform);

        Destroy(gameObject, 15f);

    }
    void FixedUpdate()
    {
        PlayerPosition = Player.transform.position;
        movementVector1 = (PlayerPosition - transform.position).normalized * speed;

        shot.velocity = transform.forward * speed;

        _distance = Vector3.Distance(transform.position, Player.transform.position);

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
            //Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);

            if (hitMetal.collider.tag == "Player")
            {
                //hitMetal.collider.AddExplosionForce(50f, hitMetal.point, 5f);
                shot.velocity = new Vector3(0f,0f,0f);
                Instantiate(Impact, hitMetal.point, hitMetal.transform.rotation);
                Destroy(this.gameObject);
                shotScript.enabled = false;
                gameObject.layer = default;
                playerScript.SetAttackDamage();

            }
            else
            {
                Instantiate(ImpactOther, hitMetal.point, hitMetal.transform.rotation);
                shotScript.enabled = false;
                gameObject.layer = default;
                Destroy(this.gameObject);
            }

            /*     private void OnCollisionEnter(Collision collision)
                {
                    if (collision.collider.tag != "Shot" && collision.collider.tag != "Enemy")
                    {
                        //Destroy(this.gameObject);
                        //Instantiate(shotExplosion, transform.position, transform.rotation);

                    }
                    if (collision.collider.tag == "Player")
                    {
                        shotScript.enabled = false;
                    }

            */
        }

    }
}