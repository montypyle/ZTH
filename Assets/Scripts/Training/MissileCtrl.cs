using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCtrl : MonoBehaviour
{
    private GameObject missileObj;
    // Start is called before the first frame update
    void Start()
    {
        missileObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == ("Shield"))
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
