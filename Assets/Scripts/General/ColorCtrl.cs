using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCtrl : MonoBehaviour
{
    private Color color;
    private SpriteRenderer sr;
    private Image img;
    private bool isButton;
    public CharAppearanceCtrl appCtrl;
    // Start is called before the first frame update

    void Start()
    {
        appCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<CharAppearanceCtrl>();
        if(this.gameObject.CompareTag("UIButton"))
        {
            isButton = true;
        }
        else
        {
            isButton = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isButton)
        {

            sr = this.GetComponent<SpriteRenderer>();

            if (this.gameObject.tag == "Skin")
            {
                color = appCtrl.skinColor;
            }
            if (this.gameObject.tag == "LegBase")
            {
                color = appCtrl.legColor;
            }
            if (this.gameObject.tag == "ArmBase")
            {
                color = appCtrl.armColor;
            }
            if (this.gameObject.tag == "WaistBase")
            {
                color = appCtrl.waistColor;
            }
            if (this.gameObject.tag == "ChestBase")
            {
                color = appCtrl.chestColor;
            }
            if (this.gameObject.tag == "FootBase")
            {
                color = appCtrl.footColor;
            }
            if (this.gameObject.tag == "Hair")
            {
                color = appCtrl.hairColor;
            }
            if (this.gameObject.tag == "Eye")
            {
                color = appCtrl.eyeColor;
            }
            sr.color = color;
        }
    }
    public void OnClick()
    {
        img = this.GetComponent<Image>();
        color = img.color;
        if (this.gameObject.name == "Skin")
        {
            appCtrl.skinColor = color;
        }
        if (this.gameObject.name == "Hair")
        {
            appCtrl.hairColor = color;
        }
        if (this.gameObject.name == "Eye")
        {
            appCtrl.eyeColor = color;
        }
    }
}
