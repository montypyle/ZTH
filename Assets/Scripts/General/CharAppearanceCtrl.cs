using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

public class CharAppearanceCtrl : MonoBehaviour
{
    public Color skinColor;
    public Color hairColor;
    public Color chestColor;
    public Color waistColor;
    public Color armColor;
    public Color legColor;
    public Color footColor;
    public Color eyeColor;
    public Color defaultColor;
    public string charName;
    private bool saving, loading;

    // Start is called before the first frame update
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

    }
    void Start()
    {
        saving = false;
        loading = false;
        if (File.Exists(Application.persistentDataPath + "/appearanceInfo.dat") && !saving)
        {
            LoadAppearance();
        }
        else
        {
            ResetAppearance();
            charName = "Hero";
            if (!loading)
            {
                SaveAppearance();
            }
            SceneManager.LoadScene("Character");
        }
    }

    // Update is called once per frame

    public void UpdateName(string name)
    {
        charName = name;
    }
    public void ResetAppearance()
    {
        charName = "Hero";
        skinColor = defaultColor;
        hairColor = defaultColor;
        chestColor = defaultColor;
        waistColor = defaultColor;
        armColor = defaultColor;
        legColor = defaultColor;
        footColor = defaultColor;
        eyeColor = defaultColor;
    }
    public void SaveAppearance()
    {
        saving = true;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(Application.persistentDataPath + "/appearanceInfo.dat"))
        {
            AppearanceData data = new AppearanceData();
            data.charName = charName;
            data.skinColor = new SerializableColor(skinColor);
            data.hairColor = new SerializableColor(hairColor);
            data.chestColor = new SerializableColor(chestColor);
            data.waistColor = new SerializableColor(waistColor);
            data.armColor = new SerializableColor(armColor);
            data.legColor = new SerializableColor(legColor);
            data.footColor = new SerializableColor(footColor);
            data.eyeColor = new SerializableColor(eyeColor);
            bf.Serialize(file, data);
            file.Close();
        }
        saving = false;
    }
    public void LoadAppearance()
    {
        loading = true;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/appearanceInfo.dat"))
        {
            using (FileStream file = File.Open(Application.persistentDataPath + "/appearanceInfo.dat", FileMode.Open))
            {
                if(file.Length > 0)
                {
                    AppearanceData data = bf.Deserialize(file) as AppearanceData;
                    charName = data.charName;
                    skinColor = data.skinColor.GetColor();
                    hairColor = data.hairColor.GetColor();
                    eyeColor = data.eyeColor.GetColor();
                    chestColor = data.chestColor.GetColor();
                    waistColor = data.waistColor.GetColor();
                    armColor = data.armColor.GetColor();
                    legColor = data.legColor.GetColor();
                    footColor = data.footColor.GetColor();
                    file.Close();
                    loading = false;
                }
            }
        }
    }
    public void DeleteAppearanceData()
    {
        if (File.Exists(Application.persistentDataPath + "/appearanceInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/appearanceInfo.dat");
            Debug.Log("Deleted Appearance data");
        }
    }
    [Serializable]

    class AppearanceData
    {
        public string charName;
        public SerializableColor hairColor;
        public SerializableColor chestColor;
        public SerializableColor waistColor;
        public SerializableColor armColor;
        public SerializableColor legColor;
        public SerializableColor footColor;
        public SerializableColor eyeColor;
        public SerializableColor defaultColor;
        public SerializableColor skinColor;
    }
    [Serializable]
    public class SerializableColor
    {
        public float _r, _g, _b, _a;

        public Color GetColor() => new Color(_r, _g, _b, _a);
        public void SetColor(Color color)
        {
            _r = color.r;
            _g = color.g;
            _b = color.b;
            _a = color.a;
        }

        public SerializableColor() { _r = _g = _b = _a = 1f; }

        public SerializableColor(Color color) : this(color.r, color.g, color.b, color.a) { }

        public SerializableColor(float r, float g, float b, float a = 0f)
        {
            _r = r;
            _g = g;
            _b = b;
            _a = a;
        }
    }
}

