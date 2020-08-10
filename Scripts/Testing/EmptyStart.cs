using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyStart : MonoBehaviour
{
    public GameManager manager;
    void Start()
    {
        manager.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
