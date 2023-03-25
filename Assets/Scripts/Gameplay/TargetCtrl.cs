using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCtrl : MonoBehaviour
{
    private GameObject targetObj;
    public GameObject source;
    private ObstacleCtrl obsCtrl;
    private float life;
    // Start is called before the first frame update
    void Start()
    {
        if(GM.instance.isTraining)
        {
            life = ATKTraining.instance.targetLife;
        }
        else if (GM.instance.isMission)
        {
            obsCtrl = source.GetComponent<ObstacleCtrl>();
            life = obsCtrl.targetLife;
        }
        StartCoroutine(Lifespan());
        targetObj = this.gameObject;
        Vector3 targetPos = this.transform.position;
        this.transform.position = new Vector3(targetPos.x, targetPos.y, 1);

    }

    private void OnMouseDown()
    {

        if (GM.instance.isTraining)
        {
            ATKTraining.instance.score++;
            if (this.transform.position.x > ATKTraining.instance.targetPos.position.x)
            {
                ATKTraining.instance.bagCtrl.HitLeft();
            }
            if (this.transform.position.x < ATKTraining.instance.targetPos.position.x)
            {
                ATKTraining.instance.bagCtrl.HitRight();
            }
        }
        else if (GM.instance.isMission)
        {
            if(MissionCtrl.instance.gameActive)
            {
                obsCtrl.obsHealth -= (int)GM.instance.power;
                if (obsCtrl.obsHealth <= 0)
                {
                    ObstacleMGR.instance.SetReward(source);
                }
            }
        
        }
        Destroy(targetObj);
    }
    
    IEnumerator Lifespan()
    {

        yield return new WaitForSeconds(life);
        if (GM.instance.isTraining)
        {
            ATKTraining.instance.missed++;
        }
        else if(GM.instance.isMission)
        { 
            if (MissionCtrl.instance.gameActive)
            {
                obsCtrl.obsHealth++;
            }
        }
        Destroy(targetObj);
    }

}
