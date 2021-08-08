using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 合成类控制器（Controller)
/// </summary>
public class CraftingPanelController : MonoBehaviour 
{
	private CraftingPanelModel m_Model;
	private CraftingPanelView m_View;
	private CraftingController m_CraftingController;

	private List<GameObject> tabList;			// 选项卡的选项列表
	private List<GameObject> contentList;		// 选项卡的内容列表
	private List<GameObject> slotList;			// 合成槽的列表


	private int tabNum;                             // 选项卡的个数
	private int currentIndex;                       // 当前的选项卡索引
	private int slotNum;                            // 合成槽的个数

	private int materialsCount;                     // 合成需要的总材料数
	private List<GameObject> materialsList;			// 合成槽填充物物体的列表


	public static CraftingPanelController Instance;


	private void Awake()
    {
		Instance = this;
    }

	private void Start()
    {
		InitOthers();

		// 初始化左边
		InitTab();
		InitContent();
		ResetTab(0);

		// 初始化中间区域
		InitCenterSlot();
		CreateSlotContent(-1);
	}


	/// <summary>
	/// 初始化乱七八糟的东西
	/// </summary>
	private void InitOthers()
    {
		materialsCount = 0;
		tabNum = 2;
		slotNum = 25;
		currentIndex = -1;
		tabList = new List<GameObject>();
		contentList = new List<GameObject>();
		slotList = new List<GameObject>();
		materialsList = new List<GameObject>();


		m_View = gameObject.GetComponent<CraftingPanelView>();
		m_Model = gameObject.GetComponent<CraftingPanelModel>();

		m_CraftingController = m_View.Right_Transform.GetComponent<CraftingController>();
		m_CraftingController.Prefab_CraftingItem = m_View.Prefab_CraftingItem;
	}


	/// <summary>
	/// 初始化标签
	/// </summary>
	private void InitTab()
    {
		for(int i = 0; i < tabNum; i++)
        {
			string str = m_Model.TabIconName[i];
			GameObject tempTab = GameObject.Instantiate(m_View.Prefab_TabItem, m_View.Tabs_Transform);
			Sprite tempSprite = m_View.GetTabSpriteByName(str);
			tempTab.GetComponent<CraftingTabController>().InitTab(tempSprite, i);

			tempTab.name = "Tab" + i;
			tabList.Add(tempTab);
		}

	}


	/// <summary>
	/// 初始化选项卡内容
	/// </summary>
	private void InitContent()
	{
		List<List<CraftingContentItem>> tempContentList = m_Model.GetContentList("CraftingContentsJsonData");
		for (int i = 0; i < tabNum; i++)
		{
			GameObject tempContent = GameObject.Instantiate(m_View.Prefab_Content, m_View.Contents_Transform);
			tempContent.name = "Contents" + i;
			tempContent.GetComponent<CraftingContentController>().InitContent(i, tempContentList[i], m_View.Prefab_ContentItem);
			contentList.Add(tempContent);
			tempContent.SetActive(false);
		}
	}


	/// <summary>
	/// 初始化中间的合成槽
	/// </summary>
	private void InitCenterSlot()
    {
        for (int i = 0; i < slotNum; i++)
        {
			GameObject tempSlot = GameObject.Instantiate(m_View.Prefab_CenterSlot, m_View.Center_Transform);
			tempSlot.name = "slot" + i;
			slotList.Add(tempSlot);
        }
    }


