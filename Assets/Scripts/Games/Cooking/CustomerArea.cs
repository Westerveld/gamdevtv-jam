using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cooking;
using Unity.VisualScripting;
using UnityEngine;

namespace Cooking
{
    public class CustomerArea : InteractArea
    {
        public Vector3 foodPosition;
        public float launchSpeed = 5;
        private CookingManager cManager;
        public AudioClip complete;

        public void Setup(CookingController p, CookingManager manager)
        {
            cManager = manager;
            base.Setup(p);
        }

        public void SetRecipe(Recipe newRecipe)
        {
            for (int i = 0; i < cManager.activeOrders.Length; ++i)
            {
                if(cManager.activeOrders[i] == null)
                {
                    newRecipe.timeAllowed += (cManager.m_BuffAmount * cManager.m_TimerBuff);
                    cManager.activeOrders[i] = newRecipe;
                    return;
                }
                else if (string.IsNullOrEmpty(cManager.activeOrders[i].name))
                {
                    newRecipe.timeAllowed += (cManager.m_BuffAmount * cManager.m_TimerBuff);
                    cManager.activeOrders[i] = newRecipe;
                    return;
                }
            }
        }
        
        public override bool PlaceItem(GameObject item)
        {
            if (cManager.activeOrders.Length == 0)
                return false;
            if (item.GetComponent<CookedFood>() != null)
            {
                CookedFood food = item.GetComponent<CookedFood>();
                for (int i = 0; i < cManager.activeOrders.Length; ++i)
                {
                    bool completedRecipe = true;
                    for (int j = 0; j < food.data.ingredients.Count; ++j)
                    {
                        if (cManager.activeOrders[i].ingredientsNeeded.Contains(food.data.ingredients[j]))
                            continue;
                        else
                        {
                            completedRecipe = false;
                        }
                    }

                    if (completedRecipe)
                    {
                        AudioManager.instance?.PlaySFX(complete);
                        cManager.SuccessfulOrder(i);
                        item.transform.parent = null;
                        item.transform.position = new Vector3(item.transform.position.x, foodPosition.y, foodPosition.z);
                        food.Launch(new Vector3(0.3f, 1f,0f) * launchSpeed);
                        
                        Destroy(item, 3f);
                        return true;
                    }
                }
            }
            return base.PlaceItem(item);
        }
    }
}