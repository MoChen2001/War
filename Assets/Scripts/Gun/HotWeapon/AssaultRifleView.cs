using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


/// <summary>
///  冲锋枪的 View 层
/// </summary>
public class AssaultRifleView : HotGunViewBase
{



	protected override void InitPrefab()
    {
		// 预制体加载
		Prefab_Shell = Resources.Load<GameObject>("Gun/Shell");
		Prefab_Bullet = Resources.Load<GameObject>("Gun/Bullet");

	}

	protected override void InitShellPos()
    {
		// 位置属性加载
		ShellPos = M_Transform.Find("Assault_Rifle/EffectPos_B").GetComponent<Transform>();
	}

	protected override void InitHoldValue()
    {
		NormalPos = M_Transform.localPosition;
		NormalRotate = M_Transform.localRotation.eulerAngles;
		HoldPos = new Vector3(-0.07f, -1.864f, 0.502f);
		HoldRotate = new Vector3(2.55f, 0.4f, 0.0f);
	}

	protected override void InitShootPos()
    {
		ShootPos = M_Transform.Find("Assault_Rifle/EffectPos_A").GetComponent<Transform>();
	}

	protected override void LoadAudioAndEffect()
    {
		M_Audio = Resources.Load<AudioClip>("Audios/Gun/AssaultRifle_Fire");
		M_Effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
	}

}
