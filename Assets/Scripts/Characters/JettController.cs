using UnityEngine;

public class JettController : MonoBehaviour
{
    #region Drift
    public bool isDrifting = false;

    //private float minimumAirTime = 0.2f;
    private float lastTimeGrounded = 0f;
    #endregion

    #region Cloudburst Variables
    public bool isSmoking = false;

    private int maxSmokes = 2;
    private int smokeCharges;
    private float smokeDelay = 0.3f;
    private float lastTimeSmokeEnd = 0f;
    private JettSmoke currentSmoke;
    [SerializeField] GameObject smokeProjectile;
    [SerializeField] Transform smokeFirePoint;
    #endregion

    #region Updraft Variables
    public bool isUpdrafting = false;

    private int maxUpdrafts = 2;
    private int updraftCharges;
    private float updraftHeight = 10.0f;
    private float updraftTime = 0.5f;
    private float lastUpdraft = 0f;
    #endregion

    #region Tailwind Variables
    public bool isDashing = false;

    // private
    private int maxDashes = 1;
    private int dashCharges;
    private float dashSpeed = 30f;
    private float dashDuration = 0.4f;
    private float dashStartTime;
    #endregion

    #region Bladestorm
    public bool isBladestorming = false;

    public const int minimumKillCount = 6;
    public float lastTimeBladeShot = 0f;

    n_GunObject bladestorm;
    #endregion

    n_PlayerHUD playerHUD;
    n_PlayerStats playerStats;
    n_PlayerMovement playerMovement;
    CharacterController characterController;

    void Start()
    {
        playerHUD = GetComponent<n_PlayerHUD>();
        playerStats = GetComponent<n_PlayerStats>();
        playerMovement = GetComponent<n_PlayerMovement>();
        characterController = GetComponent<CharacterController>();

        dashCharges = maxDashes;
        updraftCharges = maxUpdrafts;
        smokeCharges = maxSmokes;

        bladestorm = new JettBladestorm();
        bladestorm.InitializeGun();
    }

    void Update()
    {
        HandleDrift();
        HandleSmoke();
        HandleUpdraft();
        HandleDash();
        Bladestorm();
    }

    #region Drift
    void HandleDrift()
    {
        bool attemptingDrift = Input.GetKey(KeyCode.Space) && playerMovement.transform.position.y > 3f;

        if(attemptingDrift)
        {
            lastTimeGrounded = Time.time;
            Drift();
        } else
        {
            onDriftEnd();
        }
    }

    void Drift()
    {
        playerMovement.normalFalling = false;
        playerMovement.jumpVelocity.y = -4f;
    }

    void onDriftEnd()
    {
        playerMovement.normalFalling = true;
        playerMovement.jumpVelocity.y += playerStats.gravity * Time.deltaTime;
    }
    #endregion

    #region Cloudburst
    void HandleSmoke()
    {
        bool canSmoke = smokeCharges > 0;
        bool attemptingSmoke = Input.GetKeyDown(KeyCode.C) && canSmoke;

        if (!canSmoke) playerHUD.UpdateAbilityOne(true);
        else playerHUD.UpdateAbilityOne(false);

        if (attemptingSmoke && Time.time - lastTimeSmokeEnd >= smokeDelay)
        {
            Smoke();
        }

        if(isSmoking)
        {
            bool isGuided = Input.GetKey(KeyCode.C);
            currentSmoke.SetControl(isGuided);
            bool isStoppingGuide = Input.GetKeyUp(KeyCode.C);

            if(isStoppingGuide)
            {
                onSmokeEnd();
            }
        }
    }

    void Smoke()
    {
        isSmoking = true;
        smokeCharges--;

        GameObject smokeObj = Instantiate(smokeProjectile, smokeFirePoint.position, playerMovement.playerCamera.transform.rotation);
        currentSmoke = smokeObj.GetComponent<JettSmoke>();
        currentSmoke.InitializeValues(false, playerMovement.playerCamera);
    }

