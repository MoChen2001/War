using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;



public class AI : MonoBehaviour 
{

    private Transform m_Transform;              // transform 组件
    private NavMeshAgent m_NavMeshAngent;       // 导航相关组件
    private Animator m_Animator;                // 设置动画的组件
    private Transform player_Transform;         // 玩家角色的 Transform 组件
    private PlayerController playerCntroller;   // 玩家角色的控制脚本
    private Image m_Bar;                        // AI 角色的血条


    // 会用到的字段
    private Vector3 dir;                        // 移动的方向
    private AIState m_State;                    // 该 AI 的状态
    private float walkSpeed;                    // 巡逻状体的速度
    private float runSpeed;                     // 奔跑状态的速度
    private Transform effectsParent;            // 特效的父物体
    private bool findPlayer;                    // 是否寻找玩家的字段
    private bool havePlay;                      // 2s 内是否已经播放过一次音效
    private AIManagerType m_AIType = AIManagerType.NULL;

    // 自身属性的子弹
    private int life;                           // AI 的生命值
    private int attack;                         // AI 的攻击力
    private int maxHp;                          // AI 的最大生命值

    // 会使用到的资源
    private GameObject bloodEffect;             // AI 血液飞溅特效



    public Vector3 Dir { 
        set 
        { 
            dir = value;
            if(m_NavMeshAngent != null && m_NavMeshAngent.isOnNavMesh == true)
            {
                m_NavMeshAngent.SetDestination(Dir);
            }
        } 
        get { return dir; } 
    }
    public AIState M_State { get { return m_State; } set { m_State = value; } }
    public AIManagerType M_AIType { get { return m_AIType; } set { m_AIType = value; } }
    public int Life{ get { return life; } 
        set 
        {
            life = value;
            if (maxHp == 0) maxHp = life;
            if (life <= 0) ToggleState(AIState.DEATH);
            float temp = (float)life / maxHp;
            m_Bar.fillAmount = temp;
            m_Bar.color = Color.Lerp(Color.red, Color.green, temp);
        } 
    }
    public int Attack { get { return attack; } set { attack = value; } }


    private void Awake()
    {
        bloodEffect = Resources.Load<GameObject>("Effects/Gun/Bullet Impact FX_Flesh");


        m_Transform = gameObject.GetComponent<Transform>();
        m_Bar = m_Transform.Find("BloodPanel/Bar").GetComponent<Image>();
        player_Transform = GameObject.Find("FPSController").GetComponent<Transform>();
        playerCntroller = player_Transform.GetComponent<PlayerController>();
        m_NavMeshAngent = m_Transform.GetComponent<NavMeshAgent>();
        m_Animator = m_Transform.GetComponent<Animator>();
        effectsParent = GameObject.Find("TempObject/Blood_Effects").GetComponent<Transform>();

        havePlay = false;
        findPlayer = true;
        walkSpeed = 0.8f;
        runSpeed = 2.0f;

        playerCntroller.playerDeathDelegate += BackDir;
        m_NavMeshAngent.SetDestination(Dir);
        WalkState();
    }



    private void Update()
    {
        if(m_State != AIState.DEATH && findPlayer)
        {
            WanderHelp();
            AIFindPlayer();
            AIAttackPlayer();
        }
    }



    /// <summary>
    /// 初始化 AI 的函数
    /// </summary>
    public void InitAI(int life,int attack)
    {
        Life = life;
        Attack = attack;
    }

    /// <summary>
    ///  播放血液特效的函数
    /// </summary>
    /// <param name="rayHit"></param>
    public void PlayBloodEffect(RaycastHit rayHit)
    {
        GameObject temp = null;
        if (rayHit.normal == Vector3.zero)
        {
            temp = GameObject.Instantiate(bloodEffect, rayHit.point, Quaternion.identity, effectsParent);
        }
        else
        {

            temp = GameObject.Instantiate(bloodEffect, rayHit.point, Quaternion.LookRotation(rayHit.normal), effectsParent);
        }
        GameObject.Destroy(temp, 3.0f);
    }

    /// <summary>
    /// 头部受到伤害的函数
    /// </summary>
    public void HeadGetHit(int damage)
    {
        HitHead();
        Life -= damage * 2;
    }

    /// <summary>
    ///  正常受到伤害的函数
    /// </summary>
    public void NormalHit(int damage)
    {
        HitNormal();
        Life -= damage;
    }

    /// <summary>
    ///  攻击角色的伤害函数
    /// </summary>
    private void AttackPlayer()
    {
        playerCntroller.MinusHP(Attack);
    }


    #region 状态切换的相关方法

    /// <summary>
    ///  总的状态切换方法
    /// </summary>
    public void ToggleState(AIState state)
    {
        switch (state)
        {
            case AIState.IDLE:
                IdleState();
                break;
            case AIState.WALK:
                WalkState();
                break;
            case AIState.RUN:
                RunState();
                break;
            case AIState.ATTACK:
                AttackState();
                break;
            case AIState.DEATH:
                DeathState();
                break;
            default:
                break;
        }
    }


    /// <summary>
    ///  idle 状态执行的方法
    /// </summary>
    private void IdleState()
    {
        m_State = AIState.IDLE;
        m_Animator.SetTrigger("Idle");
    }


