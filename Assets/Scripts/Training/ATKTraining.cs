using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


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
    public bool gameActive;
    public float gameTime = 15;
    public GameObject endScreen;
    public TextMeshProUGUI results;
    public int missed;
    public bool trainingStarted;
    public float targetLife = 1;
    // Start is called before the first frame update
    void Start()
    { 
        instance = this;
        endScreen.SetActive(false);
        gameActive = false;
        timerTxt.enabled = false;
        countdownTxt.enabled = false;
        scoreTxt.enabled = false;
       // BalanceGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameTime == 0)
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
   /** 
     void BalanceGame()
    {
        gameTime = (gameTime*4)/ (GM.instance.atkLVL*5);
        targetLife = (GM.instance.atkLVL / targetLife);
        spawninterval = (GM.instance.atkLVL / spawninterval);
        Debug.Log("game time = " + gameTime);
        Debug.Log("target lifespan = " + targetLife);
        Debug.Log("spawn interval = " + spawninterval);
    }
   **/

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
        trainingStarted = true;
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
        endScreen.SetActive(true);
        results.text = "your score was " + score + "\n you missed " + missed + " targets";
        // TODO: calculate an overall score and record in gamedata as a high score
        // convert the overall score to a stat increase and pass to STATS
        int exp = (score - missed);
        if (trainingStarted)
        {
            GM.instance.IncreaseStat(exp, "attack");
            AnalyticsService.Instance.CustomData("attackTrainingComplete", new Dictionary<string, object>{
                    { "rawScore", score },
                    { "damage", missed },
                    { "skillLevel", GM.instance.atkLVL }
                }
            );
        }
    
        trainingStarted = false;
    }
}
