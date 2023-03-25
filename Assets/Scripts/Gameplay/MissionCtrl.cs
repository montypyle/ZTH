using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Services.Analytics;
using TMPro;

public class MissionCtrl : MonoBehaviour
{
    public TextMeshProUGUI timerTxt, scoreTxt, countdownTxt, results;
    public Transform spawn;
    public float totalDamage;
    public int score, gameTime, missed, bonusGot, missionNo, bonusScore, spawninterval, cost;
    public static MissionCtrl instance;
    public bool gameActive, missionStarted;
    public GameObject endScreen, hazard;
    public Image healthbar;
    public UIVirtualJoystick moveStick, shieldStick;
    public Transform[] spawnpoints;

    void Start()
    {
        instance = this;
        GM.instance.charCam.gameObject.SetActive(true);
        GM.instance.SetSpawn(spawn);
        healthbar = GM.instance.healthbar;
        endScreen.SetActive(false);
        gameActive = false;
        timerTxt.enabled = false;
        countdownTxt.enabled = false;
        scoreTxt.enabled = false;
        GM.instance.BalanceGame();
        StartWrapper();

    }

    void Update()
    {
        if (gameTime == 0)
        {
            gameActive = false;
            moveStick.locked = true;
            shieldStick.locked = true;
            StopCoroutine(StartTimer());
            EndGame(true);
        }
        if (gameActive)
        {
            timerTxt.text = gameTime + " s";
            scoreTxt.text = score + " pts";
        }


    }
    public void InputMove(Vector2 input)
    {
        GM.instance.InputMovement(input);
    }
    public void InputShield(Vector2 input)
    {
        GM.instance.InputShieldMovement(input);
    }
    public void StartWrapper()
    {
        GM.instance.isMission = true;
        missionStarted = true;
        moveStick.locked = true;
        shieldStick.locked = true;
        GM.instance.energy -= GM.instance.missionCost;
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
        gameActive = true;
        scoreTxt.enabled = true;
        timerTxt.enabled = true;
        moveStick.locked = false;
        shieldStick.locked = false;
        StartCoroutine(StartTimer());
        StartCoroutine(SpawnHazards());
    }

    IEnumerator StartTimer()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(1);
            gameTime--;
        }
    }
    IEnumerator SpawnHazards()
    {
        while (gameActive)
        {
            yield return new WaitForSeconds(spawninterval / 10);
            int index = Random.Range(0, spawnpoints.Length);
            Transform currentspawn = spawnpoints[index];
            Instantiate(hazard, currentspawn);
        }

    }
    public void EndGame(bool passed)
    {
        int exp;
        endScreen.SetActive(true);
        if (!passed)
        {
            results.text = "you failed! \n you earned " + score + "pts" + "\n you earned €" + score;
            GM.instance.money += score;
            exp = ((score - (int)totalDamage))/2;
        }
        else
        {
            results.text = "you passed! \n you earned " + score + "pts" + "\n you earned €" + score*2;
            GM.instance.money += score*2;
            exp = (score - (int)totalDamage);

        }

       if (missionStarted)
        {
            GM.instance.IncreaseStat(exp, "attack");
            GM.instance.IncreaseStat(exp, "defense");
            GM.instance.IncreaseStat(exp, "speed");
            GM.instance.IncreaseStat(exp, "flight");
            if (missionNo == 1)
            {
                if (score > GM.instance.m1HI)
                {
                    GM.instance.m1HI = score;
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>{
                { "rawScore", score },
                { "damage", totalDamage },
                { "bonuses", bonusGot },
                {"passed", passed} };
                AnalyticsService.Instance.CustomData("mission1Complete", parameters);
            }
            else if (missionNo == 2)
            {
                if (score > GM.instance.m2HI)
                {
                    GM.instance.m2HI = score;
                }
                Dictionary<string, object> parameters = new Dictionary<string, object>{
                { "rawScore", score },
                { "damage", totalDamage },
                { "bonuses", bonusGot },
                {"passed", passed} };
                AnalyticsService.Instance.CustomData("mission2Complete", parameters);
            }
        }
        GM.instance.charCam.gameObject.SetActive(false);
        missionStarted = false;
        GM.instance.isMission = false;
    }
    public void GetBonus()
    {
        bonusGot++;
        score += bonusScore;
    }
}
