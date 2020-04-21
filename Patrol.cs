using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    bool _patrolWaiting;

    [SerializeField]
    float _totalWaitTime = 3f;

    [SerializeField]
    float _switchProbability = 0.2f;

    [SerializeField]
    List<Waypoint> _patrolPoints;
    //----------------------------------------------------------------------------
    NavMeshAgent _navMeshAgent;
    //-----------------------------------------------------------------------------
    public GameObject shot, closeShot, deathExplosion;
    public PlayerMovement _playerScript;
    public Transform Player, shotSpawn;
    //-----------------------------------------------------------------------------
    public bool _travelling, _waiting, _patrolForward, _chasing, _isRegening, _chasingPlay, _roamingPlay, _rechargPlay, isAlarmed;
    int _currentPatrolIndex;
    //-----------------------------------------------------------------------------
    public float _waitTimer, _Distance, nextFire, damage, currentHealth, maxHealth;
    public float MinDistance, fireRate, GuardSpeed, GuardAccel, GuardStop, shootDistance, regeOverTime;
    //-----------------------------------------------------------------------------
    public Slider HealthBar;
    //-----------------------------------------------------------------------------
    private Color32 _red, _blue, _blueLight;
    private Color _redEmissions, _blueEmissions;
    public Renderer rend, rend2, rend3;
    public Light _lit;
    //-----------------------------------------------------------------------------

    public AudioSource guardAudio;
    public AudioClip roaming;
    public AudioClip chasing;
    public AudioClip resetting;
    //public MainMenu MenuScript;
    private GameObject PlayShot;
    private float shotDistance;
    public void Start()
    {
        isAlarmed = false;
        _chasingPlay = false;
        _roamingPlay = false;
        _rechargPlay = false;
        _chasing = false;
        _travelling = false;
        _isRegening = false;

        GuardAccel = 30f;
        GuardSpeed = 25f;
        GuardStop = 7f;
        fireRate = 1f;
        MinDistance = 15f;
        regeOverTime = 20f;

        guardAudio = GetComponent<AudioSource>();        
        maxHealth = 100f;
        currentHealth = maxHealth;
        HealthBar.value = 1;
        //---------------------------------------------
        _blue = new Color(255f, 255f, 255f, 255f);
        _blueLight = new Color(0f, 160f, 255f, 255f);
        _red = new Color(255f, 0f, 0f, 255f);
        _redEmissions = new Color(1f, 0f, 0f);
        _blueEmissions = new Color(0f, 0.7f, 1f);
        //--------------------------------------------- 
        _Distance = Vector3.Distance(transform.position, Player.transform.position);
        _navMeshAgent = this.GetComponent<NavMeshAgent>();



        if (_navMeshAgent == null)
        {
            Debug.LogError("The nav mesh component is not attached to " + gameObject.name);
        }
        else
        {
            if (_patrolPoints != null && _patrolPoints.Count >= 2)
            {
                _currentPatrolIndex = 0;
                SetDestination();
            }
            else
            {
                Debug.Log("insufficient patrol points for basic patrolling behavior.");
            }
        }
        _navMeshAgent.updateRotation = false;
    }

    public void Update()
    {
        //var _playerPos = playerBody.transform.position;
        //shotSpawn.Rotate(Player.position.y, Player.position.x, 0f);
        //shotSpawn.LookAt(Player);
        //shotSpawn.Rotate(Player.position);
        HealthBar.value = CalculateHealth();
        _Distance = Vector3.Distance(transform.position, Player.transform.position);
        
        bool isActive = currentHealth > 0.0f;
        //--------------------------------------------------------------------------

        if (currentHealth == maxHealth)
        {
            _isRegening = false;
        }
        else if (currentHealth <= 0.0f)
        {
            Instantiate(deathExplosion, transform.position, transform.rotation);
            Destroy(this.gameObject);
            
            
        }

        if (isActive && _isRegening == false)
        {

            if (_chasing == false)
            {
                _lit.color = _blueLight;
                _navMeshAgent.angularSpeed = 12;
                _navMeshAgent.speed = 20;
                _navMeshAgent.acceleration = 20;
                _navMeshAgent.stoppingDistance = 0;
                //--------------------------------------------------------
                rend.material.SetColor("_Color", _blue);
                rend.material.SetColor("_EmissionColor", _blueEmissions);
                rend2.material.SetColor("_Color", _blue);
                rend2.material.SetColor("_EmissionColor", _blueEmissions);
                rend3.material.SetColor("_Color", _blue);
                rend3.material.SetColor("_EmissionColor", _blueEmissions);
                //--------------------------------------------------------

                if (_travelling && _navMeshAgent.remainingDistance <= 1.0f)
                {
                    _travelling = false;


                    if (_patrolWaiting)
                    {
                        _waiting = true;
                        _waitTimer = 0f;
                    }
                    else
                    {
                        ChangePatrolPoint();
                        SetDestination();
                    }
                }
                if (_waiting)
                {
                    _waitTimer += Time.deltaTime;
                    if (_waitTimer >= _totalWaitTime)
                    {
                        _waiting = false;

                        ChangePatrolPoint();
                        SetDestination();
                    }
                }
            }
            /*if (_isRegening == false)
            {
                isAlarmed = true;
            }
            */
            if ((Vector3.Distance(transform.position, Player.transform.position) <= MinDistance))
            {

                    _chasing = true;
      
            }

            if (_chasing == true)
            {

                SetChase();
            }

        }
/*
        else if (_isRegening == true)

        {
            _chasing = false;
            isAlarmed = false;
            regenHealth();
            _lit.color = Color.green;
            //---------------------------------------------------------
            rend.material.SetColor("_Color", _blue);
            rend.material.SetColor("_EmissionColor", _blueEmissions);
            rend2.material.SetColor("_Color", _blue);
            rend2.material.SetColor("_EmissionColor", _blueEmissions);
            rend3.material.SetColor("_Color", _blue);
            rend3.material.SetColor("_EmissionColor", _blueEmissions);
            //---------------------------------------------------------
        }
        */

        if (_chasing == true && _chasingPlay == false && _isRegening == false)
        {
            guardAudio.Stop();
            guardAudio.clip = chasing;
            guardAudio.Play();
            _chasingPlay = true;
            _roamingPlay = false;
            _rechargPlay = false;

        }
        if (_travelling == true && _roamingPlay == false)
        {

            guardAudio.Stop();
            guardAudio.clip = roaming;
            guardAudio.Play();
            _roamingPlay = true;
            _rechargPlay = false;
            _chasingPlay = false;

        }

        if (_isRegening == true && _rechargPlay == false)
        {
            guardAudio.Stop();
            guardAudio.clip = resetting;
            guardAudio.Play();
            _rechargPlay = true;
            _chasingPlay = false;
            _roamingPlay = false;
        }

    }

    private void SetChase()
    {
        isAlarmed = false; 

        _navMeshAgent.angularSpeed = 900;
        _navMeshAgent.speed= GuardSpeed;
        _navMeshAgent.acceleration = GuardAccel;
        _navMeshAgent.stoppingDistance = GuardStop;
        _lit.color = _red;
        rend.material.SetColor("_Color", _red);
        rend.material.SetColor("_EmissionColor", _redEmissions);
        rend2.material.SetColor("_Color", _red);
        rend2.material.SetColor("_EmissionColor", _redEmissions);
        rend3.material.SetColor("_Color", _red);
        rend3.material.SetColor("_EmissionColor", _redEmissions);

        Vector3 targetVector = Player.transform.position;
        _navMeshAgent.SetDestination(targetVector);
        // _navMeshAgent.updateRotation = true;
        //transform.rotation = Quaternion.LookRotation(_navMeshAgent.destination);
        Vector3 noyPlayer = new Vector3(Player.transform.position.x, 0f , Player.transform.position.z);
        Vector3 noyTransfrom = new Vector3(transform.position.x, 0f, transform.position.z);
        var rotDirection = (noyPlayer - noyTransfrom).normalized;

        transform.rotation = Quaternion.LookRotation(rotDirection);
           // 
        //Quaternion.RotateTowards(transform.rotation, Player.rotation, 45f);
        _chasing = true;
        _travelling = false;
        _isRegening = false;

       
        Ray playerPosition = new Ray(shotSpawn.position, shotSpawn.forward);
        RaycastHit PlayerHit;
        if (Physics.Raycast(playerPosition, out PlayerHit, 35f))
        {
            if (PlayerHit.collider.tag == "Player")
            {
                //anim.SetBool("attacking", true);
                if (Time.time > nextFire)
                {

                    nextFire = Time.time + fireRate;
                    Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
                }
            }


        }
/*
            else
            {
                if (Time.time > nextFire)
                {
                nextFire = Time.time + fireRate;
                Instantiate(closeShot, shotSpawn.position, shotSpawn.rotation);
                }
            }
*/

    }

    private void SetDestination()
    {

        Vector3 targetVector = _patrolPoints[_currentPatrolIndex].transform.position;
        _navMeshAgent.SetDestination(targetVector);
        _travelling = true;
        _chasing = false;
        _isRegening = false;
        isAlarmed = false;
    }
    private void ChangePatrolPoint()
    {
        if ( UnityEngine.Random.Range(0f, 1f) <= _switchProbability)
        {
            _patrolForward = !_patrolForward;
        }

        if (_patrolForward)
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
        }
        else
        {
            if (--_currentPatrolIndex < 0)
            {
                _currentPatrolIndex = _patrolPoints.Count - 1;
            }
        }
    }
    /*
    void regenHealth()
    {
        isAlarmed = false;
        currentHealth = Mathf.Clamp(currentHealth + (regeOverTime * Time.deltaTime), 0.0f, maxHealth);
        if (currentHealth == maxHealth)
        {
            SetDestination();
        }
    }
    */
    public void SetDamage()
    {
        currentHealth = currentHealth - damage;
    }
    float CalculateHealth()
    {
        return currentHealth / maxHealth;
    }
}
