using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


public class SPDTraining : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results;
    public float totalDamage, minSpawn, maxSpawn, minSpeed, maxSpeed, wallSpeed;
    public Transform spawnpoint, heroSpawn;
    public GameObject[] obstacles;
    public int score, gameTime, missed, cost;
    public static SPDTraining instance;
    public bool gameActive, trainingStarted;
    private bool hasRun;
    public GameObject endScreen, gameCanv;
    public Image healthbar;
    private float spawninterval;
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
    void SetParams()
    {
        float spdT = GM.instance.spdLVL / 10;
        gameTime = (int)Mathf.Lerp(GM.instance.minTime, GM.instance.maxTime, spdT);
        spawninterval = Mathf.Lerp(minSpawn, maxSpawn, spdT);
        wallSpeed = Mathf.Lerp(minSpeed, maxSpeed, spdT);
        Debug.Log("Calculated parameters: game time = " + gameTime + "spawn interval = " + spawninterval + " wall speed = " + wallSpeed) ;

    }
    
    public void InputMovement(Vector2 input)
    {
        GM.instance.InputMovement(input);
    }
    IEnumerator SpawnObstacles()
    {
        while (gameActive)
        {
            int index = Random.Range(0, obstacles.Length);
            GameObject obstacle = obstacles[index];
            Instantiate(obstacle, spawnpoint);
            yield return new WaitForSeconds(spawninterval / 10);

        }

    }
    public void StartWrapper()
    {

        if (GM.instance.energy - cost >= 0)
        {
            trainingStarted = true;
            TrainMGR.instance.SetTraining("spd");
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
        StartCoroutine(SpawnObstacles());
    }
    public void SpawnHero()
    {
        GM.instance.SetSpawn(heroSpawn);
        healthbar = GM.instance.healthbar;
        heroCtrl = GM.instance.hero.GetComponentInChildren<CharCtrl>();
        heroCtrl.shieldEnabled = false;
        heroCtrl.healthEnabled = true;
        heroCtrl.Constrain("none");
    }
    IEnumerator StartTimer()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
        }
    }
    void GenerateCollidersAcrossScreen()
    {
        Vector2 lDCorner = camera.ViewportToWorldPoint(new Vector3(0, 0f, camera.nearClipPlane));
        Vector2 rUCorner = camera.ViewportToWorldPoint(new Vector3(1f, 1f, camera.nearClipPlane));
        Vector2[] colliderpoints;

        EdgeCollider2D upperEdge = new GameObject("upperEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = upperEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        upperEdge.points = colliderpoints;

        EdgeCollider2D lowerEdge = new GameObject("lowerEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = lowerEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        lowerEdge.points = colliderpoints;

        EdgeCollider2D leftEdge = new GameObject("leftEdge").AddComponent<EdgeCollider2D>();
        colliderpoints = leftEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderpoints;

        EdgeCollider2D rightEdge = new GameObject("rightEdge").AddComponent<EdgeCollider2D>();

        colliderpoints = rightEdge.points;
        colliderpoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderpoints;
    }
    public void EndGame(bool passed)
    {
        if (!hasRun)
        {
            endScreen.SetActive(true);
            if (!passed)
            {
                results.text = "you died! \n you earned " + score + "pts" + "\n you earned €" + score / 2;
                GM.instance.Reward(score / 2);
            }
            else
            {
                results.text = "you survived! \n you earned " + score + "pts" + "\n you earned €" + score;
                GM.instance.Reward(score);
            }
            if (score > GM.instance.spdHI)
            {
                GM.instance.spdHI = score;
            }
            int exp = (score - (int)totalDamage);
            if (trainingStarted)
            {
                GM.instance.IncreaseStat(exp, "speed");
                Dictionary<string, object> parameters = new Dictionary<string, object>{
                { "rawScore", score },
                { "damage", totalDamage },
                { "skillLevel", GM.instance.spdLVL },
                {"passed", passed}
            };
                AnalyticsService.Instance.CustomData("speedTrainingComplete", parameters);
                AnalyticsService.Instance.CustomData("speedTraindingComplete", parameters);
                AnalyticsService.Instance.CustomData("speedTraindingComplete", parameters);
            }
            trainingStarted = false;
            TrainMGR.instance.StopTraining();
            hasRun = true;
        }
    }
}





