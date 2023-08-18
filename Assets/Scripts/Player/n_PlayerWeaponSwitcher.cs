using UnityEngine;

public class n_PlayerWeaponSwitcher : MonoBehaviour
{
    private const int initialWeapon = 0;

    public int currentWeapon;

    public n_PlayerWeapon playerWeapon;

    private void Start()
    {
        currentWeapon = initialWeapon;
        ChooseWeapon();
    }

    private void Update()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        int previousWeapon = currentWeapon;

        if(wheel < 0f)
        {
            if(currentWeapon >= transform.childCount - 1)
            {
                currentWeapon = 0;
            } else
            {
                currentWeapon++;
            }
        }

        if(wheel > 0f)
        {
            if (currentWeapon <= 0)
            {
                currentWeapon = transform.childCount - 1;
            }
            else
            {
                currentWeapon--;
            }
        }

        if(previousWeapon != currentWeapon)
        {
            ChooseWeapon();
        }
    }

    void ChooseWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if(i == currentWeapon)
            {
                weapon.gameObject.SetActive(true);
                n_GunObject gun = weapon.GetComponent<n_GunObject>();
                playerWeapon.currentGun = gun;
                playerWeapon.currentGun.InitializeGun();
            } else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