    void onSmokeEnd()
    {
        isSmoking = false;
        lastTimeSmokeEnd = Time.time;
        currentSmoke.SetControl(false);
        currentSmoke = null;
    }
    #endregion

    #region Updraft
    void HandleUpdraft()
    {
        bool canUpdraft = updraftCharges > 0;
        bool attemptingUpdraft = Input.GetKeyDown(KeyCode.Q) && canUpdraft;

        if (!canUpdraft) playerHUD.UpdateAbilityTwo(true);
        else playerHUD.UpdateAbilityOne(false);

        if(Time.time - lastUpdraft >= updraftTime)
        {
            onUpdraftEnd();
        }

        if(attemptingUpdraft && canUpdraft)
        {
            onUpdraftStart();

            if (!playerMovement.isGrounded)
            {
                playerMovement.jumpVelocity.y = Mathf.Sqrt((updraftHeight / 2.5f) * -2f * playerStats.gravity);
            }
            else
            {
                playerMovement.jumpVelocity.y = Mathf.Sqrt(updraftHeight * -2f * playerStats.gravity);
            }
        } 
    }

    void onUpdraftStart()
    {
        isUpdrafting = true;
        lastUpdraft = Time.time;
        updraftCharges--;
    }

    void onUpdraftEnd()
    {
        isUpdrafting = false;
    }
    #endregion

    #region Tailwind
    void HandleDash()
    {
        bool canDash = dashCharges > 0;
        bool attemptingDash = Input.GetKeyDown(KeyCode.E) && canDash;

        if (!canDash) playerHUD.UpdateAbilityThree(true);
        else playerHUD.UpdateAbilityThree(false);

        if(attemptingDash && !isDashing)
        {
            if(dashCharges > 0)
            {
                onDashStart();
            }
        }

        if(isDashing)
        {
            if(Time.time - dashStartTime <= dashDuration)
            {
                if(playerMovement.movementVector.Equals(Vector3.zero))
                {
                    characterController.Move(transform.forward * dashSpeed * Time.deltaTime);
                } else
                {
                    characterController.Move(playerMovement.movementVector.normalized * dashSpeed * Time.deltaTime);
                }
            } else
            {
                onDashEnd();
            }
        }
    }

    void onDashStart()
    {
        isDashing = true;
        dashStartTime = Time.time;
        dashCharges--;
    }

    void onDashEnd()
    {
        isDashing = false;
        dashStartTime = 0;
    }
    #endregion

    #region Bladestorm

    /*
        Create the 5 Bladestorm blades (prefab)
        
        Shooting logic should be similar to n_PlayerWeapon.HandleShooting() except:
            - No reloading
            - No recoil
            - Blade projectiles need to move (Maybe for now just have them disappear when you fire)
     */

    void Bladestorm()
    {
        bool canUltimate = playerStats.killCount % minimumKillCount == 0;
        bool attemptingUltimate = Input.GetKeyDown(KeyCode.X) && canUltimate;

        if(attemptingUltimate)
        {
            Debug.Log("WATCH THIS");
            OnBladestorm();
        }

        if(isBladestorming)
        {
            bool attemptingShot = Input.GetMouseButton(0);

            if(attemptingShot)
            {
                HandleBladestorm();
            }
        }
    }

    void HandleBladestorm()
    {
        if(Time.time - lastTimeBladeShot >= 1f / bladestorm.fireRate)
        {
            bladestorm.currentAmmo--;
            playerHUD.UpdateAmmoText(bladestorm.currentAmmo, 0);

            if(bladestorm.currentAmmo <= 0)
            {
                EndBladestorm();
            }

            lastTimeBladeShot = Time.time;
        }
    }

    void OnBladestorm()
    {
        isBladestorming = true;
    }

    void EndBladestorm()
    {
        isBladestorming = false;
    }
    #endregion
}
