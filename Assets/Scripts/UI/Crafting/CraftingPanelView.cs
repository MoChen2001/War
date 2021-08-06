using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// 合成类视图（View）
/// </summary>
public class CraftingPanelView : MonoBehaviour 
{
	/// <summary>
	/// 会用到的数据
	/// </summary>
	private Transform m_Transform;
	private Transform tabs_Transform;						// 选项卡的父物体的 Transform
	private Transform contents_Transform;					// 选项卡的内容的父物体的 Transform
	private Transform center_Transform;						// 中间的 UI 的父物体的 Transform
	private Transform right_Transform;						// 右边的 UI 的父物体的 Transform


	private GameObject tabItem;								// 选项卡预制体
	private GameObject content;								// 选项卡内容父物体预制体
	private GameObject contentItem;                         // 选项卡内容预制体
	private GameObject center_Slot;                         // 合成槽的预制体
	private GameObject crafting_item;						// 合成出的要实例化的新物品



	private Dictionary<string, Sprite> tabDic;				// 左侧的 tab 的UI
	private Dictionary<string, Sprite> slotDic;				// 中间的合成槽的UI
	private Dictionary<string, Sprite> craftingItemDic;		// 合成出的物体的UI



	/// <summary>
	/// 封装的属性
	/// </summary>
	public Transform M_Transform { get { return m_Transform; } }
	public Transform Tabs_Transform { get { return tabs_Transform; } }
	public Transform Contents_Transform { get { return contents_Transform; } }
	public Transform Center_Transform { get { return center_Transform; } }
	public Transform Right_Transform { get { return right_Transform; } }


	public GameObject Prefab_TabItem { get { return tabItem; } }
	public GameObject Prefab_Content { get { return content; } }
	public GameObject Prefab_ContentItem { get { return contentItem; } }
	public GameObject Prefab_CenterSlot { get { return center_Slot; } }
	public GameObject Prefab_CraftingItem { get { return crafting_item; }}


	private void Awake()
    {
		InitOthers();
		InitDic();
	}


	/// <summary>
	/// 初始化杂七杂八的东西
	/// </summary>
	private void InitOthers()
    {
		m_Transform = gameObject.GetComponent<Transform>();
		tabs_Transform = m_Transform.Find("CraftingLeft/Tags").GetComponent<Transform>();
		contents_Transform = m_Transform.Find("CraftingLeft/Contents").GetComponent<Transform>();
		center_Transform = m_Transform.Find("CraftingCenter").GetComponent<Transform>();
		right_Transform = m_Transform.Find("CraftingRight").GetComponent<Transform>();


		crafting_item = Resources.Load<GameObject>("UI/Prefabs/InventoryItem");
		tabItem = Resources.Load<GameObject>("UI/Prefabs/CraftingTabItem");
		content = Resources.Load<GameObject>("UI/Prefabs/CraftingContent");
		contentItem = Resources.Load<GameObject>("UI/Prefabs/CraftingContentItem");
		center_Slot = Resources.Load<GameObject>("UI/Prefabs/CraftingSlot");


		
	}



	/// <summary>
	/// 初始化字典
	/// </summary>
	private void InitDic()
    {

		tabDic = ResourcesTool.LoadSpriteWithPath("UI/ItemTextures/Crafting/Icon");
		slotDic = ResourcesTool.LoadSpriteWithPath("UI/ItemTextures/Crafting/Material");
		craftingItemDic = ResourcesTool.LoadSpriteWithPath("UI/ItemTextures/Crafting/Item");
	}


	/// <summary>
	/// 根据名称获得 tab字典 UI
	/// </summary>
	public Sprite GetTabSpriteByName(string name)
    {
		return ResourcesTool.GetSpriteWithName(name, tabDic);
    }



	/// <summary>
	/// 根据名称获得合成槽的UI字典.
	/// </summary>
	public Sprite GetSlotSpriteByName(string name)
    {
		return ResourcesTool.GetSpriteWithName(name, slotDic);
	}



	/// <summary>
	///  根据名称获得合成出的物品的 UI 字典
	/// </summary>
	public Sprite GetCraftingItemSpriteByName(string name)
	{
		return ResourcesTool.GetSpriteWithName(name, craftingItemDic);
	}


}
