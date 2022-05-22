using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cooking;

public class UICookingManager : MonoBehaviour
{

    //need to do timers for machines

    //need to do customer orders + timers

    #region Recipes + Orders
    [System.Serializable]
    public class UIRecipe
    {
        public string m_Key;
        public Sprite m_RecipeProduct;
        public Sprite[] m_RecipeList;
    }

    public UIRecipe[] m_UIRecipes;

    public UIOrderObject[] m_OrderObjects;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetNextOrder(Cooking.Recipe recipe)
    {
        UIOrderObject selectedOrderObj = null;
        UIRecipe selectedRecipe = null
        for(int i = 0; i < m_OrderObjects.Length; i++)
        {
            if (!m_OrderObjects[i].gameObject.activeSelf)
            {
                selectedOrderObj = m_OrderObjects[i];
                break;
            }
        }
        for(int i = 0; i < m_UIRecipes.Length; i++)
        {
            if(recipe.name == m_UIRecipes[i].m_Key)
            {
                selectedRecipe = m_UIRecipes[i];
                break;
            }
        }
        if(selectedOrderObj != null && selectedRecipe != null)
        {

            selectedOrderObj.SetImages(selectedRecipe);
        }
    }
}
