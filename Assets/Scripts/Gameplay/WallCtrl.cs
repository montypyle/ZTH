using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCtrl : MonoBehaviour
{
    private GameObject obstacle;
    private Rigidbody2D rb;
    public float speed, deathtime;
    private SpriteRenderer sr;
    public Sprite intact, damaged;
    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.sprite = intact;
        obstacle = this.gameObject;
        rb = obstacle.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, -speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "EndZone")
        {
            if (SPDTraining.instance.gameActive)
            {
                SPDTraining.instance.score++;
            }
            StartCoroutine("Die");
        }
    }
    public IEnumerator Die()
    {
        sr.sprite = damaged;
        yield return new WaitForSeconds(deathtime);
        Destroy(obstacle);

    }
}
