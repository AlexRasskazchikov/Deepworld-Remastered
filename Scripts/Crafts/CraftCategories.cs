using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftCategories : MonoBehaviour
{   
    [System.Serializable]
    public struct CraftCategory {
        public string CategoryName;
        public GameObject CategoryGrid;
    }
    [SerializeField] public CraftCategory[] Categories;

    public void ChooseCategory(string Name){
        foreach(CraftCategory category in Categories){
            if (category.CategoryName == Name){
                category.CategoryGrid.SetActive(true);
            } else {
                category.CategoryGrid.SetActive(false);
            }
        }
    }
}
