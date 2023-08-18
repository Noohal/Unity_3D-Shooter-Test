using System.Collections;
using UnityEngine;

public class n_PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public n_GunObject currentGun;
    public GameObject wallHitDecal;

    private bool isShooting = false;
    private bool isReloading = false;
    private bool previousShotAttempt;

    private int recoilIndex = 0;
    private float lastTimeShot = 0f;

    private const int LEFT_MOUSE_BUTTON = 0;
    private const int RIGHT_MOUSE_BUTTON = 1;
    private const int MIDDLE_MOUSE_BUTTON = 2;

    private KeyCode reloadButton = KeyCode.R;
    public int primaryFireButton = LEFT_MOUSE_BUTTON;
    private int altFireButton = RIGHT_MOUSE_BUTTON;

    private Transform playerHand;
    private Vector3 originalHandPosition;
    private Vector3 originalCameraPosition;

    private n_PlayerHUD playerHud;
    private n_PlayerStats playerStats;
    private n_PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<n_PlayerMovement>();
        playerStats = GetComponent<n_PlayerStats>();
        playerHud = GetComponent<n_PlayerHUD>();

        playerHand = currentGun.gunPrefab.transform;
        playerHud.UpdateAmmoText(currentGun.currentAmmo, currentGun.reserveAmmo);

        originalHandPosition = playerHand.transform.localPosition;
        originalCameraPosition = playerMovement.playerCamera.transform.localEulerAngles;
    }

    private void Update()
    {
        bool canShoot = currentGun.currentAmmo > 0;
        bool attemptingShot = Input.GetMouseButton(primaryFireButton) && canShoot;
        bool attemptingReload = Input.GetKeyDown(reloadButton);

        if(attemptingReload)
        {
            StartCoroutine(Reload());
        }

        if(attemptingShot && !isReloading)
        {
            isShooting = true;
            HandleShooting();
        } else if(!attemptingShot && isShooting)
        {
            isShooting = false;
            DecayRecoil();
        } else
        {
            float newYRotation = Mathf.Lerp(playerMovement.yRotation, 0f, currentGun.fireRate * Time.deltaTime);
            playerMovement.yRotation -= newYRotation;
        }

        //Debug.DrawLine(firePoint.position, firePoint.forward * currentGun.fireRange);

        Vector3 targetHandPosition = Vector3.Lerp(playerHand.transform.localPosition, originalHandPosition, currentGun.fireRate * Time.deltaTime);
        playerHand.transform.localPosition = targetHandPosition;

        previousShotAttempt = attemptingShot;
    }

    void HandleShooting()
    {
        if(Time.time - lastTimeShot >= 1f / currentGun.fireRate)
        {
            Recoil();

            currentGun.currentAmmo--;

            playerHud.UpdateAmmoText(currentGun.currentAmmo, currentGun.reserveAmmo);

            if(currentGun.currentAmmo <= 0 && currentGun.reserveAmmo > 0)
            {
                StartCoroutine(Reload());
            }

            RaycastHit hit;
            if(Physics.Raycast(firePoint.transform.position, firePoint.forward, out hit, currentGun.fireRange)) {
                if (hit.collider.gameObject.layer == 7) return;

                GameObject wallDecal = Instantiate(wallHitDecal, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(wallDecal, 2f);

                Target target = hit.transform.gameObject.GetComponent<Target>();
                if(target != null)
                {
                    target.TakeDamage(currentGun.damage);
                    if (target.isDead)
                    {
                        playerStats.killCount++;
                        Debug.Log(playerStats.killCount);
                    }
                }
            }
            
            lastTimeShot = Time.time;
        }
    }

    void Recoil()
    {
        playerHand.localPosition -= Vector3.forward * 0.05f;

        if (Time.time - lastTimeShot >= currentGun.recoilResetTime)
        {
            playerMovement.xRotation += currentGun.recoilPattern[0].x;
            recoilIndex = 0;
        } 
        else
        {
            playerMovement.xRotation += currentGun.recoilPattern[recoilIndex].x;
            playerMovement.yRotation += currentGun.recoilPattern[recoilIndex].y;

            if(recoilIndex + 1 <= currentGun.recoilPattern.Length - 1)
            {
                recoilIndex++;
            } else
            {
                recoilIndex = 0;
            }
        }
    }

    void DecayRecoil()
    {
        recoilIndex = 0;
        isShooting = false;

        float convertedCameraXRotation = (originalCameraPosition.x > 180) ? originalCameraPosition.x - 360f : originalCameraPosition.x;
        float step = playerMovement.xRotation - convertedCameraXRotation;

        float smoothXRotation = Mathf.SmoothStep(convertedCameraXRotation, playerMovement.xRotation, currentGun.fireRate * Mathf.Abs(step) * Time.deltaTime);
        playerMovement.xRotation = smoothXRotation;
    }
    
    private IEnumerator Reload()
    {
        isReloading = true;
        yield return new WaitForSeconds(currentGun.reloadTime);

        int ammoDifference = currentGun.magazineCapacity - currentGun.currentAmmo;
        currentGun.currentAmmo = currentGun.magazineCapacity;
        currentGun.reserveAmmo -= ammoDifference;

        if (currentGun.reserveAmmo <= 0) currentGun.reserveAmmo = currentGun.maximumAmmo;

        playerHud.UpdateAmmoText(currentGun.currentAmmo, currentGun.reserveAmmo);
        isReloading = false;

    }
}
