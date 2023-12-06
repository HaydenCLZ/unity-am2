using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioSource backgroundMusic; // Reference to your background music AudioSource
    public Slider musicSlider; // Reference to the UI Slider

    private void Start()
    {
        // Set the slider's value to the current music volume
        if (backgroundMusic != null && musicSlider != null)
        {
            musicSlider.value = backgroundMusic.volume;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionGame()
    {
        // Handle options menu (if needed)
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // Method to adjust the background music volume based on the slider value
    public void AdjustMusicVolume()
    {
        if (backgroundMusic != null && musicSlider != null)
        {
            backgroundMusic.volume = musicSlider.value;
        }
    }
}