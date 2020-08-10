using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCollider : MonoBehaviour
{    
    public GameObject hero;
    public Sprite BG;
    public Vector2 position_BG;
    public bool can_destroy;
    public float Cell_Size = 0.64f;
    void Start()
    {
        hero = GameObject.FindWithTag("Player");
    }
    private void OnTriggerEnter2D(Collider2D other){
        float direction = hero.transform.position.x - gameObject.transform.position.x;

            if(gameObject.transform.position.x <= hero.transform.position.x){
                StartCoroutine(Vibrate(0.1f, 0, 1));
            }else{
                StartCoroutine(Vibrate(0.1f, 1, 0));
            }
            
            StartCoroutine(DestroyBlock(other.gameObject));
    }
    IEnumerator Vibrate(float time, float left, float right)
    {
        int playerID = 1;
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), left, right);
        yield return new WaitForSeconds(time);
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), 0, 0);
    }

    IEnumerator DestroyBlock(GameObject block){
        int playerID = 1;
        yield return new WaitForSeconds(0.1f);
        XInputDotNetPure.GamePad.SetVibration((XInputDotNetPure.PlayerIndex)((int)playerID), 0, 0);
        Destroy(block);
    }
}
