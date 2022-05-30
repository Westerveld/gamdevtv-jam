using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{

	public class TwinStickDoor : MonoBehaviour
	{
		public TwinStickManager manager;

		private void FixedUpdate()
		{
			if (Vector3.Distance(transform.position, manager.player.transform.position) < 1f)
			{
				manager.GotOut();
			}
		}
	}

}