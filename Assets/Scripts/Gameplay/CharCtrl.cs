using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharCtrl : MonoBehaviour
{
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
    public float dmgTime;
    public GameObject shield;
    public Image healthbar;
    public bool shieldEnabled;
    public bool healthEnabled;

    //TODO: lock character to screen
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        coll = this.GetComponent<CapsuleCollider2D>();
        rb.isKinematic = false;
    }

    public void Constrain(string type)
    {
        if (type == "all")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else if (type == "x")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else if (type == "y")
        {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else if (type == "rotate")
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        else if (type == "none")
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //LimitMovement();
        shield.SetActive(shieldEnabled);
    }
    private void LimitMovement()
    {
        float yMin = -Screen.height/2; // lower bound
        float yMax = Screen.height / 2; // upper bound

        float xMin = -Screen.width/2; // left bound
        float xMax = Screen.width/2; // right bound 
        if (transform.position.x < xMin)
        {
            Debug.Log("Player left screen left");
            transform.position = new Vector2(xMin, transform.position.y);
        }
        if (transform.position.x > xMax) 
        {
            Debug.Log("Player left screen right");
            transform.position = new Vector2(xMax, transform.position.y);
        }
        if (transform.position.y < yMin)
        {
            Debug.Log("Player left screen bottom");
            transform.position = new Vector2(transform.position.x, yMin);
        }
        if (transform.position.y > yMax)
        {
            Debug.Log("Player left screen top");
            transform.position = new Vector2(transform.position.x, yMax);
        }
    }
    public void InputShieldMovement(Vector2 input)
    {
        GM.instance.shieldscript.Move(input);
    }
    public void InputMovement(Vector2 moveinput)
    {
        rb.AddForce(moveinput * GM.instance.speed, ForceMode2D.Impulse);
        Debug.Log("added force of " + moveinput * GM.instance.speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hazard"))
        {
            Debug.Log("collided");
            collision.enabled = false;
            TakeDamage(GM.instance.damage);

        }       
    }
    IEnumerator FlashDamage()
    {
        coll.enabled = false;
        yield return new WaitForSeconds(dmgTime);
        coll.enabled = true;
    }
    public void TakeDamage(float dmg)
    {
        //SFX
        StartCoroutine(FlashDamage());
        if (GM.instance.isTraining)
        {
            healthbar.fillAmount -= (dmg / 10);
            Debug.Log("took " + dmg + " damage");
            Debug.Log(healthbar.fillAmount + " health remaining");
            if(TrainMGR.instance.type.Equals("spd"))
            {
                SPDTraining.instance.totalDamage = 1 - (healthbar.fillAmount);
                if (healthbar.fillAmount == 0)
                {
                    SPDTraining.instance.EndGame(false);
                }
            }
            if (TrainMGR.instance.type.Equals("def") && healthbar.fillAmount == 0)
            {
                DEFTraining.instance.missed++;
            }
            if (TrainMGR.instance.type.Equals("fly") && healthbar.fillAmount == 0)
            {
                FLYTraining.instance.totalDamage = 1 - (healthbar.fillAmount);
            }

        }
        else if (GM.instance.isMission)
        {
            healthbar.fillAmount -= (dmg / 10);
            MissionCtrl.instance.totalDamage = 1 - (healthbar.fillAmount);
            if (healthbar.fillAmount == 0)
            {
                MissionCtrl.instance.EndGame(false);
            }
        }

    }
}
