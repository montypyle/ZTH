using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


public class SPDTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results;
    public float spawninterval, damage, totalDamage;
    public Transform spawnpoint;
    public GameObject[] obstacles;
    public int score, gameTime, missed;
    public static SPDTraining instance;
    public bool gameActive, trainingStarted;
    public GameObject endScreen;
    public Image healthbar;

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

    IEnumerator SpawnObstacles()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawninterval / 10);
            int index = Random.Range(0, obstacles.Length);
            GameObject obstacle = obstacles[index];
            Instantiate(obstacle, spawnpoint);
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
        StartCoroutine(SpawnObstacles());
    }

    IEnumerator StartTimer()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
        }
    }

    void EndGame(bool passed)
    {
        endScreen.SetActive(true);
        if (!passed)
        {
            results.text = "you died! \n you earned " + score + "pts";
        }
        else
        {
            results.text = "you survived! \n you earned " + score + "pts";
        }
        // TODO: calculate an overall score and record in gamedata as a high score
        int exp = (score - (int)damage);
        if (trainingStarted)
        {
            GM.instance.IncreaseStat(exp, "speed");
            AnalyticsService.Instance.CustomData("speedTrainingComplete", new Dictionary<string, object>{
                { "rawScore", score },
                { "damage", totalDamage },
                { "skillLevel", GM.instance.spdLVL },
                {"passed", passed}

            }
                );
        }
        trainingStarted = false;
    }
    public void TakeDamage()
    {
        //SFX
        healthbar.fillAmount -= (damage/10);
        totalDamage = 1 - (healthbar.fillAmount);
        if(healthbar.fillAmount == 0)
        {
            EndGame(false);
        }
    }
}





