using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Mob : MonoBehaviour
{
    

    [Header( "Mob Info" )]
    [SerializeField] float _my_hp;
    [SerializeField] float _my_see_distance;
    [SerializeField] float _my_attack_distance;
    [SerializeField] float _my_speed;
    [SerializeField] float _my_damage;

    [Header( "Mob type" )]
    [SerializeField] MobType _mob_type;

    [Header( "Mob drop" )]
    [SerializeField] bool _is_i_drop;
    [SerializeField] GameObject _item_drop;
    [SerializeField] float _chance_drop;

    public MobType mob_type {
        get {return _mob_type;}
    }
    public float my_hp {
        get {return _my_hp;}
        set {_my_hp = value;}
    }

    public float my_see_distance {
        get {return _my_see_distance;}
        set {_my_see_distance = value;}
    }

    public float my_attack_distance {
        get {return _my_attack_distance;}
        set {_my_attack_distance = value;}
    }
    
    public float my_speed {
        get {return _my_speed;}
        set {_my_speed = value;}
    }

    public float my_damage {
        get {return _my_damage;}
        set {_my_damage = value;}
    }

    public bool is_i_drop {
        get {return _is_i_drop;}
        set {_is_i_drop = value;}
    }

    public GameObject item_drop {
        get {return _item_drop;}
    }

    public float chance_drop {
        get {return _chance_drop;}
        set {_chance_drop = value;}
    }

    void Start() {
        MobType mobtype = mob_type;
        GetMob(mobtype);
    }

    void GetMob(MobType Mobtype) {
        switch (Mobtype)
        {
            case MobType.snowman:
                my_speed = 2f;
                my_damage = 0.1f;
                my_see_distance = 7f;
                my_attack_distance = 0.5f;
                my_hp = 10f;
                is_i_drop = true;
                chance_drop = 10f;
                break;

            case MobType.puckman:
                my_speed = 3f;
                my_damage = 3f;
                my_see_distance = 10f;
                my_attack_distance = 1f;
                my_hp = 20f;
                is_i_drop = true;
                chance_drop = 20f;
                break;

            case MobType.pulman:
                my_speed = 10f;
                my_damage = 1f;
                my_see_distance = 6f;
                my_attack_distance = 1f;
                my_hp = 3f;
                is_i_drop = true;
                chance_drop = 30f;
                break;

            default:

                my_speed = my_speed;
                my_damage = my_damage;
                my_see_distance = my_see_distance;
                my_attack_distance = my_attack_distance;
                my_hp = my_hp;
                is_i_drop = is_i_drop;
                chance_drop = chance_drop;
                break;
        }
    }
}
