using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Analytics;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine.SceneManagement;




public class GM : MonoBehaviour
{
    public static GM instance;

    [Header("Character Settings")]
    public GameObject hero, shieldObj;
    public Image healthbar;
    public Shield shieldscript;
    public CharCtrl charScript;
    public CharAppearanceCtrl appCtrl;
    public SpriteRenderer headSlot, capeSlot, chestSlot, gloveSlot_l, gloveSlot_r, bootSlot_l, bootSlot_r;
    public Camera charCam;

    private bool saving, loading;
    [Header("Status Settings")]
    public TextMeshProUGUI moneyTxt;
    public int money, energy;
    public Image energyBar;
    public float energyRechargeSpeed;
    private bool recharging;
    public Color full, mid, danger;
    public GameObject status, warning;

    [Header("Training Settings")]
    public float minTime;
    public float maxTime;
    public int atkCap, defCap, spdCap, flyCap;
    public int atkHI, defHI, spdHI, flyHI;
    public int atkLVL, atkModded, atkEXP, defLVL, defModded, defEXP, spdLVL, spdModded, spdEXP, flyLVL, flyModded, flyEXP;

    [Header("Mission Settings")]
    public int missionCost, m1HI, m2HI;
    public float speed, power, damage, shield;
    public bool isMission, isTraining, paused, gameplay;

