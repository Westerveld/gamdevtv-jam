using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{

    /// <summary>
    /// Used to mix up the ingredients over time
    /// </summary>
    public class Mixer : InteractArea
    {
        private float timeToComplete = 10f;
        private float defaultIncrement = 2.5f;
        
        public float timerIncrement = 0.25f;
        public float mixSpeed = 5f;
        public GameObject mixedFoodPrefab;
        private bool canPlay = false;

        private MixData currentMix;

        private GameObject mixing;
        public Transform mixingLocation;
        public List<GameObject> activeIngredients = new List<GameObject>();
        public void Setup()
        {
            currentMix = new MixData();
            currentMix.ingredients = new List<IngredientType>();
            canPlay = true;
        }
        
        public void AddIngredient(IngredientType type)
        {
            if (currentMix == null)
            {
                currentMix = new MixData();
            }
            currentMix.ingredients.Add(type);

            if (currentMix.ingredients.Count > 1)
            {
                currentMix.cookCompleteTime = currentMix.mixCompleteTime += defaultIncrement;
                currentMix.cooked = currentMix.mixed = false;
            }
            else
            {
                currentMix.cookCompleteTime = currentMix.mixCompleteTime = timeToComplete;
            }
        }
        
        public GameObject TakeFromMixer(Transform parentObj)
        {
            GameObject tmp = Instantiate(mixedFoodPrefab, parentObj);
            tmp.transform.localPosition = Vector3.zero;
            MixedIngredients ingredients = tmp.GetComponent<MixedIngredients>();
            ingredients.mixData.Copy(currentMix);
            ingredients.Setup(player);
            currentMix.ingredients.Clear();
            for (int i = 0; i < activeIngredients.Count; ++i)
            {
                Destroy(activeIngredients[i]);
            }
            activeIngredients.Clear();
            currentMix = new MixData();
            return tmp;
        }

        void Update()
        {
            if (!canPlay) return;

            if (currentMix.ingredients.Count > 0 && !currentMix.mixed)
            {
                currentMix.mixTime += timerIncrement * Time.deltaTime;
                if (currentMix.mixTime >= currentMix.mixCompleteTime)
                {
                    currentMix.mixed = true;
                }
                mixingLocation.Rotate(new Vector3( 1,1,0), mixSpeed);
            }
        }

        public override bool PlaceItem(GameObject item)
        {
            if (item.GetComponent<Ingredient>())
            {
                AddIngredient(item.GetComponent<Ingredient>().type);
                player.currentObject = null;
                item.transform.parent = mixingLocation;
                item.transform.localPosition = Random.insideUnitSphere * 0.5f;
                item.GetComponent<Ingredient>().rigid.isKinematic = true;
                item.GetComponent<Ingredient>().col.enabled = false;
                activeIngredients.Add(item);
                return true;
            }
            if (item.GetComponent<MixedIngredients>())
            {
                if (!item.GetComponent<MixedIngredients>().mixData.mixed)
                {
                    SetupIngredients(item);
                    return true;
                }
            }

            return base.PlaceItem(item);
        }

        public override GameObject GetItem(Transform parent)
        {
            return TakeFromMixer(parent);
        }

        private void SetupIngredients(GameObject mixedIngredients)
        {
            MixedIngredients mix = mixedIngredients.GetComponent<MixedIngredients>();

            currentMix = mix.mixData;
            mix.enabled = false;
            mixedIngredients.transform.parent = mixingLocation;
            mixedIngredients.transform.localPosition = Random.insideUnitSphere * 0.5f;
            activeIngredients.Add(mixedIngredients);
        }
    }

    
}
