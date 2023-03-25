using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class EquipMGR : MonoBehaviour
{
    public static EquipMGR instance;
    public static GameObject selected;
    public Transform spawn;
    public GameObject headHighlight, capeHighlight, chestHighlight, gloveHighlight, bootHighlight;
    public TextMeshProUGUI oldStats, newStats, category, title, description;
    public UnityEngine.UI.Button equip, unequip;
    public GameObject newPanel, slot1, slot2, slot3, infobox;
    public bool headSelected, capeSelected, chestSelected, gloveSelected, bootSelected;
    public List<GameObject> headItems, capeItems, chestItems, gloveItems, bootItems;
    public GameObject[] toDisplay;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GM.instance.SetSpawn(spawn);
        infobox.SetActive(false);
        headHighlight.SetActive(false);
        capeHighlight.SetActive(false);
        chestHighlight.SetActive(false);
        gloveHighlight.SetActive(false);
        bootHighlight.SetActive(false);
        headSelected = true;
        oldStats.text = "attack \t" + (GM.instance.atkLVL + GM.instance.atkModded)
            + "\n defense \t" + (GM.instance.defLVL + GM.instance.defModded) +
            "\n speed \t" + (GM.instance.spdLVL + GM.instance.spdModded) +
            "\n flight \t" + (GM.instance.flyLVL + GM.instance.flyModded);
        UpdateDisplay();
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
    private void Update()
    {
    }
    public void ShowStats()
    {
        newPanel.SetActive(true);
        newStats.text = "attack \t" + (GM.instance.atkLVL + GM.instance.atkModded)
            + "\n defense \t" + (GM.instance.defLVL + GM.instance.defModded) + 
            "\n speed \t" + (GM.instance.spdLVL + GM.instance.spdModded) + 
            "\n flight \t" + (GM.instance.flyLVL + GM.instance.flyModded);

    }
    public void UpdateDisplay()
    {
        InvMGR.instance.UpdateInventory();
        toDisplay = InvMGR.instance.ownedItems.ToArray();
        foreach (GameObject item in toDisplay)
        {
            ItemCtrl script = item.GetComponent<ItemCtrl>();
            EquipSO SO = script.scriptableObj;
            if (SO.slot == "Head")
            {
                headItems.Add(item);
            }
            else if (SO.slot == "Cape")
            {
                capeItems.Add(item);
            }
            else if (SO.slot == "Chest")
            {
                chestItems.Add(item);
            }
            else if (SO.slot == "Gloves")
            {
                gloveItems.Add(item);
            }
            else if (SO.slot == "Boots")
            {
                bootItems.Add(item);
            }
        }
    }
    public void SelectItem(GameObject item)
    {
        selected = item;
        ItemCtrl script = item.GetComponent<ItemCtrl>();
        infobox.SetActive(true);
        description.text = script.scriptableObj.description;
        title.text = script.scriptableObj.itemName;
        if (!script.scriptableObj.equipped)
        {
            equip.interactable = true;
            unequip.interactable = false;
        }
        else
        {
            unequip.interactable = true;
            equip.interactable = false;
        }

    }
    public void EquipSelected()
    {
        GM.instance.Equip(selected);
        ShowStats();
        InvMGR.instance.UpdateInventory();
    }
    public void UnequipSelected()
    {
        GM.instance.UnEquip(selected);
        ShowStats();
        InvMGR.instance.UpdateInventory();
    }
    private void Clear()
    {
        selected = null;
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
    }
    public void SelectHead()
    {
        Clear();
        headSelected = true;
        category.text = "headgear";
        capeSelected = false;
        chestSelected = false;
        gloveSelected = false;
        bootSelected = false;
        headHighlight.SetActive(true);
        capeHighlight.SetActive(false);
        chestHighlight.SetActive(false);
        gloveHighlight.SetActive(false);
        bootHighlight.SetActive(false);
        if (headItems.Count > 0)
        {
            GameObject item1 = Instantiate(headItems[0], slot1.transform);
        }
        else if (headItems.Count > 1)
        {
            GameObject item2 = Instantiate(headItems[1], slot2.transform);
        }
        else if (headItems.Count > 2)
        {
            GameObject item3 = Instantiate(headItems[2], slot3.transform);
        }
    }
    public void SelectCape()
    {
        Clear();
        headSelected = false;
        capeSelected = true;
        category.text = "capes";
        chestSelected = false;
        gloveSelected = false;
        bootSelected = false;
        headHighlight.SetActive(false);
        capeHighlight.SetActive(true);
        chestHighlight.SetActive(false);
        gloveHighlight.SetActive(false);
        bootHighlight.SetActive(false);
        if (capeItems.Count > 0)
        {
            GameObject item1 = Instantiate(capeItems[0], slot1.transform);
        }
        else if (capeItems.Count > 1)
        {
            GameObject item2 = Instantiate(capeItems[1], slot2.transform);
        }
        else if (capeItems.Count > 2)
        {
            GameObject item3 = Instantiate(capeItems[2], slot3.transform);
        }
    }
    public void SelectChest()
    {
        Clear();
        headSelected = false;
        capeSelected = false;
        chestSelected = true;
        category.text = "armour";
        gloveSelected = false;
        bootSelected = false;
        headHighlight.SetActive(false);
        capeHighlight.SetActive(false);
        chestHighlight.SetActive(true);
        gloveHighlight.SetActive(false);
        bootHighlight.SetActive(false);
        if (chestItems.Count > 0)
        {
            GameObject item1 = Instantiate(chestItems[0], slot1.transform);
        }
        else if (chestItems.Count > 1)
        {
            GameObject item2 = Instantiate(chestItems[1], slot2.transform);
        }
        else if (chestItems.Count > 2)
        {
            GameObject item3 = Instantiate(chestItems[2], slot3.transform);
        }
    }
    public void SelectGloves()
    {
        Clear();
        headSelected = false;
        capeSelected = false;
        chestSelected = false;
        gloveSelected = true;
        category.text = "gloves";
        bootSelected = false;
        headHighlight.SetActive(false);
        capeHighlight.SetActive(false);
        chestHighlight.SetActive(false);
        gloveHighlight.SetActive(true);
        bootHighlight.SetActive(false);
        if (gloveItems.Count > 0)
        {
            GameObject item1 = Instantiate(gloveItems[0], slot1.transform);
        }
        else if (gloveItems.Count > 1)
        {
            GameObject item2 = Instantiate(gloveItems[1], slot2.transform);
        }
        else if (gloveItems.Count > 2)
        {
            GameObject item3 = Instantiate(gloveItems[2], slot3.transform);
        }
    }
    public void SelectBoots()
    {
        Clear();
        headSelected = false;
        capeSelected = false;
        chestSelected = false;
        gloveSelected = false;
        bootSelected = true;
        headHighlight.SetActive(false);
        capeHighlight.SetActive(false);
        chestHighlight.SetActive(false);
        gloveHighlight.SetActive(false);
        bootHighlight.SetActive(true);
        category.text = "boots";
        if (bootItems.Count > 0)
        {
            GameObject item1 = Instantiate(bootItems[0], slot1.transform);
        }
        else if (bootItems.Count > 1)
        {
            GameObject item2 = Instantiate(bootItems[1], slot2.transform);
        }
        else if (bootItems.Count > 2)
        {
            GameObject item3 = Instantiate(bootItems[2], slot3.transform);
        }

    }
    public void CycleForward()
    {
        if (headSelected)
        {
            SelectCape();
        }
        else if (capeSelected)
        {
            SelectChest();
        }
        else if (chestSelected)
        {
            SelectGloves();
        }
        else if (gloveSelected)
        {
            SelectBoots();
        }
        else if (bootSelected)
        {
            SelectHead();
        }
    }
    public void CycleBack()
    {
        if (headSelected)
        {
            SelectBoots();
        }
        else if (bootSelected)
        {
            SelectGloves();
        }
        else if (gloveSelected)
        {
            SelectChest();
        }
        else if (chestSelected)
        {
            SelectCape();
        }
        else if (capeSelected)
        {
            SelectHead();
        }
    }
}
