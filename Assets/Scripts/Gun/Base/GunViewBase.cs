using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 枪械 view 层父类
/// </summary>
public abstract class GunViewBase : MonoBehaviour
{


    #region 字段
    // 会用到的组件字段
    private Transform m_Transform;      // Transform 组件
	private Animator m_Animator;        // 动作控制组件
	private Camera m_Camera;            // 环境摄像机
	private Transform bullets_Parent;   // 子弹父物体

	// 会使用到的位置数据
	private Vector3 normalPos;          // 普通状态的位置
	private Vector3 normalRotate;       // 普通状态的旋转
	private Vector3 holdPos;            // 开镜状态的位置
	private Vector3 holdRotate;         // 开镜状态的旋转


	// 位置
	private Transform shootPos;         // 枪口射击位置
	private RectTransform gunStar;      // 准星的位置


	// 预制体
	private AudioClip m_Audio;          // 枪械音效
	private GameObject m_Effect;        // 枪械特效

	#endregion


	#region 属性
	// 会用到的组件属性
	public Transform M_Transform { get { return m_Transform; } }
	public Animator M_Animator { get { return m_Animator; } }
	public Camera Env_Camera { get { return m_Camera; } }
	public Transform Bullet_Parents { get { return bullets_Parent; } }
	public Vector3 NormalPos { get{ return normalPos; } protected set { normalPos = value; } }
	public Vector3 NormalRotate { get { return normalRotate; } protected set { normalRotate = value; }  }
	public Vector3 HoldPos { get { return holdPos; } protected set { holdPos = value; } }
	public Vector3 HoldRotate { get { return holdRotate; } protected set { holdRotate = value; } }



	/// <summary>
	/// 设计点的属性
	/// </summary>
	public Transform ShootPos { get { return shootPos; }  set { shootPos = value; } }
	/// <summary>
	/// 准星的属性
	/// </summary>
	public RectTransform GunStar { get { return gunStar; } set { gunStar = value; } }
	/// <summary>
	/// 射击音效资源
	/// </summary>
	public AudioClip M_Audio { get { return m_Audio; } set { m_Audio = value; } }
	/// <summary>
	/// 射击特效资源
	/// </summary>
	public GameObject M_Effect { get { return m_Effect; } set { m_Effect = value; } }

	#endregion

	private void Awake()
    {
		// 组件加载
		m_Transform = gameObject.GetComponent<Transform>();
		m_Animator = m_Transform.GetComponent<Animator>();
		m_Camera = GameObject.Find("FPSController/PersonCamera/EnvCamera").GetComponent<Camera>();
		gunStar = GameObject.Find("GunStar").GetComponent<RectTransform>();
		bullets_Parent = GameObject.Find("BulletParents").GetComponent<Transform>();


		InitHoldValue();                    // 初始化四个状态值
		InitShootPos();						// 初始化枪口和准星
		LoadAudioAndEffect();               // 初始化音效和特效
		InitOther();                        // 初始化其他杂七杂八的数据
	}




	#region 常规方法
	/// <summary>
	///  普通状态
	/// </summary>
	public virtual void NormalState(float time = 0.2f ,int FOV = 60)
	{
		M_Transform.DOLocalMove(NormalPos, time);
		M_Transform.localRotation = Quaternion.Euler(NormalRotate);
		Env_Camera.DOFieldOfView(FOV, time);
	}

	/// <summary>
	///  开镜状态.
	/// </summary>
	public virtual void HoldState(float time = 0.2f, int FOV = 45)
	{
		M_Transform.DOLocalMove(HoldPos, time);
		M_Transform.localRotation = Quaternion.Euler(HoldRotate);
		Env_Camera.DOFieldOfView(FOV, time);
	}

	#endregion



	#region 子类应该重载的虚方法
	/// <summary>
	/// 初始化设计的位置和准星
	/// </summary>
	protected abstract void InitShootPos();

	/// <summary>
	///  初始化开镜动作的四个值的函数
	/// </summary>
	protected abstract void InitHoldValue();

	/// <summary>
	/// 初始化音效和特效
	/// </summary>
	protected abstract void LoadAudioAndEffect();

	/// <summary>
	/// 初始化其他物品的方法
	/// </summary>
	protected abstract void InitOther();

	#endregion
}
