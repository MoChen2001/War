using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 背包控制脚本.
/// </summary>
public class InventoryPanelController : MonoBehaviour,IUIPanelShowHide {



    public static InventoryPanelController Instance;

    /// <summary>
    /// 获得视图和数据的引用
    /// </summary>
    private InventoryPanelModel m_Model;
    private InventoryPanelView m_View;



    // 背包的格子个数，后面也会用到
    private int slotNumber;
    // 管理格子的数组
    private List<GameObject> slotList;
    public Vector3 NowPosition { get { return m_View.M_Transform.position; } }      //返回当前位置


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        InitOthers();
        InitSlot();
        InitItems();
    }


    private void OnDisable()
    {
        CraftingPanelController.Instance.ResetSlotItem();
        ToolBarPanelController.Instance.ResetSlotItem();
        m_Model.ObjectToJson(slotList, "InventoryJsonData.txt");
    }


    /// <summary>
    ///  初始化乱七八糟的变量的函数
    /// </summary>
    private void InitOthers()
    {
        slotNumber = 30;
        slotList = new List<GameObject>();

        m_Model = gameObject.GetComponent<InventoryPanelModel>();
        m_View = gameObject.GetComponent<InventoryPanelView>();
    }



    /// <summary>
    /// 初始化背包的格子的函数
    /// </summary>
    private void InitSlot()
    {
        for(int i =0; i< slotNumber; i++)
        {
            GameObject temp = GameObject.Instantiate(m_View.Prefab_Slot, m_View.Grid_Transform);
            temp.name = "InventorySlot_" + i;
            slotList.Add(temp);
        }
    }


    /// <summary>
    /// 初始化背包中物体的函数
    /// </summary>
    private void InitItems()
    {
        List<InventoryItem> itemList = m_Model.GetItemList("InventoryJsonData.txt");
        for(int i = 0; i < itemList.Count; i++)
        {
            if(itemList[i].ItemName != "")
            {
                GameObject temp = GameObject.Instantiate(m_View.Prefab_Item, slotList[i].transform);
                temp.name = "InventoryItem";
                Sprite tempSprite = m_View.GetInventorySpriteByName(itemList[i].ItemName);
                temp.GetComponent<InventoryItemController>().InitSingleItem(itemList[i].ItemId, tempSprite,
                    itemList[i].ItemNum, itemList[i].ItemBar);
            }
        }
    }


    /// <summary>
    /// 向背包槽中添加物品
    /// 当然这里的背包存储的物品的个数没有上限
    /// </summary>
    /// <param name="items">要添加的物品链表</param>
    public void AddItem(List<GameObject> items)
    {
        for(int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            if(tempTransform == null)
            {
                items[0].transform.SetParent(slotList[i].transform);
                items[0].GetComponent<InventoryItemController>().IsInventory = true;
                items.RemoveAt(0);
                if(0 >= items.Count)
                {
                    break;
                }
            }
        }
    }


    /// <summary>
    /// 将字典中的物品加入背包，传过来的字典应该是没有相同项的
    /// 进行两次遍历，第一次先查找里面有没有同类的东西，第二次再查找空的背包槽
    /// 当然这里的背包存储的物品的个数没有上限
    /// </summary>
    /// <param name="items"></param>
    public void AddItem(Dictionary<int,GameObject> items)
    {
        // 第一次遍历，找出背包槽中和字典中ID一样的物体合并
        for(int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            GameObject tempGameObject;
            if(tempTransform != null)
            {
                InventoryItemController tempInventory = tempTransform.GetComponent<InventoryItemController>();
                if(items.TryGetValue(tempInventory.ItemID, out tempGameObject))
                {
                    tempInventory.ChangeNum(tempGameObject.GetComponent<InventoryItemController>().Number);
                    items.Remove(tempInventory.ItemID);
                    GameObject.Destroy(tempGameObject);
                }
            }
        }
        // 如果一次遍历无法达到目标
        if(items.Count > 0)
        {
            // 寻找背包槽中的空槽放入字典中的第一个物品就可以
            for (int i = 0; i < slotList.Count; i++)
            {
                Transform tempTransform = slotList[i].transform.Find("InventoryItem");
                if (tempTransform == null && items.Count > 0)
                {
                    // 遍历中没法改变其中的值，但是直接 break 是可以的
                    foreach (var k in items)
                    {
                        k.Value.transform.SetParent(slotList[i].transform);
                        k.Value.GetComponent<InventoryItemController>().IsInventory = true;
                        items.Remove(k.Key);
                        break;
                    }
                }
                if (items.Count <= 0) break;
            }
        }
    }




    /// <summary>
    ///  根据 ID 值进行添加材料
    ///  name 用来确定图标
    /// </summary>
    public void AddItemWithID(int id, string name, int number = 1)
    {
        Show();
        /// 第一次寻找字典中的相应 ID 的值并合并
        for (int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            if (tempTransform != null)
            {
                InventoryItemController tempInventory = tempTransform.GetComponent<InventoryItemController>();
                if (tempInventory.ItemID == id)
                {
                    tempInventory.ChangeNum(number);
                    Hide();
                    return;
                }
            }
        }


        /// 没找到的时候添加一个
        for (int i = 0; i < slotList.Count; i++)
        {
            Transform tempTransform = slotList[i].transform.Find("InventoryItem");
            if (tempTransform == null)
            {
                GameObject temp = GameObject.Instantiate(m_View.Prefab_Item, slotList[i].transform);
                temp.name = "InventoryItem";
                Sprite tempSprite = m_View.GetInventorySpriteByName(name);
                
                temp.GetComponent<InventoryItemController>().InitSingleItem(id, tempSprite,
                    number, 0);
                Hide();
                return;
            }
        }
        Hide();
    }

    
    /// <summary>
    /// 向合成槽中添加物品的通知函数
    /// </summary>
    /// <param name="tempGameObject"></param>
    public void SendAddSlotItem(GameObject tempGameObject,bool flag)
    {
        CraftingPanelController.Instance.AddSlotItem(tempGameObject, flag);
    }


    /// <summary>
    /// 从合成槽中取出物品的通知函数
    /// </summary>
    public void SendRemoveSlotItem(GameObject obj)
    {
        CraftingPanelController.Instance.RemoveSlotItem(obj);
    }

    public void Show()
    {
        m_View.transform.localScale = new Vector3(1, 1, 1);
        m_View.CraftingCenter.SetActive(true);
        //gameObject.SetActive(true);
    }

    public void Hide()
    {
        m_View.transform.localScale = new Vector3(0,0,0);
        m_View.CraftingCenter.SetActive(false);
        //gameObject.SetActive(false);
    }
}
