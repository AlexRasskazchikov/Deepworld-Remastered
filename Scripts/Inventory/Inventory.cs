using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class Inventory : MonoBehaviour
{
    public List<InventoryCell> Cells = new List<InventoryCell>(){};
    [SerializeField] List<InventoryCell> Full_Cells = new List<InventoryCell>(){};

    public List<Block> item_list = new List<Block>(){};
    public List<InventoryCell> Hud_Cells = new List<InventoryCell>() {};

    private Inventory _Inv;
    private bool state = false;
    public GameObject Inv;
    public bool can_add = true;

    public List<InventoryCell> tho_choose = new List<InventoryCell>(2) {};

    void ChangeCells(InventoryCell cell_one, InventoryCell cell_two) {
        ChangeInts(cell_one, cell_two);
        ChangeGameObjects(cell_one, cell_two);
        

        string copy_name_cell_one = cell_one.name_item;
        

        cell_one.name_item = cell_two.name_item;
        cell_two.name_item = copy_name_cell_one;
        cell_one.i_choose = false;
        cell_two.i_choose = false;
    }

    void ChangeGameObjects(InventoryCell cell_one, InventoryCell cell_two) {

        Sprite sprite_copy_cell_one = cell_one.object_icon.GetComponent<UnityEngine.UI.Image>().sprite;
        cell_one.object_icon.GetComponent<UnityEngine.UI.Image>().sprite = cell_two.object_icon.GetComponent<UnityEngine.UI.Image>().sprite;
        cell_two.object_icon.GetComponent<UnityEngine.UI.Image>().sprite = sprite_copy_cell_one;

        string cell_one_name_copy_object = cell_one.object_icon.name;
        cell_one.object_icon.name = cell_two.object_icon.name;
        cell_two.object_icon.name = cell_one_name_copy_object;

        GameObject cell_one_object_copy = cell_one.object_save;
        cell_one.object_save = cell_two.object_save;
        cell_two.object_save= cell_one_object_copy;
        
    }

    void ChangeInts(InventoryCell cell_one, InventoryCell cell_two) {

        int cell_one_count_copy = cell_one.count_item;
        cell_one.count_item = cell_two.count_item;
        cell_two.count_item = cell_one_count_copy;

        int cell_one_id_item_in_cell_copy = cell_one.id_item_in_cell;
        cell_one.id_item_in_cell = cell_two.id_item_in_cell;
        cell_two.id_item_in_cell = cell_one_id_item_in_cell_copy;

    }



    void SetIcon(InventoryCell cell, Block item) {
        cell.object_icon.GetComponent<UnityEngine.UI.Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;
        cell.object_icon.name = item.name;
    }

    public void AddItemHud(InventoryCell cell, GameObject item, int count) {
        if (item.GetComponent<Block>() != null) {
            AddItem(cell, item.GetComponent<Block>(), count);
            return;
        }
        if (item.GetComponent<Tool>() != null) {
            AddTool(cell, item);
            return;
        }
    }

    void AddTool(InventoryCell cell, GameObject ToolObject) {
        Tool tool = ToolObject.GetComponent<Tool>();
        cell.count_item = 1;
        cell.is_item = true;
        cell.id_item_in_cell = tool.my_id;
        cell.name_item = tool.name;
    }

    public void AddItem(InventoryCell cell, Block item, int count = 1, bool must_find = false) {

        if (Cells.Count == Full_Cells.Count) return;
        if(must_find && IsSimilar(item) ) {
            AddItem(FindSimilar(item), item, count);
        }
        else {
            if (!CheckCount(cell, item, count)) { 
                AddItem(FindEmptyCell(), item, item.limit);
                AddItem(FindEmptyCell(), item, count - item.limit);
                
                Full_Cells.Add(cell);
            }

            else {
                SetIcon(cell, item);
                cell.count_item += count;
                cell.is_item = true;
                cell.id_item_in_cell = item.BlockID;
                cell.name_item = item.name;
                cell.object_save = item.ObjectToSave;
                for (int i = 0; i < count; i++){
                    item_list.Add(item.ObjectToSave.GetComponent<Block>());
                }
            }
        }
        
    }

    public int CountItem(GameObject Item) {
        Block item = Item.GetComponent<Block>();
        int count =0;
        for (int item_i=0; item_i<item_list.Count; item_i++) {
            if (item_list[item_i].name == item.name) {
                count+=1;
            }
        }
        return count;
    }

    public void DeleteItem(Block item) {
        for (int cell_i = 0; cell_i<Cells.Count; cell_i++) {
            if (Cells[cell_i].is_item) {
                if(Cells[cell_i].id_item_in_cell == item.BlockID) {
                    Cells[cell_i].count_item -= 1;
                    break;
                }
            }
        }

        for (int item_i=0; item_i<item_list.Count; item_i++) {
            if (item_list[item_i].name == item.name) {
                item_list.RemoveAt(item_i);
                break;
            }
        }
    }

    void Awake() {
        _Inv = this;
    }

    void Update() {
        if (tho_choose.Count == 2) {
            ChangeCells(tho_choose[0], tho_choose[1]);
            tho_choose = new List<InventoryCell>();
        }
    }

    public InventoryCell FindEmptyCell() {
        for (int cell_i = 0; cell_i<Cells.Count; cell_i++) {
            if(!Cells[cell_i].is_item && !Full_Cells.Contains(Cells[cell_i])) {
                return Cells[cell_i];
                
            }

        }
        return null;
    }   

    public InventoryCell FindSimilar(Block item) {
        for (int cell_i = 0; cell_i<Cells.Count; cell_i++) {
            if (Cells[cell_i].is_item) {
                if (item.BlockID == Cells[cell_i].id_item_in_cell && !(Cells[cell_i].count_item-item.limit == 0)) {
                    return Cells[cell_i];
                }
            } 
        }
        return null;
    }
    
    public bool IsSimilar(Block item) {
        if (item_list.Count>0 &&item_list.Contains(item)) return true;
        return false;
    }

    bool CheckCount(InventoryCell cell, Block item, int count) {
        if (cell.count_item+count<=item.limit) {
            return true;
        }
        return false;
    }
}   

