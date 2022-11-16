using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrl : MonoBehaviour
{
    private GameObject missileObj;
    public float speed;
    private Vector2 missile;

    // Start is called before the first frame update
    void Start()
    {
        missileObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float step = speed * Time.deltaTime;
        Vector2 target = DEFTraining.instance.target.position;
        missileObj.transform.position = Vector3.MoveTowards(missileObj.transform.position, target, step);
        LookAt2D(missileObj.transform, target);
    }
    public void LookAt2D(Transform transform, Vector2 target)
    {
        Vector2 current = transform.position;
        var direction = target - current;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            Debug.Log("target hit");
            DEFTraining.instance.score++;
            Destroy(missileObj);
        }
        else if(collision.gameObject.name == ("EndZone"))
        {
            if (DEFTraining.instance.gameActive)
            {
                DEFTraining.instance.missed++;
            }
            Destroy(missileObj);
        }
    }

}
