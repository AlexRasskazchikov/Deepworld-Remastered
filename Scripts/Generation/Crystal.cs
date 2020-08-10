using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal : MonoBehaviour
{
    public GameObject TheLight;

    void FixedUpdate()
    {
        UnityEngine.Experimental.Rendering.Universal.Light2D The2DLight = TheLight.GetComponent<UnityEngine.Experimental.Rendering.Universal.Light2D>();
        The2DLight.intensity = Random.Range(0.3f, 1f);
    }
}
