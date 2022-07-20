using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public GameObject slottedItem;

    public InventoryManager inventoryManager;

    public void SetItem(GameObject item, int count)
    {
        GameObject a = Instantiate(item);
        a.transform.SetParent(inventoryManager.canvas.transform);
        a.GetComponent<Item>().inventoryManager = inventoryManager;
        slottedItem = a;
        a.GetComponent<Item>().slotObject = gameObject;
        a.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
        a.GetComponent<Item>().itemCount = count;
        a.name = a.GetComponent<Item>().itemName + "(" + a.GetComponent<Item>().itemCount + ")";
    }

    public void RemoveItem()
    {
        if(slottedItem != null)
        {
            Destroy(slottedItem.gameObject);
            slottedItem = null;
        }
    }

    public void CombineStacks(Item item)
    {
        int stackSizeSum = slottedItem.GetComponent<Item>().itemCount + item.GetComponent<Item>().itemCount;
    }
}
