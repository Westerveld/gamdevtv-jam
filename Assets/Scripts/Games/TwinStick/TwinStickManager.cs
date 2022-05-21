using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinStickManager : GameManager
{

    public TwinStickController player;
    
    // Start is called before the first frame update
    void Start()
    {
        player.SetupPlayer();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
