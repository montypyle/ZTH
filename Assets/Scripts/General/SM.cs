using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SM : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {

    }
    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load(string scenename)
    {
        if(scenename == "Minigame 1")
        {
            if(GM.instance.energy - GM.instance.missionCost >= 0)
            {
                SceneManager.LoadSceneAsync(scenename);
            }
            else
            {
                GM.instance.EnergyWarning();
            }
        }
        else
        {
            SceneManager.LoadSceneAsync(scenename);

        }
    }

    public void DebugLog(string debug)
    {
        Debug.Log(debug);
    }
}
