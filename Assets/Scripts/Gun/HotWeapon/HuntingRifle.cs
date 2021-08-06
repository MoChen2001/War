using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntingRifle : HotGunControllerBase
{
    private HuntingRifleView m_View;


    protected override void InitOther()
    {
        base.InitOther();

        GunWeaponType = GunType.HuntingRifle;
        m_View = (HuntingRifleView)M_BaseView;


    }

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



    private IEnumerator ShootHelp()
    {
        if (Durable > 0)
        {
            PlayAudio();
            PlayEffect();

            GameObject temp = GameObject.Instantiate(m_View.Prefab_Bullet, m_View.ShootPos.position, 
                Quaternion.identity, m_View.Bullet_Parents);
            BulletHelp tempBulletHelp = temp.GetComponent<BulletHelp>();
            if (tempBulletHelp == null)
            {
                tempBulletHelp = temp.AddComponent<BulletHelp>();
            }
            tempBulletHelp.Shoot(m_View.ShootPos.forward, 3000, Damage);
            Durable--;


            yield return new WaitForSeconds(1.0f);

            CanShoot = true;
        }
    }
}
