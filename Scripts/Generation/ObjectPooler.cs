using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public GameObject parentHolder;

    public static ObjectPooler Instance;

    private void Awake() {
        Instance = this;
        poolDict = new Dictionary<string, Queue<GameObject>>();
        foreach(Pool pool in pools) 
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++) 
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDict.Add(pool.tag, objectPool);
        }
    }

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDict;

    public GameObject SpawnFromPool(string tag, Vector3 pos)
    {
        if (!poolDict.ContainsKey(tag)) {
            Debug.Log("Pool Dictionary Didnt Exist. Wupsie. - " + tag);
            return null;
        }
        GameObject objectToSpawn = poolDict[tag].Dequeue();
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = pos;
        poolDict[tag].Enqueue(objectToSpawn);
        return objectToSpawn;
    }
}
