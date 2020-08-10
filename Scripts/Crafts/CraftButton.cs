using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CraftButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Craft craft;
    Inventory Inv;

    [HideInInspector]
    public List<Block> need_to_craft;
    private Block _to_craft;

    public Block to_craft {
        get {return _to_craft;}
        set {_to_craft = value;}
    }

    void Awake(){
        Inv = craft.Inv;
    }

    public void CraftItem() {
        if (!craft.can_it_craft) {
            craft.ErrorSoundPlay();
            return;
        }
        craft.CraftSoundPlay();
        if (Inv.IsSimilar(to_craft)) {
            //print(to_craft.name + " найден похожий блок");
            Inv.AddItem(Inv.FindSimilar(to_craft), to_craft,craft.count,true);
        }

        else {
            //print(to_craft.name + " не найден похожий блок");
            Inv.AddItem(Inv.FindEmptyCell(), to_craft, craft.count);
        }

        for (int block_i = 0; block_i<need_to_craft.Count; block_i++) {
            
            Inv.DeleteItem(need_to_craft[block_i]);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!craft.can_it_craft) craft.ItemPanel.SetActive(true);     
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        if (!craft.can_it_craft) craft.ItemPanel.SetActive(false);
    }
}
