using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cooking
{
    public class CookingManager : GameManager
    {
        public Recipe chocolateCake;
        public Recipe vanillaCake;

        public List<Recipe> activeOrders;

        public Oven[] ovens;
        public Mixer[] mixers;
        public IngredientStore[] ingredients;

        public CookingController player;


        private bool canPlay;
        
        public float timeBetweenCustomers = 15f;
        public float customerTimer = 0f;

        public GameObject customerPrefab;
        
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            player.Setup();
            for (int i = 0; i < ovens.Length; ++i)
            {
                ovens[i].Setup(player);
            }

            for (int i = 0; i < mixers.Length; ++i)
            {
                mixers[i].Setup();
            }

            for (int i = 0; i < ingredients.Length; ++i)
            {
                ingredients[i].Setup(player);
            }

            customerTimer = 1f;
            canPlay = true;
        }

        void Update()
        {
            if (!canPlay) return;
            customerTimer -= Time.fixedDeltaTime;
            if (customerTimer <= 0)
            {
                SpawnCustomer();
            }
        }


        void SpawnCustomer()
        {
            //ToDo: spawn customer and reset timer. setup a new recipe needed
        }

        public void SendOrder(MixData foodData)
        {
            bool success = true;
            for (int i = 0; i < activeOrders.Count; ++i)
            {
                foreach (IngredientType type in activeOrders[i].ingredientsNeeded)
                {
                    if (foodData.ingredients.Contains(type))
                        continue;
                    else
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (success)
            {
                //Continue playing
            }
            else
            {
                //ToDo: go to next game
            }
        }
        
        [ContextMenu("Test")]
        void Test()
        {
            StartGame();
        }
    }

    [Serializable]
    public class Recipe
    {
        public IngredientType[] ingredientsNeeded;
        
    }
    
}


