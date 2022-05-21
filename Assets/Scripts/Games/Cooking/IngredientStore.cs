using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{

    public class IngredientStore : MonoBehaviour
    {
        public IngredientType type;

        public GameObject ingredientPrefab;
        private GameObject tmp;

        public GameObject GetIngredient(Transform parent)
        {
            tmp = Instantiate(ingredientPrefab, parent.position, parent.rotation);
            tmp.transform.parent = parent;
            return tmp;
        }
    }


    public enum IngredientType
    {
        Flour,
        Eggs,
        Milk,
        Chocolate
    }
}