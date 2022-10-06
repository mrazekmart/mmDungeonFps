using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class InventoryItem
{

    public int itemType = 0;
    public Sprite sprite2d;

    public InventoryItem()
    {
        this.sprite2d = null;
    }
    public InventoryItem(Sprite sprite2d)
    {
        this.sprite2d = sprite2d;
    }


}
public class InventoryHandlerScript : MonoBehaviour
{

    public GameObject buttonAS1;
    public GameObject buttonAS2;
    public GameObject buttonAS3;
    public GameObject buttonAS4;
    public GameObject buttonAS5;
    public GameObject buttonAS6;
    public GameObject buttonAS7;
    public GameObject buttonAS8;


    public static InventoryHandlerScript Instance
    {
        get; private set;
    }

    private List<InventoryItem> regularItems;
    private List<InventoryItem> weapons;
    private List<GameObject> weaponsGO;
    private InventoryItem[] actionBarList;
    private List<GameObject> actionBarButtons;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        regularItems = new List<InventoryItem>();
        weapons = new List<InventoryItem>();
        weaponsGO = new List<GameObject>();
        actionBarList = new InventoryItem[8];
        actionBarButtons = new List<GameObject>();

        actionBarButtons.Add(buttonAS1);
        actionBarButtons.Add(buttonAS2);
        actionBarButtons.Add(buttonAS3);
        actionBarButtons.Add(buttonAS4);
        actionBarButtons.Add(buttonAS5);
        actionBarButtons.Add(buttonAS6);
        actionBarButtons.Add(buttonAS7);
        actionBarButtons.Add(buttonAS8);

    }
    public void AddInventoryItem(GameObject gameObject)
    {
        SpriteRenderer sprite = gameObject.GetComponentInChildren(typeof(SpriteRenderer), true) as SpriteRenderer;

        InventoryItem item = new InventoryItem(sprite.sprite);
        switch (gameObject.tag)
        {
            case "Weapon":
                {
                    item.itemType = 1;
                    weapons.Add(item);
                    weaponsGO.Add(gameObject);
                    UpdateActionBar();
                    break;
                }
            default:
                {
                    item.itemType = 0;
                    regularItems.Add(item);
                    break;
                }
        }

    }
    public void UpdateActionBar()
    {
        int i = 0;
        foreach (InventoryItem item in weapons)
        {
            if (i < actionBarList.Length - 1)
            {
                actionBarList[i] = item;
                i++;
            }
            else
            {
                break;
            }
        }

        for (int j = 0; j < actionBarList.Length; j++)
        {
            if (actionBarList[j] != null)
            {
                actionBarButtons[j].GetComponent<Image>().sprite = actionBarList[j].sprite2d;
            }
        }
    }
    public GameObject GetActionBarItem(int number)
    {
        if (number <= weaponsGO.Count && number > 0)
        {
            return weaponsGO[number - 1];
        }
        else
        {
            return null;
        }
    }
}