	/// <summary>
	/// 填充合成槽
	/// </summary>
	private void CreateSlotContent(int id)
    {
		CraftingSlotContentItem tempItem = m_Model.GetMapItemById(id);
		if(tempItem != null)
		{
			/// 重置合成槽
			ResetSlot();
			ResetSlotItem();
			/// 改变合成目标UI
			Sprite temp = m_View.GetCraftingItemSpriteByName(tempItem.MapName);
			m_View.Right_Transform.GetComponent<CraftingController>().SetCraftingContentSprite(tempItem.MapId, temp);
			materialsCount = tempItem.MaterialsCount;
			/// 改变合成槽UI
			for (int i = 0; i < slotNum; i++)
			{
				if(tempItem.MapContents[i] != "0")
                {
					Sprite target = m_View.GetSlotSpriteByName(tempItem.MapContents[i]);
					int itemId = int.Parse(tempItem.MapContents[i]);
					slotList[i].GetComponent<CraftingSlotController>().SetSlotItemUI(itemId, target);
					slotList[i].GetComponent<CanvasGroup>().blocksRaycasts = true;
				}
                else
                {
					slotList[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
				}
			}

		}
        else
        {
			for (int i = 0; i < slotNum; i++)
			{
				slotList[i].GetComponent<CanvasGroup>().blocksRaycasts = false;
			}
		}
    }







	/// <summary>
	/// 重置选项卡和其中的内容的函数.
	/// </summary>
	/// <param name="index">被选中的选项卡的索引</param>
	public void ResetTab(int index)
    {
		if(index != currentIndex)
        {
			if (currentIndex != -1)
            {
				tabList[currentIndex].GetComponent<CraftingTabController>().ButtonBGNormal();
				contentList[currentIndex].SetActive(false);
				ResetSlot();
				ResetSlotItem();
			}
			currentIndex = index;
			tabList[currentIndex].GetComponent<CraftingTabController>().ButtonBGClick();
			contentList[currentIndex].SetActive(true);
		}
	}




	/// <summary>
	/// 选项卡切换时重置合成槽
	/// </summary>
	private void ResetSlot()
    {
		for (int i = 0; i < slotNum; i++)
		{
			slotList[i].GetComponent<CraftingSlotController>().ResetSlot();
		}
	}


	/// <summary>
	/// 重置图谱槽的时候将里面已经填充的物体合并填充回背包
	/// </summary>
	public void ResetSlotItem()
    {
		GameObject tempV = null;
		InventoryItemController tempController = null;
		// 用来合并合成面板中同类的东西的临时字典
		Dictionary<int, GameObject> map = new Dictionary<int, GameObject>();
		for(int i = 0; i < slotList.Count; i++)
        {
			Transform temp = slotList[i].transform.Find("InventoryItem");
			if (temp != null)
            {
				// 字典中查询到的值
				// 当前的物体的控制器
				tempController = temp.GetComponent<InventoryItemController>();
				// 如果字典中有该类型的物体
				if (map.TryGetValue(tempController.ItemID, out tempV))
                {
					tempV.GetComponent<InventoryItemController>().ChangeNum(tempController.Number);
					GameObject.Destroy(tempController.gameObject) ;
				}
				// 字典中没有该类型的物体
                else
                {
					map.Add(tempController.ItemID, tempController.gameObject);
				}
				tempController = null;
			}

		}

		// 合成出的物品槽的检查
		tempController  = m_CraftingController.Bg_Transform.GetComponentInChildren<InventoryItemController>();
		if (tempController != null)
        {
			map.Add(tempController.ItemID, tempController.gameObject);
		}


		InventoryPanelController.Instance.AddItem(map);
	}







	/// <summary>
	/// 拖拽入合成槽的时候执行的函数
	/// </summary>
	/// <param name="tempGameobject">要加入 List 的物体</param>
	/// <param name="flag">为真是加入，为假时仅仅合并不加入</param>
	public void AddSlotItem(GameObject tempGameobject,bool flag)
    {
		// 如果为真，说明确实增加了物品
		if(flag)
        {
			materialsList.Add(tempGameobject);
		}
		UpdateCraftingBtn();
	}


	/// <summary>
	/// 拖拽出合成槽时执行的函数
	/// </summary>
	public void RemoveSlotItem(GameObject tempGameObject)
    {
		materialsList.Remove(tempGameObject);
		UpdateCraftingBtn();
	}







	/// <summary>
	/// 合成结束后进行合成槽物品数量的减少,合成辅助函数
	/// </summary>
	private void CraftingOk(int number)
    {
		// 父物体回调这个函数进行合成
		m_CraftingController.CraftingSlotItem(number);

		for (int i = 0; i < materialsList.Count; i++)
        {
			InventoryItemController m_Controller = materialsList[i].GetComponent<InventoryItemController>();
			int tempN = m_Controller.Number - number;
			if(tempN <= 0)
            {

				GameObject.Destroy(materialsList[i]);
				materialsList.RemoveAt(i);
				i--;
			}
			else
            {
				m_Controller.MinusNum(number);
			}
		}
		UpdateCraftingBtn();
	}

	/// <summary>
	/// 单个合成使用函数
	/// </summary>
	public void CraftingOneOk()
    {
		CraftingOk(1);
	}


	/// <summary>
	/// 全部合成使用函数
	/// </summary>
	public void CraftingAllOk()
    {
		int min = int.MaxValue;
		for(int i = 0; i < materialsList.Count; i++)
        {
			if(min > materialsList[i].GetComponent<InventoryItemController>().Number)
            {
				min = materialsList[i].GetComponent<InventoryItemController>().Number;
			}
        }
		CraftingOk(min);
    }



	/// <summary>
	/// 更新两个按钮的函数
	/// </summary>
	private void UpdateCraftingBtn()
    {
		// 说明此时至少可以合成物品
		if(materialsList.Count == materialsCount)
        {
			int min = int.MaxValue;
			for (int i = 0; i < materialsList.Count; i++)
			{
				if (materialsList[i].GetComponent<InventoryItemController>().Number < min)
				{
					min = materialsList[i].GetComponent<InventoryItemController>().Number;
				}
			}
			if (min > 1)
			{
				m_CraftingController.CanUseCraftingAll();
			}
			else
			{
				m_CraftingController.CanUseCraftingOne();
				m_CraftingController.CannotUseCraftingAll();
			}
		}
		else
        {
			m_CraftingController.CannotUseCraftingOne();

		}
	
	}

}
