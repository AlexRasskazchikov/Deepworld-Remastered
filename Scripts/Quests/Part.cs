using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField] TypePart _MyPart;
    [SerializeField] bool _is_part_ended;

    public TypePart MyPart {
        get {return _MyPart;}
    }

    public bool is_part_ended {
        get {return _is_part_ended;}
        set {_is_part_ended = value;}
    }

}
