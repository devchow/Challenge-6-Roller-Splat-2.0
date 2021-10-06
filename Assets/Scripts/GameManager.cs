using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Singleton;  // Creating only one instance of the Game Manager

    private GroundPiece[] _allGroundPieces;  // An array of all ground pieces
    
    private void Start()
    {
        SetUpNewLevel();
    }

    private void SetUpNewLevel()
    {
        _allGroundPieces = FindObjectsOfType<GroundPiece>();
    }

    private void Awake()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else if (Singleton != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetUpNewLevel();
    }

    public void CheckComplete()
    {
        bool isFinished = true;
        for (int i = 0; i < _allGroundPieces.Length; i++)
        {
            if (_allGroundPieces[i].isColored == false)
            {
                isFinished = false;
                break;
            }
        }

        if (isFinished)
        {

            // Load Next Level
            NextLevel();

        }
    }

    private void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 10)
        {
            // Load the Thank you Scene
            SceneManager.LoadScene("Thank You");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
