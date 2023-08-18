using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text ammoText;
    [SerializeField] Image abilityOneImage;
    [SerializeField] Image abilityTwoImage;
    [SerializeField] Image abilityThreeImage;
    [SerializeField] Image abilityFourImage;

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        string format = currentHealth + "/" + maxHealth;
        healthText.text = format;
    }

    public void UpdateAmmoText(int currentAmmo, int maxAmmo)
    {
        string format = currentAmmo + "/" + maxAmmo;
        ammoText.text = format;
    }

    public void UpdateAbilityOne(bool onCooldown)
    {
        if(onCooldown)
        {
            abilityOneImage.color = Color.red;
        } else
        {
            abilityOneImage.color = Color.green;
        }
    }

    public void UpdateAbilityTwo(bool onCooldown)
    {
        if (onCooldown)
        {
            abilityTwoImage.color = Color.red;
        }
        else
        {
            abilityTwoImage.color = Color.green;
        }
    }

    public void UpdateAbilityThree(bool onCooldown)
    {
        if (onCooldown)
        {
            abilityThreeImage.color = Color.red;
        }
        else
        {
            abilityThreeImage.color = Color.green;
        }
    }

    public void UpdateAbilityFour(bool onCooldown)
    {
        if (onCooldown)
        {
            abilityFourImage.color = Color.red;
        }
        else
        {
            abilityFourImage.color = Color.green;
        }
    }


}
