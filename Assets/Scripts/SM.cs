using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SM : MonoBehaviour
{

    // Start is called before the first frame update
    void Awake()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load(string scenename)
    {
        SceneManager.LoadSceneAsync(scenename);
    }

    public void DebugLog(string debug)
    {
        Debug.Log(debug);
    }
}
