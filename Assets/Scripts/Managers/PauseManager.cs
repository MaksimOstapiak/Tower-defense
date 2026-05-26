using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseManager : MonoBehaviour
{
    [Header("UI Елементи")]
    public GameObject pausePanel;
    public TextMeshProUGUI speedButtonText;
    
    private bool isPaused = false;
    private bool isFastForward = false;

    void Start()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) 
                ResumeGame();
            else 
                PauseGame();
        }
    }


    public void PauseGame()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        
        Time.timeScale = isFastForward ? 2f : 1f; 
    }

    public void ToggleSpeed()
    {
        if (isPaused) return;

        isFastForward = !isFastForward;

        if (isFastForward)
        {
            Time.timeScale = 2f;
            if (speedButtonText != null) speedButtonText.text = "x2";
        }
        else
        {
            Time.timeScale = 1f;
            if (speedButtonText != null) speedButtonText.text = "x1";
        }
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuScene");
    }
}