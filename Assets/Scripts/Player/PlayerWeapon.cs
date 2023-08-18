using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    //[SerializeField] Animator handAnimator;
    [SerializeField] Transform firePoint;
    //[SerializeField] Camera weaponCamera;
    //[SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject wallHitDecal;
    [SerializeField] Vector3 originalHandPos;
    [SerializeField] Vector3 originalCameraPos;
    //[SerializeField] GameObject projectile;

    public Transform hand;
    public GunObject currentGun;
    private PlayerMovement playerMovement;
    private PlayerHUD playerHUD;

    public bool isShooting { get; private set; } = false;
    public bool isReloading { get; private set; } = false;
    public bool previousShotAttempt { get; private set; }

    private float lastTimeShot = 0f;
    private int currentRecoilIndex = 0;
    private bool isReverse;

    private int killCount = 0;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHUD = GetComponent<PlayerHUD>();

        originalHandPos = hand.transform.localPosition;
        originalCameraPos = playerMovement.cam.transform.localEulerAngles;
        previousShotAttempt = false;

        currentGun.currentAmmo = currentGun.magazineCapacity;
    }

    private void Update()
    {
        bool canShoot = currentGun.currentAmmo > 0;
        bool attemptingShot = Input.GetMouseButton(0) && canShoot;
        bool attempingReload = Input.GetButtonDown("Reload");


        if (attempingReload)
        {
            StartCoroutine(Reload());
        }

        if (attemptingShot && !isReloading)
        {
            isShooting = true;

            if (currentRecoilIndex == 0)
            {
                originalCameraPos = playerMovement.cam.transform.localEulerAngles;
            }

            HandleShoot();
        }
        else if (attemptingShot == false && previousShotAttempt == true)
        {
            DecayRecoil();
        }
        else
        {
            float newY = Mathf.Lerp(playerMovement.yRotation, 0f, currentGun.fireRate * Time.deltaTime);
            playerMovement.yRotation = newY;
        }

        Vector3 targetHandPos = Vector3.Lerp(hand.transform.localPosition, originalHandPos, currentGun.fireRate * Time.deltaTime);
        hand.transform.localPosition = targetHandPos;

        previousShotAttempt = attemptingShot;
    }

    void HandleShoot()
    {

        if (Time.time - lastTimeShot >= 1f / currentGun.fireRate)
        {
            HandleRecoil();

            currentGun.currentAmmo--;
            playerHUD.UpdateAmmoText(currentGun.currentAmmo, currentGun.maxAmmo);
            if (currentGun.currentAmmo <= 0 && currentGun.maxAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            RaycastHit hit;
            if (Physics.Raycast(firePoint.transform.position, firePoint.transform.TransformDirection(Vector3.forward), out hit, currentGun.range))
            {
                if (hit.collider.gameObject.layer == 7) return;
                GameObject wallDecal = Instantiate(wallHitDecal, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));
                StartCoroutine(DestroyHitObject(wallDecal));

                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    target.TakeDamage(currentGun.damage);
                    if(target.isDead) 
                    { 
                        killCount++;
                        Debug.Log(killCount);
                    }
                }
            }

            lastTimeShot = Time.time;
        }
    }

    private IEnumerator Reload()
    {
        isReloading = true;
        //Debug.Log("Reloading...");
        yield return new WaitForSeconds(currentGun.reloadTime);

        int difference = currentGun.magazineCapacity - currentGun.currentAmmo;
        currentGun.currentAmmo = currentGun.magazineCapacity;
        currentGun.maxAmmo -= difference;
        if (currentGun.maxAmmo <= 0) currentGun.maxAmmo = 100;
        playerHUD.UpdateAmmoText(currentGun.currentAmmo, currentGun.maxAmmo);

        isReloading = false;

    }

    void HandleRecoil()
    {
        hand.localPosition -= Vector3.forward * 0.05f;

        if (Time.time - lastTimeShot >= currentGun.recoilResetTime)
        {
            playerMovement.xRotation += currentGun.recoilPattern[0].x;

            currentRecoilIndex = 0;
        }
        else if (Time.time - lastTimeShot < currentGun.recoilResetTime)
        {
            isReverse = DetermineReverse();
            // Change for recoil
            playerMovement.xRotation += currentGun.recoilPattern[currentRecoilIndex].x;
            if (isReverse)
                playerMovement.yRotation -= currentGun.recoilPattern[currentRecoilIndex].y;
            else
                playerMovement.yRotation += currentGun.recoilPattern[currentRecoilIndex].y;

            if (currentRecoilIndex + 1 <= currentGun.recoilPattern.Length - 1)
            {
                currentRecoilIndex++;
            }
            else
            {
                currentRecoilIndex = 0;
            }

        }
    }

    void DecayRecoil()
    {
        currentRecoilIndex = 0;
        float converedCamX = (originalCameraPos.x > 180) ? originalCameraPos.x - 360 : originalCameraPos.x;
        float step = playerMovement.xRotation - converedCamX;
        //Debug.Log(step);

        float newX = Mathf.SmoothStep(converedCamX, playerMovement.xRotation, currentGun.fireRate * Mathf.Abs(step) * Time.deltaTime);
        playerMovement.xRotation = newX;
    }

    private bool DetermineReverse()
    {
        if (currentRecoilIndex + 1 > currentGun.recoilPattern.Length - 1)
        {
            return true;
        }
        else if (currentRecoilIndex == (currentGun.recoilPattern.Length - 1) * 3 / 4 && isReverse)
        {
            return false;
        }

        return false;
    }

    private IEnumerator DestroyHitObject(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
        Destroy(obj);
    }
}
