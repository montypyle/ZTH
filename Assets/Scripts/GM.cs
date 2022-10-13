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
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine(renewEnergy());
        //TODO: on Start load energy value from save data
        //if no save data, then

        energy = 50;
    }

    // Update is called once per frame
    void Update()
    {
        energy = Mathf.Clamp(energy, 0, 100);
        fillAmt = (float)energy/100;
        energyBar.fillAmount = fillAmt;
        Debug.Log(energy);
    }
    IEnumerator renewEnergy()
    {
        while (energy < 100)
        {
            yield return new WaitForSeconds(1 / energyRechargeSpeed);
            energy++;
        }
    }
}
