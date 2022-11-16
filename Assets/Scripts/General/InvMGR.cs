using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvMGR : MonoBehaviour
{
    public static InvMGR instance;
    public GameObject[] allItems;
    public List<GameObject> ownedItems;
    public int count;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in allItems)
        {
            EquipHandler script = item.GetComponent<EquipHandler>();
            if (script.obtained)
            {
                ownedItems.Add(item.gameObject);
            }
        }
        count = ownedItems.Count;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
