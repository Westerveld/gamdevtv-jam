using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinStick
{
	public class TwinStickDoor : MonoBehaviour
	{
		public bool m_CanLeave = false;
		public float m_Distance = 5f;

		public GameObject[] m_Doors;
		public Collider m_Col;

		public TwinStickManager manager;

		private void FixedUpdate()
		{
			if (m_CanLeave)
			{
				if (Vector3.Distance(transform.position, manager.player.transform.position) < m_Distance)
				{
					manager.GotOut();
				}
			}
		}

		public void OpenDoors()
        {
			for(int i = 0; i < m_Doors.Length; i++)
            {
				m_Doors[i].gameObject.SetActive(false);
			}
			m_Col.enabled = false;
			m_CanLeave = true;
        }
	}

}