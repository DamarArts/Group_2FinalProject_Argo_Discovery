using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class skeletonController : MonoBehaviour
{
    public float triggerDistance = 25f, attackRange = 10f, normalRange = 6f, quickRange = 3f;
    private Animator anim;
    public float playrate = 1f, nextplay, runSpeed, walkSpeed;
    
    private AudioSource skeleAudio;
    public int attackClip;
    public bool startChase, attacking, switchClip, switchAttack, soundPlayed;

    public Rigidbody skeleton, sword;
    public GameObject canvas;
    public Slider HealthBar;

    //HealthAndStats
    //[SerializeField]
    public float maxHp, currentHp, updatedHp, damage;
    //--------------------------------------------

    private GameObject player;
    int _currentClipIndex, _currentAttackIndex;
    public List<int> AttackCounts;
    float _switchProbability = 0.2f;
    List<Waypoint> PatrolPoints;
    //----------------------------------------------------------------------------
    NavMeshAgent navMeshAgent;
    void Start()
    {
        soundPlayed = false;
        canvas.SetActive(false);
        HealthBar.value = 1;
        maxHp = 150f;
        currentHp = maxHp;
        //currentHp = currentHp;
        startChase = false;
        //ChangeAttack();
        skeleAudio = GetComponent<AudioSource>();
        
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        navMeshAgent = this.GetComponent<NavMeshAgent>();

        anim.SetBool("active", false);

        StartCoroutine("DoCheck");


        if (AttackCounts != null && AttackCounts.Count >= 2)
        {
            _currentAttackIndex = 0;
        }

    }


    void FixedUpdate()
    {
        bool isAlive = currentHp > 0;

        HealthBar.value = CalculateHealth();
        Vector3 noyPlayer = new Vector3(player.transform.position.x, 0f, player.transform.position.z);
        Vector3 noyTransfrom = new Vector3(transform.position.x, 0f, transform.position.z);
        var rotDirection = (noyPlayer - noyTransfrom).normalized;

     if (isAlive)
        {
            var playerDistance = Vector3.Distance(transform.position, player.transform.position);
                anim.SetBool("active", true);

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Resurrection"))
                {
                if (!soundPlayed)
                {
                    skeleAudio.Play();
                    soundPlayed = true;
                }
                }

                if (anim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    startChase = true;
                   // PlaySound();
                }

                if (playerDistance > attackRange)
                {
                    anim.SetBool("heavy", false);
                    anim.SetBool("normal", false);
                    anim.SetBool("quick", false);


                    if (startChase == true)
                    {

                        if (playerDistance < attackRange)
                        {


                            anim.SetBool("walking", true);
                            anim.SetBool("running", false);


                        }
                        else if (playerDistance > attackRange)
                        {

                            anim.SetBool("walking", false);
                            anim.SetBool("running", true);


                        }
                    }

                }
                else if (playerDistance < attackRange && startChase == true)
                {

                    transform.rotation = Quaternion.LookRotation(rotDirection);
                    anim.SetBool("walking", false);
                    anim.SetBool("running", false);
                    SetAttack();
                }
            

            //movement------------------------------------------------------
            bool isWalking = anim.GetCurrentAnimatorStateInfo(0).IsName("walking");
            bool isRunning = anim.GetCurrentAnimatorStateInfo(0).IsName("RunFixed");

            if (startChase == true)
            {
                canvas.SetActive(true);
                //HealthBar.enabled = true;
                Vector3 targetVector = player.transform.position;
                navMeshAgent.SetDestination(targetVector);
                navMeshAgent.speed = walkSpeed;
                transform.rotation = Quaternion.LookRotation(rotDirection);
            }
            else if (startChase == true && isRunning)
            {
                Vector3 targetVector = player.transform.position;
                navMeshAgent.SetDestination(targetVector);
                navMeshAgent.speed = runSpeed;
                transform.rotation = Quaternion.LookRotation(rotDirection);
            }
            //movementEnd------------------------------------------------------
        }
        else
        {
            canvas.SetActive(false);
            anim.SetBool("walking", false);
            anim.SetBool("running", false);
            anim.SetBool("heavy", false);
            anim.SetBool("normal", false);
            anim.SetBool("quick", false);
            anim.SetBool("dead", true);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                anim.enabled = false;
            }
            var collider = GetComponent<CapsuleCollider>();
            collider.enabled = false;
            var skeleCollider = skeleton.gameObject.GetComponent<BoxCollider>();
            sword.transform.parent = null;
            sword.isKinematic = false;
            skeleCollider.enabled = true;
            skeleton.isKinematic = false;
            navMeshAgent.enabled = false;
        }
    }

    IEnumerator DoCheck()
    {
        while (true)
        {
            ChangeAttack();
            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    private void ChangeAttack()
    {
        if (UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            switchAttack = !switchAttack;
        }

        if (switchAttack)
        {
            _currentAttackIndex = (_currentAttackIndex + 1) % AttackCounts.Count;
        }
        else
        {
            if (--_currentAttackIndex < 0)
            {
                _currentAttackIndex = AttackCounts.Count - 1;
            }
        }
        attackClip = AttackCounts[_currentAttackIndex];
    }

    private void SetAttack()
    {
        bool isWalking = anim.GetBool("walking") == true;
        bool isRunning = anim.GetBool("running") == true;
        if(!isWalking && !isRunning)
        {
            if (attackClip == 0)
            {
                anim.SetBool("heavy", true);
                anim.SetBool("normal", false);
                anim.SetBool("quick", false);
            }
            else if (attackClip == 1)
            {
                anim.SetBool("normal", true);
                anim.SetBool("heavy", false);
                anim.SetBool("quick", false);
            }
            else if (attackClip == 2)
            {
                anim.SetBool("quick", true);
                anim.SetBool("normal", false);
                anim.SetBool("heavy", false);
            }
        }
        else
        {
            anim.SetBool("quick", false);
            anim.SetBool("normal", false);
            anim.SetBool("heavy", false);
        }

        return;
    }

    public void SetDamage()
    {
        currentHp = Mathf.Clamp(currentHp, 0f, 150f);
        if (currentHp > 0 && startChase == true)
        {
            currentHp = currentHp - damage;
        }
        
    }
    float CalculateHealth()
    {
        return currentHp / maxHp;
    }
}




