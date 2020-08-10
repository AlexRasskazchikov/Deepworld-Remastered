using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMob : MonoBehaviour
{
    public GameManager manager;



    void Start() {
        
    }

    void CreateMob(GameObject mob,Vector2 position) {
        Instantiate(mob, position, Quaternion.identity);
    
    }

    public void SpawnMobs(GameObject mob, int count_mobs, int Width) {
        for (int i = 0; i<count_mobs; i++) {
            CreateMob(mob, new Vector2(Random.Range(1,Width-3),10));
        }
    }
}
