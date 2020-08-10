using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroParameters : MonoBehaviour
{
    [SerializeField] float _hp = 1000;
    [SerializeField] float _steam = 100;
    [SerializeField] float _player_speed = 5f;
    [SerializeField] float _SteamAddAmount = 10f;
    [SerializeField] float _SteamSubAmount = 20f;
    float _player_opacity = 1f;
    bool _Can_move = true;
    bool _is_grounded = true;
    bool _SteamEnded = false;
    Vector2 _SpawnPoint;
    [SerializeField] Tool _my_tool;

    public Tool my_tool {
        get {return _my_tool;}
        set {_my_tool = value;}
    }
    
    public Vector2 SpawnPoint {
        get {return _SpawnPoint;}
        set {_SpawnPoint = value;}
    }

    public bool SteamEnded {
        get {return _SteamEnded;}
        set {_SteamEnded = value;}
    }


    public bool Can_move {
        get {return _Can_move;}
        set {_Can_move = value;}
    }

    public bool is_grounded {
        get {return _is_grounded;}
        set {_is_grounded = value;}
    }

    public float hp {
        get {return _hp;}
        set {_hp = value;}
    }

    public float player_speed {
        get {return _player_speed;}
        set {_player_speed = value;}
    }

    public float steam {
        get{return _steam;}
        set {_steam = value;}
    }

    public float SteamAddAmount {
        get {return _SteamAddAmount;}
    }

    public float SteamSubAmount {
        get {return _SteamSubAmount;}
    }

    public float player_opacity {
        get {return _player_opacity;}
        set {_player_opacity = value;}
    }
    
}
