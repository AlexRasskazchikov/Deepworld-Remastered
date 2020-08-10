using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Get : MonoBehaviour
{
    [SerializeField] List<GameObject> _ToGet;
    [SerializeField] List<int> _count_item;
    [SerializeField] bool _is_all_need;
    public Inventory Inv;

    public List<int> count_item {
        get {return _count_item;}
        set {_count_item = value;}
    }

    public List<GameObject> ToGet {
        get {return _ToGet;}
        set {_ToGet = value;}
    }

    public bool is_all_need {
        get {return _is_all_need;}
        set {_is_all_need = value;}
    }

    void Start() {
        Inv = GameObject.Find("InventoryScript").GetComponent<Inventory>();
    }

    bool CheckBool(List<bool> boolslist) {
        foreach (var item in boolslist)
        {
            if (!item) {
                return false;
            }
        }
        return true;
    }

    void Update() {
        if (is_all_need) {
            List<bool> bools = new List<bool>() {};
            for (int item_i = 0; item_i<ToGet.Count; item_i++) {
                if (Inv.CountItem(ToGet[item_i]) >= count_item[item_i]) {
                    bools.Add(true);
                }
                else {
                    bools.Add(false);
                }
            }
            if (CheckBool(bools)) {
                GetComponent<Part>().is_part_ended = true;
            }
        }
        else {
            for (int item_i = 0; item_i<ToGet.Count; item_i++) {
                if (Inv.CountItem(ToGet[item_i]) >= count_item[item_i]) {

                    GetComponent<Part>().is_part_ended = true;
                }
            }
        }
    }
}