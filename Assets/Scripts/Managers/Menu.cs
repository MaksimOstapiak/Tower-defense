using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [Header("Налаштування звуку")]
    public Image muteButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    private bool isMuted = false;

    void Start()
    {
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateMuteIcon();
    }

    private void UpdateMuteIcon()
    {
        if (muteButtonImage != null)
        {
            muteButtonImage.sprite = isMuted ? soundOffSprite : soundOnSprite;
        }
    }

public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
        PlayerPrefs.Save();

        UpdateMuteIcon();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene"); 
    }

}