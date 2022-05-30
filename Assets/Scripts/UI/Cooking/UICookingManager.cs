using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cooking;
using TMPro;

public class UICookingManager : MonoBehaviour
{

    //need to do timers for machines
    #region Machines

    public UIOvenObject[] m_OvenObjects;
    public UIMixerObjects[] m_MixerObjects;

    #endregion

    public TMP_Text m_CustomersServedText;

    //need to do customer orders + timers
    #region Customer Orders + Timers
    public UICustomerObject[] m_UICustomerObjects;
    #endregion

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

    private void Start()
    {
        HideAllUIObjects();
    }

    public void SetNextOrder(Cooking.Recipe recipe)
    {
        UICustomerObject selectedCustomerObj = null;
        UIOrderObject selectedOrderObj = null;
        UIRecipe selectedRecipe = null;
        for(int i = 0; i < m_OrderObjects.Length; i++)
        {
            if (!m_OrderObjects[i].gameObject.activeSelf)
            {
                selectedOrderObj = m_OrderObjects[i];
                selectedCustomerObj = m_UICustomerObjects[i];
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
            selectedCustomerObj.SetCustomerAsk(selectedRecipe.m_RecipeProduct, recipe.timeAllowed.ToString());
        }
    }

    public void HideAllUIObjects()
    {
        for(int i = 0; i < m_UICustomerObjects.Length; i++)
        {
            m_UICustomerObjects[i].TurnOffUI();
        }
        for (int i = 0; i < m_OrderObjects.Length; i++)
        {
            m_OrderObjects[i].TurnOffUI();
        }
        for (int i = 0; i < m_OvenObjects.Length; i++)
        {
            m_OvenObjects[i].TurnOffUI();
        }
        for (int i = 0; i < m_MixerObjects.Length; i++)
        {
            m_MixerObjects[i].TurnOffUI();
        }
    }

    #region customer orders methods
    public void SetUICustomerTimer(int index, string time)
    {
        if (index >= m_UICustomerObjects.Length) //null index check
        {
            Debug.LogError("TOO MANY CUSTOMERS FOR UI TO HANDLE (JC)");
            return;
        }
        m_UICustomerObjects[index].SetTimer(time);
    }

    public void CloseCustomerOrder(int index)
    {
        m_UICustomerObjects[index].TurnOffUI();
        m_OrderObjects[index].TurnOffUI();
    }
    #endregion

    #region mixer methods

    #endregion

    public void SetCustomersServed(int filled, int required)
    {
        m_CustomersServedText.text = filled.ToString() + "/" + required.ToString();
    }
}
