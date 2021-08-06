using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ToolBarPanelController : MonoBehaviour, IUIPanelShowHide
{

	public static ToolBarPanelController Instance;		 // 实现单例
	private ToolBarPanelModel m_Model;					 // M层访问器
	private ToolBarPanelView m_View;                     // V层访问器


	private GameObject currWeapon;						 // 用来存储当前武器模型
	private List<GameObject> slotItems;					 // 用来管理每个工具槽
	private Dictionary<GameObject, GameObject>  gunDic;  // 用来存储当前拥有的枪械物体

	private int index;									 // 当前的选中槽的索引，索引从一开始，所以每次使用要减一！
	private bool isHide;								 // 判断是否隐藏的函数


	public bool IsHide {get { return isHide; }}
	public GameObject CurrWeapon { get { return currWeapon; } }


	private void Awake()
    {
		Instance = this;
	}

	private void Start()
    {
		InitOthers();
		InitSlot();

	}


	/// <summary>
	/// 初始化乱七八糟的东西的函数
	/// </summary>
	private void InitOthers()
    {
		m_Model = gameObject.GetComponent<ToolBarPanelModel>();
		m_View = gameObject.GetComponent<ToolBarPanelView>();


		currWeapon = null;
		slotItems = new List<GameObject>();
		gunDic = new Dictionary<GameObject, GameObject>();
		index = -1;

		isHide = false;
	}


	/// <summary>
	/// 初始化生成八个工具槽
	/// </summary>
	private void InitSlot()
    {
		for(int i = 0; i < 8; i++)
        {
			GameObject tempObj = GameObject.Instantiate(m_View.Prefab_ToolBarSlot, m_View.Grid_Transform);
			tempObj.name = m_View.Prefab_ToolBarSlot.name + i;
			tempObj.GetComponent<ToolBarSlotController>().InitSlot((i + 1).ToString());
			slotItems.Add(tempObj);
        }
    }


	/// <summary>
	/// 外部调用改变选中状态 
	/// </summary>
	/// <param name="index">改变都工具槽索引【1 ~ 8】</param>
	public void ChangeSelectSlot(int index)
    {
		if(this.index != -1)
        {
			ToolBarSlotController temp = slotItems[this.index - 1].GetComponent<ToolBarSlotController>();
			if (this.index == index)
			{
				temp.Selected = !temp.Selected;
				this.index = -1;
			}
			else
			{
				temp.Selected = false;
				temp = slotItems[index - 1].GetComponent<ToolBarSlotController>();
				temp.Selected = true;
				this.index = index;
			}
		}
        else
        {
			ToolBarSlotController temp = slotItems[index - 1].GetComponent<ToolBarSlotController>();
			temp.Selected = true;
			this.index = index;
		}
		StartCoroutine(CallGunFactory());
	}





	/// <summary>
	///  调用枪械函数
	/// </summary>
	private IEnumerator CallGunFactory()
    {
		float waitTime = 1.0f;
		// 确定索引没有越界的函数
		if(index >= 1 && index <= slotItems.Count)
        {
			// 使用索引的判断该槽中是否有可实例化的物体
			Transform objTransform = slotItems[index - 1].transform.Find("InventoryItem");
			if (objTransform != null)
			{

				// 判断字典中是否存在该物体
				GameObject temp = null;
				gunDic.TryGetValue(objTransform.gameObject, out temp);
				
				// 不存在则实例化出一个新物体
				if (temp == null)
                {
					gunDic.Remove(objTransform.gameObject);
					string str = objTransform.GetComponent<Image>().sprite.name;
					temp = GunFactory.Instance.CreateGun(str, objTransform.GetComponent<InventoryItemController>());
					gunDic.Add(objTransform.gameObject, temp);
				}


				// 当前武器不为空时将其隐藏
				if (currWeapon != null )
				{
					// 动画过滤  如果是建造就不执行该动画方法
					if(currWeapon.tag != "BuildPlan")
                    {
						currWeapon.GetComponent<GunControlBase>().Holster();
						yield return new WaitForSeconds(waitTime);
					}
					currWeapon.SetActive(false);
				}
				if(temp != null)
                {
					currWeapon = temp;
					currWeapon.SetActive(true);
				}
			}
			else if (currWeapon != null)
			{
				if (currWeapon.tag != "BuildPlan")
				{
					currWeapon.GetComponent<GunControlBase>().Holster();
					yield return new WaitForSeconds(waitTime);
				}
				currWeapon.SetActive(false);
            }
			// 都为空时则置为空
            else
            {
				currWeapon = null;
            }
		}
		// 由于是使用的索引控制，所以如果索引越界，说明是隐藏武器的算法
        else
        {
			if (currWeapon != null)
			{
				if (currWeapon.tag != "BuildPlan")
				{
					currWeapon.GetComponent<GunControlBase>().Holster();
					yield return new WaitForSeconds(waitTime);
				}
				currWeapon.SetActive(false);
			}
		}
    }



    #region 接口方法
    /// <summary>
    /// 接口的显示方法
    /// </summary>
    public void Show()
    {
		// 按其他的键也可能调用因此加一个判断
		if(isHide)
        {
			isHide = false;
			m_View.M_Transform.position = m_View.M_Transform.position + new Vector3(0, 0.1f, 0);
		}
	}

	/// <summary>
	/// 接口的隐藏方法
	/// </summary>
    public void Hide()
    {

		isHide = true;
		m_View.M_Transform.position = m_View.M_Transform.position + new Vector3(0, -0.1f, 0);
	}


	#endregion
}
