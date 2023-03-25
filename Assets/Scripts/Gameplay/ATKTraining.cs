using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


public class ATKTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results, prompt;
    public Atk_BagCtrl bagCtrl;
    public Transform targetPos, spawn;
    public Transform[] spawnpoints;
    public GameObject target, endScreen, gameCanv;
    public int score, missed, cost;
    public static ATKTraining instance;
    public bool gameActive, trainingStarted;
    private bool hasRun;
    private float gameTime, spawninterval;
    public float targetLife, minSpawn, maxSpawn, minLife, maxLife;
    void Start()
    { 
        instance = this;
        endScreen.SetActive(false);
        gameActive = false;
        timerTxt.enabled = false;
        countdownTxt.enabled = false;
        scoreTxt.enabled = false;
        hasRun = false;
        GM.instance.BalanceGame();
        SetParams();
        StartWrapper();
    }
    void Update()
    {
        if (gameTime == 0)
        {
            gameActive = false;
            StopCoroutine(StartTimer());
            if (!hasRun) 
            { 
                EndGame(); 
            }
        }
        if(gameActive)
        {
            timerTxt.text = gameTime + " s";
            scoreTxt.text = score + " pts";
        }
        
    }
    void SetParams()
    {
        float atkT = GM.instance.atkLVL / 10;
        gameTime = Mathf.Lerp(GM.instance.minTime, GM.instance.maxTime, atkT);
        spawninterval = Mathf.Lerp(minSpawn, maxSpawn, atkT);
        targetLife = Mathf.Lerp(minLife, maxLife, atkT);
        Debug.Log("Calculated parameters: game time = " + gameTime + "spawn interval = " + spawninterval + "target lifespan = " + targetLife);

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
        if(GM.instance.energy - cost >= 0)
        {
            trainingStarted = true;
            TrainMGR.instance.SetTraining("atk");
            GM.instance.energy -= cost;
            score = 0;
            missed = 0;
            GM.instance.SetSpawn(spawn);
            StartCoroutine(StartCountdown());
        }
        else
        {
            gameCanv.SetActive(false);
            GM.instance.EnergyWarning();
        }
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
        bagCtrl.GoDown();
        timerTxt.enabled = false;
        scoreTxt.enabled = false;
        prompt.enabled = false;
        endScreen.SetActive(true);
        results.text = "your score was " + score + "\n you missed " + missed + " targets" + "\n you earned €" + score;
        GM.instance.Reward(score);
        if(score > GM.instance.atkHI)
        {
            GM.instance.atkHI = score;
        }
        int exp = (score - missed);
        if (trainingStarted)
        {
            GM.instance.IncreaseStat(exp, "attack");
            Dictionary<string, object> parameters = new Dictionary<string, object>{
                    { "rawScore", score },
                    { "damage", missed },
                    { "skillLevel", GM.instance.atkLVL } };
            AnalyticsService.Instance.CustomData("attackTrainingComplete", parameters);
        }
        trainingStarted = false;
        TrainMGR.instance.StopTraining();
        hasRun = true;
    }
}
