using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 单个背包物体的控制类，负责实例化等等
/// </summary>
public class InventoryItemController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{


	private RectTransform m_RectTransform;
	private Image m_Image;          // 物品的 Image 图片
	private Image barImage;			// 物品耐久度的图片
	private Text m_Text;            // 存放数量的 UI

	private Transform parent;		// 当前的父物体
	private Transform preParent;	// 之前的父物体

	private int itemId;	            // 这个物体的 ID 值，用于和合成槽中的对应
	private bool isDragging;        // 用来确定是否在拖拽中
	private bool isInventory;       // 用来确定是否在背包中
	private int itemBar;            // 用来确定物品是否持有血条， 0 则不需要， 1 则需要；


	public delegate void AddNumber(int num);

	public AddNumber addNumer;
	public bool IsInventory
    {
        get { return isInventory; }
        set
        {
			isInventory = value;
			m_RectTransform.localPosition = Vector3.zero;
			// 在背包内
			if (value)
            {
				ResizeImage(m_RectTransform, 80, 80);
			}
			// 不在背包内
            else
            {
				ResizeImage(m_RectTransform, 70, 70);
			}
		}
    }
	public int Number { 
		get 
		{
			return int.Parse(m_Text.text); 
		}
        set
        {
			int addValue = value - int.Parse(m_Text.text);
			m_Text.text = value.ToString();

			// 如果数量更新且委托不为空，则执行委托更新 UI
			if(addNumer!= null)
            {
				addNumer(addValue);
			}
		}
	}
	public int ItemID { get { return itemId; } }
	public Image BarImage { get { return barImage; } }


	void Awake () 
	{
		m_RectTransform = gameObject.GetComponent<RectTransform>();
		m_Image = m_RectTransform.GetComponent<Image>();
		barImage = m_RectTransform.Find("InventoryItemBar").GetComponent<Image>();
		m_Text = m_RectTransform.Find("InventoryItemNumText").GetComponent<Text>();

		parent = GameObject.Find("CanvasCamera").GetComponent<Transform>();


		itemId = -1;
		isDragging = false;
		isInventory = true;


	}


	private void Update()
    {
		// 拖拽中且单击鼠标右键才可以拆分
		if(isDragging && Input.GetMouseButtonDown(1))
        {
			BreakMaterial();
		}
    }



    #region 初始化函数

    /// <summary>
    /// 初始化单个的背包物体
    /// </summary>
    /// <param name="name">背包物体要加载的名称</param>
    /// <param name="info">背包物体要赋值的个数</param>
    public void InitSingleItem(int itemId ,Sprite name,int info,int itemBar)
    {
		this.itemId = itemId;
		m_Image.sprite = name;
		Number = info;
		this.itemBar = itemBar;
		 
		InitBarOrText();

	}

	/// <summary>
	///  决定该物体显示血条还是显示图片
	/// </summary>
	private void InitBarOrText()
    {
		if (itemBar == 0)
		{
			barImage.enabled = false;
			m_Text.enabled = true;
		}
		else
		{
			barImage.enabled = true;
			m_Text.enabled = false;
		}
	}


	#endregion



	#region 拖拽事件及其辅助函数

	/// <summary>
	/// 开始拖拽事件
	/// </summary>
	/// <param name="eventData"></param>
	void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {
		// 防止鼠标左右键同时按下导致的 preParent 变化问题
		if(!isDragging)
        {
			isDragging = true;
			// 说明从合成槽中拿出的数据
			if (m_RectTransform.parent.tag == "CraftingSlot")
            {
				InventoryPanelController.Instance.SendRemoveSlotItem(gameObject);
            }
			preParent = m_RectTransform.parent;
			m_RectTransform.SetParent(parent);
			m_RectTransform.GetComponent<CanvasGroup>().blocksRaycasts = false;
		}
	}


	/// <summary>
	/// 拖拽中事件
	/// </summary>
	/// <param name="eventData"></param>
	void IDragHandler.OnDrag(PointerEventData eventData)
	{
		Vector3 pos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(m_RectTransform, eventData.position, eventData.enterEventCamera, out pos);
		m_RectTransform.position = pos;
	}


    /// <summary>
    /// 拖拽结束事件
    /// </summary>
    /// <param name="eventData"></param>
    void IEndDragHandler.OnEndDrag(PointerEventData eventData)
    {
		GameObject back = eventData.pointerEnter;

		if (back != null)
        {
			BackNotNullJudge(back);
		}
		// 为UI外部的空
		else
		{
			JudgeMergeOrNot();
		}
		ResetState();
	}





