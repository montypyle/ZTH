using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatMGR : MonoBehaviour
{
    public TextMeshProUGUI attackLevel;
    public TextMeshProUGUI defenseLevel;
    public TextMeshProUGUI speedLevel;
    public TextMeshProUGUI flightLevel;
    public Slider attackExp;
    public Slider defenseExp;
    public Slider speedExp;
    public Slider flightExp;


    // Start is called before the first frame update
    void Start()
    {
        attackExp.maxValue = GM.instance.levelCap;
        defenseExp.maxValue = GM.instance.levelCap;
        speedExp.maxValue = GM.instance.levelCap;
        flightExp.maxValue = GM.instance.levelCap;

    }

    // Update is called once per frame
    void Update()
    {
        attackLevel.text = "Level " + GM.instance.atkLVL;
        defenseLevel.text = "Level " + GM.instance.defLVL;
        speedLevel.text = "Level " + GM.instance.spdLVL;
        flightLevel.text = "Level " + GM.instance.flyLVL;
        attackExp.value = GM.instance.atkEXP;
        defenseExp.value = GM.instance.defEXP;
        speedExp.value = GM.instance.spdEXP;
        flightExp.value = GM.instance.flyEXP;
    }
}
