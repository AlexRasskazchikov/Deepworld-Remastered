using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.Pooling;

[RequireComponent(typeof(SpriteRenderer))]
public class Block : MonoBehaviour
{   
    [System.Serializable]
    public struct DisabledBlockComponent{
        public GameObject BlockComponent;
        public bool DestroyOnDrop;
    }

    [Header("Block Meta Settings")]
    [SerializeField][Tooltip("Unique Block Name")] string _name = "Block";

    [SerializeField][Tooltip("Block's health")] int _hp = 4;

    int _MaxHp;
    [SerializeField][Tooltip("Description for the block.")] [Multiline] string _Description = "Usual block. Can be obtained in Normal worlds.";
    [SerializeField][Tooltip("Unique Block ID")] int _BlockID = 0;
    [HideInInspector] string _tag;

    [Header("Inventory Settings")]

    [SerializeField][Tooltip("Player's Inventrory")] Inventory _Inventory;
    [SerializeField][Tooltip("Object limit in player's inventory.")] int _InventoryLimit = 50;
    GameObject _ObjectToSave;

    [Header("Block Destroy Settings")]

    [SerializeField] [Tooltip("Gameobjects that script deletes, when block is dropped.")] List<DisabledBlockComponent> DisableWhileDropped;

    [SerializeField] [Tooltip("Change spriterenderer color to gray or not.")] public bool ChangeBackgroundColor = true;
    [SerializeField] [Tooltip("Place background or not")] public bool PlaceBackground = true;
    [SerializeField] [Tooltip("If item is dropped.")] bool _IsDropped = false;

    [HideInInspector] public Block _Block;

    [HideInInspector] bool _IsBreaking; 

    public IEnumerator BreakBlockCoroutine(float speed, Material Standart){
        _IsBreaking = true;
        yield return new WaitForSeconds(speed);
        _IsBreaking = false;
        StartCoroutine(IsBreakingAwait(Standart));
    }

    public IEnumerator IsBreakingAwait(Material Standart){
        yield return new WaitForSeconds(0.05f);
        if(!IsBreaking){
            hp = MaxHp;
            GetComponent<SpriteRenderer>().material = Standart;
        }
    }
    public void BreakBlock(float speed, Material Standart){
        StartCoroutine(BreakBlockCoroutine(speed, Standart));
    }

    public bool IsBreaking
    {
        get { return _IsBreaking; }
    }
    public int BlockID
    {
        get { return _BlockID; }
    }

    public int limit
    {
        get { return _InventoryLimit; }
    }
    public string name
    {
        get { return _name; }
    }

    public int hp
    {
        get { return _hp; }
        set { _hp = value; }
    }

    public string tag
    {
        get { return _tag; }
        set { _tag = value; }
    }

    public string GetName()
    {
        return _name;
    }


    public GameObject ObjectToSave
    {
        get {return _ObjectToSave;
             }
        set {  _ObjectToSave = value; }
    }

    public bool IsDropped
    {
        get { return _IsDropped; }
        set { _IsDropped = value;
            if(_IsDropped){
                foreach(DisabledBlockComponent elem in DisableWhileDropped){
                    if(elem.DestroyOnDrop){
                        Destroy(elem.BlockComponent);
                    } else {
                        elem.BlockComponent.SetActive(false);
                    }
                }
            } else
                {
                foreach(DisabledBlockComponent elem in DisableWhileDropped){
                        if(!elem.DestroyOnDrop) {
                            elem.BlockComponent.SetActive(true);
                        } else {
                            elem.BlockComponent.SetActive(false);
                        }
                } 
            }
        }   
    }
    public int MaxHp
    {
        get { return _MaxHp; }
    }

    public string Description
    {
        get { return _Description; }
    }

    void Start()
    {   
        _ObjectToSave = gameObject;
        _MaxHp = hp; 
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        transform.name = name;
        _Block = this;
        
    }
    public void ReturnName()
    {
        transform.name = name;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (IsDropped && coll.transform.tag == "Player")
        {
            if (_Inventory.FindSimilar(_Block) != null) _Inventory.AddItem(_Inventory.FindSimilar(_Block), _Block, 1, true);
            else _Inventory.AddItem(_Inventory.FindEmptyCell(), _Block, 1, false);
            SpawningPool.ReturnToCache(gameObject, gameObject.transform.name);
            gameObject.SetActive(false);
        }
    }
}
