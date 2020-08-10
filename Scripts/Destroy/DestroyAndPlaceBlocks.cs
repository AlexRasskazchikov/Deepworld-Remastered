using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DigitalRuby.Pooling;
public class DestroyAndPlaceBlocks : MonoBehaviour
{ 
    public GameObject hero;
    private float Cell_Size = 0.64f;
    private Camera camera;
    public List<GameObject> blocks;
    public GameObject block_to_place;
    public InventoryCell cell;
    public GameObject break_point;

    GameObject created_break_point;

    bool PlacedBlock = false;

    void Start() {
        camera = Camera.main;
    }
    
    void Update()
    {
        
        if (Input.GetMouseButton(0)) {
            
            Vector2 mousePos2D = Input.mousePosition;
            float screenToCameraDistance = camera.nearClipPlane;
            Vector3 mousePosNearClipPlane = new Vector3(mousePos2D.x, mousePos2D.y, screenToCameraDistance);
            Vector3 worldPointPos = camera.ScreenToWorldPoint(mousePosNearClipPlane);


            created_break_point = ObjectPooler.Instance.SpawnFromPool("BreakPoint",  new Vector2(worldPointPos.x, worldPointPos.y));
        }


        if (Input.GetMouseButtonDown(1)) {
                Vector2 mousePos2D = Input.mousePosition;
                float screenToCameraDistance = camera.nearClipPlane;
                Vector3 mousePosNearClipPlane = new Vector3(mousePos2D.x, mousePos2D.y, screenToCameraDistance);
                Vector3 worldPointPos = camera.ScreenToWorldPoint(mousePosNearClipPlane);

                float delta = 0.64f;
                if(worldPointPos.y <= 0){
                    delta = 0;
                }
                if (cell != null && cell.is_item) {
                    Inventory Inv = GameObject.Find("InventoryScript").GetComponent<Inventory>();
                    
                    PlaceBlock(block_to_place, new Vector2(worldPointPos.x - worldPointPos.x % 0.64f, worldPointPos.y - worldPointPos.y % 0.64f + delta));
                    //NewBlock.GetComponent<Block>().hp = NewBlock.GetComponent<Block>().MaxHp;
                    Inv.DeleteItem(block_to_place.GetComponent<Block>());
                }
                
    
            
        }
    }

    public void PlaceBlock(GameObject block, Vector2 position){
        GameObject i = Instantiate(block, position, Quaternion.identity);
        i.tag = "block";
        i.GetComponent<Block>().ReturnName();
        i.GetComponent<Block>().IsDropped = false;
    }

    
}
