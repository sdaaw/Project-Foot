using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public int inventorySlotCount;
    public float slotSize; //1 recommended'
    public float slotGap;
    public int lineCount;

    public GameObject inventorySlotPrefab;

    private RectTransform _thisRect;

    public List<GameObject> inventorySlotList = new List<GameObject>();

    public GameObject inventoryManagerObject;

    private InventoryManager _inventoryManager;


    private void Start()
    {
        _inventoryManager = inventoryManagerObject.GetComponent<InventoryManager>();
        _inventoryManager.inventory = this;
        _thisRect = GetComponent<RectTransform>();
        AdjustInventory(inventorySlotCount, lineCount);
        PopulateSlots();
    }

    public void AdjustInventory(int slots, int lines)
    {
        if(inventorySlotList.Count > 0)
        {
            foreach(GameObject a in inventorySlotList)
            {
                Destroy(a);
            }
            inventorySlotList = new List<GameObject>();
        }
        _thisRect.sizeDelta = new Vector2(slotGap * lines + slotGap, slotGap * (slots / lines) + slotGap);
        for (int i = 0; i < slots; i++)
        {
            GameObject a = Instantiate(inventorySlotPrefab);
            a.transform.SetParent(transform, false);
            inventorySlotList.Add(a);
        }
        RectTransform r;
        float yVal = 1;
        int slotIdx = 0;
        for (int i = 0; i < inventorySlotList.Count; i++)
        {
            slotIdx++;
            r = inventorySlotList[i].GetComponent<RectTransform>();
            r.anchoredPosition = _thisRect.anchoredPosition + new Vector2(slotIdx * slotGap, yVal - slotGap);
            if (slotIdx == lines)
            {
                yVal -= slotGap;
                slotIdx = 0;
            }
        }
    }

    public void UpdateInventory()  
    {
        AddItem(_inventoryManager.items[Random.Range(0, _inventoryManager.items.Count)]);
    }

    public void MoveItem(GameObject item, GameObject slot)
    {
        //if anyone ever reads this, skip this function for your own well-being.
        InventorySlot targetSlot = slot.GetComponent<InventorySlot>();
        InventorySlot itemSlot = item.GetComponent<Item>().slotObject.GetComponent<InventorySlot>();
        if(targetSlot.slottedItem == null)
        {
            itemSlot.slottedItem.GetComponent<RectTransform>().position = targetSlot.GetComponent<RectTransform>().position;
            targetSlot.inventoryManager = _inventoryManager;
            targetSlot.SetItem(item, itemSlot.slottedItem.GetComponent<Item>().itemCount);
            itemSlot.RemoveItem();
            return;
        }
         
        itemSlot.slottedItem.GetComponent<RectTransform>().position = targetSlot.GetComponent<RectTransform>().position;
        targetSlot.slottedItem.GetComponent<RectTransform>().position = itemSlot.GetComponent<RectTransform>().position;

        GameObject oldItem = itemSlot.slottedItem;
        GameObject oldTargetItem = targetSlot.slottedItem;


        targetSlot.slottedItem = oldItem;
        itemSlot.slottedItem = oldTargetItem;

        GameObject oldOgSlot = itemSlot.slottedItem.GetComponent<Item>().slotObject;

        itemSlot.slottedItem.GetComponent<Item>().slotObject = targetSlot.slottedItem.GetComponent<Item>().slotObject;
        targetSlot.slottedItem.GetComponent<Item>().slotObject = oldOgSlot;
    }

    public void PopulateSlots()
    {
        InventorySlot slot;
        foreach(GameObject a in inventorySlotList)
        {
            if(Random.Range(0, 100) > 50)
            {
                slot = a.GetComponent<InventorySlot>();
                slot.inventoryManager = _inventoryManager;
                slot.SetItem(_inventoryManager.items[Random.Range(0, _inventoryManager.items.Count)], 1);
            }
        }
    }

    public void AddItem(GameObject item)
    {
        foreach(GameObject a in inventorySlotList)
        {
            if(a.GetComponent<InventorySlot>().slottedItem != null)
            {
                print("?");
                if (item.GetComponent<Item>().itemName == a.GetComponent<InventorySlot>().slottedItem.GetComponent<Item>().itemName)
                {
                    //print("?");
                    a.GetComponent<InventorySlot>().slottedItem.GetComponent<Item>().IncreaseCount(1);
                    return;
                }
            }
        }
        foreach(GameObject a in inventorySlotList)
        {
            if(a.GetComponent<InventorySlot>().slottedItem == null)
            {
                a.GetComponent<InventorySlot>().SetItem(item, 1);
            }
        }
    }
}
