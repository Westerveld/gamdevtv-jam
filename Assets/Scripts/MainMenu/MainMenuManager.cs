using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int firstScene = 1, lastScene = 7;

    /// <summary>
    /// Loads random game scene 
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene(Random.Range(firstScene, lastScene + 1));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
