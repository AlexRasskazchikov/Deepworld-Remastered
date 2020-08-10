using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craftbl : MonoBehaviour
{
    [SerializeField] GameObject PrefabMenu;
    [SerializeField] string name;
    private Block BlockScript;
    private bool state = false;

    void Start() {
        BlockScript = GetComponent<Block>();
        PrefabMenu.SetActive(false);
    }

    void OnMouseDown() {
        state = !state;
        PrefabMenu.SetActive(state);
    }
}
