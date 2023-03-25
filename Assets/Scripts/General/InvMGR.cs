using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using UnityEngine.UI;

public class InvMGR : MonoBehaviour
{
    public static InvMGR instance;
    public GameObject[] allItems;
    public List<GameObject> ownedItems;
    public List<GameObject> equippedHead, equippedCape, equippedChest, equippedGlove, equippedBoot;
    public GameObject currentHead, currentCape, currentChest, currentGlove, currentBoot;
    public int count;
    private bool saving, loading;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        saving = false;
        loading = false;

        if(!saving)
        {
            LoadInventory();
        }
        UpdateInventory();
    }
    private void Update()
    {
        UpdateAppearance();
    }
    public void UpdateAppearance()
    {
        if(currentHead != null)
        {
            GM.instance.headSlot.sprite = currentHead.GetComponent<Image>().sprite;
        }
        if (currentCape != null)
        {
            GM.instance.capeSlot.sprite = currentCape.GetComponent<Image>().sprite;
        }
        if (currentChest != null)
        {
            GM.instance.chestSlot.sprite = currentChest.GetComponent<Image>().sprite;
        }
        if (currentGlove != null)
        {
            GM.instance.gloveSlot_l.sprite = currentGlove.GetComponent<Image>().sprite;
            GM.instance.gloveSlot_r.sprite = currentGlove.GetComponent<Image>().sprite;
        }
        if (currentBoot != null)
        {
            GM.instance.bootSlot_l.sprite = currentBoot.GetComponent<Image>().sprite;
            GM.instance.bootSlot_r.sprite = currentBoot.GetComponent<Image>().sprite;
        }
    }
    public void UpdateInventory()
    {
        foreach (GameObject item in allItems)
        {
            ItemCtrl script = item.GetComponent<ItemCtrl>();
            EquipSO SO = script.scriptableObj;
            if (SO.obtained && !SO.inInventory)
            {
                ownedItems.Add(item.gameObject);
                SO.inInventory = true;
            }
        }
        foreach (GameObject item in ownedItems)
        {
            ItemCtrl script = item.GetComponent<ItemCtrl>();

            if (script.scriptableObj.equipped && script.scriptableObj.slot == "Head")
            {
                equippedHead.Add(item);
                GM.instance.Equip(item);
            }
            else if (script.scriptableObj.equipped && script.scriptableObj.slot == "Cape")
            {
                equippedCape.Add(item);
                GM.instance.Equip(item);
            }
            else if (script.scriptableObj.equipped && script.scriptableObj.slot == "Chest")
            {
                equippedChest.Add(item);
                GM.instance.Equip(item);
            }
            else if (script.scriptableObj.equipped && script.scriptableObj.slot == "Gloves")
            {
                equippedGlove.Add(item);
                GM.instance.Equip(item);
            }
            else if (script.scriptableObj.equipped && script.scriptableObj.slot == "Boots")
            {
                equippedBoot.Add(item);
                GM.instance.Equip(item);
            }
        }
        if (equippedHead.Count == 0)
        {
            currentHead = null;
            GM.instance.headSlot.sprite = null;
        }
        else if (equippedHead.Count > 1)
        {
            int count = equippedHead.Count;
            for (int i = 1; i < count; i++)
            {
                ItemCtrl script = equippedHead[i].GetComponent<ItemCtrl>();
                GM.instance.UnEquip(equippedHead[i]);
                if (!ownedItems.Contains(equippedHead[i]))
                {
                    ownedItems.Add(equippedHead[i]);
                }
                equippedHead.RemoveAt(i);
            }
            currentHead = equippedHead[0];
        }
        else
        {
            currentHead = equippedHead[0];
            if (!ownedItems.Contains(currentHead))
            {
                ownedItems.Add(currentHead);
            }
        }
        if (equippedCape.Count == 0)
        {
            currentCape = null;
            GM.instance.capeSlot.sprite = null;
        }
        else if (equippedCape.Count > 1)
        {
            int count = equippedCape.Count;
            for (int i = 1; i < count; i++)
            {
                ItemCtrl script = equippedCape[i].GetComponent<ItemCtrl>();
                GM.instance.UnEquip(equippedCape[i]);
                if (!ownedItems.Contains(equippedCape[i]))
                {
                    ownedItems.Add(equippedCape[i]);
                }
                equippedCape.RemoveAt(i);
            }
            currentCape = equippedCape[0];
        }
        else
        {
            currentCape = equippedCape[0];
            if (!ownedItems.Contains(currentCape))
            {
                ownedItems.Add(currentCape);
            }
        }
        if (equippedChest.Count == 0)
        {
            currentChest = null;
            GM.instance.chestSlot.sprite = null;
        }
        else if (equippedChest.Count > 1)
        {
            int count = equippedChest.Count;
            for (int i = 1; i < count; i++)
            {
                ItemCtrl script = equippedChest[i].GetComponent<ItemCtrl>();
                GM.instance.UnEquip(equippedChest[i]);
                if (!ownedItems.Contains(equippedChest[i]))
                {
                    ownedItems.Add(equippedChest[i]);
                }
                equippedChest.RemoveAt(i);
            }
            currentChest = equippedChest[0];
        }
        else
        {
            currentChest = equippedChest[0];
            if (!ownedItems.Contains(currentChest))
            {
                ownedItems.Add(currentChest);
            }
        }
        if (equippedGlove.Count == 0)
        {
            currentGlove = null;
            GM.instance.gloveSlot_l.sprite = null;
            GM.instance.gloveSlot_r.sprite = null;
        }
        else if (equippedGlove.Count > 1)
        {
            int count = equippedGlove.Count;
            for (int i = 1; i < count; i++)
            {
                ItemCtrl script = equippedGlove[i].GetComponent<ItemCtrl>();
                GM.instance.UnEquip(equippedGlove[i]);
                if (!ownedItems.Contains(equippedGlove[i]))
                {
                    ownedItems.Add(equippedGlove[i]);
                }
                equippedGlove.RemoveAt(i);
            }
            currentGlove = equippedGlove[0];
        }
        else
        {
            currentGlove = equippedGlove[0];
            if (!ownedItems.Contains(currentGlove))
            {
                ownedItems.Add(currentGlove);
            }

        }
        if (equippedBoot.Count == 0)
        {
            currentBoot = null;
            GM.instance.bootSlot_l.sprite = null;
            GM.instance.bootSlot_r.sprite = null;
        }
        else if (equippedBoot.Count > 1)
        {
            int count = equippedBoot.Count;
            for (int i = 1; i < count; i++)
            {
                ItemCtrl script = equippedBoot[i].GetComponent<ItemCtrl>();
                GM.instance.UnEquip(equippedBoot[i]);
                if (!ownedItems.Contains(equippedBoot[i]))
                {
                    ownedItems.Add(equippedBoot[i]);
                }
                equippedBoot.RemoveAt(i);
            }
            currentBoot = equippedBoot[0];
        }
        else
        {
            currentBoot = equippedBoot[0];
            if(!ownedItems.Contains(currentBoot))
            {
                ownedItems.Add(currentBoot);
            }

        }
        UpdateAppearance();
        if (!loading)
        {
            SaveInventory();
        }
    }

    public void SaveInventory()
    {
        saving = true;
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream file = File.Create(Application.persistentDataPath + "/invInfo.dat"))
        {
            InventoryData data = new InventoryData();
            if (equippedHead.Count != 0)
            {
                data.equippedHead = equippedHead[0].name;
            }
            if (equippedChest.Count != 0)
            {
                data.equippedChest = equippedChest[0].name;
            }
            if (equippedCape.Count != 0)
            {
                data.equippedCape = equippedCape[0].name;
            }
            if (equippedGlove.Count != 0)
            {
                data.equippedGlove = equippedGlove[0].name;
            }
            if (equippedBoot.Count != 0)
            {
                data.equippedBoot = equippedBoot[0].name;
            }
            if (ownedItems.Count > 0)
            {
                Hashtable items = new Hashtable();
                foreach (GameObject item in ownedItems)
                {
                    EquipSO so = item.GetComponent<ItemCtrl>().scriptableObj;
                    string itemName = item.name;
                    if(!items.ContainsKey(itemName))
                    {
                        items.Add(itemName, so.equipped);
                    }
                    else
                    {
                        items[itemName] = so.equipped;
                    }
                    data.items = items;
                }
            }

            try
            {
                bf.Serialize(file, data);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                file.Close();
            }
        }
        saving = false;
    }
    GameObject GetItemFromData(GameObject[] items, string name)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].name == name)
                return items[i];
        }
        return null;
    }
    public void LoadInventory()
    {
        loading = true;
        BinaryFormatter bf = new BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/invInfo.dat"))
        {
            using (FileStream file = File.Open(Application.persistentDataPath + "/invInfo.dat", FileMode.Open))
            {
                if (file.Length > 0)
                {
                    InventoryData data = bf.Deserialize(file) as InventoryData;
                    if (data.equippedHead != null)
                    {
                        equippedHead.Add(GetItemFromData(allItems, data.equippedHead));
                    }
                    if (data.equippedChest != null)
                    {
                        equippedChest.Add(GetItemFromData(allItems, data.equippedChest));

                    }
                    if (data.equippedCape != null)
                    {
                        equippedCape.Add(GetItemFromData(allItems, data.equippedCape));

                    }
                    if (data.equippedGlove != null)
                    {
                        equippedGlove.Add(GetItemFromData(allItems, data.equippedGlove));

                    }
                    if (data.equippedBoot != null)
                    {
                        equippedBoot.Add(GetItemFromData(allItems, data.equippedBoot));

                    }
                    if (data.ownedItems != null)
                    {
                        for (int i = 0; i < data.ownedItems.Count; ++i)
                        {
                            GameObject item = GameObject.Find(ownedItems[i].name);
                            ownedItems.Add(item);
                        }
                    }
                    file.Close();

                }
            }
        }
        loading = false;
    }
    public void DeleteInventory()
    {
        if(File.Exists(Application.persistentDataPath + "/invInfo.dat"))
        {
            File.Delete(Application.persistentDataPath + "/invInfo.dat");
            Debug.Log("Deleted inventory data");
        }
        foreach (GameObject item in ownedItems)
        {
            ItemCtrl script = item.GetComponent<ItemCtrl>();
            foreach(GameObject head in equippedHead)
            {
                GM.instance.UnEquip(head);
            }
            foreach (GameObject cape in equippedCape)
            {
                GM.instance.UnEquip(cape);
            }
            foreach (GameObject chest in equippedChest)
            {
                GM.instance.UnEquip(chest);
            }
            foreach (GameObject glove in equippedGlove)
            {
                GM.instance.UnEquip(glove);
            }
            foreach (GameObject boot in equippedBoot)
            {
                GM.instance.UnEquip(boot);
            }
            script.scriptableObj.obtained = false;
            script.scriptableObj.inInventory = false;
            ownedItems.Remove(item);
        }
        equippedHead.Clear();
        equippedCape.Clear();
        equippedChest.Clear();
        equippedGlove.Clear();
        equippedBoot.Clear();
        currentHead = null;
        currentCape = null;
        currentChest = null;
        currentGlove = null;
        currentBoot = null;
    }

    [Serializable]
    class InventoryData
    {
        public string equippedHead;
        public string equippedChest;
        public string equippedCape;
        public string equippedBoot;
        public string equippedGlove;
        public List<string> ownedItems;
        public Hashtable items;
    }

}



