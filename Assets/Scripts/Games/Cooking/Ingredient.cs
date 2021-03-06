using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class Ingredient : InteractArea, IDroppable
    {
        public IngredientType type;
        public Collider triggerCol;
        public Rigidbody rigid;
        public Collider col;

        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
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


        public void DropItem()
        {
            triggerCol.enabled = true;
            transform.parent = null;
            player.hasObject = false;
            rigid.constraints = RigidbodyConstraints.None;
            rigid.isKinematic = false;
        }
        
    }
}