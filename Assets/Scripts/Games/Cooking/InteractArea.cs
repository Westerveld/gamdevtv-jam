using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class InteractArea : MonoBehaviour
    {
        protected CookingController player;
        public bool allowPickup = true;
        public virtual void Setup(CookingController p)
        {
            player = p;
        }
        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (player == null)
                    player = other.gameObject.GetComponent<CookingController>();
                player.canInteractWithArea = true;
                player.currentArea = this;
            }
        }

        protected void OnTriggerStay(Collider other)
        {
            OnTriggerEnter(other);
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (player == null)
                    player = other.gameObject.GetComponent<CookingController>();
                player.canInteractWithArea = false;
                player.currentArea = null;
            }
        }

        public virtual GameObject GetItem(Transform parent)
        {
            return null;
        }

        public virtual bool PlaceItem(GameObject item)
        {
            return false;
        }
    }
}