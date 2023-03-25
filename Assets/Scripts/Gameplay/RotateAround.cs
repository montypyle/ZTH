using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public float rotationSpeed = 90f;

    public Transform _pivot;
    public Vector3 _offsetDirection;
    public float _distance;

    public void SetPivot(Transform pivot)
    {
        if (pivot != null)
        {
            _pivot = pivot;
            _offsetDirection = transform.position - pivot.position;
            _distance = _offsetDirection.magnitude;
        }
        else
        {
            _pivot = null;
        }
    }

    void Update()
    {

    }
    public void RotateShield(Vector2 input)
    {

        Quaternion rotate = Quaternion.Euler(0, input.x * rotationSpeed, input.y * rotationSpeed);

        _offsetDirection = (rotate * _offsetDirection).normalized;

        transform.position = _pivot.position + _offsetDirection * _distance;
    }
}

