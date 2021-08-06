using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenSpearView : ColdGunViewBase
{
    protected override void InitHoldValue()
    {
        NormalPos = M_Transform.localPosition;
        NormalRotate = M_Transform.localRotation.eulerAngles;

        HoldPos = new Vector3(0.09167805f, -1.667f, 0.7542844f);
        HoldRotate = new Vector3(-9.574f, -0.678f, 4.062f);
    }

    protected override void InitOther()
    {
        Prefab_Bullet = Resources.Load<GameObject>("Gun/Wooden_Spear");
    }

    protected override void InitShootPos()
    {
        ShootPos = M_Transform.Find("Armature/Arm_R/Forearm_R/Wrist_R/Weapon/EffectPos_A").GetComponent<Transform>();
    }

    protected override void LoadAudioAndEffect()
    {
        M_Audio = Resources.Load<AudioClip>("Audios/Gun/Arrow Release");
    }
}
