using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{

    public static GameInstance instance;

    private List<bool> completedGames = new List<bool>();

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
            for (int i = 0; i < ((int)GameType.TwinStick + 1); i++)
            {
                completedGames[i] = false;
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
    }
}
