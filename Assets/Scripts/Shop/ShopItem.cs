using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;

public class ShopItem : MonoBehaviour
{
    public int price;
    public TextMeshProUGUI priceTxt;
    public Button buyButton;
    private bool bought = false;
    public bool locked;

    // Start is called before the first frame update
    void Start()
    {
        priceTxt.text = "€" + price;
    }

    // Update is called once per frame
    void Update()
    {
        if (price > GM.instance.money || bought || locked)
        {
            buyButton.interactable = false;
        }
        else if (!bought && !locked)
        {
            buyButton.interactable = true;
        }
    }
    public void BuyItem()
    {
        bought = true;
        ShopMGR.instance.equipNow.SetActive(true);
        AnalyticsService.Instance.CustomData("itemBought", new Dictionary<string, object>{
                    { "itemName", this.gameObject.name } });
        //add item to inventory
    }
}
