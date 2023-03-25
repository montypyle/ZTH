using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMGR : MonoBehaviour
{
    public Transform spawn;
    void Start()
    {
        GM.instance.SetSpawn(spawn);
    }

}
