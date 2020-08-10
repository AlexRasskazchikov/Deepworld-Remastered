using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAreaScript : MonoBehaviour
{   
    [System.NonSerialized]
    public List<GameObject> blocks = new List<GameObject>() {};
    public BlocksManager Materials;

    public List<GameObject> BlocksPreview = new List<GameObject>() {};
    private Collider2D col;

    [SerializeField] bool Debug = false;
    void Start()
    {
        col = gameObject.GetComponent<CircleCollider2D>();
    }

    void Update(){
        for(int i = 0; i < blocks.Count; i++){
            if (blocks[i] == null) {
                blocks.RemoveAt(i);
                BlocksPreview.RemoveAt(i);
            }
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        string tag = other.gameObject.tag;
        if (other.gameObject != null){
            blocks.Add(other.gameObject);
            BlocksPreview.Add(other.gameObject);
            if(Debug){
                other.gameObject.GetComponent<SpriteRenderer>().material = Materials.OutlineMat;
            }  
      }
    }
    
    void OnTriggerExit2D(Collider2D other){
        if(blocks.Contains(other.gameObject)) blocks.Remove(other.gameObject);
        if(BlocksPreview.Contains(other.gameObject)) BlocksPreview.Remove(other.gameObject);
        if(Debug){ 
            other.gameObject.GetComponent<SpriteRenderer>().material = Materials.Default;
        }
    }

    
}
