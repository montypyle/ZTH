using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardCtrl : MonoBehaviour
{
    private GameObject hazard;
    // Start is called before the first frame update
    void Start()
    {
        hazard = this.gameObject;
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
        if (collision.gameObject.name == "Shield")
        {
            if (MissionCtrl.instance.gameActive)
            {
                MissionCtrl.instance.score++;
            }
            Destroy(hazard);
        }
    }
}
