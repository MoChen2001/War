using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HotGunControllerBase : GunControlBase
{


	/// <summary>
	/// 对应的 view 层访问器
	/// </summary>
	private HotGunViewBase m_HotView;


    protected override void InitOther()
    {

		m_HotView = (HotGunViewBase)M_BaseView;
	}


    #region 常规方法

    /// <summary>
    /// 特效函数
    /// </summary>
    protected void PlayEffect()
	{
		PlayShellEffect();
		PlayFireEffect();
	}

	/// <summary>
	/// 枪火特效的播放
	/// 同时还有对象池的使用
	/// </summary>
	protected void PlayFireEffect()
	{
		// 枪口喷火的特效
		GameObject fireTemp = null;
		if (m_HotView.Pools[0].PoolIsEmpty())
		{
			fireTemp = GameObject.Instantiate(m_HotView.M_Effect, m_HotView.ShootPos.position, Quaternion.identity, m_HotView.Fire_Effects);
			fireTemp.name = "FireEffect";
		}
		else
		{
			fireTemp = m_HotView.Pools[0].GetObject();
			// 重置位置参数
			fireTemp.transform.position = m_HotView.ShootPos.position;
		}
		fireTemp.GetComponent<ParticleSystem>().Play();
		m_HotView.Pools[0].AddObject(fireTemp, 1.0f);
	}


	/// <summary>
	/// 弹壳弹出的播放
	/// 同时还有对象池的使用
	/// </summary>
	protected void PlayShellEffect()
	{
		GameObject shell_temp = null;
		Vector3 vec = (m_HotView.ShellPos.up + m_HotView.ShellPos.right) / 2;
		if (m_HotView.Pools[1].PoolIsEmpty())
		{
			// 子弹壳弹出的特效
			shell_temp = GameObject.Instantiate(m_HotView.Prefab_Shell, m_HotView.ShellPos.position,
				Quaternion.Euler(90,90,90), m_HotView.Shell_Effects);
		}
		else
		{
			shell_temp = m_HotView.Pools[1].GetObject();
			shell_temp.GetComponent<Rigidbody>().isKinematic = true;
			shell_temp.transform.position = m_HotView.ShellPos.position;
			shell_temp.GetComponent<Rigidbody>().isKinematic = false;
		}

		shell_temp.GetComponent<Rigidbody>().AddForce(vec * 2.0f, ForceMode.Impulse);
		m_HotView.Pools[1].AddObject(shell_temp, 1.5f);
	}

	/// <summary>
	/// 枪械的射击控制
	/// </summary>
	protected override void HoldControl()
	{
		if (Input.GetMouseButton(1))
		{
			m_HotView.M_Animator.SetBool("HoldPose", true);
			m_HotView.HoldState();
			// 隐藏瞄准UI
			m_HotView.GunStar.sizeDelta = new Vector2(0, 0);
		}
		if (Input.GetMouseButtonUp(1))
		{
			m_HotView.M_Animator.SetBool("HoldPose", false);
			m_HotView.NormalState();
			// 隐藏瞄准UI
			m_HotView.GunStar.sizeDelta = new Vector2(256, 256);
		}
	}


	#endregion
}
