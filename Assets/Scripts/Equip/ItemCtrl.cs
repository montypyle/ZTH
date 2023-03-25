using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Analytics;


public class ItemCtrl : MonoBehaviour
{
    public EquipSO scriptableObj;
    public TextMeshProUGUI priceTxt;
    public GameObject highlight;
    public Button button;
    public string selected;
    public bool locked;
    private Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = this.GetComponent<Image>();
        highlight.SetActive(false);
        priceTxt.gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        img.sprite = scriptableObj.sprite;

        if (SceneManager.GetActiveScene().name == "Shop")
        {
            priceTxt.gameObject.SetActive(true);
            priceTxt.text = "€" + scriptableObj.price;
            selected = ShopMGR.instance.IdentifySelected();
 
        }
        else if (SceneManager.GetActiveScene().name == "Equip")
        {
            priceTxt.gameObject.SetActive(false);
            selected = EquipMGR.instance.IdentifySelected();
        }
        else
        {
            priceTxt.gameObject.SetActive(false);
        }
        if (CheckIfSelected(selected))
        {
            highlight.SetActive(true);
        }
        else
        {
            highlight.SetActive(false);
        }

    }
    public bool CheckIfSelected(string selected)
    {
        if(selected == null)
        {
            return false;
        }
        else if (!selected.Equals(this.gameObject.name))
        {
            return false;
        }
        else if (selected.Equals(this.gameObject.name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ContextWrapper()
    {
        if (SceneManager.GetActiveScene().name == "Shop")
        {
            ShopMGR.instance.SelectItem(this.gameObject);
            highlight.SetActive(true);
        }
        if (SceneManager.GetActiveScene().name == "Equip")
        {
            EquipMGR.instance.SelectItem(this.gameObject);
            highlight.SetActive(true);
        }
    }
    public void Purchase()
    {
        ShopMGR.instance.Buy(this.gameObject);
        scriptableObj.obtained = true;
        ShopMGR.instance.equipNow.SetActive(true);
        Dictionary<string, object> parameters = new Dictionary<string, object> { { "itemName", this.gameObject.name } };
        AnalyticsService.Instance.CustomData("itemBought", parameters);
        InvMGR.instance.UpdateInventory();
    }
    public void ToggleEquip()
    {
        if(scriptableObj.equipped)
        {
            GM.instance.UnEquip(this.gameObject);
        }
        else if (!scriptableObj.equipped)
        {
            GM.instance.Equip(this.gameObject);
        }
        EquipMGR.instance.ShowStats();
        InvMGR.instance.UpdateInventory();
    }
}
