using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
        float length; 
        Vector2 startpose;
        public GameObject Cam;
        public float ParallaxEffectScaleX, ParallaxEffectScaleY;

        private void Start() {
            startpose = transform.position;
            length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void FixedUpdate() {
            float temp = (Cam.transform.position.x * (1 - ParallaxEffectScaleX));
            float dist = (Cam.transform.position.x * ParallaxEffectScaleX);

            float distY = (Cam.transform.position.y * ParallaxEffectScaleY / 2f);
            if (distY < 0) distY = 0;
            transform.position = new Vector2(startpose.x + dist, startpose.y + distY);

            if (temp > startpose.x + length) startpose.x += length;
            else if (temp < startpose.x - length) startpose.x -= length;
        }
    }