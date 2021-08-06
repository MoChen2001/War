using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : HotGunControllerBase
{

    private ShotgunView m_View;

    protected override void InitOther()
    {
        base.InitOther();

        GunWeaponType = GunType.ShotGun;
        CanShoot = true;
        m_View = (ShotgunView)M_BaseView;
    }

    protected override void Shoot()
    {
        // 射击状态中且不在延迟时间内
        if (Input.GetMouseButtonDown(0) && CanShoot)
        {
            m_View.M_Animator.SetTrigger("Fire");
            PlayFireEffect();
            PlayAudio();
            StartCoroutine(ShotHelp());
        }

    }


    private IEnumerator ShotHelp()
    {
        for (int i = 0; i < 5; i++)
        {
            float distance = 0.2f;
            Vector3 random = new Vector3(Random.Range(-distance, distance), Random.Range(-distance, distance), Random.Range(-distance, distance));
            GameObject temp = GameObject.Instantiate(m_View.Prefab_Bullet, m_View.ShootPos.position + random,
                Quaternion.identity, m_View.Bullet_Parents);
            BulletHelp tempBulletHelp = temp.GetComponent<BulletHelp>();
            if (tempBulletHelp == null)
            {
                tempBulletHelp = temp.AddComponent<BulletHelp>();
            }
            tempBulletHelp.Shoot(m_View.ShootPos.forward, 3000, Damage / 5);
            Durable--;
            yield return new WaitForSeconds(0.01f);
        }
        StartCoroutine(DelayTime());
    }



    /// <summary>
    /// 动画事件更改设计状态
    /// </summary>
    public void ChangeShoot(int val)
    {
        CanShoot = val == 1;
    }
    public void ChangeShell()
    {
        AudioSource.PlayClipAtPoint(m_View.Audio_Shell, m_View.ShootPos.position);
    }


    /// <summary>
    /// 延迟一段时间再播放弹壳特效
    /// </summary>
    /// <returns></returns>
    private IEnumerator DelayTime()
    {
        yield return new WaitForSeconds(1.0f);
        PlayShellEffect();
    }


}
