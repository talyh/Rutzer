using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public float weight;
    public enum Rarity { Common, Uncommon, Rare, Epic, Impossible }
    public Rarity rarity;
    public int cost;
    public Sprite icon;
}
