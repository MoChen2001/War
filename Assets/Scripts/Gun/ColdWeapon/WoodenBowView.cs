using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenBowView : ColdGunViewBase
{

    protected override void InitHoldValue()
    {
        NormalPos = M_Transform.localPosition;
        NormalRotate = M_Transform.localRotation.eulerAngles;

        HoldPos = new Vector3(0.38f, -1.49f, 0.75f);
        HoldRotate = new Vector3(-0.506f, -4.731f,18.605f);
        
    }

    protected override void InitShootPos()
    {
        ShootPos = M_Transform.Find("Armature/Arm_L/Forearm_L/Wrist_L/Weapon/EffectPos_A").GetComponent<Transform>();
    }

    protected override void InitOther()
    {
        Prefab_Bullet = Resources.Load<GameObject>("Gun/Arrow");
    }

    protected override void LoadAudioAndEffect()
    {
        M_Audio = Resources.Load<AudioClip>("Audios/Gun/Arrow Release");
    }
}
