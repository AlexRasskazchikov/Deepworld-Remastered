using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curves : MonoBehaviour
{
    [SerializeField] List<AnimationCurve> _Curves_List;
    public List<AnimationCurve> Curves_List {
        get {return _Curves_List;}
    }
}
