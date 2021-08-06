using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingRifleView : HotGunViewBase
{
    protected override void InitHoldValue()
    {
        NormalPos = M_Transform.localPosition;
        NormalRotate = M_Transform.localRotation.eulerAngles;

        HoldPos = new Vector3(-0.092f, -1.645f, 0.142f);
        HoldRotate = new Vector3(-2.858f, -0.1f, -1.46f);

    }

    protected override void InitPrefab()
    {
        Prefab_Shell = Resources.Load<GameObject>("Gun/Shell");
        Prefab_Bullet = Resources.Load<GameObject>("Gun/Bullet");
    }

    protected override void InitShellPos()
    {
        ShellPos = M_Transform.Find("Hunting_Rifle/EffectPos_B").GetComponent<Transform>();
    }

    protected override void InitShootPos()
    {
        ShootPos = M_Transform.Find("Hunting_Rifle/EffectPos_A").GetComponent<Transform>();
    }

    protected override void LoadAudioAndEffect()
    {
        M_Audio = Resources.Load<AudioClip>("Audios/Gun/AssaultRifle_Fire");
        M_Effect = Resources.Load<GameObject>("Effects/Gun/AssaultRifle_GunPoint_Effect");
    }
}
