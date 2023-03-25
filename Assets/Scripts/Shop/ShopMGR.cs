using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine.UI;
using System;

public class ShopMGR : MonoBehaviour
{
    public static ShopMGR instance;
    public static GameObject selected;
    public Transform spawn;
    public TextMeshProUGUI title, description, timer;
    public GameObject equipNow, infobox, slot1, slot2, slot3;
    public List<GameObject> onDisplay;
    private GameObject item1, item2, item3;
    public Time refreshTime, currentTime;
    public Button buy;
    private float timeRemaining;
    public float interval, hour = 0, minute = 0, second = 0;
    public bool timerIsRunning = false;

    private void Awake()
    {
        instance = this;
        timerIsRunning = true;
        interval = hour * 3600 + minute * 60 + second;
        timeRemaining = interval;
        RefreshShop();
    }
    void Start()
    {
        GM.instance.SetSpawn(spawn);
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float hours = Mathf.FloorToInt(timeToDisplay / 3600);
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timer.text = TimeSpan.FromSeconds(timeToDisplay).ToString(@"h\:mm\:ss"); ;
    }


    public void RefreshShop()
    {
        foreach (GameObject item in InvMGR.instance.allItems)
        {
            ItemCtrl script = item.GetComponent<ItemCtrl>();
            if(!InvMGR.instance.ownedItems.Contains(item) && !script.scriptableObj.locked && !onDisplay.Contains(item))
            {
                onDisplay.Add(item);
            }
        }
        int i = UnityEngine.Random.Range(0, onDisplay.Count);
        item1 = onDisplay[i];
        onDisplay.Remove(item1);
        i = UnityEngine.Random.Range(0, onDisplay.Count);
        item2 = onDisplay[i];
        onDisplay.Remove(item2);
        i = UnityEngine.Random.Range(0, onDisplay.Count);
        item3 = onDisplay[i];
        onDisplay.Remove(item3);
        ClearStock();
    }
    public void ClearStock()
    {
        foreach (Transform child in slot1.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in slot2.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in slot3.transform)
        {
            Destroy(child.gameObject);
        }
        StockInventory();
    }
    public void StockInventory()
    {

        Instantiate(item1, slot1.transform);
        Instantiate(item2, slot2.transform);
        Instantiate(item3, slot3.transform);
        onDisplay.Add(item1);
        onDisplay.Add(item2);
        onDisplay.Add(item3);

    }
    // Update is called once per frame
    void Update()
    {

            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            }
            else
            {
                Debug.Log("Time has run out!");
                RefreshShop();
                timeRemaining = interval;
            }

    }
    public string IdentifySelected()
    {
        if (selected == null)
        {
            return null;
        }
        else
        {
            return selected.name;
        }
    }
    private bool CheckPrice(GameObject toBuy)
    {
        toBuy.TryGetComponent<ItemCtrl>(out ItemCtrl script);
        if (script.scriptableObj.price <= GM.instance.money)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SelectItem(GameObject item)
    {
        selected = item;
        ItemCtrl script = item.GetComponent<ItemCtrl>();
        infobox.SetActive(true);
        description.text = script.scriptableObj.description;
        title.text = script.scriptableObj.itemName;
        if (CheckPrice(selected))
        {
            buy.enabled = true;
        }
        else
        {
            buy.enabled = false;
        }
    }
    public void BuySelected()
    {
        Buy(selected);
    }
    public void Buy(GameObject buy)
    {
        if (CheckPrice(buy))
        {
            buy.TryGetComponent<ItemCtrl>(out ItemCtrl script);
            GM.instance.money = GM.instance.money - script.scriptableObj.price;
            script.scriptableObj.obtained = true;
            equipNow.SetActive(true);
            Dictionary<string, object> parameters = new Dictionary<string, object> { { "itemName", this.gameObject.name } };
            AnalyticsService.Instance.CustomData("itemBought", parameters);
            InvMGR.instance.UpdateInventory();

        }
    }
}
