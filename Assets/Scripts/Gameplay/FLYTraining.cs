using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Analytics;
using Unity.Services.Analytics;

public class FLYTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results;
    public float spawninterval, totalDamage, minSpawn, maxSpawn, minSpeed, maxSpeed, minDmg, maxDmg, minLife, maxLife;
    public Transform[] spawnpoints;
    public GameObject missile, endScreen, gameCanv;
    public int score;
    private int gameTime, cost;
    public static FLYTraining instance;
    public bool gameActive, trainingStarted;
    private bool hasRun;
    public Transform target, spawn;
    private CharCtrl heroCtrl;
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
            EndGame(true);
        
        }   
        if (gameActive)
        {
            timerTxt.text = gameTime + " s";
            scoreTxt.text = score - totalDamage + " pts";
        }
    }
    public void Input(Vector2 input)
    {
        GM.instance.InputMovement(input);
    }
    void SetParams()
    {
        float flyT = GM.instance.flyLVL / 10;
        gameTime = (int)Mathf.Lerp(GM.instance.minTime, GM.instance.maxTime, flyT);
        spawninterval = Mathf.Lerp(minSpawn, maxSpawn, flyT);
        EnemyCtrl missileScript = missile.GetComponent<EnemyCtrl>();
        missileScript.speed = Mathf.Lerp(minSpeed, maxSpeed, flyT);
        missileScript.attackPower = Mathf.Lerp(minDmg, maxDmg, flyT);
        missileScript.life = Mathf.Lerp(minLife, maxLife, flyT);
        Debug.Log("Calculated parameters: game time = " + gameTime + "spawn interval = " + spawninterval + " missile speed = " + missileScript.speed + " attack power = " + missileScript.attackPower + "missile lifespan = " + missileScript.life);
    }
    IEnumerator SpawnMissiles()
    {
        while (gameActive)
        {
            int index = Random.Range(0, spawnpoints.Length);
            Transform currentspawn = spawnpoints[index];
            Instantiate(missile, currentspawn);
            yield return new WaitForSeconds(spawninterval / 10);
        }

    }
    public void StartWrapper()
    {
        if (GM.instance.energy - cost >= 0)
        {
            trainingStarted = true;
            TrainMGR.instance.SetTraining("fly");
            GM.instance.energy -= cost;
            totalDamage = 0;
            SpawnHero();
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
        StartCoroutine(SpawnMissiles());
    }
    public void SpawnHero()
    {
        GM.instance.SetSpawn(spawn);
        target = GM.instance.hero.GetComponent<Transform>();
        heroCtrl = GM.instance.hero.GetComponentInChildren<CharCtrl>();
        heroCtrl.Constrain("none");
        heroCtrl.shieldEnabled = false;
        heroCtrl.healthEnabled = false;
    }
    IEnumerator StartTimer()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
        }
    }

    public void EndGame(bool passed)
    {
        if(!hasRun)
        {
            endScreen.SetActive(true);
        if (!passed)
        {
            results.text = "you died! \n you took " + totalDamage + " damage" + "\n you earned €" + score/2;
            GM.instance.Reward(score/2);
        }
        else
        {
            results.text = "you survived! \n you took " + totalDamage + " damage" + "\n you earned €" + score;
            GM.instance.Reward(score);
        }
        if(score > GM.instance.flyHI)
        {
            GM.instance.flyHI = score;
        }
        int exp = (score);
        if (trainingStarted)
        {
            GM.instance.IncreaseStat(exp, "flight");
            Dictionary<string, object> parameters = new Dictionary<string, object>{
                { "rawScore", score + totalDamage },
                { "damage", totalDamage },
                { "skillLevel", GM.instance.defLVL },
                { "passed", passed }
            };
            AnalyticsService.Instance.CustomData("flightTrainingComplete", parameters);
        }
        trainingStarted = false;
        TrainMGR.instance.StopTraining();
        hasRun = true;
        }
    }
}
