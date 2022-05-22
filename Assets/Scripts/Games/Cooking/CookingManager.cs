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

        public List<Recipe> activeOrders;

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

        public GameObject customerPrefab;
        
        public override void StartGame(float value1 = 0, float value2 = 0)
        {
            ordersFilled = (int)value1;
            activeOrders = new List<Recipe>();
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

            customerTimer = 5f;
            canPlay = true;
        }

        void FixedUpdate()
        {
            if (!canPlay) return;
            customerTimer -= Time.fixedDeltaTime;
            if (customerTimer <= 0)
            {
                SpawnCustomer();
            }

            for (int i = 0; i < activeOrders.Count; ++i)
            {
                activeOrders[i].timer += Time.fixedDeltaTime;
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
        }

        public void SuccessfulOrder(int id)
        {
            activeOrders.RemoveAt(id);
            ordersFilled++;
            if (ordersFilled >= neededOrders)
            {
                GameInstance.instance.SetPersistantData(gameType, ordersFilled);
                GameInstance.instance.SetGameComplete(gameType);
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


