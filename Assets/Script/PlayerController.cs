using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator playerAnimator;
    public float speed;
    public GameController gc;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!rb.isKinematic)
            rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * speed, Input.GetAxisRaw("Vertical") * speed);
    }

    public void TimeEnd()
    {
        playerAnimator.SetBool("isAlive", false);
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Energy")
            gc.GetComponent<GameController>().IncreaseTimer();
    }
}
