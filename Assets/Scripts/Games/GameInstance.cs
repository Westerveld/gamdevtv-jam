using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameInstance : MonoBehaviour
{
    public static GameInstance instance;

    public Volume volume;
    public ChromaticAberration ca;
    public LensDistortion lens;
    public float effectSpeed;

    public GameManager currentGame;

    public PersistantData data;

    private List<bool> completedGames = new List<bool>();

    private string[] scenes = new string[]
    {
        "Cooking", "Dating", "Runner", "Maze", "Souls", "TurnBased", "TwinStick"
    };

    private List<string> availableScenes;

    private List<string> nextScenes = new List<string>();
    Random rand = new Random();

    public AudioClip gameSwap;
    public AudioClip gameWinSFX;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            for (int i = 0; i < ((int)GameType.TwinStick + 1); i++)
            {
                completedGames.Add(false);
            }

            availableScenes = scenes.ToList();
            //Randomise Order
            nextScenes = scenes.OrderBy(x => rand.Next()).ToList();

            volume.profile.TryGet(out ca);
            volume.profile.TryGet(out lens);
            if (currentGame != null)
            {
                StartGame();
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void SetGameComplete(GameType gameType)
    {
        completedGames[(int)gameType] = true;
        availableScenes.Remove(gameType.ToString());
        UIMasterGameManager.instance.SetIconComplete(gameType);
        //AudioManager.instance?.PlaySFX(gameWinSFX);
        foreach (bool game in completedGames)
        {
            if (!game)
            {
                //Randomise Order 
                nextScenes = availableScenes.OrderBy(x => rand.Next()).ToList();
                GameEnd();
                return;
            }
        }
        
        //Load final scene if we completed all games
        if (availableScenes.Count == 0)
        {
            StartCoroutine(LoadEndScene());
        }
    }

    public int GetNumberOfCompletedGames()
    {
        int won = 0;
        for(int i = 0; i < completedGames.Count; i++)
        {
            if(completedGames[i])
            {
                won++;
            }
        }
        return won;
    }

    public void GameEnd()
    {
        AudioManager.instance?.PlaySFX(gameSwap);
        //ToDo: Choose new scene to load, ignoring all completed games
        StartCoroutine(EffectsThenLeave());
    }

    IEnumerator EffectsThenLeave()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        while (lens.intensity.value < 1)
        {
            lens.intensity.value += Time.fixedDeltaTime * effectSpeed;
            ca.intensity.value += Time.fixedDeltaTime * effectSpeed;
            yield return null;
        }

        if (availableScenes.Count > 0)
        {

            SceneManager.LoadScene(nextScenes[0]);
            nextScenes.RemoveAt(0);

            if (nextScenes.Count == 0)
            {
                nextScenes = availableScenes.OrderBy(x => rand.Next()).ToList();
            }

            while (lens.intensity.value > 0)
            {
                lens.intensity.value -= Time.fixedDeltaTime * effectSpeed;
                ca.intensity.value -= Time.fixedDeltaTime * effectSpeed;
                yield return null;
            }
            //yield return new WaitForSeconds(1f);

            currentGame = FindObjectOfType<GameManager>();

            StartGame();
        }
        else
        {
            //Lost game on the final game
            SceneManager.LoadScene("Failed");
        }
    }

    IEnumerator LoadEndScene()
    {
        while (lens.intensity.value < 1)
        {
            lens.intensity.value += Time.fixedDeltaTime * effectSpeed;
            ca.intensity.value += Time.fixedDeltaTime * effectSpeed;
            yield return null;
        }

        SceneManager.LoadScene("Final");
        volume.enabled = false;
        currentGame = FindObjectOfType<GameManager>();
        currentGame.StartGame();
    }
   
    public void SetPersistantData(GameType type, float val1 = 0f, float val2 = 0f)
    {
        switch (type)
        {
            case GameType.Cooking:
                data.ordersFilled = (int)val1;
                break;
            case GameType.Dating:
                data.currentCharm = val1;
                break;
            case GameType.Runner:
                break;
            case GameType.Maze:
                break;
            case GameType.Souls:
                data.currentBossHealth = (int)val1;
                break;
            case GameType.TurnBased:
                data.currentMonsterHealth = (int)val1;
                break;
            case GameType.TwinStick:
                data.killedEnemies = (int)val1;
                break;
        }
    }

    public int GetCompletedGames()
    {
        int completed = 0;
        for (int i = 0; i < completedGames.Count; i++)
        {
            if (completedGames[i])
                completed++;
        }

        return completed;
    }

    private void StartGame()
    {
        switch (currentGame.gameType)
        {
            case GameType.Cooking:
                if(data.didCookingTutorial)
                    currentGame.StartGame();
                else
                {
                    data.didCookingTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.Dating:
                if(data.didDatingTutorial)
                    currentGame.StartGame();
                else
                {
                    data.didDatingTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.Runner:
                if(data.didRunnerTutorial)
                    currentGame.StartGame(data.distance);
                else
                {
                    data.didRunnerTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.Maze:
                if (data.didMazeTutorial)
                    currentGame.StartGame();
                else
                {
                    data.didMazeTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.Souls:
                if(data.didSoulsTutorial)
                    currentGame.StartGame(data.currentBossHealth);
                else
                {
                    data.didSoulsTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.TurnBased:
                if(data.didTurnBasedTutorial)
                    currentGame.StartGame();
                else
                {
                    data.didTurnBasedTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
            case GameType.TwinStick:
                if(data.didTwinStickTutorial)
                    currentGame.StartGame(data.killedEnemies);
                else
                {
                    data.didTwinStickTutorial = true;
                    currentGame.ShowTutorial();
                }
                break;
        }
    }
}


[Serializable]
public class PersistantData
{
    //Dating
    public float currentCharm = 0f;

    public bool didDatingTutorial;
    //Souls
    public float currentBossHealth = 0f;
    public bool didSoulsTutorial;

    //turn based
    public int currentMonsterHealth = 0;
    public bool didTurnBasedTutorial;

    //TwinStick
    public int killedEnemies;
    public bool didTwinStickTutorial;

    //Cooking
    public int ordersFilled;
    public bool didCookingTutorial;
    
    //Runner
    public int distance;
    public bool didRunnerTutorial;

    public bool didMazeTutorial;
}