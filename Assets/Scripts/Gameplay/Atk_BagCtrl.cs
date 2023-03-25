using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk_BagCtrl : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = this.GetComponent<Animator>();
        anim.SetBool("Down", false);
    }
    public void HitRight()
    {
        anim.SetTrigger("Right");
    }
    public void HitLeft()
    {
        anim.SetTrigger("Left");
    }
    public void GoDown()
    {
        anim.SetBool("Down", true);
    }
}
