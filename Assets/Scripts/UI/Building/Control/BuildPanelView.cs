using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelView : MonoBehaviour
{
	private Transform m_Transform;
	private Transform bg_Transform;                             // 子物体的transform
	private Transform player_Transform;                         // 玩家角色的 Transform
	private Text m_ItemName;                                    // 中心的物品的名称 UI
	private Camera envCamera;                                   // 环境摄像机，进行射线检测的摄像机？
	private Transform buildModeParent;                          // 所有生成物体的父物体

	private List<BuildItemControl> itemControlList;              // 管理所有的环状 UI
	private List<List<BuildMaterilControl>> materialControlList; // 管理所有的环状 UI



	private string[] itemName = new string[]                     // 所有的环状 UI 的名称	
	{
			"[拆除]", "[杂项]", "[屋顶]", "[楼梯]",
			"[窗户]", "[门]","[墙壁]", "[地板]", "[地基]"
	};                 // 所有的环状 UI 的名称	
	private List<string[]> materialsName;                        // 管理所有的材料的名称
	public Dictionary<string, string> materialNameDic;           // 存储对应的中文材料的英文名称

	private GameObject prefab_Item;                             // 环状UI当个项的预制体 
	private GameObject prefab_ItemChild;                        // 环状 UI 子物体的预制体
	private List<Sprite> iconList;                              // 环状UI当个项的图标
	private List<Sprite[]> materialIcons;                       // 材料的 Icon UI数组



	public Transform M_Transform { get { return m_Transform; } }
	public Transform BG_Transform { get { return bg_Transform; } }
	public Transform Player_Transform { get { return player_Transform; } }
	public Text M_ItemName { get { return m_ItemName; } }
	public Camera EnvCamera { get { return envCamera; } }
	public Transform BuildModelsParent { get { return buildModeParent; } }

	public List<BuildItemControl> ItemControlList { get { return itemControlList; } }
	public List<List<BuildMaterilControl>> MaterialControlList { get { return materialControlList; } }
	public GameObject Prefab_Item { get { return prefab_Item; } }
	public GameObject Prefab_ItemChild { get { return prefab_ItemChild; } }
	public List<Sprite> IconList { get { return iconList; } }
	public List<Sprite[]> MaterialIcons { get { return materialIcons; } }


	public string[] ItemName { get { return itemName; } }
	public List<string[]> MaterialsName { get { return materialsName; } }                      
	public Dictionary<string, string> MaterialNameDic { get { return materialNameDic; } }



	private void Awake()
    {
		InitOther();
		InitMaterialIconName();
		InitNameDic();



		prefab_Item = Resources.Load<GameObject>("Building/Prefab/Item");
		prefab_ItemChild = Resources.Load<GameObject>("Building/Prefab/ItemChild");

		LoadMaterialIcon();
		LoadIconList();


		CreateItems();
		CreateItemChild();
	}



	/// <summary>
	///  初始化杂项的函数
	/// </summary>
	private void InitOther()
    {

		materialControlList = new List<List<BuildMaterilControl>>();
		itemControlList = new List<BuildItemControl>();
		materialIcons = new List<Sprite[]>();
		iconList = new List<Sprite>();
		materialNameDic = new Dictionary<string, string>();
		materialsName = new List<string[]>();

		m_Transform = gameObject.GetComponent<Transform>();
		buildModeParent = GameObject.Find("BuildModels").GetComponent<Transform>();
		player_Transform = GameObject.Find("FPSController").GetComponent<Transform>();
		envCamera = player_Transform.Find("PersonCamera/EnvCamera").GetComponent<Camera>();
		bg_Transform = m_Transform.Find("WheelBG");
		m_ItemName = m_Transform.Find("WheelBG/ItemName").GetComponent<Text>();


		materialIcons = new List<Sprite[]>();
		iconList = new List<Sprite>();

	}




	/// <summary>
	///  设置二级UI材料的名称
	/// </summary>
	private void InitMaterialIconName()
	{
		materialsName.Add(null);
		materialsName.Add(new string[] { "[吊灯]", "[木棍]", "[梯子]" });
		materialsName.Add(new string[] { "[屋顶]" });
		materialsName.Add(new string[] { "[直梯]", "[L形梯]" });
		materialsName.Add(new string[] { "[窗户]" });
		materialsName.Add(new string[] { "[门]" });
		materialsName.Add(new string[] { "[普通墙壁]", "[门型墙壁]", "[窗型墙壁]" });
		materialsName.Add(new string[] { "[地板]" });
		materialsName.Add(new string[] { "[地基]" });
	}

	/// <summary>
	///  设置对应的字典集合
	/// </summary>
	private void InitNameDic()
	{
		materialNameDic.Add("[吊灯]", "Ceiling_Light");
		materialNameDic.Add("[木棍]", "Pillar");
		materialNameDic.Add("[梯子]", "Ladder");
		materialNameDic.Add("[屋顶]", "Roof");
		materialNameDic.Add("[直梯]", "Stairs");
		materialNameDic.Add("[L形梯]", "L_Shaped_Stairs");
		materialNameDic.Add("[窗户]", "Window");
		materialNameDic.Add("[门]", "Door");
		materialNameDic.Add("[普通墙壁]", "Wall");
		materialNameDic.Add("[门型墙壁]", "Doorway");
		materialNameDic.Add("[窗型墙壁]", "Window_Frame");
		materialNameDic.Add("[地板]", "Floor");
		materialNameDic.Add("[地基]", "Platform");

	}








	/// <summary>
	///  Start中调用的的初始化IconList资源
	/// </summary>
	private void LoadIconList()
	{
		iconList.Add(null);
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Question Mark"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Roof_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Stairs_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Window_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Door_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Wall_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Floor_Category"));
		iconList.Add(Resources.Load<Sprite>("Building/Icon/Foundation_Category"));
	}


	/// <summary>
	///  材料的 Icon UI 加载
	/// </summary>
	private void LoadMaterialIcon()
	{
		materialIcons.Add(null);
		materialIcons.Add(new Sprite[]{ LoadMaterialIcon("Ceiling Light"),
			LoadMaterialIcon("Pillar_Wood"), LoadMaterialIcon("Wooden Ladder") });
		materialIcons.Add(new Sprite[] { null, LoadMaterialIcon("Roof_Metal"), null });
		materialIcons.Add(new Sprite[] { LoadMaterialIcon("Stairs_Wood"), LoadMaterialIcon("L Shaped Stairs_Wood"), null });
		materialIcons.Add(new Sprite[] { null, LoadMaterialIcon("Window_Wood"), null });
		materialIcons.Add(new Sprite[] { null, LoadMaterialIcon("Wooden Door"), null });
		materialIcons.Add(new Sprite[] { LoadMaterialIcon("Wall_Wood"),
			LoadMaterialIcon("Doorway_Wood"), LoadMaterialIcon("Window Frame_Wood") });
		materialIcons.Add(new Sprite[] { null, LoadMaterialIcon("Floor_Wood"), null });
		materialIcons.Add(new Sprite[] { null, LoadMaterialIcon("Platform_Wood"), null });
	}

	/// <summary>
	///  加载 MaterialIcon 文件夹中的文件
	/// </summary>
	private Sprite LoadMaterialIcon(string name)
	{
		return Resources.Load<Sprite>("Building/MaterialIcon/" + name);
	}










	/// <summary>
	///  创建 Item 物体
	/// </summary>
	private void CreateItems()
	{
		for (int i = 0; i < 9; i++)
		{
			GameObject temp = GameObject.Instantiate(prefab_Item, bg_Transform);

			temp.GetComponent<BuildItemControl>().Init(i, iconList[i]);

			itemControlList.Add(temp.GetComponent<BuildItemControl>());
		}
	}


	/// <summary>
	///  创建 Item 物体的(逻辑上的)子物体
	/// </summary>
	private void CreateItemChild()
	{
		for (int i = 0; i < 9; i++)
		{
			// 临时存储的每个环状项的子物体
			List<BuildMaterilControl> materilControls = new List<BuildMaterilControl>();
			// 为空则直接跳过
			if (materialIcons[i] == null)
			{
				materialControlList.Add(null);
				continue;
			}
			// 不为空则添加
			for (int j = 0; j < materialIcons[i].Length; j++)
			{
				// 初始化的设置
				if (materialIcons[i][j] != null)
				{
					GameObject material = GameObject.Instantiate(prefab_ItemChild, bg_Transform);
					int offset = 20 + (i - 1) * 40 + j * 14;
					material.GetComponent<BuildMaterilControl>().Init(offset, materialIcons[i][j]);
					materilControls.Add(material.GetComponent<BuildMaterilControl>());
				}
			}
			materialControlList.Add(materilControls);
		}
	}

}
