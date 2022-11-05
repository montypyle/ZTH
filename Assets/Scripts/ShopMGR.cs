using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopMGR : MonoBehaviour
{
    public static ShopMGR instance;
    public TextMeshProUGUI currentCash;
    public GameObject equipNow;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        currentCash.text = "€" + GM.instance.money;
    }
    private bool CheckPrice(GameObject toBuy)
    {
        toBuy.TryGetComponent<ShopItem>(out ShopItem script);
        if (script.price <= GM.instance.money)
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
            GM.instance.money = GM.instance.money - script.price;

        }
    }
}
