using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private Transform shield;
    public Transform parent;
    public float turnSpeed;
    private Vector2 distance, pivot, jPos, idle_pos, idleV, jV, nullInput;



    // Start is called before the first frame update
    void Start()
    {
        nullInput.Set(0, 0);
        idleV.Set(0, 1);
        pivot = parent.position;
        shield = this.transform;
        distance = parent.position - shield.position;

    }

    // Update is called once per frame
    void Update()
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
    public void Move(Vector2 input)
    {
        if (input.Equals(nullInput))
        {
            Debug.Log("no input");
            idle_pos = pivot + idleV.normalized * distance.magnitude;
            shield.position = idle_pos;
        }
        else
        {
            jV = input - jPos;
            idleV = input - pivot;
            Vector2 new_pos = pivot + jV.normalized * distance.magnitude;
            shield.position = new_pos;
        }
    }

}
