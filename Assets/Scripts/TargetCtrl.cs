using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    public float targetlife;
    private GameObject targetObj;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Lifespan());
        targetObj = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("target hit");
        ATKTraining.instance.score++;
        Destroy(targetObj);
    }
    IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(targetlife);
        ATKTraining.instance.missed++;
        Destroy(targetObj);
    }
}
