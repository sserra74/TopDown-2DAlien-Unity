using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public GameObject explosionEffect;
    Rigidbody2D rb;
    void Start()
    {
        Invoke("LifespanEnd", 3f);
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    void LifespanEnd(){
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(explosion, 0.2f);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Enemy" || collision.tag == "Wall"){
            LifespanEnd();
        }
    }
}
