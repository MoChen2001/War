using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ObjectPool))]
public class BulletMark : MonoBehaviour 
{

    private ObjectPool pool;
    private Transform effect_Parents;           // 物体飞溅特效的父物体

    private Texture2D mark_Texture;             // 弹痕贴图
    private Texture2D main_Texture;             // 主帖图
    private Texture2D main_Texture_Back;        // 主帖图的备份
    private Texture2D main_Texture_Back_Shoot;  // 射击时的备份贴图

    [SerializeField]
    private MaterialType materialType;          // 材质类型
    private GameObject bulletEffect;            // 弹痕特效

    private Queue<Vector2> markQueue;           // 管理弹痕的队列
    [SerializeField]private int hp;             // 物体的生命值              


    public int HP
    {
        get { return hp; }
        set 
        {
            hp = value;
            if(hp <= 0)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }



    private void Start()
    {
        hp = 500;
        effect_Parents = GameObject.Find("TempObject/Decal_" + materialType.ToString() + "_Effects").GetComponent<Transform>();

        bulletEffect = Resources.Load<GameObject>("Effects/Gun/Bullet Impact FX_" + materialType.ToString());
        mark_Texture = Resources.Load<Texture2D>("Gun/BulletMarks/Bullet Decal_" + materialType.ToString());



        if(gameObject.name == "Conifer")
        {
            main_Texture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[2].mainTexture;
        }
        else if(gameObject.name == "Broadleaf")
        {
            main_Texture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[3].mainTexture;
        }
        else if(gameObject.name == "Palm")
        {
            main_Texture = (Texture2D)gameObject.GetComponent<MeshRenderer>().materials[1].mainTexture;
        }
        else
        {
            main_Texture = (Texture2D)gameObject.GetComponent<MeshRenderer>().material.mainTexture;
        }




        main_Texture_Back = GameObject.Instantiate<Texture2D>(main_Texture);
        main_Texture_Back_Shoot = GameObject.Instantiate<Texture2D>(main_Texture);
        gameObject.GetComponent<Renderer>().material.mainTexture = main_Texture_Back_Shoot;

        markQueue = new Queue<Vector2>();

        // 对象池对象
        pool = gameObject.GetComponent<ObjectPool>();
        if(pool == null)
        {
            pool = gameObject.AddComponent<ObjectPool>();
        }
    }



    /// <summary>
    /// 创造弹痕的函数
    /// </summary>
    public void CreateBulletMark(RaycastHit hit)
    {
        PlayerHitSound(hit);
        Vector2 uv = hit.textureCoord;
        markQueue.Enqueue(uv);

        for (int i = 0; i < mark_Texture.width; i++)
        {
            for(int j = 0; j < mark_Texture.height; j++)
            {
                float x = uv.x * main_Texture_Back_Shoot.width - mark_Texture.width / 2 + i;
                float y = uv.y * main_Texture_Back_Shoot.height - mark_Texture.height / 2 + j;

                Color color = mark_Texture.GetPixel(i, j);

                if (color.a > 0.2)
                    main_Texture_Back_Shoot.SetPixel((int)x, (int)y, color);
            }
        }

        main_Texture_Back_Shoot.Apply();
        PlayEffects(hit);
        Invoke("DestoryBulletMark", 3.0f);
    }


    /// <summary>
    ///  播放三类音效
    /// </summary>
    private void PlayerHitSound(RaycastHit hit)
    {
        switch (materialType)
        {
            case MaterialType.Stone:
                AudioManager.Instance.PlayAudioClipByName(ClipName.BulletImpactStone, hit.point);
                break;
            case MaterialType.Metal:
                AudioManager.Instance.PlayAudioClipByName(ClipName.BulletImpactMetal, hit.point);
                break;
            case MaterialType.Wood:
                AudioManager.Instance.PlayAudioClipByName(ClipName.BulletImpactWood, hit.point);
                break;
            default:
                break;
        }
    }


    /// <summary>
    /// 弹痕消失的相关函数
    /// </summary>
    private void DestoryBulletMark()
    {
        if(markQueue.Count > 0)
        {
            Vector2 uv = markQueue.Dequeue();
            for (int i = 0; i < mark_Texture.width; i++)
            {
                for (int j = 0; j < mark_Texture.height; j++)
                {
                    float x = uv.x * main_Texture_Back_Shoot.width - mark_Texture.width / 2 + i;
                    float y = uv.y * main_Texture_Back_Shoot.height - mark_Texture.height / 2 + j;

                    Color color = main_Texture_Back.GetPixel((int)x, (int)y);

                    main_Texture_Back_Shoot.SetPixel((int)x, (int)y, color);
                }
            }
            main_Texture_Back_Shoot.Apply();
        }
    }




    /// <summary>
    /// 播放特效的函数
    /// 使用对象池管理资源
    /// </summary>
    private void PlayEffects(RaycastHit hit)
    {
        GameObject temp = null;
        if (pool.PoolIsEmpty())
        {
            temp = GameObject.Instantiate(bulletEffect, hit.point, Quaternion.LookRotation(hit.normal), effect_Parents);
            temp.name = "Effect_" + materialType.ToString();
        }
        else
        {
            temp = pool.GetObject();
            temp.transform.rotation = Quaternion.LookRotation(hit.normal);
            temp.transform.position = hit.point;
        }
        pool.AddObject(temp, 1.0f);
    }
}
