using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHelp : BulletHelpBase
{

    private RaycastHit uiHit;
    private RaycastHit aiHit;


    protected override void InitOther()
    {
        Damage = 0;
    }


    /// <summary>
    /// 射击辅助函数
    /// </summary>
    /// <param name="vec">方向</param>
    /// <param name="force">力度</param>
    /// <param name="damgae">伤害</param>
    public override void Shoot(Vector3 dir,int force,int bulletDamage)
    {
        Damage = bulletDamage;
        Ray tRay = new Ray(M_Transform.position, dir);

        //1 << 12 , 结果是 4096, 11 是 AI 的层数
        if (Physics.Raycast(tRay, out aiHit, 1000, 1 << 12)) { };

        //1 << 11 , 结果是 2048, 11 是 Env 的层数
        if (Physics.Raycast(tRay, out uiHit, 1000, 1 << 11)) { }

        M_Rigidbody.AddForce(dir * force);

        StartCoroutine(DestorySelf());
    }


    /// <summary>
    ///  碰撞时执行的方法
    /// </summary>
    /// <param name="coll"></param>
    protected override void Collision(Collision coll)
    {
        M_Rigidbody.Sleep();
        /// 和环境交互
        if (coll.gameObject.layer == LayerMask.NameToLayer("Envionment"))
        {
            BulletMark bulletMark = coll.gameObject.GetComponent<BulletMark>();
            if (bulletMark != null)
            {
                bulletMark.CreateBulletMark(uiHit);
                bulletMark.HP -= Damage;
            }
            GameObject.Destroy(gameObject);
        }
        /// 和 AI 交互
        else if (coll.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            AI tempAI = coll.collider.GetComponentInParent<AI>();
            if (tempAI != null)
            {
                // 头部伤害加倍
                if(coll.collider.gameObject.name == "Head")
                {
                    tempAI.HeadGetHit(Damage);
                }
                else
                {
                    tempAI.NormalHit(Damage);
                }



                tempAI.PlayBloodEffect(aiHit);

            }
            GameObject.Destroy(gameObject);
        }
        else
        {
            GameObject.Destroy(gameObject, 2.0f);
        }
    }





}
