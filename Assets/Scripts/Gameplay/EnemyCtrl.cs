using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    private GameObject missileObj;
    public float speed, attackPower, life, range;
    public GameObject target;
    private NavMeshAgent agent;
    public Animator anim;
    private Transform startPos;
    public bool hasEyeline, timerIsRunning;
    private float timeRemaining;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        timerIsRunning = true;
        timeRemaining = life;
        target = GameObject.FindWithTag("Player");
        startPos = this.transform;
        missileObj = this.gameObject;
        if(TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
            agent.speed = speed;
        }

    }

    private void FixedUpdate()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                if (GM.instance.isTraining && TrainMGR.instance.type.Equals("fly"))
                {
                    FLYTraining.instance.score++;
                }
                timeRemaining = 0;
                timerIsRunning = false;
                timeRemaining = life;
                Die();
            }
        }

        if (hasEyeline)
        {
            Vector2 direction = target.transform.position - this.transform.position;
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, range);
            Debug.DrawRay(this.transform.position, direction);
            if (hit)
            {
                if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Shield"))
                {
                    HomeIn(target.transform);
                }
            }
            else
            {
                HomeIn(startPos);
            }
        }
        else
        {
            HomeIn(target.transform);
        }

    }
    public void Die()
    {
        //triggered in Die animation
        Destroy(missileObj);
    }
    public void HomeIn(Transform target)
    {
        float step = speed * Time.deltaTime;
        Vector2 targetPos = target.position;
        if(agent != null)
        {
            agent.SetDestination(targetPos);
        }
        else
        {
            missileObj.transform.position = Vector3.MoveTowards(missileObj.transform.position, targetPos, step);
        }
        LookAt2D(missileObj.transform, targetPos);
    }
    public void LookAt2D(Transform transform, Vector2 target)
    {
        Vector2 current = transform.position;
        var direction = target - current;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 180;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Attacked Player");
            if(target.TryGetComponent<CharCtrl>(out CharCtrl targetScript))
            {
                targetScript.TakeDamage(attackPower);
            }
            if (GM.instance.isTraining)
            {
                if(TrainMGR.instance.type.Equals("def"))
                {
                    DEFTraining.instance.missed++;
                }
                if (TrainMGR.instance.type.Equals("fly"))
                {
                    FLYTraining.instance.totalDamage += attackPower;
                }
            }
            else if(GM.instance.isMission)
            {
                MissionCtrl.instance.score--;
            }
            anim.SetTrigger("Die");
        }
        if (collision.gameObject.CompareTag("Shield"))
        {
            if (GM.instance.isTraining)
            {
                Debug.Log("missile blocked");
                DEFTraining.instance.score++;
            }
            else if (GM.instance.isMission)
            {
                Debug.Log("Enemy blocked");
                MissionCtrl.instance.score++;
            }
            anim.SetTrigger("Die");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shield"))
        {
            if (GM.instance.isTraining)
            {
                Debug.Log("missile blocked");
                DEFTraining.instance.score++;
            }
            else if (GM.instance.isMission)
            {
                Debug.Log("Enemy blocked");
                MissionCtrl.instance.score++;
            }
            anim.SetTrigger("Die");
        }
    }
}
