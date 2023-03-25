using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainMGR : MonoBehaviour
{
    public static TrainMGR instance;
    public GameObject atk, def, spd, fly;
    public string type;
    void Start()
    {
        instance = this;
        GM.instance.isTraining = false;
    }
    public void SetTraining(string gametype)
    {
        GM.instance.isTraining = true;
        if (gametype == "atk")
        {
            type = "atk";

        }
        if (gametype == "def")
        {
            type = "def";
        }
        if (gametype == "spd")
        {
            type = "spd";
        }
        if (gametype == "fly")
        {
            type = "fly";
        }
    }
    public void StopTraining()
    {
        GM.instance.isTraining = false;
        type = null;
    }
    void Update()
    {

    }
}
