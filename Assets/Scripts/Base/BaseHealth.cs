using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BaseHealth : MonoBehaviour
{
    [Header("Налаштування здоров'я")]
    public int maxHealth = 20;
    private int currentHealth;

    [Header("UI Елементи")]
    public Text healthText;
    public GameObject gameOverPanel;

    void Start()
    {
        Time.timeScale = 1f; 
        
        currentHealth = maxHealth;
        UpdateHealthUI();
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "HP: " + currentHealth;
        }
    }

    void GameOver()
    {
        
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
            
        Time.timeScale = 0f; 
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}