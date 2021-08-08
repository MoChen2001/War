using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public class InputManager : MonoBehaviour 
{

	public static InputManager Instance;



	private bool inventoryControl;                  // 控制背包显示还是隐藏
	private bool buildPanelControl;


	private GameObject m_BuildPanelControl;			// 建造控制字段
	private FirstPersonController firstPerson;      // 第一人称控制器字段
	private Image gunStar;							// 第一人称的瞄准物体



	public bool InventoryControl { get { return inventoryControl; } }


	/// <summary>
	///  设置属性时改变状态
	/// </summary>
	public bool BuildPanelControl 
	{
		set
		{
			buildPanelControl = value;
			if(gunStar != null)
            {
				gunStar.enabled = !buildPanelControl;
			}
			if(m_BuildPanelControl != null)
            {
				m_BuildPanelControl.SetActive(buildPanelControl);
			}
		}
	}

	void Awake()
    {
		Instance = this;
	}


	void Start () 
	{
		// 这样背包就会默认为 隐藏状态
		inventoryControl = false;
		buildPanelControl = false;
		m_BuildPanelControl = GameObject.Find("CanvasCamera/BuildPanel");

		m_BuildPanelControl.SetActive(false);
		InventoryPanelController.Instance.Hide();
		GunInit();
	}
	
	void Update () 
	{
		//ChangeToolState();
		ChangeInventoryState();
		ToolBarKeyAllLisitener();
	}


    #region 背包枪械切换函数
    /// <summary>
    ///  初始化枪械字段的函数
    /// </summary>
    private void GunInit()
    {
		GameObject obj = GameObject.Find("FPSController");
		firstPerson = obj.GetComponent<FirstPersonController>();
		gunStar = GameObject.Find("Canvas/GunStar").GetComponent<Image>();

	}


	/// <summary>
	///  切换到背包状态
	/// </summary>
	private void InventoryState()
    {
		inventoryControl = true;                                            // 背包显示状态切换
		ToolBarPanelController.Instance.Show();                             // 调用背包显示函数
		InventoryPanelController.Instance.Show();                           // 背包显示的时候同时显示工具栏
		gunStar.enabled = false;                                            // 隐藏瞄准点物体 
		if (ToolBarPanelController.Instance.CurrWeapon != null)
		{
			if (ToolBarPanelController.Instance.CurrWeapon.GetComponent<GunControlBase>() != null)
			{
				// 第一人称枪械控制器可用
				ToolBarPanelController.Instance.CurrWeapon.GetComponent<GunControlBase>().enabled = false;
			}
		}
		firstPerson.enabled = false;                                        // 第一人称控制器不可用
		Cursor.lockState = CursorLockMode.None;                             // 鼠标解锁
		Cursor.visible = true;                                              // 鼠标可见					
	}

	/// <summary>
	///  切换到第一人称状态
	/// </summary>
	private void FirstPersonState()
    {
		inventoryControl = false;												// 切换到背包隐藏状态
		InventoryPanelController.Instance.Hide();                               // 调用背包隐藏函数
		if (ToolBarPanelController.Instance.CurrWeapon != null)
        {
			if(ToolBarPanelController.Instance.CurrWeapon.GetComponent<GunControlBase>() != null)
            {
				// 第一人称枪械控制器可用
				ToolBarPanelController.Instance.CurrWeapon.GetComponent<GunControlBase>().enabled = true;
			}
		}
		firstPerson.enabled = true;												// 第一人称控制器可用
		Cursor.lockState = CursorLockMode.Locked;								// 鼠标锁定
		Cursor.visible = false;													// 鼠标不可见
		gunStar.enabled = true;													// 设置瞄准点可见
	}

	/// <summary>
	/// 改变背包显示状态的函数
	/// </summary>
	private void ChangeInventoryState()
    {
		if(!buildPanelControl)
        {
			if (Input.GetKeyDown(GameConst.InventoryPanelKey))
			{
				if (inventoryControl)
				{
					FirstPersonState();
				}
				else
				{
					InventoryState();
				}
			}
		}
	}
    #endregion


	#region 工具栏按键检测函数

	/// <summary>
	/// 检测所有的工具栏按键
	/// </summary>
	private void ToolBarKeyAllLisitener()
    {
		for(int i = 0; i < 8; i++)
        {
			ToolBarKeyListener(GameConst.ToolBarPanelKeys[i]);
		}
		
	}
	/// <summary>
	/// 检测单个的工具栏按键
	/// </summary>
	private void ToolBarKeyListener(KeyCode key)
    {
		if(Input.GetKeyDown(key) && inventoryControl == false)
        {
			if(buildPanelControl)
			{
				m_BuildPanelControl.GetComponent<BuildPanelControl>().ResetBuild();
			}
			ToolBarPanelController.Instance.Show();
			int index = int.Parse(key.ToString().Substring(5));
			ToolBarPanelController.Instance.ChangeSelectSlot(index);
        }
    }

    #endregion






}
