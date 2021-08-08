using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoneHatchetControl : MonoBehaviour
{
	private Ray ray;                                             // 射线检测的射线
	public RaycastHit hit;                                       // 射线检测辅助字段
	private Animator m_Animator;
	private Transform m_Transform;
	private Camera envCamera;


	[SerializeField] private int id;                             //  ID
	[SerializeField] private int damage;                         // 伤害
	[SerializeField] private int durable;                        // 耐久值
	private int signaleDurable;                                  // 单个的耐久值
	private float maxDurage;                                     // 最大的耐久度
	private InventoryItemController iic;                         // 枪械对应的 UI 的控制器
	private bool canHit;										 // 可以击打的对应字段


	#region 属性


	public int Id { get { return id; } set { id = value; } }

	public int Damage { get { return damage; } set { damage = value; } }

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

	public InventoryItemController IIc
	{
		get { return iic; }
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



	#endregion





	private void Start()
	{
		canHit = true;

		m_Animator = gameObject.GetComponent<Animator>();
		m_Transform = gameObject.GetComponent<Transform>();
		envCamera = GameObject.Find("FPSController/PersonCamera/EnvCamera").GetComponent<Camera>();
	}


	private void Update()
	{
		if(Input.GetMouseButtonDown(0) && canHit)
        {
			HitEnv();
		}
		canHit = !InputManager.Instance.InventoryControl;
	}


	/// <summary>
	///  击打的函数
	/// </summary>
	private void HitEnv()
    {
		canHit = false;
		m_Animator.SetTrigger("Hit");
		StartCoroutine("ChangeHitState");
	}



	private IEnumerator ChangeHitState()
    {
		yield return new WaitForSeconds(0.8f);
		ray = envCamera.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, 2.0f))
		{
			BulletMark bm = hit.collider.gameObject.GetComponent<BulletMark>();
			if (bm != null)
			{
				Durable--;
				bm.HP -= Damage;
				bm.PlayEffectsWithAxe(hit);
			}
		}
		yield return new WaitForSeconds(1.0f);
		canHit = true;
    }

	/// <summary>
	///  放下的函数
	/// </summary>
	public void Holster()
    {
		m_Animator.SetTrigger("Holster");
    }


	/// <summary>
	///  更新 UI 的委托函数 
	/// </summary>
	private void UpdateMaxDurage(int addValue)
	{
		maxDurage = iic.Number * signaleDurable;
		Durable += addValue * signaleDurable;
	}

}
