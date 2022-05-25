using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{

	public class TwinStickDoor : MonoBehaviour
	{
		public TwinStickManager manager;
		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
			{
				if (other.GetComponent<TwinStickController>())
				{
					if (manager.NoEnemiesLeftToKill())
					{
						manager.GotOut();
					}
				}
			}
		}
	}

}