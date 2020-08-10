using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_Destroy : MonoBehaviour
{
    public GameObject hero;

    public Sprite BG;
    public Vector2 position_BG;
    
    public bool can_destroy;

    public float Cell_Size = 0.64f;

    private float hp = 5f;
    private float damage = 5f; 
    GameObject Item_Manager;
    void Start()
    {
        Item_Manager = GameObject.Find("ItemGenerate");
        
        can_destroy = true;
        hero = GameObject.FindWithTag("Player");
    }


    void HitBlock()
    {   
        float direction = hero.transform.position.x - gameObject.transform.position.x;
        if (Mathf.Abs(direction) < Cell_Size * 5) {
            can_destroy = true;
        }else{can_destroy = false;}

        if (can_destroy) {   
            hp -= damage;
            if(gameObject.transform.position.x <= hero.transform.position.x){
                StartCoroutine(Vibrate(0.1f, 0, 1));
            }else{
                StartCoroutine(Vibrate(0.1f, 1, 0));
            }
        }

        if (hp <= 0){
            StartCoroutine(DestroyBlock());
            CreateBlockBackground(BG, position_BG, "BackGround", -2);
        }
    }

    //void OnMouseDown(){
       // Item_Manager.GetComponent<ItemDrop>().GenerateItemDrop(gameObject, transform.position);
        
        
    //}
    
    IEnumerator Vibrate(float time, float left, float right)
    {
        int playerID = 1;
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), left, right);
        yield return new WaitForSeconds(time);
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), 0, 0);
    }

    IEnumerator DestroyBlock(){
        int playerID = 1;
        yield return new WaitForSeconds(0.1f);
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), 0, 0);
        Destroy(this.gameObject);
    }

    public void CreateBlockBackground(Sprite sprite_backblock, Vector2 position, string name, int Layer)
    {
        GameObject backblock = new GameObject(name);
        SpriteRenderer renderer = backblock.AddComponent<SpriteRenderer>();
        renderer.sprite = sprite_backblock;
        renderer.sortingOrder = Layer;
        backblock.transform.position = position;
    }

}
