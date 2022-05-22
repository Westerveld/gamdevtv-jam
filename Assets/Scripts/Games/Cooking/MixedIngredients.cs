using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class MixedIngredients : InteractArea, IDroppable
    {
        public MixData mixData;
        public Collider triggerCol;

        public void DropItem()
        {
            transform.parent = null;
            triggerCol.enabled = true;
            player.hasObject = false;
        }

        public override GameObject GetItem(Transform parent)
        {
            triggerCol.enabled = false;
            transform.parent = parent;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            return gameObject;
        }
    }

    [Serializable]
    public class MixData
    {
        public bool cooked;
        public float cookTime;
        public float cookCompleteTime;
        
        public bool mixed;
        public float mixTime;
        public float mixCompleteTime;
        public List<IngredientType> ingredients = new List<IngredientType>();

        public void Copy(MixData copy)
        {
            cooked = copy.cooked;
            cookTime = copy.cookTime;
            cookCompleteTime = copy.cookCompleteTime;

            mixed = copy.mixed;
            mixTime = copy.mixTime;
            mixCompleteTime = copy.mixCompleteTime;

            for (int i = 0; i < copy.ingredients.Count; ++i)
            {
                ingredients.Add(copy.ingredients[i]);
            }
        }
    }

}