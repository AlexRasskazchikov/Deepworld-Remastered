using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    [SerializeField] GameObject _ToKill;
    public GameObject ToKill {
        get {return _ToKill;}
        set {_ToKill = value;}
    }
}
