using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class MixedIngredients : MonoBehaviour
    {
        public MixData mixData;
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
        public List<IngredientType> ingredients;
    }

}