using UnityEngine;

public class PlayerHealthController : MonoBehaviour, IDamagable
{

    public static PlayerHealthController instance;

    public float invicncibleLength = 1f;
    private float invincibleCounter;

    public int maxHealth, currentHealth;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        currentHealth = maxHealth;

        UIController.instance.healthSlider.maxValue = maxHealth;
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text= "HEALTH: " + currentHealth + "/"+maxHealth;


    }


    void Update()
    {
        if (invincibleCounter>0)
        {
            invincibleCounter-=Time.deltaTime;
        }


    }

    public void DamagePlayer (int damageAmount)
    {
        


    }

    public void healPlayer (int healAmount)
    {
        currentHealth += healAmount;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UIController.instance.healthSlider.value = currentHealth;
        UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int damage, bool attackPlayer)
    {
        if (attackPlayer)
        {
            if (invincibleCounter <= 0)
            {
                currentHealth -= damage;

                if (currentHealth <= 0)
                {
                    transform.parent.gameObject.SetActive(false);

                    currentHealth = 0;

                    GameManager.instance.PlayerDied();
                }
            }

            invincibleCounter = invicncibleLength;

            UIController.instance.healthSlider.value = currentHealth;
            UIController.instance.healthText.text = "HEALTH: " + currentHealth + "/" + maxHealth;
        }
    }
}

