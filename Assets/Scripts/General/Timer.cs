using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Timer : MonoBehaviour
{
    public float time;
    private float timeRemaining;
    public bool timerIsRunning = false;

    private void Start()
    {
        timeRemaining = time;
    }
    private void Disable()
    {
       Scene scene = SceneManager.GetActiveScene();
       if(scene.name =="Train")
        {
            SceneManager.LoadSceneAsync("Train");
        }
        this.gameObject.SetActive(false);

    }
    private void Update()
    {
        if(timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                timeRemaining = time;
                Disable();
            }
        }
    }
}
