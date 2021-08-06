using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildPanelControl : MonoBehaviour,IUIPanelShowHide
{

	private BuildPanelView m_View;




	private int currentIndex;                                    // 一级菜单的当前索引
	private int secondCurrentIndex;								 // 二级菜单的当前索引
	private bool isHide;									     // 是否隐藏 圆环的标志
	private bool isLock;                                         // 是否锁定一级菜单的标志
	private bool isSelected;                                     // 是否选中物体
	private bool isDelete;										 // 是否删除模式
	private GameObject currentSelectedObj;                       // 当前选中的物体的引用
	private GameObject currentObj;								 // 要实例化的物体的引用,涉及到了透明着色器所以只能这样了
	private Ray ray;
	private RaycastHit hit;

	private Transform deleteCurrentObj;

	private void OnEnable()
    {
		if(m_View != null)
        {
			m_View.BG_Transform.gameObject.SetActive(true);
		}
		isHide = false;
		isDelete = false;
	}


    private void Start()
    {
		InitOther();			// 初始化


		SetTextName();			// 设置中心名称

		Hide();					// 默认隐藏
	}

	private void Update()
    {
		// 鼠标事件监听
		ListenerMouse();

		// 滚轮事件监听
		MouseScrollControl();

		ray = m_View.EnvCamera.ScreenPointToRay(Input.mousePosition);

		if (isDelete)
        {
			DeleteHelp();
		}
        else
        {
			SetModelPosition();
		}
	}


    /// <summary>
    ///  初始化一些字段和变量
    /// </summary>
    private void InitOther()
    {
		m_View = gameObject.GetComponent<BuildPanelView>();


		currentIndex = 0;
		secondCurrentIndex = 0;
		isLock = false;
		isSelected = false;
		isDelete = false;
	}





	#region  鼠标单机事件监听

	/// <summary>
	///  鼠标事件监听的函数
	///  原理有点类似于环形队列吧
	/// </summary>
	private void ListenerMouse()
    {
		// 打开或者关闭监听事件
		if (Input.GetMouseButtonDown(1))
		{
			ShowOrHide();
		}
		JudgeInitChild();
		HideOrShowWithClick();
	}


	/// <summary>
	///  单机左键的时候应该进行的逻辑判断
	/// </summary>
	private void HideOrShowWithClick()
    {
		if(!isHide && Input.GetMouseButtonDown(0) && currentIndex == 0)
        {
			isDelete = true;
			Hide();
		}
		if (!isHide && Input.GetMouseButtonDown(0) && currentIndex != 0)
		{
			ShowChild();
		}
		if (!isHide && Input.GetMouseButtonDown(0) && isLock)
		{
			if (isSelected)
			{
				Hide();
			}
			else
			{
				isSelected = true;
			}
		}
	}

	/// <summary>
	///  判断是否可以实例化物体的函数
	/// </summary>
	private void JudgeInitChild()
    {
		if (isSelected && Input.GetMouseButtonDown(0) && currentSelectedObj != null && isHide)
		{
			if (currentSelectedObj.GetComponent<BuildingBase>().IsCanPut)
			{
				GameObject temp = GameObject.Instantiate(currentObj, currentSelectedObj.transform.position,
					currentSelectedObj.transform.rotation, m_View.BuildModelsParent);
				if(temp.tag == "Wall") temp.layer = 14;

				temp.name = currentSelectedObj.name;

				temp.gameObject.GetComponent<BuildingBase>().Normal();
				currentSelectedObj.GetComponent<BuildingBase>().DestoryTrigger();
				GameObject.Destroy(temp.gameObject.GetComponent<BuildingBase>());
			}
		}
	}

	#endregion



	#region  鼠标滚轮事件监听

	/// <summary>
	///  鼠标事件监听的总控制函数
	/// </summary>
	private void MouseScrollControl()
    {
		if(!isHide)
        {
			if (!isLock)
			{
				MouseScrollIconListener();
			}
			else
            {
				MouseScrollMaterialListener();
			}
        }
    }






	/// <summary>
	///  鼠标滚轮一级菜单监听事件
	/// </summary>
	private void MouseScrollIconListener()
    {
		// 滚轮监听事件
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				m_View.ItemControlList[currentIndex].HideBG();
				currentIndex = (currentIndex + 1) % (m_View.ItemControlList.Count);

				m_View.ItemControlList[currentIndex].ShowBG();
			}
			else
			{
				m_View.ItemControlList[currentIndex].HideBG();
				currentIndex = (currentIndex - 1) % (m_View.ItemControlList.Count);

				if (currentIndex == -1)
				{
					currentIndex = m_View.ItemControlList.Count - 1;
				}
				m_View.ItemControlList[currentIndex].ShowBG();
			}
			SetTextName();
		}
	}

	/// <summary>
	///  鼠标二级菜单监听事件
	/// </summary>
	private void MouseScrollMaterialListener()
    {
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			int maxIndex = m_View.MaterialControlList[currentIndex].Count;
			if (Input.GetAxis("Mouse ScrollWheel") > 0)
			{
				if (secondCurrentIndex < maxIndex - 1)
                {
					m_View.MaterialControlList[currentIndex][secondCurrentIndex].Normal();
					secondCurrentIndex++;
					ChangeCurrentObj();
					m_View.MaterialControlList[currentIndex][secondCurrentIndex].HighLight();
				}
			}
			else
			{
				if (secondCurrentIndex > 0)
				{
					m_View.MaterialControlList[currentIndex][secondCurrentIndex].Normal();
					secondCurrentIndex--;
					m_View.MaterialControlList[currentIndex][secondCurrentIndex].HighLight();
				}
			}
			SetTextName();
			ChangeCurrentObj();
		}

	}




    #endregion



    #region 辅助函数

    /// <summary>
    ///  更换当前的物体引用
    /// </summary>
    private void ChangeCurrentObj()
    {
		if(currentSelectedObj != null)
        {
			GameObject.Destroy(currentSelectedObj);
			currentObj = null;
		}
		if(m_View.MaterialControlList[currentIndex][secondCurrentIndex].M_Model != null)
		{
			currentObj = m_View.MaterialControlList[currentIndex][secondCurrentIndex].M_Model;
			currentSelectedObj = GameObject.Instantiate(m_View.MaterialControlList[currentIndex][secondCurrentIndex].M_Model,
			m_View.Player_Transform.position + m_View.Player_Transform.forward * 5, Quaternion.identity);
			currentSelectedObj.name = m_View.MaterialControlList[currentIndex][secondCurrentIndex].M_Model.name;
		}

	}


	/// <summary>
	///  设置 UI Text 的内容
	/// </summary>
	private void SetTextName()
	{
		if (isLock)
		{
			m_View.M_ItemName.text = m_View.MaterialsName[currentIndex][secondCurrentIndex];
			// 此时对名称进行初始化
			// 逻辑可能有点绕，根据名称字典查找到对应的名称，然后在另一个脚本中加载资源
			m_View.MaterialControlList[currentIndex][secondCurrentIndex].Name = m_View.MaterialNameDic[m_View.M_ItemName.text];
		}
		else
		{
			m_View.M_ItemName.text = m_View.ItemName[currentIndex];
		}
	}



	/// <summary>
	///  设置当前模型的位置函数
	/// </summary>
	private void SetModelPosition()
    {

		if (Physics.Raycast(ray, out hit,15,~(1 << 13)) && currentSelectedObj != null) 
		{
			if (currentSelectedObj.GetComponent<BuildingBase>() != null && currentSelectedObj.GetComponent<BuildingBase>().IsAttach == false)
			{
				currentSelectedObj.transform.position = hit.point;
			}
			if(Vector3.Distance(hit.point,currentSelectedObj.transform.position) > 2)
            {
				currentSelectedObj.GetComponent<BuildingBase>().IsAttach = false;
			}
		}
    }



	/// <summary>
	///  删除的辅助函数
	/// </summary>
	private void DeleteHelp()
    {
		// 按键点击检测
		if (Input.GetMouseButtonDown(0))
		{
			if(deleteCurrentObj != null)
            {
				GameObject.Destroy(deleteCurrentObj.gameObject);
				deleteCurrentObj = null;
			}
		}
		if (Physics.Raycast(ray, out hit, 15 ,  1 << 13))
		{
			/// 找出检测到的物体
			Transform target = null;
			if(hit.collider.gameObject.GetComponent<Renderer>() == null)
            {
				target = hit.collider.gameObject.transform.parent;
			}
			else
            {
				target = hit.collider.gameObject.transform;
			}

			/// 决定是不是要进行颜色更新
			if(target != deleteCurrentObj && deleteCurrentObj != null)
            {
				deleteCurrentObj.GetComponent<Renderer>().material.color = Color.white;
			}
			deleteCurrentObj = target;
			if(deleteCurrentObj != null)
            {
				deleteCurrentObj.GetComponent<Renderer>().material.color = Color.red;
			}
		}
	}



	#endregion



	#region 隐藏或者显示的辅助函数

	/// <summary>
	///  显示还是隐藏的开关
	/// </summary>
	private void ShowOrHide()
	{
		if (isHide)
		{
			Show();
		}
		else
		{
			// 如果是锁定状态则解锁
			if (isLock)
			{
				HideChild();
			}
			else
			{
				Hide();
			}
		}
	}





	/// <summary>
	///  显示面板
	/// </summary>
	public void Show()
	{
		if(deleteCurrentObj != null)
        {
			deleteCurrentObj.GetComponent<MeshRenderer>().material.color = Color.white;
			deleteCurrentObj = null;
		}
		m_View.BG_Transform.gameObject.SetActive(true);
		isHide = false;
		isSelected = false;
		isDelete = false;
	}
	/// <summary>
	///  隐藏面板
	/// </summary>
	public void Hide()
	{

		m_View.BG_Transform.gameObject.SetActive(false);
		isHide = true;
	}




	/// <summary>
	///  显示二级菜单,同时设置属性
	/// </summary>
	private void ShowChild()
	{
		isLock = true;
		m_View.MaterialControlList[currentIndex][secondCurrentIndex].HighLight();
		for (int i = 0; i < m_View.MaterialControlList[currentIndex].Count; i++)
		{
			m_View.MaterialControlList[currentIndex][i].Show();
		}
		SetTextName();
		ChangeCurrentObj();
	}

	/// <summary>
	///  隐藏二级菜单，同时还原属性
	/// </summary>
	private void HideChild()
	{
		isLock = false;
		m_View.MaterialControlList[currentIndex][secondCurrentIndex].Normal();
		secondCurrentIndex = 0;      // 还原二级索引
		for (int i = 0; i < m_View.MaterialControlList[currentIndex].Count; i++)
		{
			m_View.MaterialControlList[currentIndex][i].Hide();
		}
		if (currentSelectedObj != null)
		{
			GameObject.Destroy(currentSelectedObj);
		}
		SetTextName();
	}


	public void ResetBuild()
    {
		Hide();

		GameObject.Destroy(currentSelectedObj);
	}

	#endregion
}
