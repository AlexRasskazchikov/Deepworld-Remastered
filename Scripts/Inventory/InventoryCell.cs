using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour
{
    [SerializeField] int _id_item_in_cell;
    [SerializeField] int _count_item = 0;
    [SerializeField] GameObject _object_icon;
    [SerializeField] GameObject _object_save;
    [SerializeField] bool _is_item = false;
    [SerializeField] Text _count_text;
    [SerializeField] string _name_item;
    [SerializeField] bool _i_choose = false;
    public bool i_choose {
        get {return _i_choose;}
        set {_i_choose = value;}
    }

    public string name_item {
        get {return _name_item;}
        set {_name_item = value;}
    }

    public InventoryCell _InventoryCell;

    public bool is_item {
        get {return _is_item;}
        set{_is_item = value;}
    }

    public int id_item_in_cell {
        get {return _id_item_in_cell;}
        set{_id_item_in_cell = value;}
    }

    public int count_item {
        get {return _count_item;}
        set {_count_item = value;}
    }

    public GameObject object_icon {
        get {return _object_icon;}
        set {_object_icon = value;}
    }

    public GameObject object_save {
        get {return _object_save;}
        set {_object_save = value;}
    }

    public Text count_text {
        get {return _count_text;}
        set{_count_text = value;}
    }
    void Start() {
        _InventoryCell = this;
        count_text.text = "";
    }

    void Update() {
        if (count_item ==0 || count_item<0) {
            is_item = false;
        }

        if (is_item) {
            count_text.text = count_item.ToString();
            object_icon.SetActive(true);
            
        }
        else if(!is_item) {
            count_text.text = "";
            name_item = "";
            object_save = null;
            count_item = 0;
            object_icon.SetActive(false);
        }
    }
}