    /// <summary>
    /// 当背后的UI不为空的时候调用的函数
    /// </summary>
    private void BackNotNullJudge(GameObject back)
    {
		// 为背包的空槽
		if (back.tag == "InventorySlot")
		{
			m_RectTransform.SetParent(back.transform);
			IsInventory = true;
		}
		// 为合成物品的空槽
		else if (back.tag == "CraftingSlot" &&
			back.GetComponent<CraftingSlotController>().ItemId == itemId
			&& back.GetComponentInChildren<InventoryItemController>() == null)
		{
			InventoryPanelController.Instance.SendAddSlotItem(gameObject,true);
			m_RectTransform.SetParent(back.transform);
			IsInventory = false;
		}
		// 为非空槽（不管是背包还是合成槽）
		else if (back.tag == "InventoryItem")
		{
			InventoryItemController backTempController = back.GetComponent<InventoryItemController>();
			// 如果 ID 相等直接执行合并代码
			if (backTempController.itemId == itemId)
			{
				MergeMaterial(backTempController);
				if(!backTempController.isInventory)
                {
					InventoryPanelController.Instance.SendAddSlotItem(null, false);
                }
			}
			// 如果都在背包里面就执行交换代码
			else if (backTempController.IsInventory && IsInventory)
			{
				m_RectTransform.SetParent(backTempController.gameObject.transform.parent);
				backTempController.gameObject.transform.SetParent(preParent);
				backTempController.gameObject.transform.localPosition = Vector3.zero;
			}
			// 直接就返回之前的状态
			else
			{
				JudgeMergeOrNot();
			}
		}
		// 为UI界面的无用槽
		else
		{
			JudgeMergeOrNot();
		}
	}

	// 拖拽结束后的重置函数
	public void ResetState()
    {
		isDragging = false;
		m_RectTransform.localPosition = Vector3.zero;
		m_RectTransform.GetComponent<CanvasGroup>().blocksRaycasts = true;
	}


	// 如果返回父物体的时候需要合并则合并，否则直接设置
	private void JudgeMergeOrNot()
    {
		if (preParent.GetComponentInChildren<InventoryItemController>() == null)
		{
			ReturnOldParent();
		}
		else
		{
			MergeMaterial(preParent.GetComponentInChildren<InventoryItemController>());
		}
	}

	#endregion



	#region 辅助函数

	/// <summary>
	/// 将父物体仍然设置为原来的父物体
	/// </summary>
	private void ReturnOldParent()
    {
		m_RectTransform.SetParent(preParent);
		if(preParent.tag == "CraftingSlot")
        {
			InventoryPanelController.Instance.SendAddSlotItem(gameObject, true);
		}

	}

	// 调整图片的大小
	public void ResizeImage(RectTransform tempTransform,float width,float height)
    {
		tempTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
		tempTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
	}




    #endregion



    #region 拆分辅助函数

    // 拆分材料
    private void BreakMaterial()
    {

		// 射线检测相关的
		RaycastHit rayhit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		Physics.Raycast(ray, out rayhit);
		// 是不是检测到物体,并且这个物体的标签是合成槽
		if(rayhit.transform != null && rayhit.transform.tag == "CraftingSlot")
        {
			GameObject target = rayhit.transform.gameObject;
			
			// 判断是不是同类型的合成槽
			if(target.GetComponent<CraftingSlotController>().ItemId == itemId)
            {
				InventoryItemController childController = target.GetComponentInChildren<InventoryItemController>();
				// 如果子物体带有这个，说明合成槽不是空的，则执行else 语句
				// 否则复制一份出来进行初始化作为合成槽新物品
				if(childController == null)
                {
					GameObject item = GameObject.Instantiate(gameObject, target.transform);
					item.name = gameObject.name;
					childController = item.GetComponent<InventoryItemController>();
					item.transform.localPosition = Vector3.zero;
					ResizeImage(item.GetComponent<RectTransform>(), 70, 70);
					childController.isDragging = false;
					childController.isInventory = false;
					childController.InitSingleItem(itemId, m_Image.sprite, 1,itemBar);
					item.GetComponent<CanvasGroup>().blocksRaycasts = true;
					MinusNum();
					// 更新合成槽管理的 List
					InventoryPanelController.Instance.SendAddSlotItem(item, true);
				}
                else
                {
					if (MinusNum())
                    {
						childController.Number = int.Parse(childController.m_Text.text) + 1;
						childController.isInventory = false;
					}
					// 更新合成槽管理的 List
					InventoryPanelController.Instance.SendAddSlotItem(null, false);
				}

			}
		}

	}

	// 拆分时的辅助函数，用来减少数量
	private bool MinusNum()
    {
		int num = Number;
		if (num >= 1)
        {
			m_Text.text = (num - 1).ToString();
			if (num - 1 == 0)
			{
				GameObject.Destroy(gameObject);
			}
			return true;
		}
        else
        {
			return false;
        }
	}

	// 较少 number 个数的材料
	public bool MinusNum(int number)
    {
		int num = Number;
		if (num >= number)
		{
			Number = num - number;
			if (num - number == 0)
			{
				GameObject.Destroy(gameObject);
			}
			return true;
		}
		else
		{
			return false;
		}
	}

    #endregion



    #region 合并材料函数

    // 合并材料代码
    private void MergeMaterial(InventoryItemController target)
    {


		// 防止 ID 不同或者自身赋值而触发丢失材料的现象
		if (target.itemId == itemId && target.transform != gameObject.transform)
        {
			int num = target.Number;
			int targetNum = num + Number;
			target.Number = targetNum;
			if (preParent.tag == "CraftingSlot")
			{
				InventoryPanelController.Instance.SendAddSlotItem(null, false);
			}
			GameObject.Destroy(gameObject);
		}
        else
        {
			ReturnOldParent();

		}
	}

	/// <summary>
	/// 改变该物体的数量
	/// </summary>
	/// <param name="num">要增加的数量</param>
	public void ChangeNum(int num)
    {
		int currNum = Number;
		if(currNum + num < 0)
        {
			currNum = 0;
			GameObject.Destroy(gameObject);
			return;
        }
		Number = currNum + num;

    }

	#endregion



}
