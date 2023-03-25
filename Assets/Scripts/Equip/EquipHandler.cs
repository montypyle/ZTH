using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipHandler : MonoBehaviour
{
    public string type;
    public Sprite sprite;
    public string itemName;
    public bool selected;
    public bool obtained;
    public string statBuff;
    public string statDebuff; //name of stat to be debuffed
    public int buff; //amount to add to stat
    public int debuff; //amount to subtract from stat

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        if (TryGetComponent<SpriteRenderer>(out SpriteRenderer img))
        {
            img.sprite = sprite;
        }
        itemName = gameObject.name;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private string BuffText(string statBuff, int buff)
    {
        if (statBuff == "ATK")
        {
            return "Attack +" + buff;
        }
        else if (statBuff == "DEF")
        {
            return "Defense +" + buff;
        }
        else if (statBuff == "SPD")
        {
            return "Speed +" + buff;
        }
        else if (statBuff == "FLY")
        {
            return "Flight +" + buff;
        }
        else
        {
            return "Error";
        }
    }
    private string DebuffText(string statDebuff, int debuff)
    {
        if (statDebuff == "ATK")
        {
            return "Attack -" + debuff;
        }
        else if (statDebuff == "DEF")
        {
            return "Defense -" + debuff;
        }
        else if (statDebuff == "SPD")
        {
            return "Speed -" + debuff;
        }
        else if (statDebuff == "FLY")
        {
            return "Flight -" + debuff;
        }
        else
        {
            return "Error";
        }
    }
    private string GenerateItemInfo()
    {
        string text1 = BuffText(statBuff, buff);
        string text2 = DebuffText(statDebuff, debuff);
        return text1 + " \n " + text2;
    }

    private void OnMouseDown()
    {
        selected = true;
        EquipMGR.instance.newStats.text = GenerateItemInfo();
        if (type == "head")
        {
            //character "head" slot . sprite = sprite;
            // buff and debuff added to stats new/old;
        }
        if (type == "cape")
        {
            //character "cape" slot . sprite = sprite;
        }
        if (type == "chest")
        {
            //character "torso" slot . sprite = sprite;
        }
        if (type == "gloves")
        {
            //character "glove" slot . sprite = sprite;
        }
        if (type == "boot")
        {
            //character "boot" slot . sprite = sprite;
        }
        else
        {
            Debug.Log("Unassigned Item" + gameObject.name);
        }
    }
}
