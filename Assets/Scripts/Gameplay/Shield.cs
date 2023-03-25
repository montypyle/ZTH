using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public Transform shield;
    public Transform parent;
    public SpriteRenderer sr;
    public Vector2 distance, pivot, jPos, idle_pos, idleV, jV, nullInput;



    // Start is called before the first frame update
    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.size.Set(0.5f, GM.instance.shield);
        nullInput.Set(0, 0);
        idleV.Set(0, 3);
        pivot = parent.position;
        distance = shield.localPosition;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        pivot = parent.position;
        jPos.Set(0, 0);
        LookAt2D(shield, parent.position);
    }
    public void LookAt2D(Transform transform, Vector2 target)
    {
        Vector2 current = transform.position;
        var direction = target - current;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnEnable()
    {
        sr = this.GetComponent<SpriteRenderer>();
        sr.size.Set(0.5f, GM.instance.shield);
        nullInput.Set(0, 0);
        idleV.Set(0, 3);
        pivot = parent.position;
        distance = shield.localPosition;
    }
    public void Move(Vector2 input)
    {
        if (input.Equals(nullInput))
        {
            idleV = input - pivot;
            idle_pos = pivot + idleV.normalized * new Vector2(0, 4f).magnitude;
            this.transform.position = idle_pos;
        }
        else
        {
            jV = input - jPos;
            Vector2 new_pos = pivot + input.normalized * new Vector2 (0, 4f).magnitude;
            this.transform.position = new_pos;
        }
    }

}
