using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{

    public GameObject inventoryObject;

    public Inventory inventory;

    public List<GameObject> items = new List<GameObject>();
    public Camera playerCamera;
    public Canvas canvas;

    public GameObject tooltipPanel;
    public TMP_Text toolTipText;

    private void Start()
    {
    }

}
