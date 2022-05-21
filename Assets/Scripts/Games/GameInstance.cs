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
            }
        }
        
        //Load final scene if we completed all games
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
    }
}
