using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ATKTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt;
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI countdownTxt;
    public float spawninterval;
    public Transform[] spawnpoints;
    public GameObject target;
    public int score;
    public static ATKTraining instance;
    private bool gameActive;
    private int gameTime;
    public GameObject endScreen;
    public TextMeshProUGUI results;
    public int missed;
    private int finalMissed;
    // Start is called before the first frame update
    void Start()
    { 
        instance = this;
        endScreen.SetActive(false);
        gameActive = false;
        timerTxt.enabled = false;
        countdownTxt.enabled = false;
        scoreTxt.enabled = false;
        //TODO: based on level of stat, change gametime, spawninterval, and target lifespan
        gameTime = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime <= 0)
        {
            gameActive = false;
            StopCoroutine(StartTimer());
            EndGame();
        }
        if(gameActive)
        {
            timerTxt.text = gameTime + " s";
            scoreTxt.text = score + " pts";
        }
        
    }

    IEnumerator SpawnTargets()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawninterval/10);
            int index = Random.Range(0, spawnpoints.Length);
            Transform currentspawn = spawnpoints[index];
            Instantiate(target, currentspawn);
        }

    }
    public void StartWrapper()
    {
        GM.instance.energy -= 30;
        score = 0;
        missed = 0;
        StartCoroutine(StartCountdown());
    }
    public IEnumerator StartCountdown()
    {
        countdownTxt.enabled = true;
        countdownTxt.text = "Ready...";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "3...";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "2...";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "1...";
        yield return new WaitForSeconds(1);
        countdownTxt.text = "GO!";
        countdownTxt.enabled = false;
        //start timer, start spawning targets
        gameActive = true;
        scoreTxt.enabled = true;
        timerTxt.enabled = true;
        StartCoroutine(StartTimer());
        StartCoroutine(SpawnTargets());
    }

    IEnumerator StartTimer()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
        }
    }

    void EndGame()
    {
        finalMissed = missed; //to prevent it from continuing to update with targets that despawn after the game ends
        endScreen.SetActive(true);
        results.text = "your score was " + score + "\n you missed " + finalMissed + " targets";
        // TODO: calculate an overall score and record in gamedata as a high score
        // convert the overall score to a stat increase and pass to STATS
    }
}
