using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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

    public TMP_Text stackSizeText;

    public int itemCount;

    public InventoryManager inventoryManager; //set on instantiating

    public GameObject slotObject;

    private bool _isDragging;
    private RectTransform _thisRect;
    private void Start()
    {
        _thisRect = GetComponent<RectTransform>();
        if (GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = itemIcon;
        }
        EventTrigger trigger = GetComponentInParent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();

        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { E_OnCursorHover(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback.AddListener((eventData) => { E_OnCursorHold(); });
        trigger.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback.AddListener((eventData) => { E_OnCursorRelease(); });
        trigger.triggers.Add(entry);

        stackSizeText.text = itemCount.ToString();

    }

    private void FixedUpdate()
    {
        if(_isDragging)
        {
            Vector3 mPos = Input.mousePosition;
            mPos.z = -1f;
            _thisRect.position = mPos; 
        }
    }

    public void IncreaseCount(int amount)
    {
        itemCount += amount;
        if(itemCount >= 99)
        {
            //split stack
            itemCount = 99;
            return;
        }
        stackSizeText.text = itemCount.ToString();
    }


    public void E_OnCursorHover()
    {
        print(itemName);
        if(!_isDragging)
        {
        }
    }

    public void E_OnCursorHold()
    {
        _isDragging = true;
    }

    public void E_OnCursorRelease()
    {
        _isDragging = false;
        SnapToClosestEmptySlot();
    }

    public void SnapToClosestEmptySlot()
    {
        float closestDist = 9999f;
        GameObject closestObj = null;
        foreach(GameObject a in inventoryManager.inventory.inventorySlotList)
        {
            float dist = Vector3.Distance(GetComponent<RectTransform>().position, a.gameObject.GetComponent<RectTransform>().position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestObj = a;
            } 
        }

        inventoryManager.inventory.MoveItem(slotObject.GetComponent<InventorySlot>().slottedItem, closestObj);

        /*InventorySlot closestInvSlot = closestObj.GetComponent<InventorySlot>();
        InventorySlot thisInvSlot = thisSlot.GetComponent<InventorySlot>();

        closestInvSlot.slottedItem.GetComponent<RectTransform>().position = thisSlot.GetComponent<RectTransform>().position;
        GetComponent<RectTransform>().position = closestInvSlot.GetComponent<RectTransform>().position;*/
    }
}
