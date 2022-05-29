using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cooking
{
    public class CookingManager : GameManager
    {
        public Recipe[] recipes;

        public Recipe[] activeOrders;

        public Oven[] ovens;
        public Mixer[] mixers;
        public IngredientStore[] ingredients;

        public CookingController player;

        public CustomerArea customerArea;

        private bool canPlay;
        
        public float timeBetweenCustomers = 35f;
        public float customerTimer = 0f;
        public int neededOrders = 5;
        public int ordersFilled;
        private int activeOrderCount = 0;

        public GameObject customerPrefab;

        public UICookingManager m_UICookingManager;
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            ordersFilled = (int)value1;
            activeOrders = new Recipe[3];
            player.Setup();
            customerArea.Setup(player, this);
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

            //hide UI on game start - CALLING ON START OF MANAGER INSTEAD DUE TO QUICK TUTORIAL SCREEN FOR EACH GAME, leaving here in case of need later on
            /*m_UICookingManager.HideAllUIObjects();*/

            customerTimer = 5f;
            canPlay = true;
        }

        void FixedUpdate()
        {
            if (!canPlay) return;

            if (activeOrderCount < 3)
            {
                customerTimer -= Time.fixedDeltaTime;
            }

            if (customerTimer <= 0)
            {
                SpawnCustomer();
            }
            
            for (int i = 0; i < activeOrders.Length; ++i)
            {
                if (activeOrders[i].name == null) continue;
                activeOrders[i].timer += Time.fixedDeltaTime;

                //set timer for customer
                m_UICookingManager.SetUICustomerTimer(i, (activeOrders[i].timeAllowed - activeOrders[i].timer).ToString("0"));

                if (activeOrders[i].timer > activeOrders[i].timeAllowed)
                {
                    
                    GameInstance.instance?.SetPersistantData(gameType, ordersFilled);
                    GameInstance.instance?.GameEnd();
                }
            }
        }


        void SpawnCustomer()
        {
            Recipe recipe = new Recipe();
            recipe.Copy(recipes[Random.Range(0, recipes.Length)]);
            customerArea.SetRecipe(recipe);
            customerTimer = timeBetweenCustomers;
            activeOrderCount++;
            

            //set up the UI for the chosen order
            m_UICookingManager.SetNextOrder(recipe);
        }

        public void SuccessfulOrder(int id)
        {
            if (activeOrders.Length == 3)
            {
                //Reset the customer timer so we have a customer quicker
                customerTimer = timeBetweenCustomers * 0.25f;
            }
            activeOrders[id] = null;
            activeOrderCount--;
            

            m_UICookingManager.CloseCustomerOrder(id);
            ordersFilled++;
            if (ordersFilled >= neededOrders)
            {
                GameInstance.instance.SetPersistantData(gameType, ordersFilled);
                GameInstance.instance.SetGameComplete(gameType);
            }
        }

        
    }

    [Serializable]
    public class Recipe
    {
        public string name;
        public IngredientType[] ingredientsNeeded;
        public float timeAllowed = 30f;
        public float timer = 0f;


        public void Copy(Recipe copy)
        {
            name = copy.name;
            ingredientsNeeded = copy.ingredientsNeeded;
            timeAllowed = copy.timeAllowed;
            timer = 0f;
        }
    }
    
}


