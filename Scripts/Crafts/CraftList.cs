using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftList : MonoBehaviour
{
    [SerializeField] List<Block> _craft_variant;
    public List<Block> craft_variant {
        get {return _craft_variant;}
    }
}
