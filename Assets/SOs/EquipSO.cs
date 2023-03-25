using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/EquipSO", order = 1)]

public class EquipSO : ScriptableObject
{
    public string itemName;
    public string slot;
    public int price;
    public int ATKMod;
    public int DEFMod;
    public int SPDMod;
    public int FLYMod;
    public bool obtained;
    public bool equipped;
    public bool locked;
    public bool inInventory;
    public bool classic, popular, space, rustic, hitech, occult, natural, dramatic;
    public Sprite sprite;
    public string description;

}
