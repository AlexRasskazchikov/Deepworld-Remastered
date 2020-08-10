using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class ItemPanel : MonoBehaviour
{
    [SerializeField] Craft craft;
    [SerializeField] GameObject RecipeElementPrefab;
    [SerializeField] GameObject RecepieElementsGrid;
    [SerializeField] GameObject SpacerPrefab;
    public Dictionary<string, int> GetIngridientsNames(List<Block> Recipe){
        Dictionary<string, int> names = new Dictionary<string, int>();
        foreach(Block block in Recipe){
            if(!names.ContainsKey(block.name)){
                names.Add(block.name, 1);
            } else {
                names[block.name]++;
            }
        }
        return names;
    }

    public Dictionary<string, Block> GetIngridientsObjects(List<Block> Recipe){
        Dictionary<string, Block> Blocks = new Dictionary<string, Block>();
        foreach(Block block in Recipe){
            if(!Blocks.ContainsKey(block.name)){
                Blocks.Add(block.name, block);
            }
        }
        return Blocks;
    }

    public string GetTextForRecipe(Dictionary<string, int> names, int i){
        return "x" + names.ElementAt(i).Value + " " + names.ElementAt(i).Key;
    }

    private void Start() {
        List<CraftList> Crafts = craft.gameObject.GetComponents<CraftList>().ToList();
        for (int i = 0; i < Crafts.Count; i++){
            Dictionary<string, Block> Blocks = GetIngridientsObjects(Crafts[i].craft_variant);
            Dictionary<string, int> names = GetIngridientsNames(Crafts[i].craft_variant);

            for(int j = 0; j < names.Count; j++){
                GameObject Element = Instantiate(RecipeElementPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Element.transform.SetParent(RecepieElementsGrid.transform);
                Element.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                UIRecipeElement ElementUI = Element.GetComponent<UIRecipeElement>();
                ElementUI.RecipeElemImage.sprite = Blocks[names.ElementAt(j).Key].ObjectToSave.GetComponent<SpriteRenderer>().sprite;
                ElementUI.RecipeElemInfo.text = GetTextForRecipe(names, j);
            }

            if (Crafts.Count - 1 != i){
                GameObject Spacer = Instantiate(SpacerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                Spacer.transform.SetParent(RecepieElementsGrid.transform);
                Spacer.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
            }
        }
        SpacerPrefab.SetActive(false);
        RecipeElementPrefab.SetActive(false);

    }
}
