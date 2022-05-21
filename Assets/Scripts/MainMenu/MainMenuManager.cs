using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    /// <summary>
    /// Loads random game scene 
    /// </summary>
    public void PlayGame()
    {
        GameInstance.instance.GameEnd();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
