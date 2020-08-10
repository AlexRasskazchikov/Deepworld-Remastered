using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnCount : MonoBehaviour
{
    [SerializeField] GameObject _ToKill;
    [SerializeField] int _count;

    public GameObject ToKill {
        get {return _ToKill;}
        set {_ToKill = value;}
    }

    public int count {
        get {return _count;}
    }

}
