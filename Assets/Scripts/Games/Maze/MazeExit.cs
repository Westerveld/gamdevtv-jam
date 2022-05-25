using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maze
{
    public class MazeExit : MonoBehaviour
    {
	    public MazeManager manager;
	    public void OnTriggerEnter(Collider other)
	    {
		    if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
		    {
			    manager.Exit();
		    }
	    }
    }

}