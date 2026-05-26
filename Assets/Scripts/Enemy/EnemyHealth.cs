using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;
    private bool isDead = false;

    [Header("UI")]
    public Image healthBarFill;
    [Header("Нагорода")]
    public int goldReward = 10;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;
        currentHealth -= damage;

        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = (float)currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        PlayerEconomy economy = FindObjectOfType<PlayerEconomy>();
        if (economy != null)
        {
            economy.AddGold(goldReward);
        }
        AudioManager.Instance.PlaySFX(AudioManager.Instance.enemyDeathSound);
        gameObject.SetActive(false); 
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
        isDead = false;
        
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = 1f; 
        }
    }
}