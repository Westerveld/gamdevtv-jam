using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class CookedFood : InteractArea, IDroppable
    {
        public MixData data = new MixData();

        public Collider triggerCol;
        private Rigidbody rigid;

        private void Awake()
        {
            rigid = GetComponent<Rigidbody>();
        }

        public void DropItem()
        {
            transform.parent = null;
            triggerCol.enabled = true;
            player.hasObject = false;
            rigid.constraints = RigidbodyConstraints.None;
            rigid.isKinematic = false;
        }

        public override GameObject GetItem(Transform parent)
        {
            triggerCol.enabled = false;
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            rigid.constraints = RigidbodyConstraints.FreezePosition;
            rigid.isKinematic = true;
            return gameObject;
        }

        public void Launch(Vector3 velocity)
        {
            rigid.isKinematic = false;
            rigid.AddForce(velocity, ForceMode.Impulse);
            rigid.constraints = RigidbodyConstraints.None;
        }
    }
}
