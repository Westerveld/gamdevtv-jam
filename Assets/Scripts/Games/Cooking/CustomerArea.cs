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

        public void Setup(CookingController p, CookingManager manager)
        {
            cManager = manager;
            base.Setup(p);
        }

        public void SetRecipe(Recipe newRecipe)
        {
            cManager.activeOrders.Add(newRecipe);
        }
        
        public override bool PlaceItem(GameObject item)
        {
            if (cManager.activeOrders.Count == 0)
                return false;
            if (item.GetComponent<CookedFood>() != null)
            {
                CookedFood food = item.GetComponent<CookedFood>();
                
                for (int i = 0; i < cManager.activeOrders.Count; ++i)
                {
                    bool completedRecipe = true;
                    for (int j = 0; j < cManager.activeOrders[i].ingredientsNeeded.Length; ++j)
                    {
                        if (food.data.ingredients.Contains(cManager.activeOrders[i].ingredientsNeeded[j]))
                            continue;
                        else
                        {
                            completedRecipe = false;
                        }
                    }

                    if (completedRecipe)
                    {
                        cManager.SuccessfulOrder(i);
                        cManager.activeOrders.RemoveAt(i);
                        item.transform.parent = null;
                        item.transform.position = new Vector3(item.transform.position.x, foodPosition.y, foodPosition.z);
                        food.Launch(Vector3.back * launchSpeed);
                        
                        Destroy(item, 3f);
                        return true;
                    }
                }
            }
            return base.PlaceItem(item);
        }
    }
}