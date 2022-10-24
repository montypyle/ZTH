using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public static GM instance;
    public int money;
    public int energy;
    public Image energyBar;
    public float energyRechargeSpeed;
    private float fillAmt;
    public int atkLVL;
    public int atkEXP;
    public int defLVL;
    public int defEXP;
    public int spdLVL;
    public int spdEXP;
    public int flyLVL;
    public int flyEXP;
    public int levelCap = 50;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine(renewEnergy());
        //TODO: on Start load energy value from save data
        //if no save data, then
        NewGame();
    }
    void NewGame()
    {
        energy = 100;
        atkEXP = 0;
        defEXP = 0;
        spdEXP = 0;
        flyEXP = 0;
        atkLVL = 1;
        defLVL = 1;
        spdLVL = 1;
        flyLVL = 1;
    }

    // Update is called once per frame
    void Update()
    {
        energy = Mathf.Clamp(energy, 0, 100);
        energyBar.fillAmount = (float)energy / 100;
    }
    IEnumerator renewEnergy()
    {
        while (energy < 100)
        {
            yield return new WaitForSeconds(1 / energyRechargeSpeed);
            energy++;
        }
    }

    public void IncreaseStat(int exp, string stat)
    {
        if (stat=="attack")
        {
            atkEXP += exp;
            if (atkEXP >= (levelCap*atkLVL))
            {
                atkLVL++;
            }
        }
        if (stat =="defense")
        {
            defEXP += exp;
            if (defEXP >= (levelCap * defLVL))
            {
                defLVL++;
            }
        }
        if (stat == "speed")
        {
            spdEXP += exp;
            if (spdEXP >= (levelCap * spdLVL))
            {
                spdLVL++;
            }
        }
        if (stat == "flight")
        {
            flyEXP += exp;
            if (flyEXP >= (levelCap * flyLVL))
            {
                flyLVL++;
            }
        }
        Debug.Log(stat + " increased by " + exp);
    }
}