    void Awake()
    {
        AnalyticsService.Instance.SetAnalyticsEnabled(true);
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


    }
    public void NewGame()
    {
        File.Delete(Application.persistentDataPath + "/playerInfo.dat");

        paused = false;
        energy = 100;
        money = 100;
        atkEXP = 0;
        defEXP = 0;
        spdEXP = 0;
        flyEXP = 0;
        atkLVL = 1;
        defLVL = 1;
        spdLVL = 1;
        flyLVL = 1;

    }
    public void QuitGame()
    {
        AnalyticsService.Instance.Flush();
        if (!loading)
        {
            Save();
        }
        Application.Quit();
    }
    private void Start()
    {
        hero = GameObject.FindGameObjectWithTag("Player");
        charScript = hero.GetComponent<CharCtrl>();
        appCtrl = hero.GetComponent<CharAppearanceCtrl>();
        saving = false;
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat") && !saving)
        {
            Load();
        }
        else
        {
            NewGame();
        }
        StartCoroutine(RenewEnergy());
    }
    public void Recharge()
    {

        StartCoroutine(RenewEnergy());

    }
    void Update()
    {
        if (energy < 100 && !recharging)
        {
            StartCoroutine(RenewEnergy());
        }
        energy = Mathf.Clamp(energy, 0, 100);
        float healthFill = (float)energy / 100;
        energyBar.fillAmount = healthFill;
        moneyTxt.text = money.ToString();
        EnergyColor();

        if (isTraining || isMission)
        {
            gameplay = true;
        }

        else
        {
            gameplay = false;
        }
        if (gameplay)
        {
            status.SetActive(false);
            recharging = false;
        }
        else if (!gameplay)
        {
            status.SetActive(true);
        }
    }
    void EnergyColor()
    {
        if (energyBar.fillAmount <= 1 && energyBar.fillAmount > 0.75f)
        {
            energyBar.color = full;

        }
        else if (energyBar.fillAmount <= 0.75f && energyBar.fillAmount > 0.35f)
        {
            energyBar.color = mid;
        }
        else if (energyBar.fillAmount <= 0.35f)
        {
            energyBar.color = danger;
        }
    }
    public void ResetEnergy()
    {
        energy = 100;
    }
    IEnumerator RenewEnergy()
    {
        while (energy < 100 && !paused && energyBar.isActiveAndEnabled)
        {
            recharging = true;
            yield return new WaitForSeconds(1 / energyRechargeSpeed);
            energy++;
        }
    }
    public void EnergyWarning()
    {
        warning.SetActive(true);
        Timer timer = warning.GetComponent<Timer>();
        timer.timerIsRunning = true;
    }
    public void IncreaseStat(int exp, string stat)
    {

        if (stat == "attack")
        {
            atkEXP += exp;
            if (atkEXP >= (atkCap))
            {
                atkLVL++;
                atkEXP = 0;
                atkCap += atkCap / 4;
            }
        }
        if (stat == "defense")
        {
            defEXP += exp;
            if (defEXP >= (defCap))
            {
                defLVL++;
                defEXP = 0;
                defCap += defCap / 4;
            }
        }
        if (stat == "speed")
        {
            spdEXP += exp;
            if (spdEXP >= (spdCap))
            {
                spdLVL++;
                spdEXP = 0;
                spdCap += spdCap / 4;
            }
        }
        if (stat == "flight")
        {
            flyEXP += exp;
            if (flyEXP >= (flyCap))
            {
                flyLVL++;
                flyEXP = 0;
                flyCap += flyCap / 4;
            }
        }
        Debug.Log(stat + " increased by " + exp);
        if (!loading)
        {
            Save();
        }
    }
    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            paused = true;
            recharging = false;
            if(!loading)
            {
                Save();
            }
        }
        else if (!isPaused)
        {
            paused = false;
        }
    }
    public void Save()
    {
        saving = true;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat"))
        {
            PlayerData data = new PlayerData();
            data.energy = energy;
            data.money = money;
            data.atkCap = atkCap;
            data.defCap = defCap;
            data.spdCap = spdCap;
            data.flyCap = flyCap;
            data.atkHI = atkHI;
            data.atkEXP = atkEXP;
            data.atkLVL = atkLVL;
            data.defHI = defHI;
            data.defEXP = defEXP;
            data.defLVL = defLVL;
            data.spdHI = spdHI;
            data.spdEXP = spdEXP;
            data.spdLVL = spdLVL;
            data.flyHI = flyHI;
            data.flyEXP = flyEXP;
            data.flyLVL = flyLVL;
            data.m1HI = m1HI;
            data.m2HI = m2HI;
            bf.Serialize(file, data);
            file.Close();
        }
        saving = false;
    }
    public void InputMovement(Vector2 input)
    {
        charScript.InputMovement(input);
    }
    public void InputShieldMovement(Vector2 input)
    {
        shieldscript.Move(input);
    }
    public void Load()
    {
        loading = true;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
            {
            using (FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open))
            {
                if (file.Length > 0)
                {
                    PlayerData data = bf.Deserialize(file) as PlayerData;
                    file.Close();
                    energy = data.energy;
                    money = data.money;
                    atkEXP = data.atkEXP;
                    atkLVL = data.atkLVL;
                    defEXP = data.defEXP;
                    defLVL = data.defLVL;
                    spdEXP = data.spdEXP;
                    spdLVL = data.spdLVL;
                    flyEXP = data.flyEXP;
                    flyLVL = data.flyLVL;
                }
                loading = false;
            }
        }
    }
    public void DeleteSave()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/playerInfo.dat");
            appCtrl.DeleteAppearanceData();
            InvMGR.instance.DeleteInventory();
            
            Debug.Log("Deleted save");
        }
    }
    public void BalanceGame()
    {
        float spdT = (spdLVL + spdModded) / 10;
        speed = Mathf.Lerp(5, 20, spdT);
        float defT = (defLVL + defModded) / 10;
        shield = Mathf.Lerp(2, 8, defT);
        float flyT = (flyLVL + flyModded) / 10;
        damage = Mathf.Lerp(4, .2f, flyT);
        float atkT = (atkLVL + atkModded) / 10;
        power = Mathf.Lerp(1, 5, atkT);
        Debug.Log("Balanced speed = " + speed + " shield = " + shield + " damage = " + damage + " power = " + power);
    }
    public void Equip(GameObject item)
    {

        ItemCtrl script = item.GetComponent<ItemCtrl>();
        EquipSO SO = script.scriptableObj;
        if (!SO.equipped)
        {
            atkModded += SO.ATKMod;
            defModded += SO.DEFMod;
            spdModded += SO.SPDMod;
            flyModded += SO.FLYMod;
            if (SO.slot == "Head")
            {
                headSlot.sprite = SO.sprite;
            }
            else if (SO.slot == "Cape")
            {
                capeSlot.sprite = SO.sprite;
            }
            else if (SO.slot == "Chest")
            {
                chestSlot.sprite = SO.sprite;
            }
            else if (SO.slot == "Gloves")
            {
                gloveSlot_l.sprite = SO.sprite;
                gloveSlot_r.sprite = SO.sprite;
            }
            else if (SO.slot == "Boots")
            {
                bootSlot_l.sprite = SO.sprite;
                bootSlot_r.sprite = SO.sprite;
                SpriteCuller culler = bootSlot_l.gameObject.GetComponent<SpriteCuller>();
                culler.hiding = true;
                SpriteCuller culler2 = bootSlot_r.gameObject.GetComponent<SpriteCuller>();
                culler2.hiding = true;
            }
            SO.equipped = true;

        }
    }
    public void UnEquip(GameObject item)
    {

        ItemCtrl script = item.GetComponent<ItemCtrl>();
        EquipSO SO = script.scriptableObj;
        if (SO.equipped)
        {
            atkModded -= SO.ATKMod;
            defModded -= SO.DEFMod;
            spdModded -= SO.SPDMod;
            flyModded -= SO.FLYMod;
            if (SO.slot == "Head")
            {
                InvMGR.instance.currentHead = null;
                InvMGR.instance.equippedHead.Clear();
                headSlot.sprite = null;
            }
            else if (SO.slot == "Cape")
            {
                InvMGR.instance.currentCape = null;
                InvMGR.instance.equippedCape.Clear();
                capeSlot.sprite = null;
            }
            else if (SO.slot == "Chest")
            {
                InvMGR.instance.currentChest = null;
                InvMGR.instance.equippedChest.Clear();
                chestSlot.sprite = null;
            }
            else if (SO.slot == "Gloves")
            {
                InvMGR.instance.currentGlove = null;
                InvMGR.instance.equippedGlove.Clear();
                gloveSlot_l.sprite = null;
                gloveSlot_r.sprite = null;
            }
            else if (SO.slot == "Boots")
            {
                InvMGR.instance.currentBoot = null;
                InvMGR.instance.equippedBoot.Clear();
                bootSlot_l.sprite = null;
                bootSlot_r.sprite = null;
                SpriteCuller culler = bootSlot_l.gameObject.GetComponent<SpriteCuller>();
                culler.hiding = false;
                SpriteCuller culler2 = bootSlot_r.gameObject.GetComponent<SpriteCuller>();
                culler2.hiding = false;
            }
            SO.equipped = false;

        }
    }
    public void Reward(int reward)
    {
        money += reward;
        Debug.Log(reward + " + " + (money - reward) + " = " + money);
    }

    public void SetSpawn(Transform spawn)
    {
        hero.transform.position = spawn.position;
        hero.transform.localScale = spawn.localScale;

    }
    
    [Serializable]
    class PlayerData
    {
        public int energy;
        public int money;
        public int atkLVL;
        public int atkHI;
        public int atkEXP;
        public int defLVL;
        public int defHI;
        public int defEXP;
        public int spdLVL;
        public int spdHI;
        public int spdEXP;
        public int flyLVL;
        public int flyHI;
        public int flyEXP;
        public int atkCap;
        public int defCap;
        public int spdCap;
        public int flyCap;
        public int m1HI;
        public int m2HI;
    }
}
