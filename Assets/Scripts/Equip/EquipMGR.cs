using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class EquipMGR : MonoBehaviour
{
    public static EquipMGR instance;
    public TextMeshProUGUI newStats;
    public float xPos = 0;
    public float pad = 15;
    public GameObject parent;
    public Transform target;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        xPos = target.position.x;
       
        GameObject[] toDisplay = InvMGR.instance.ownedItems.ToArray();
        foreach(GameObject item in toDisplay)
        {
            Debug.Log("spawning");
            target.position = new Vector3(xPos, target.position.y, 0);
            Vector3 objectPos = target.position;
            Instantiate(item, objectPos, Quaternion.identity);
            xPos = target.position.x + pad;
            Debug.Log("instantiated " + item + "at " + objectPos);
        }
        
    }
}
