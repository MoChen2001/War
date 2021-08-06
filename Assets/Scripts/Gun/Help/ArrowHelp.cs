using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHelp : BulletHelpBase
{

	private BoxCollider m_BoxCollider;
	private Transform m_Pivot;
	private RaycastHit hit;
	private RaycastHit aiEffectHit;


	protected override void InitOther () 
	{
		Damage = 0;
		m_BoxCollider = gameObject.GetComponent<BoxCollider>();
		if (m_BoxCollider == null)
		{
			m_BoxCollider = gameObject.AddComponent<BoxCollider>();
		}
		m_Pivot = M_Transform.Find("Pivot").GetComponent<Transform>();
	}


	/// <summary>
	/// 射击
	/// </summary>
	/// <param name="dir"></param>
	public override  void Shoot(Vector3 dir, int force, int arrowDamage)
    {
		Damage = arrowDamage;
		Ray tRay = new Ray(M_Transform.position, dir);
		M_Rigidbody.AddForce(dir * force);
		//1 << 11?
		if (Physics.Raycast(tRay, out hit, 1000, 1 << 11)) { }

		if (Physics.Raycast(tRay, out aiEffectHit, 1000, 1 << 12)) { };

		M_Rigidbody.AddForce(dir * force);
		StartCoroutine("DestorySelf");
	}


	protected override void Collision(Collision coll)
	{
		M_Rigidbody.Sleep();
		if (coll.gameObject.layer == LayerMask.NameToLayer("Envionment"))
		{
			GameObject.Destroy(m_BoxCollider);
			GameObject.Destroy(M_Rigidbody);
			BulletMark tempBulletMark = coll.collider.GetComponent<BulletMark>();
			if (tempBulletMark != null)
			{
				tempBulletMark.HP -= Damage;
			}
			StartCoroutine("AnimationPlay", m_Pivot);
			M_Transform.SetParent(coll.gameObject.transform);
		}
		else if (coll.gameObject.layer == LayerMask.NameToLayer("AI"))
		{
			GameObject.Destroy(m_BoxCollider);
			GameObject.Destroy(M_Rigidbody);
			AI tempAI = coll.collider.GetComponentInParent<AI>();
			if (tempAI != null)
			{
				// 头部伤害加倍
				if (coll.collider.gameObject.name == "Head")
				{
					tempAI.HeadGetHit(Damage);
				}
				else
				{
					tempAI.NormalHit(Damage);
				}

				// 播放血液特效
				tempAI.PlayBloodEffect(aiEffectHit);

			}
			StartCoroutine("AnimationPlay", m_Pivot);
			M_Transform.SetParent(coll.gameObject.transform);
		}
	}




}
