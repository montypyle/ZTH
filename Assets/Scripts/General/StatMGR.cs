using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatMGR : MonoBehaviour
{
    public TextMeshProUGUI attackLevel, defenseLevel, speedLevel, flightLevel, atkBonusTxt, defBonusTxt, spdBonusTxt, flyBonusTxt;
    public Slider attackExp, defenseExp, speedExp, flightExp;
    private EquipSO headSO, capeSO, chestSO, gloveSO, bootSO;
    private int atkBuff, defBuff, spdBuff, flyBuff, headATK, chestATK, capeATK, gloveATK, bootATK, headDEF, capeDEF, chestDEF, gloveDEF, bootDEF;
    private int headSPD, capeSPD, chestSPD, gloveSPD, bootSPD, headFLY, capeFLY, chestFLY, gloveFLY, bootFLY;

    void Start()
    {
        attackExp.maxValue = GM.instance.atkCap;
        defenseExp.maxValue = GM.instance.defCap;
        speedExp.maxValue = GM.instance.spdCap;
        flightExp.maxValue = GM.instance.flyCap;
        if(InvMGR.instance.currentHead != null)
        {
            ItemCtrl headScript = InvMGR.instance.currentHead.GetComponent<ItemCtrl>();
            headSO = headScript.scriptableObj;
            headATK = headSO.ATKMod;
            headDEF = headSO.DEFMod;
            headSPD = headSO.SPDMod;
            headFLY = headSO.FLYMod;
        }
        else
        {
            headATK = 0;
            headDEF = 0;
            headSPD = 0;
            headFLY = 0;
        }
        if (InvMGR.instance.currentCape != null)
        {
            ItemCtrl capeScript = InvMGR.instance.currentCape.GetComponent<ItemCtrl>();
            capeSO = capeScript.scriptableObj;
            capeATK = capeSO.ATKMod;
            capeDEF = capeSO.DEFMod;
            capeSPD = capeSO.SPDMod;
            capeFLY = capeSO.FLYMod;
        }
        else
        {
            capeATK = 0;
            capeDEF = 0;
            capeSPD = 0;
            capeFLY = 0;
        }
        if (InvMGR.instance.currentChest != null)
        {
            ItemCtrl chestScript = InvMGR.instance.currentChest.GetComponent<ItemCtrl>();
            chestSO = chestScript.scriptableObj;
            chestATK = chestSO.ATKMod;
            chestDEF = chestSO.DEFMod;
            chestSPD = chestSO.SPDMod;
            chestFLY = chestSO.FLYMod;
        }
        else
        {
            chestATK = 0;
            chestDEF = 0;
            chestSPD = 0;
            chestFLY = 0;
        }
        if (InvMGR.instance.currentGlove != null)
        {
            ItemCtrl gloveScript = InvMGR.instance.currentGlove.GetComponent<ItemCtrl>();
            gloveSO = gloveScript.scriptableObj;
            gloveATK = gloveSO.ATKMod;
            gloveDEF = gloveSO.DEFMod;
            gloveSPD = gloveSO.SPDMod;
            gloveFLY = gloveSO.FLYMod;
        }
        else
        {
            gloveATK = 0;
            gloveDEF = 0;
            gloveSPD = 0;
            gloveFLY = 0;
        }
        if (InvMGR.instance.currentBoot != null)
        {
            ItemCtrl bootScript = InvMGR.instance.currentBoot.GetComponent<ItemCtrl>();
            bootSO = bootScript.scriptableObj;
            bootATK = bootSO.ATKMod;
            bootDEF = bootSO.DEFMod;
            bootSPD = bootSO.SPDMod;
            bootFLY = bootSO.FLYMod;
        }
        else
        {
            bootATK = 0;
            bootDEF = 0;
            bootSPD = 0;
            bootFLY = 0;
        }
        CalculateBuffs();
    }
    public void CalculateBuffs()
    {
        atkBuff = headATK + capeATK + chestATK + gloveATK + bootATK;
        defBuff = headDEF + capeDEF + chestDEF + gloveDEF + bootDEF;
        spdBuff = headSPD + capeSPD + chestSPD + gloveSPD + bootSPD;
        flyBuff = headFLY + capeFLY + chestFLY + gloveFLY + bootFLY;
    }
    void Update()
    {
        attackLevel.text = "Level " + GM.instance.atkLVL;
        defenseLevel.text = "Level " + GM.instance.defLVL;
        speedLevel.text = "Level " + GM.instance.spdLVL;
        flightLevel.text = "Level " + GM.instance.flyLVL;
        atkBonusTxt.text = "Equipment bonus: " + atkBuff;
        defBonusTxt.text = "Equipment bonus: " + defBuff;
        spdBonusTxt.text = "Equipment bonus: " + spdBuff;
        flyBonusTxt.text = "Equipment bonus: " + flyBuff;
        attackExp.value = GM.instance.atkEXP;
        defenseExp.value = GM.instance.defEXP;
        speedExp.value = GM.instance.spdEXP;
        flightExp.value = GM.instance.flyEXP;
    }
}
