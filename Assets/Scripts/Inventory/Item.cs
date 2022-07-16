using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //Item -> Subclasses of items
    public enum Rarity
    {
        Common,
        Uncommon,
        Epic,
        Legendary
    }

    public Rarity rarity;
    public string itemName;
    public Sprite itemIcon;
    public string itemDescription;
}
