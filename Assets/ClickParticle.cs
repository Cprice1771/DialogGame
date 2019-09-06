using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickParticle : MonoBehaviour
{

    [SerializeField]
    public float LifeTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Color tmp = GetComponent<SpriteRenderer>().color;
        tmp.a -= (Time.deltaTime / LifeTime);

        if (tmp.a <= 0) {
            Destroy(this.gameObject);
        }
        GetComponent<SpriteRenderer>().color = tmp;
    }
}
