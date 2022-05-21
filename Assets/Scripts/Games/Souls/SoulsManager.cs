using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsManager : GameManager
{
    public Transform cameraRoot;

    public Transform opponent;

    public Transform player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        LookAt();
    }

    void LookAt()
    {
        player.LookAt(opponent, Vector3.up);
    }
}
