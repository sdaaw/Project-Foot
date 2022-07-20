using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipableItem : Item
{
    public enum SlotType
    {
        ring0,
        ring1,
        ring2,
        ring3,
    }
    public float movementSpeedBonus;
    public float damageBonus;
    public float healthBonus;
}