    /// <summary>
    ///  walk 状态执行的方法
    /// </summary>
    private void WalkState()
    {
        m_State = AIState.WALK;
        m_NavMeshAngent.speed = walkSpeed;
        m_Animator.SetTrigger("Walk");
    }



    /// <summary>
    ///  run 状态执行的方法
    /// </summary>
    private void RunState()
    {
        m_State = AIState.RUN;
        m_NavMeshAngent.speed = runSpeed;
        m_Animator.SetTrigger("Run");
    }
       

    /// <summary>
    ///  攻击状态
    /// </summary>
    private void AttackState()
    {
        m_State = AIState.ATTACK;
        m_Animator.SetTrigger("Attack");
    }


    /// <summary>
    ///  被普通攻击使用的方法
    /// </summary>
    private void HitNormal()
    {
        m_Animator.SetTrigger("HitNormal");
        if (m_AIType == AIManagerType.BOAR)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.BoarInjured, m_Transform.position);
        }
        else if (m_AIType == AIManagerType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.ZombieInjured, m_Transform.position);
        }
    }


    /// <summary>
    ///  被攻击头部调用的方法
    /// </summary>
    private void HitHead()
    {
        m_Animator.SetTrigger("HitHead");
        if (m_AIType == AIManagerType.BOAR)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.BoarInjured, m_Transform.position);
        }
        else if (m_AIType == AIManagerType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.ZombieInjured, m_Transform.position);
        }
    }


    /// <summary>
    ///  角色的死亡状态调用的方法
    /// </summary>
    private void DeathState()
    {
        m_State = AIState.DEATH;
        m_NavMeshAngent.isStopped = true;
        m_Transform.position = m_Transform.position;
        m_Animator.SetTrigger("Death");
        StartCoroutine("Death");
    }


    #endregion


    #region 状态辅助方法


    /// <summary>
    ///  辅助巡逻的函数
    ///  当距离较近的时候就切换巡逻点
    /// </summary>
    private void WanderHelp()
    {
        if (M_State == AIState.WALK || M_State == AIState.IDLE)
        {
            if (Vector3.Distance(Dir, m_Transform.position) < 1.0f)
            {
                SendMessageUpwards("ChangeAIDir", gameObject);
                ToggleState(AIState.IDLE);
            }
            else
            {
                ToggleState(AIState.WALK);
            }
        }
    }


    /// <summary>
    ///  AI 寻找到角色的时候进行奔跑
    /// </summary>
    private void AIFindPlayer()
    {
        if (Vector3.Distance(m_Transform.position, player_Transform.position) < 20)
        {
            m_NavMeshAngent.SetDestination(player_Transform.position);
            ToggleState(AIState.RUN);
        }
        else
        {
            m_NavMeshAngent.SetDestination(Dir);
            ToggleState(AIState.WALK);
        }
    }

    /// <summary>
    ///  AI 攻击玩家角色
    /// </summary>
    private void AIAttackPlayer()
    {
        if (M_State == AIState.RUN)
        {
            if (Vector3.Distance(m_Transform.position, player_Transform.position) <= 2)
            {
                m_NavMeshAngent.isStopped = true;
                ToggleState(AIState.ATTACK);
                if(!havePlay)
                {
                    havePlay = true;
                    if (AIManagerType.BOAR == m_AIType)
                    {
                        AudioManager.Instance.PlayAudioClipByName(ClipName.BoarAttack, m_Transform.position);
                    }
                    else if (m_AIType == AIManagerType.CANNIBAL)
                    {
                        AudioManager.Instance.PlayAudioClipByName(ClipName.ZombieAttack, m_Transform.position);
                    }
                    StartCoroutine("AudioPlayTime");
                }
            }
            else
            {
                m_NavMeshAngent.isStopped = false;
                m_NavMeshAngent.SetDestination(player_Transform.position);
                ToggleState(AIState.RUN);
            }

        }
    }


    private IEnumerator AudioPlayTime()
    {
        yield return new WaitForSeconds(2.0f);
        havePlay = false;
    }




    /// <summary>
    ///  角色死亡辅助执行方法
    /// </summary>
    private IEnumerator Death()
    {
        if (m_AIType == AIManagerType.BOAR)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.BoarDeath, m_Transform.position);
        }
        else if (m_AIType == AIManagerType.CANNIBAL)
        {
            AudioManager.Instance.PlayAudioClipByName(ClipName.ZombieDeath, m_Transform.position);
        }
        Collider[] temp = m_Transform.GetComponentsInChildren<Collider>();
        for(int i = 0; i < temp.Length; i++)
        {
            temp[i].enabled = false;
        }
        yield return new WaitForSeconds(2.0f);
        SendMessageUpwards("AIDeath", gameObject);
        GameObject.Destroy(gameObject);
    }


    /// <summary>
    ///  AI 角色死亡时进行执行该方法
    /// </summary>
    private void BackDir()
    {
        findPlayer = false;
        if(m_NavMeshAngent != null)
        {
            m_NavMeshAngent.SetDestination(Dir);
        }
    }

    #endregion

}
