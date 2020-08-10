using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Check;

public class Craft : MonoBehaviour
{
    private List<List<Block>> variant = new List<List<Block>> () {};

    private Color cant_craft_selected = new Color(1f, 0.1f, 0.1f, 0.5f);
    private Color cant_craft_pressed = new Color(1f, 1f, 1f, 1f);
    public GameObject ItemPanel;
    bool _can_it_craft = false;
    [SerializeField] Block _BlockToCraft;
    [SerializeField] GameObject _image_not_craft;
    [SerializeField] int _count;

    public GameObject image;

    public Inventory Inv;
    public GameObject buttoncraft;
    
    public List<Block> sorting_inv;

    [Header("Sounds")]
    public AudioSource ErrorSound;
    public AudioSource CraftSound;

    public Block BlockToCraft {
        get {return _BlockToCraft;}
    }

    public int count {
        get {return _count;}
    }
    public bool can_it_craft {
        get {return _can_it_craft;}
        set {_can_it_craft = value;}
    }

    public GameObject image_not_craft {
        get {return _image_not_craft;}
    }

    public void ErrorSoundPlay(){
        ErrorSound.Play();
    }

    public void CraftSoundPlay(){
        CraftSound.Play();
    }


    void Start() {
        
        CraftList[] craft_variant_to_add = gameObject.GetComponents<CraftList>();
        for (int i = 0; i<craft_variant_to_add.Length; i++) {
            variant.Add(craft_variant_to_add[i].craft_variant);
        }
        buttoncraft.GetComponent<CraftButton>().to_craft = BlockToCraft;
        image.GetComponent<UnityEngine.UI.Image>().sprite = BlockToCraft.GetComponent<SpriteRenderer>().sprite;
        ItemPanel.SetActive(false);

    }

    List<Block> ListN() {
        List<bool> bools = new List<bool>(){};
        for (int variant_i = 0; variant_i<variant.Count; variant_i++) {
            if (CheckCount(Inv, variant[variant_i]) && ListCheck.ListContainsNew(variant[variant_i], Inv.item_list)) {
                bools.Add(true);
            }
            else {
                bools.Add(false);
            }
        }
        
        for (int bool_i = 0; bool_i<bools.Count; bool_i++) {
            if (bools[bool_i]) {
                return variant[bool_i];
            }
        }
        return null;
    }

    bool CheckCount(Inventory Inv, List<Block> need_to_craft) {
        if (Inv.item_list.Count==0) return false;
        sorting_inv = new List<Block>() {};
        sorting_inv = Inv.item_list.ToList();
        sorting_inv.RemoveAll(el=> !need_to_craft.Exists(el2=>el2.name== el.name));
        if (sorting_inv.Count==0) return false;
        if (sorting_inv.Count>= need_to_craft.Count) {
            return ListCheck.ListContainsNew(need_to_craft, sorting_inv);
        }
        return false;
    }

    bool Check() {
        for (int i = 0; i<variant.Count; i++) {
            if (CheckCount(Inv, variant[i])) {

                return true;
                
            }
        }
        return false;
    }
    void Update() {
        if (Check()) {
            buttoncraft.GetComponent<CraftButton>().need_to_craft = ListN();
            can_it_craft = true;
            image_not_craft.SetActive(false);
            //buttoncraft.SetActive(true);
        }
        else {
            
            //UnityEngine.UI.ColorBlock colorBlock = buttoncraft.GetComponent<UnityEngine.UI.Button>().colors;
            //colorBlock.pressedColor = cant_craft_pressed;
            //colorBlock.selectedColor = cant_craft_pressed;
            can_it_craft = false;
            image_not_craft.SetActive(true);
            //buttoncraft.SetActive(false);
        }
    }
}
