using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class AssaultRifle : HotGunControllerBase
{

	private AssaultRifleView m_View;
	 
	/// <summary>
	/// 初始化其他的东西
	/// </summary>
	protected override void InitOther()
    {
		base.InitOther();

		GunWeaponType = GunType.AssaultRifle;
		m_View = (AssaultRifleView)M_BaseView;

	}





	/// <summary>
	/// 射击辅助函数.
	/// </summary>
	/// <returns></returns>
	private IEnumerator ShootHelp()
	{
		if (Durable > 0)
		{
			PlayAudio();
			PlayEffect();

			GameObject temp = GameObject.Instantiate(m_View.Prefab_Bullet,m_View.ShootPos.position,Quaternion.identity,m_View.Bullet_Parents);
			BulletHelp tempBulletHelp = temp.GetComponent<BulletHelp>();
			if(tempBulletHelp == null)
            {
				tempBulletHelp = temp.AddComponent<BulletHelp>();
			}
			tempBulletHelp.Shoot(m_View.ShootPos.forward,3000,Damage);
			Durable--;


			yield return new WaitForSeconds(0.1f);

			CanShoot = true;
		}

	}

	/// <summary>
	/// 射击.
	/// </summary>
	protected override void Shoot()
    {
		// 射击状态中且不在延迟时间内
		if (Input.GetMouseButton(0) && CanShoot)
		{
			CanShoot = false;
			StartCoroutine("ShootHelp");
			m_View.M_Animator.SetBool("Fire", true);
		}
		if (Input.GetMouseButtonUp(0))
		{
			m_View.M_Animator.SetBool("Fire", false);
			CanShoot = true;
			StopCoroutine("ShootHelp");
		}
	}





}
