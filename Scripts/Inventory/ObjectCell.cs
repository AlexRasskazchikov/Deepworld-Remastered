using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ObjectCell : MonoBehaviour
{
    public DestroyAndPlaceBlocks place;
    public InventoryCell Cell;
    public Inventory Inv;

    void Start(){
        place = GameObject.Find("BreakBlocks").GetComponent<DestroyAndPlaceBlocks>();
        Inv = GameObject.Find("InventoryScript").GetComponent<Inventory>();
    }
    
    public void ChooseBlock() {
        place.block_to_place = Cell.object_save;
        place.cell = Cell;
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (!Cell.i_choose) {
                
                Cell.i_choose = true;
                Inv.tho_choose.Add(Cell);
            }
        }
    }
}
