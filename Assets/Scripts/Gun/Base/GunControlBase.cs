using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class GunControlBase : MonoBehaviour 
{
	private Ray ray;											 // 射线检测的射线
	public RaycastHit hit;										 // 射线检测辅助字段
	private GunViewBase m_BaseView;								 // 组件字段

	[SerializeField] private int id;                             // 枪械 ID
	[SerializeField] private int damage;                         // 枪械的伤害
	[SerializeField] private int durable;                        // 枪械的耐久值
	private int signaleDurable;									 // 单个枪械的耐久值
	private float maxDurage;                                     // 最大的耐久度
	[SerializeField] private GunType gunWeaponType;              // 枪械类型
	private InventoryItemController iic;						 // 枪械对应的 UI 的控制器

	private bool canShoot;                                       // 判断是否可以射击的字段


    #region 属性


	public int Id { get { return id; }  set { id = value; } }

	public int Damage { get { return damage; }  set { damage = value; } }

	public int Durable
	{
		get { return durable; }
		set
		{
			durable = value;
			float currFill = durable / maxDurage;
			IIc.BarImage.fillAmount = currFill;
			IIc.BarImage.color = Color.Lerp(Color.red, Color.green, currFill);
			if (durable <= 0)
			{
				GameObject.Destroy(gameObject);
				GameObject.Destroy(iic.gameObject);
			}
		}
	}

	public int SignaleDurable { set { signaleDurable = value; } }

	public GunType GunWeaponType { get { return gunWeaponType; } protected set { gunWeaponType = value; } }

	public InventoryItemController IIc { get { return iic; } 
		set 
		{
			iic = value;
			iic.addNumer += UpdateMaxDurage;
		}
	}

	public float MaxDurage 
	{
		set { maxDurage = value; }
	}
	/// <summary>
	/// 判断是否可以射击的字段
	/// </summary>
	public bool CanShoot { get { return canShoot; }  set { canShoot = value; } }
	/// <summary>
	///  view 基类层的属性
	/// </summary>
	public GunViewBase M_BaseView { get { return m_BaseView; } protected set { m_BaseView = value; } }

	#endregion





	private void Start()
    {
		m_BaseView = gameObject.GetComponent<GunViewBase>();
		canShoot = true;
		InitOther();
	}


	private void Update()
    {
		StarHelp();
		MouseControl();
	}





	/// <summary>
	///  更新 UI 的委托函数 
	/// </summary>
	private void UpdateMaxDurage(int addValue)
    {
		maxDurage = iic.Number * signaleDurable;
		Durable += addValue * signaleDurable;
	}




	#region 子函数需要重写的纯虚函数
	/// <summary>
	/// 初始化杂七杂八的数据
	/// </summary>
	protected abstract void InitOther();

	/// <summary>
	///  射击虚函数
	/// </summary>
	protected abstract void Shoot();
	#endregion


	#region  父类中不可改变的函数
	/// <summary>
	///  播放声音的函数
	/// </summary>
	protected void PlayAudio()
    {
		AudioSource.PlayClipAtPoint(m_BaseView.M_Audio, m_BaseView.ShootPos.position);
	}

	/// <summary>
	/// 准星辅助函数
	/// </summary>
	protected void StarHelp()
	{
		ray = new Ray(m_BaseView.ShootPos.position, m_BaseView.ShootPos.forward);
		if (Physics.Raycast(ray, out hit, 1000, 1 << 11) && CanShoot)
		{
			m_BaseView.GunStar.position = RectTransformUtility.WorldToScreenPoint(m_BaseView.Env_Camera, hit.point);
		}
		else
		{
			hit.point = Vector3.zero;
		}
	}


	/// <summary>
	///  开镜控制
	/// </summary>
	protected abstract void HoldControl();

	/// <summary>
	/// 鼠标控制.
	/// </summary>
	private void MouseControl()
	{
		// 射击控制
		Shoot();
		// 开镜控制
		HoldControl();
	}

	/// <summary>
	///  放下枪械的动画
	/// </summary>
	public void Holster()
    {
		M_BaseView.M_Animator.SetTrigger("Holster");
    }

    #endregion



}
