using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class Oven : InteractArea
    {
        private MixData currentMix;
        private bool canPlay = false;
        public float scaleMultiplier = 1.1f;
        public float lerpSpeed = 1f;

        private Vector3 baseScale, animScale;

        public GameObject cookedPrefab;

        public UIOvenObject m_UIOvenObject;

        public AudioClip place, complete;

        public override void Setup(CookingController p) 
        {
            base.Setup(p);
            currentMix = new MixData();
            canPlay = true;
            baseScale = animScale = transform.localScale;
            animScale.y *= scaleMultiplier;
        }

        public override bool PlaceItem(GameObject item)
        {
            if (item.GetComponent<MixedIngredients>())
            {
                AudioManager.instance?.PlaySFX(place);
                MixedIngredients mix = item.GetComponent<MixedIngredients>();
                if (!mix.mixData.mixed)
                {
                    Debug.Log("Can't place unmixed items into oven");
                    return false;
                }
                currentMix.Copy(mix.mixData);
                Destroy(item);
                allowPickup = false;
                m_UIOvenObject.StartTimer();
                return true;

            }
            
            return false;
        }

        public override GameObject GetItem(Transform parent)
        {
            GameObject tmp = Instantiate(cookedPrefab, parent);
            tmp.transform.localPosition = Vector3.zero;
            CookedFood food = tmp.GetComponent<CookedFood>();
            food.Setup(player);
            food.data.Copy(currentMix);
            currentMix = new MixData();
            m_UIOvenObject.TurnOffUI();
            return tmp;
        }


        private void FixedUpdate()
        {
            if (!canPlay) return;
            
            if (currentMix.ingredients.Count < 1) return;
            
            if (!currentMix.cooked)
            {
                currentMix.cookTime += Time.fixedDeltaTime;
                m_UIOvenObject.SetTimer(currentMix.cookCompleteTime - currentMix.cookTime);
                if (currentMix.cookTime >= currentMix.cookCompleteTime)
                {
                    currentMix.cooked = true;
                    allowPickup = true;
                    AudioManager.instance?.PlaySFX(complete);
                    m_UIOvenObject.OvenReady();
                }

                transform.localScale = Vector3.Lerp(baseScale, animScale, Mathf.Sin(Time.time * lerpSpeed));
            }
            
        }
    }
}