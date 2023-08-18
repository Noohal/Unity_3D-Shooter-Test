using UnityEngine;
using UnityEngine.UI;

public class n_PlayerHUD : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text ammoText;

    [SerializeField] Image abilityOneImage;
    [SerializeField] Image abilityTwoImage;
    [SerializeField] Image abilityThreeImage;
    [SerializeField] Image abilityFourImage;

    public void UpdateHealthText(int currentHealth, int maxHealth)
    {
        string formatted = currentHealth + "/" + maxHealth;
        healthText.text = formatted;
    }

    public void UpdateAmmoText(int currentAmmo, int maximumAmmo)
    {
        string formatted = currentAmmo + "/" + maximumAmmo;
        ammoText.text = formatted;
    }

    public void UpdateAbilityOne(bool onCooldown)
    {
        if (onCooldown)
        {
            abilityOneImage.color = Color.red;
        }
        else
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