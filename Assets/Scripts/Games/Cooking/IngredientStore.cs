using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{

    public class IngredientStore : InteractArea
    {
        public IngredientType type;

        public GameObject ingredientPrefab;
        private GameObject tmp;

        public override GameObject GetItem(Transform parent)
        {
            tmp = Instantiate(ingredientPrefab, parent.position, parent.rotation);
            tmp.transform.parent = parent;
            tmp.GetComponent<Ingredient>().Setup(player);
            return tmp;
        }
    }


    public enum IngredientType
    {
        Flour,
        Eggs,
        Milk,
        Chocolate,
        Butter
    }

}