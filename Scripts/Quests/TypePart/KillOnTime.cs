using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTime : MonoBehaviour
{
    [SerializeField] GameObject _ToKill;
    [SerializeField] int _time;
    [SerializeField] int _count;

    public GameObject ToKill {
        get {return _ToKill;}
        set {_ToKill = value;}
    }

    public int time {
        get {return _time;}
    }
    public int count {
        get {return _count;}
    }
}
