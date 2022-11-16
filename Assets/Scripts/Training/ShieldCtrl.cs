using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldCtrl : MonoBehaviour
{
    private float distance;
    private Vector2 velocity;
    private Rigidbody2D shieldRB;
    private Vector3 position;
    private float boundX;
    public float speed;
    private bool isDraggable;
    private bool isDragging;
    private Collider2D objectCollider;
    private Transform shieldPos;

    // Start is called before the first frame update
    void Start()
    {
        boundX = Screen.width;
        shieldRB = GetComponent<Rigidbody2D>();
        objectCollider = GetComponent<Collider2D>();
        shieldPos = GetComponent<Transform>();
        isDraggable = false;
        isDragging = false;
    }

    void Update()
    {
        DragAndDrop();
        shieldPos.position = new Vector2(Mathf.Clamp(transform.position.x, -boundX, boundX), shieldPos.position.y);
    }

    void DragAndDrop()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (objectCollider == Physics2D.OverlapPoint(mousePosition) && DEFTraining.instance.gameActive)
            {
                isDraggable = true;
            }
            else
            {
                isDraggable = false;
            }

            if (isDraggable)
            {
                isDragging = true;
            }
        }
        if (isDragging)
        {
            shieldPos.position = new Vector3(mousePosition.x, shieldPos.position.y, 0);
            this.transform.position = shieldPos.position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDraggable = false;
            isDragging = false;
        }
    }
}
