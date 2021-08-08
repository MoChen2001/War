using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



/// <summary>
///  背包视图脚本.
/// </summary>
public class InventoryPanelView : MonoBehaviour 
{

	/// <summary>
	/// 会使用到的字段.
	/// </summary>
	private Transform m_Transform;
	private Transform gird_Transform;

	private GameObject prefab_Slot;
	private GameObject prefab_Item;

	private Dictionary<string, Sprite> inventoryDic;
	private GameObject craftingCenter;


	/// <summary>
	/// 为外部提供访问的属性
	/// </summary>
	public Transform M_Transform
    {
        get { return m_Transform; }
    }
	public Transform Grid_Transform
    {
        get { return gird_Transform; }
    }

	public GameObject Prefab_Slot
    {
		get { return prefab_Slot; }
    }
	public GameObject Prefab_Item
    {
        get { return prefab_Item; }
    }

	public GameObject CraftingCenter
	{ 
		get
		{ return craftingCenter; } 
	}

	
	
	void Awake () 
	{
		m_Transform = gameObject.GetComponent<Transform>();
		gird_Transform = m_Transform.Find("InventoryBackground/InventoryGrid").GetComponent<Transform>();
		craftingCenter = GameObject.Find("CanvasCamera/InventoryPanel/CraftingPanel/CraftingCenter");

		prefab_Slot = Resources.Load<GameObject>("UI/Prefabs/InventorySlot");
		prefab_Item = Resources.Load<GameObject>("UI/Prefabs/InventoryItem");


		inventoryDic = ResourcesTool.LoadSpriteWithPath("UI/ItemTextures/Inventory");

	}


	

	/// <summary>
	/// 根据贴图的名称获取对应的 sprite 
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public Sprite GetInventorySpriteByName(string name)
    {
		return ResourcesTool.GetSpriteWithName(name,inventoryDic);
    }


}
