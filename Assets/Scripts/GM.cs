using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;


public class GM : MonoBehaviour
{
    public bool paused;
    public static GM instance;
    public int money;
    public int energy;
    public Image energyBar;
    public float energyRechargeSpeed;
    public int atkLVL;
    public int atkEXP;
    public int defLVL;
    public int defEXP;
    public int spdLVL;
    public int spdEXP;
    public int flyLVL;
    public int flyEXP;
    public int levelCap = 50;
    private bool recharging;

    private float healthFill;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            Load();
        }
        else
        {
            NewGame();
        }
    }
    void NewGame()
    {
        paused = false;
        energy = 100;
        money = 50;
        atkEXP = 0;
        defEXP = 0;
        spdEXP = 0;
        flyEXP = 0;
        atkLVL = 1;
        defLVL = 1;
        spdLVL = 1;
        flyLVL = 1;

    }
    private void Start()
    {
        StartCoroutine(RenewEnergy());
    }
    public void Recharge()
    {

        StartCoroutine(RenewEnergy());

    }

    // Update is called once per frame
    void Update()
    {
        if (energy < 100 && !recharging)
        {
            StartCoroutine(RenewEnergy());
        }
        energy = Mathf.Clamp(energy, 0, 100);
        healthFill = (float)energy / 100;
        energyBar.fillAmount = healthFill;
    }
    IEnumerator RenewEnergy()
    {
        while (energy < 100 && !paused)
        {
            recharging = true;
            yield return new WaitForSeconds(1 / energyRechargeSpeed);
            energy++;
        }
    }

    public void IncreaseStat(int exp, string stat)
    {

        if (stat == "attack")
        {
            atkEXP += exp;
            if (atkEXP >= (levelCap * atkLVL))
            {
                atkLVL++;
            }
        }
        if (stat == "defense")
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
        Save();
    }
    public void PauseGame(bool isPaused)
    {
        if (isPaused)
        {
            paused = true;
            recharging = false;
            Save();
        }
        else if (!isPaused)
        {
            paused = false;
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 100, 30), "Money: " + money);
        GUI.Label(new Rect(500, 40, 300, 60), "Attack LVL: " + atkLVL + "\nAttack EXP: " + atkEXP );
        GUI.Label(new Rect(500, 100, 300, 60), "Defense LVL: " + defLVL + "\nDefense EXP: " + defEXP);

    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
        PlayerData data = new PlayerData();
        data.energy = energy;
        data.money = money;
        data.levelCap = levelCap;
        data.atkEXP = atkEXP;
        data.atkLVL = atkLVL;
        data.defEXP = defEXP;
        data.defLVL = defLVL;
        data.spdEXP = spdEXP;
        data.spdLVL = spdLVL;
        data.flyEXP = flyEXP;
        data.flyLVL = flyLVL;
        bf.Serialize(file, data);
        file.Close();
    }
    public void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            energy = data.energy;
            money = data.money;
            levelCap = data.levelCap;
            atkEXP = data.atkEXP;
            atkLVL = data.atkLVL;
            defEXP = data.defEXP;
            defLVL = data.defLVL;
            spdEXP = data.spdEXP;
            spdLVL = data.spdLVL;
            flyEXP = data.flyEXP;
            flyLVL = data.flyLVL;
        }
    }
    [Serializable]
    class PlayerData
    {
        public int energy;
        public int money;
        public int atkLVL;
        public int atkEXP;
        public int defLVL;
        public int defEXP;
        public int spdLVL;
        public int spdEXP;
        public int flyLVL;
        public int flyEXP;
        public int levelCap;
    }
}
