using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{

    /// <summary>
    /// Used to mix up the ingredients over time
    /// </summary>
    public class Mixer : MonoBehaviour
    {
        public List<IngredientType> currentIngredients;
        private float mixPercentage = 0;

        private float timeToComplete = 10f;
        private float defaultIncrement = 2.5f;
        
        private float timerIncrement = 0.25f;
        public GameObject mixedFoodPrefab;
        private bool canPlay = false;

        private MixData currentMix;

        public void Setup()
        {
            currentIngredients = new List<IngredientType>();
            canPlay = true;
        }
        
        public void AddIngredient(IngredientType type)
        {
            currentMix.ingredients.Add(type);

            if (currentIngredients.Count > 1)
            {
                currentMix.cookCompleteTime = currentMix.mixTime += defaultIncrement;
                currentMix.cooked = currentMix.mixed = false;
            }
            else
            {
                currentMix.cookCompleteTime =  currentMix.mixCompleteTime = timeToComplete;
            }
        }
        
        public MixedIngredients TakeFromMixer(Transform parentObj)
        {
            GameObject tmp = Instantiate(mixedFoodPrefab, parentObj);
            MixedIngredients ingredients = tmp.GetComponent<MixedIngredients>();
            ingredients.mixData = currentMix;
            currentIngredients.Clear();
            return ingredients;
        }

        void Update()
        {
            if (!canPlay) return;

            if (currentIngredients.Count > 0 && !currentMix.mixed)
            {
                currentMix.mixTime += timerIncrement * Time.deltaTime;
                if (currentMix.mixTime >= currentMix.mixCompleteTime)
                {
                    currentMix.mixed = true;
                }
            }
        }
    }

    
}
