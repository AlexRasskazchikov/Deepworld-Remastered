using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] string _OtherName;
    [SerializeField] [TextArea] List<string> _Phrases = new List<string>() {"Hello there! I have a quest for you!", "Well, you know i REALLY love shiny stuff. I'm keened on jewelery.", "Can you please find some rare blocks for me? Of course, i would pay you."};

    public string OtherName{
        get {return _OtherName;}
    }

    public List<string> Phrases{
        get {return _Phrases;}
        set {_Phrases = value;}
    }

    public void OnEnded(){
        print("OnEnded");
    }
}
