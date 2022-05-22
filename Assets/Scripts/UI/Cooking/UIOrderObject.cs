using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOrderObject : MonoBehaviour
{
    public Image m_Product;
    public Image[] m_Ingrients;


    public void SetImages(UICookingManager.UIRecipe recipeImages)
    {
        m_Product.sprite = recipeImages.m_RecipeProduct;
        for (int i = 0; i < 5; i++)
        {
            if (i < recipeImages.m_RecipeList.Length)
            {
                m_Ingrients[i].transform.parent.gameObject.SetActive(true);
                m_Ingrients[i].sprite = recipeImages.m_RecipeList[i];
            }
            else
            {
                m_Ingrients[i].transform.parent.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(true);
    }
}
