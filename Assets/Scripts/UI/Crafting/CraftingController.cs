using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class CraftingController : MonoBehaviour 
{

    private Transform m_Transform;
    private Transform bg_Transform;

    private Image itemImage;            // 合成的物品的图片
    private Button CraftingOne;         // 合成单个按钮
    private Button CraftingAll;         // 合成全部按钮
    private GameObject craftingItem;    // 合成出的物品的预制体
    private int itemId;                 // 合成出的物品的 ID
    private int haveBar;                // 物品是否有耐久条

    public GameObject Prefab_CraftingItem { set { craftingItem = value; } }
    public Transform Bg_Transform { get { return bg_Transform; } }






	private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();

        itemImage = m_Transform.Find("CraftingRightSlot/CraftingRightContent").GetComponent<Image>();
        CraftingOne = m_Transform.Find("CraftingClick").GetComponent<Button>();
        CraftingAll = m_Transform.Find("CraftingAllClick").GetComponent<Button>();
        bg_Transform = m_Transform.Find("CraftingRightSlot").GetComponent<Transform>();


        CraftingOne.onClick.AddListener(OnClickCraftingOne);
        CraftingAll.onClick.AddListener(OnClickCraftingAll);


        CannotUseCraftingOne();
        SetCraftingContentSprite(0,null,0);
    }





    // 外部设置要合成的物品的图片
    public void SetCraftingContentSprite(int id, Sprite sprite,int haveBar = 0)
    {
        this.haveBar = haveBar;
        itemId = id;
        if (sprite != null)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            return;
        }
        itemImage.gameObject.SetActive(false);

    }










    /// <summary>
    /// 禁止使用单个合成
    /// </summary>
    public void CannotUseCraftingOne()
    {
        CraftingOne.interactable = false;
        CraftingOne.transform.Find("Text").GetComponent<Text>().color = Color.gray;

        // 单个合成禁止时全部合成必然禁止
        CannotUseCraftingAll();
    }


    // 禁止使用全部合成
    public void CannotUseCraftingAll()
    {
        CraftingAll.interactable = false;
        CraftingAll.transform.Find("Text").GetComponent<Text>().color = Color.gray;
    }

    // 允许单个合成
    public void CanUseCraftingOne()
    {
        CraftingOne.interactable = true;
        CraftingOne.transform.Find("Text").GetComponent<Text>().color = Color.white;
    }

    // 允许全部合成
    public void CanUseCraftingAll()
    {
        CanUseCraftingOne();
        CraftingAll.interactable = true;
        CraftingAll.transform.Find("Text").GetComponent<Text>().color = Color.white;
    }








    // 合成单个物品时的方法
    private void OnClickCraftingOne()
    {
        SendMessageUpwards("CraftingOneOk");
    }


    // 合成全部物品时的方法
    private void OnClickCraftingAll()
    {
        SendMessageUpwards("CraftingAllOk");
    }

    // 父物体会调用，算是个回调
    // 逻辑大概是，子物体发送消息调用父物体的函数
    // 父物体接收到消息之后进行调用该函数进行合成处理
    // 类似回调
    public void CraftingSlotItem(int addNumber)
    {
        // 子物体如果有该组件说明不为空，否则为空
        InventoryItemController m_Controller = bg_Transform.GetComponentInChildren<InventoryItemController>();
        if (m_Controller == null)
        {
            GameObject target = GameObject.Instantiate<GameObject>(craftingItem, bg_Transform);
            target.name = "InventoryItem";
            RectTransform rectTransform = target.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 185.2f);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 184.2f);
            target.GetComponent<InventoryItemController>().InitSingleItem(itemId, itemImage.sprite, addNumber,haveBar);
        }
        else
        {
            m_Controller.Number += addNumber;
        }
    }

}
