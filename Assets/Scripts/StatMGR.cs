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
        
    }

    // Update is called once per frame
    void Update()
    {
        attackLevel.text = "Level " + GM.instance.atkLVL;
        defenseLevel.text = "Level " + GM.instance.defLVL;
        speedLevel.text = "Level " + GM.instance.spdLVL;
        flightLevel.text = "Level " + GM.instance.flyLVL;
        attackExp.value = GM.instance.atkEXP/GM.instance.levelCap;
        defenseExp.value = GM.instance.defEXP/GM.instance.levelCap;
        speedExp.value = GM.instance.spdEXP / GM.instance.levelCap;
        flightExp.value = GM.instance.flyEXP / GM.instance.levelCap;

    }
}
