using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    
    public CharacterController controller;
    //____________________________________FLOATS_____________________________________________________________
    public float    RotationSpeed, incrementalIncrease, pushPower = 2.0f, 
                    jumpHeight = 3f, gravity = -9.81f, Speed = 12f, 
                    triggerDistance, groundDistance = 0.4f;
    public int urFlames, kaFlames, raFlames;
    //_______________________________________________________________________________________________________
    //____________________________________HEALTHSTATS________________________________________________________
    public float maxHp, heat, fuel, currentHp, damage, heatdamage, MaxHeat = 100f, MinHeat = 0f;
    //_______________________________________________________________________________________________________
    //____________________________________BOOLS______________________________________________________________
    public bool isGrounded, gameWon, isDead, hasFlameofUr, hasFlameofRa, hasFlameofKa;
    //_______________________________________________________________________________________________________
    //____________________________________TRANSFORMS_________________________________________________________
    public Transform groundCheck, Turbin, gate;
    //_______________________________________________________________________________________________________
    //____________________________________GAME OBJECTS_______________________________________________________
    public GameObject[] skeletons;
    public GameObject  flameOfUr, flameOfRa, flameOfKa , magneticWeapon, kaBarrier, urBarrier, raBarrier;
    //________________________________________________________________________________________________________
    //____________________________________LAYERS______________________________________________________________
    public LayerMask groundMask;
    //________________________________________________________________________________________________________
    //____________________________________VECTORS_____________________________________________________________
    private Vector3 move, velocity;
    //________________________________________________________________________________________________________
    //____________________________________TEXT MESH PRO_______________________________________________________
    public TextMeshProUGUI Warning, Log;
    //________________________________________________________________________________________________________
    //____________________________________SCRIPTS_____________________________________________________________
    public skeletonController[] skeletonScript;
    public Magnetism magnetismScript;
    //________________________________________________________________________________________________________

    public float Stamina = 100.0f;
    public float MaxStamina = 100.0f;

    //---------------------------------------------------------
    private float StaminaRegenTimer = 0.0f;
    //---------------------------------------------------------
    private const float StaminaDecreasePerFrame = 20f;
    private const float StaminaIncreasePerFrame = 10f;
    private const float StaminaTimeToRegen = 1.5f;

    //________________________________________________________________________________________________________
    Color32 orange = new Color(255, 89, 11, 255);
    Color32 invis = new Color(255, 89, 11, 0);

    private void Start()
    {
        Warning.enabled = true;
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Warning.color = invis;
        urFlames = 0;
        skeletons = GameObject.FindGameObjectsWithTag("Enemy");      
        skeletonScript = new skeletonController[skeletons.Length];
        magnetismScript = magneticWeapon.GetComponent<Magnetism>();
        // Find the skeletons
        for (var i = 0; i < skeletons.Length; i++)
        {
            skeletonScript[i] = skeletons[i].GetComponent<skeletonController>();
            skeletonScript[i].enabled = false;
        }
    

        maxHp = 200f;
        currentHp = maxHp;
       // gateText.SetActive(false);
        incrementalIncrease = 1f;
        RotationSpeed = 0f;
       
    }
    void Update()
    {

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        if (isRunning && Stamina > 0)
        {
            Speed = 30f;
            Stamina = Mathf.Clamp(Stamina - (StaminaDecreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            StaminaRegenTimer = 0.0f;
        }
        else if (Stamina < MaxStamina)
        {
            Speed = 22f;
            if (StaminaRegenTimer >= StaminaTimeToRegen)
                Stamina = Mathf.Clamp(Stamina + (StaminaIncreasePerFrame * Time.deltaTime), 0.0f, MaxStamina);
            else
                StaminaRegenTimer += Time.deltaTime;
        }

        if (kaFlames == 4)
        {
            kaBarrier.SetActive(false);
        }
        if (magnetismScript.kaCount == 0)
        {
            hasFlameofKa = false;
            
        }
        else
        {
            hasFlameofKa = true;
        }

        //------------------------------
        if (urFlames == 1)
        {
            urBarrier.SetActive(false);
        }
        if (magnetismScript.urCount == 0)
        {
            hasFlameofUr = false;

        }
        else
        {
            hasFlameofUr = true;
        }
        //--------------------------------
        if (raFlames == 3)
        {
            raBarrier.SetActive(false);
        }
        if (magnetismScript.raCount == 0)
        {
            hasFlameofRa = false;

        }
        else
        {
            hasFlameofRa = true;
        }
        //-----------------------------------
        bool isAlive = currentHp > 0;
        if (isAlive)
        {
            //skeleton detection
            for (var i = 0; i < skeletons.Length; i++)
            {
                var skeletonDistance = Vector3.Distance(transform.position, skeletons[i].gameObject.transform.position);
                skeletonScript[i] = skeletons[i].GetComponent<skeletonController>();
                if (skeletonDistance < triggerDistance)
                skeletonScript[i].enabled = true;
            }

        
            damage = Random.Range(10f, 15f);
            if (heat > 70)
            {
                
                

                Color32 lerpedColor = Color32.Lerp(Warning.color, orange, Time.deltaTime);
                Warning.color = lerpedColor;
                Warning.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, Mathf.PingPong(Time.time, 1));
                //Warning.fontSize = 49;

            }
            else
            {
                Color32 lerpedColor = Color32.Lerp(Warning.color, invis,  Time.deltaTime);
                Warning.color = lerpedColor;
                //Warning.enabled = false;
            }
            if (heat == MaxHeat)
            {
                SetHeatDamage();
            }


            Turbin.Rotate(new Vector3(0, 0, -90) * Time.deltaTime);

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            {

                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }
                float x = Input.GetAxis("Horizontal");
                float z = Input.GetAxis("Vertical");
                move = transform.right * x + transform.forward * z;
                controller.Move(move * Speed * Time.deltaTime);
            
                if (Input.GetButtonDown("Jump") && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                velocity.y += gravity * Time.deltaTime;

                controller.Move(velocity * Time.deltaTime);
            }
            bool _isMoving = move.x != 0;
            if (_isMoving)
            {
                RotationSpeed = RotationSpeed + incrementalIncrease;
                RotationSpeed = Mathf.Clamp(RotationSpeed, 0f, 25f);
                Turbin.localRotation = Quaternion.Euler(Vector3.back - new Vector3(0, 0, RotationSpeed * RotationSpeed) * Time.time);
            }
        

        }

        else
        {
            isDead = currentHp <= 0;
            PlayerMovement playerScript = GetComponent<PlayerMovement>();
            playerScript.enabled = false;
            gameWon = false;
        }
        if (Input.GetKey(KeyCode.F))
        {
            //gateText.SetActive(false);
            //reachText.SetActive(false);
            //finishText.SetActive(false);

        }
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        if (hit.moveDirection.y < -0.3)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        body.velocity = pushDir * pushPower;
    }

    private void OnTriggerEnter(Collider other)
    {

            if (other.gameObject.CompareTag("orb"))
        {
             gameWon = true;
        }
    }

    void SetHeatDamage()
    {
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
        if (currentHp > 0)
        {
            //currentHp = currentHp - heatdamage;

            const float HeatDecreasePerFrame = 10f;
            //const float HeatIncreasePerFrame = 1f;
            //const float HeatTimeToRegen = 1.5f;

            currentHp = Mathf.Clamp(currentHp- (HeatDecreasePerFrame * Time.deltaTime), 0f, maxHp);
        }
    }
    public void SetAttackDamage()
    {
        currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
        if (currentHp > 0)
        {
            currentHp = currentHp - damage;
        }
    }
    public void SetHeating()
    {
     //float HeatRegenTimer = 0.0f;


     //const float HeatDecreasePerFrame = 20f;
     const float HeatIncreasePerFrame = 10f;
     //const float HeatTimeToRegen = 1.5f;

        heat = Mathf.Clamp(heat + (heatdamage* Time.deltaTime), MinHeat, MaxHeat);
    }
    public void SetCooling()
    {
        //float HeatRegenTimer = 0.0f;

        const float HeatDecreasePerFrame = 10f;
        //const float HeatIncreasePerFrame = 1f;
        //const float HeatTimeToRegen = 1.5f;

        heat = Mathf.Clamp(heat - (HeatDecreasePerFrame * Time.deltaTime), MinHeat, MaxHeat);
    }
}
