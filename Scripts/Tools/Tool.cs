using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tool : MonoBehaviour
{
    [SerializeField, HideInInspector] Sprite _sprite;
    [SerializeField, HideInInspector] float _my_strength;
    [SerializeField] ToolMaterial _material;
    [SerializeField] List<Block> _blocks_destroy;
    [SerializeField, HideInInspector] AnimationCurve _my_curve;
    [SerializeField] int _my_id;

    public Curves curves;

    public AnimationCurve my_curve {
        get {return _my_curve;}
        set {_my_curve = value;}
    }

    public int my_id {
        get {return _my_id;}
        set {_my_id = value;}
    }
    
    public Sprite sprite {
        get {return _sprite;}
        set {_sprite = value;}
    }

    public List<Block> blocks_destroy {
        get {return _blocks_destroy;}
        set {_blocks_destroy = value;}
    }

    public float my_strength {
        get {return _my_strength;}
        set {_my_strength = value;}
    }

    private void AddDescendantsWithTag(Transform parent, string tag, List<Block> list)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag == tag)
            {
                list.Add(child.gameObject.GetComponent<Block>());
            }
            AddDescendantsWithTag(child, tag, list);
        }
    }

    public ToolMaterial material {
        get {return _material;}
    }

    void Start() {
        sprite = GetComponent<SpriteRenderer>().sprite;
        AddDescendantsWithTag(GameObject.Find("DestroyableBlocks").transform, "block", blocks_destroy);
        
        SwitchMaterial(material);
    }

    void Update() {
        if (my_strength <=0) {
            Destroy(gameObject);
        }
    }


    void SwitchMaterial(ToolMaterial material_tool) {
        switch(material_tool) {

            case ToolMaterial.Wood:
                my_strength = 20;
                my_curve = curves.Curves_List[0];
                break;
            
            case ToolMaterial.Iron:
                my_strength = 40;
                my_curve = curves.Curves_List[1];
                break;
            
            case ToolMaterial.Diamond:
                my_strength = 60;
                my_curve = curves.Curves_List[2];
                break;

        }
    }
}
