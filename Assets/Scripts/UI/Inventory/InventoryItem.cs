using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem  {

    private string itemName;
    private int itemNum;
    private int itemId;
    private int itemBar;


    public int ItemId
    {
        get { return itemId; }
        set { itemId = value; }
    }
    public string ItemName
    {
        get { return itemName; }
        set { itemName = value; }
    }

    public int ItemNum
    {
        get { return itemNum; }
        set { itemNum = value; }
    }

    public int ItemBar
    {
        get { return itemBar; }
        set { itemBar = value; }
    }

    public InventoryItem() { }

    public InventoryItem(int itemId,string itemName, int number,int itemBar)
    {
        this.ItemId = itemId;
        this.ItemName = itemName;
        this.ItemNum = number;
        this.ItemBar = itemBar;
    }

}
