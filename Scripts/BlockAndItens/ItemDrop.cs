using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
   

    public Inventory inv;
    public BlocksManager Materials;
    private GameObject inventory_objects_Gameobj;

    public InGameOverlayManager OverlayManager;

    bool GlowUp(){
        //return GameObject.Find("BlockManager").GetComponent<BlocksManager>().GlowUp;
        return false;
    }

    void Start(){
        inventory_objects_Gameobj = GameObject.Find("inventory_objects");
    }

    public void GenerateItemDrop(GameObject item, Vector2 position) {
        if (item.tag != "dropped_block"){
            GameObject ObjectToSave;
            if(inventory_objects_Gameobj.transform.Find(item.name) == null){
                ObjectToSave = Instantiate(item, new Vector2(-100,0), Quaternion.identity);
                ObjectToSave.transform.parent = inventory_objects_Gameobj.transform;
                
                OverlayManager.NewBlockImage.sprite = ObjectToSave.GetComponent<SpriteRenderer>().sprite;
                OverlayManager.NewBlockName.text = ObjectToSave.GetComponent<Block>().name;
                OverlayManager.NewBlockDescription.text = ObjectToSave.GetComponent<Block>().Description;
                StartCoroutine(ShowAchivement());
                
            } else{
                ObjectToSave = inventory_objects_Gameobj.transform.Find(item.name).gameObject;
            }
            item.GetComponent<Block>().ObjectToSave = ObjectToSave;
            item.GetComponent<SpriteRenderer>().sortingOrder = 10;
            
            switch (GlowUp()){
                case true:
                    //item.GetComponent<SpriteRenderer>().material = GameObject.Find("BlockManager").GetComponent<BlocksManager>().OutlineMat;
                break;
            }
            item.tag = "dropped_block";
            item.GetComponent<Block>().IsDropped = true;
            item.transform.localScale = new Vector2(item.transform.localScale.x/2, item.transform.localScale.y/2);
            item.transform.position = position;
            Rigidbody2D rb = item.AddComponent<Rigidbody2D>();
            rb.mass = 0.1f;

            Destroy(item.GetComponent<BoxCollider2D>());

            BoxCollider2D Coll = item.AddComponent<BoxCollider2D>();
            
            Vector2 SizeCollider = item.GetComponent<BoxCollider2D>().size;
            item.GetComponent<BoxCollider2D>().size = new Vector2(SizeCollider.x/2, SizeCollider.y/2);
            item.name += "_item";
        }
    }


    IEnumerator ShowAchivement(){
        OverlayManager.OverlayAnim.SetBool("NewBlock", true);
        yield return new WaitForSeconds(1);
        OverlayManager.OverlayAnim.SetBool("NewBlock", false);
    }
}
