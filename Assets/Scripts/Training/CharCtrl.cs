using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed, dmgTime;
    private BoxCollider2D collider;
    private SpriteRenderer sr;
    public Color dmgColor;
    private Color regColor;

    //TODO: lock character to screen
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        sr = GetComponent<SpriteRenderer>();
        regColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        LimitMovement();
    }
    private void LimitMovement()
    {
        Vector3 screenPos = Camera.main.ScreenToWorldPoint(transform.position);

        if (screenPos.x < 0)
        {
            screenPos = new Vector2(0, screenPos.y);
        }
        if (screenPos.x > Screen.width) 
        {
            screenPos = new Vector2(Screen.width, screenPos.y);
        }
        if (screenPos.y < 0)
        {
            screenPos = new Vector2(screenPos.x, 0);
        }
        if (screenPos.y > Screen.height)
        {
            screenPos = new Vector2(screenPos.x, Screen.height);
        }
    }
    private void LateUpdate()
    {
        /**
        Vector3 bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        Vector3 topRight = Camera.main.ScreenToWorldPoint(new Vector3(
            Camera.main.pixelWidth, Camera.main.pixelHeight));

        Rect cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax),
            Mathf.Clamp(transform.position.y, cameraRect.yMin, cameraRect.yMax),
                        transform.position.z);
        **/
    }
    public void InputMovement(Vector2 input)
    {
        rb.velocity = input * moveSpeed;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("collided");
            SPDTraining.instance.TakeDamage();
            StartCoroutine(FlashDamage());
        }       
    }
    IEnumerator FlashDamage()
    {
        collider.enabled = false;
        sr.color = dmgColor;
        yield return new WaitForSeconds(dmgTime);
        collider.enabled = true;
        sr.color = regColor;
    }
}
