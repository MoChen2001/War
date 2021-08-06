using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunView : HotGunViewBase
{
    private AudioClip audio_Shell;                                  // 切换子弹时的音效

    public AudioClip Audio_Shell { get { return audio_Shell; } }



    protected override void InitOther()
    {
        base.InitOther();

        // 预制体加载
        audio_Shell = Resources.Load<AudioClip>("Audios/Gun/Shotgun_Pump");
    }

    protected override void InitPrefab()
    {
        Prefab_Shell = Resources.Load<GameObject>("Gun/Shotgun_Shell");
        Prefab_Bullet = Resources.Load<GameObject>("Gun/Bullet");
    }

    protected override void InitShellPos()
    {
        ShellPos = M_Transform.Find("Armature/Weapon/EffectPos_B").GetComponent<Transform>();
    }

    protected override void InitHoldValue()
    {
        NormalPos = M_Transform.localPosition;
        NormalRotate = M_Transform.localRotation.eulerAngles;

        HoldPos = new Vector3(-0.146f, -1.78f, 0.2f);
        HoldRotate = new Vector3(-1.514f, -6f, 0.0f);
    }

    protected override void InitShootPos()
    {
        ShootPos = M_Transform.Find("Armature/Weapon/EffectPos_A").GetComponent<Transform>();
    }

    protected override void LoadAudioAndEffect()
    {
        M_Audio = Resources.Load<AudioClip>("Audios/Gun/Shotgun_Fire");
        M_Effect = Resources.Load<GameObject>("Effects/Gun/Shotgun_GunPoint_Effect");
    }



}
