using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


public class DEFTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results;
    public float minSpawn, maxSpawn, minDmg, maxDmg, minSpeed, maxSpeed;
    private float spawninterval;
    public Transform[] spawnpoints;
    public GameObject missile, endScreen, gameCanv;
    public int score, missed, gameTime, cost;
    public static DEFTraining instance;
    public bool gameActive, trainingStarted;
    private bool hasRun;
    private int finalMissed;
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
            scoreTxt.text = score + " pts";
        }


    }
    public void Input(Vector2 input)
    {
        GM.instance.InputShieldMovement(input);
    }
    void SetParams()
    {
        float defT = GM.instance.defLVL / 10;
        gameTime = (int)Mathf.Lerp(GM.instance.minTime, GM.instance.maxTime, defT);
        spawninterval = Mathf.Lerp(minSpawn, maxSpawn, defT);
        EnemyCtrl missileScript = missile.GetComponent<EnemyCtrl>();
        missileScript.speed = Mathf.Lerp(minSpeed, maxSpeed, defT);
        missileScript.attackPower = Mathf.Lerp(minDmg, maxDmg, defT);
        Debug.Log("Calculated parameters: game time = " + gameTime + "spawn interval = " + spawninterval + " missile speed = " + missileScript.speed + " attack power = " + missileScript.attackPower);

    }
    IEnumerator SpawnMissiles()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawninterval / 10);
            int index = Random.Range(0, spawnpoints.Length);
            Transform currentspawn = spawnpoints[index];
            Instantiate(missile, currentspawn);
        }

    }
    public void StartWrapper()
    {

        if (GM.instance.energy - cost >= 0)
        {
            trainingStarted = true;
            TrainMGR.instance.SetTraining("def");
            GM.instance.energy -= cost;
            score = 0;
            missed = 0;
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
        heroCtrl.shieldEnabled = true;
        heroCtrl.healthEnabled = false;
        heroCtrl.Constrain("all");
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
        if (!hasRun)
        {
            finalMissed = missed; //to prevent it from continuing to update with targets that despawn after the game ends
            endScreen.SetActive(true);
            if (!passed)
            {
                results.text = "you died! \n your score was " + score + "\n you missed " + finalMissed + " targets" + "\n you earned €" + score / 2;
                GM.instance.Reward(score / 2);
            }
            else
            {
                results.text = "you survived! \n your score was " + score + "\n you missed " + finalMissed + " targets" + "\n you earned €" + score;
                GM.instance.Reward(score);
            }
            if (score > GM.instance.defHI)
            {
                GM.instance.defHI = score;
            }
            int exp = (score - missed);
            if (trainingStarted)
            {
                GM.instance.IncreaseStat(exp, "defense");
                Dictionary<string, object> parameters = new Dictionary<string, object>{
                { "rawScore", score },
                { "damage", missed },
                { "skillLevel", GM.instance.defLVL },
                { "passed", passed }
            };
                AnalyticsService.Instance.CustomData("defenseTrainingComplete", parameters);
            }
            trainingStarted = false;
            TrainMGR.instance.StopTraining();
            hasRun = true;
        }
    }
}
