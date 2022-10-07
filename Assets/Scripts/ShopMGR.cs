using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMGR : MonoBehaviour
{
    public static ShopMGR instance;
    public TextMeshProUGUI currentCash;
    public int cash;
    public GameObject equipNow;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        cash = 50;
    }

    // Update is called once per frame
    void Update()
    {
        currentCash.text = "€" + cash;
    }
    private bool CheckPrice(GameObject toBuy)
    {
        toBuy.TryGetComponent<ShopItem>(out ShopItem script);
        if (script.price <= cash)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Buy(GameObject buy)
    {
        if (CheckPrice(buy))
        {
            buy.TryGetComponent<ShopItem>(out ShopItem script);
            cash = cash - script.price;

        }
    }
}
