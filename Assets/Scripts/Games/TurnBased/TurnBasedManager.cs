using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnBased
{
    public class TurnBasedManager : GameManager
    {


        public void PlayCard(int number)
        {
            Debug.Log($"Playing card {number}");
        }
    }

}