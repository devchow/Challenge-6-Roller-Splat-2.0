using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Menu Click Sounds")]
    public AudioSource playBtn;
    public AudioSource quitBtn;
    
    // Play Button
    public void Play()
    {
        playBtn.Play();
        SceneManager.LoadScene("Level-1");
    }
    
    // Quit Button
    public void QuitGame()
    {
        playBtn.Play();
        Application.Quit();
    }
    
    // Load Menu
    public void Menu()
    {
        playBtn.Play();
        SceneManager.LoadScene("Main-Menu");
    }
}
