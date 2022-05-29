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
        "CookingGame", "DatingGame", "FiniteRunner", "Maze", "Souls", "TurnBased", "TwinStick"
    };

    private List<string> availableScenes;

    private List<string> nextScenes = new List<string>();
    Random rand = new Random();

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
                switch (currentGame.gameType)
                {
                    case GameType.Cooking:
                        currentGame.StartGame(data.ordersFilled);
                        break;
                    case GameType.Dating:
                        currentGame.StartGame(data.currentCharm);
                        break;
                    case GameType.Runner:
                        currentGame.StartGame(data.distance);
                        break;
                    case GameType.Maze:
                        currentGame.StartGame();
                        break;
                    case GameType.Souls:
                        currentGame.StartGame(data.currentBossHealth);
                        break;
                    case GameType.TurnBased:
                        currentGame.StartGame();
                        break;
                    case GameType.TwinStick:
                        currentGame.StartGame(data.killedEnemies);
                        break;
                }

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
        availableScenes.RemoveAt((int)gameType);
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

    public void GameEnd()
    {
        //ToDo: Choose new scene to load, ignoring all completed games
        StartCoroutine(EffectsThenLeave());
    }

    IEnumerator EffectsThenLeave()
    {
        while (lens.intensity.value < 1)
        {
            lens.intensity.value += Time.fixedDeltaTime * effectSpeed;
            ca.intensity.value += Time.fixedDeltaTime * effectSpeed;
            yield return null;
        }

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
        
        switch (currentGame.gameType)
        {
            case GameType.Cooking:
                currentGame.StartGame(data.ordersFilled);
                break;
            case GameType.Dating:
                currentGame.StartGame(data.currentCharm);
                break;
            case GameType.Runner:
                currentGame.StartGame(data.distance);
                break;
            case GameType.Maze:
                currentGame.StartGame();
                break;
            case GameType.Souls:
                currentGame.StartGame(data.currentBossHealth);
                break;
            case GameType.TurnBased:
                currentGame.StartGame();
                break;
            case GameType.TwinStick:
                currentGame.StartGame(data.killedEnemies);
                break;
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
}


[Serializable]
public class PersistantData
{
    //Dating
    public float currentCharm = 0f;
    //Souls
    public float currentBossHealth = 0f;

    //turn based
    public int currentMonsterHealth = 0;

    //TwinStick
    public int killedEnemies;

    //Cooking
    public int ordersFilled;
    
    //Runner
    public int distance;
}