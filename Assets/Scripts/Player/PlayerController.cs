using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;


public delegate void PlayerDeathFunction();

public class PlayerController : MonoBehaviour {

	private Transform m_Transform;
	private FirstPersonController FPS;          // 第一人称角色控制器脚本
	private CameraEffects cameraTemp;           // 进行屏幕后处理的脚本
	private AudioSource breathAudio;			// 呼吸声的音效

	private int hp;								// 角色生命值
	private int vit;							// 角色体力值
	private int maxHP;							// 最大生命值
	private int maxVit;							// 最大体力值

	private Image hpBar;						// 血条UI
	private Image vitBar;                       // 耐力值UI
	private bool playerDeath;                   // 角色是否死亡的标志位
	private bool climp;							// 是否爬行的标志位


	public event PlayerDeathFunction playerDeathDelegate;		// 角色死亡委托

	public bool PlayerDeathState { get { return playerDeath;  } }
	/// <summary>
	///  赋值时切换状态
	/// </summary>
	public bool Climp 
	{ 
		set 
		{
			FPS.enabled = !value;
			gameObject.GetComponent<Rigidbody>().useGravity = !value;
			climp = value; 
		} 
	}

	public int Hp  
	{ 
		set 
		{ 
			hp = value;
			if (maxHP == 0)
			{
				maxHP = hp;
			}
			PlayerDeath();
			BreathAudioControl();
			hpBar.fillAmount = (float)hp / maxHP;
			cameraTemp.BloodTextureAlpha =  1 - hpBar.fillAmount;
		}  
		get { return hp; } 
	}
	public int Vit 
	{ 
		set 
		{ 
			vit = value;
			if (maxVit == 0) maxVit = vit;
			vitBar.fillAmount = (float)vit / maxVit;
		} 
		get { return vit;  } 
	}


	void Start () 
	{
		m_Transform = gameObject.GetComponent<Transform>();
		FPS = gameObject.GetComponent<FirstPersonController>();

		breathAudio = AudioManager.Instance.AddAudioClipComponentToGameObject(gameObject, 
			ClipName.PlayerBreathingHeavy, true, false);
		cameraTemp = GameObject.Find("FPSController/PersonCamera/EnvCamera").GetComponent<CameraEffects>();
		vitBar = GameObject.Find("CanvasCamera/MainPanel/PlayerInfoPanel/VIT/Bar").GetComponent<Image>();
		hpBar = GameObject.Find("CanvasCamera/MainPanel/PlayerInfoPanel/HP/Bar").GetComponent<Image>();

		playerDeath = false;
		Hp = 100;
		Vit = 100;


		StartCoroutine("RestoreVit");
	}
	


	void Update()
    {
		if(climp)
        {
			ClimpHelp();
		}
    }


	/// <summary>
	///  外部调用的生命值减少的函数
	/// </summary>
	/// <param name="damage">伤害值</param>
	public void MinusHP(int damage)
    {
		Hp -= damage;
		if(!playerDeath)
        {
			AudioManager.Instance.PlayAudioClipByName(ClipName.PlayerHurt, m_Transform.position);
		}
    }



	/// <summary>
	///  体力值的控制函数
	/// </summary>
	private IEnumerator RestoreVit()
    {
		while(true)
        {
			if(FPS.M_PlayerState != PlayerState.RUN && Vit < 100)
            {
				Vit += 1;
			}
			if(FPS.M_PlayerState == PlayerState.RUN && Vit > 3)
            {
				Vit -= 3;
			}

			
			float addRunSpeed = 0.05f * Vit;
			FPS.M_RunSpeed = addRunSpeed + FPS.M_WalkSpeed;
			yield return new WaitForSeconds(0.05f);
		}
    }

	/// <summary>
	///  跳跃时触发的函数, FPS 教本 Line 139 行调用
	/// </summary>
	public void JumpMessage()
    {
		if(Vit > 20)
        {
			Vit -= 20;
			FPS.M_JumpSpeed = 5;
		}
		else
        {
			FPS.M_JumpSpeed = 0;
        }
    }





	/// <summary>
	///  角色死亡辅助函数
	/// </summary>
	private void PlayerDeath()
    {
		if (hp <= 0 && !playerDeath)
        {
			playerDeathDelegate();
			FPS.enabled = false;
			playerDeath = true;
			breathAudio.Stop();
			AudioManager.Instance.PlayAudioClipByName(ClipName.PlayerDeath, m_Transform.position);
			GameObject.Find("Manager").GetComponent<InputManager>().enabled = false;
		}
	}

	/// <summary>
	///  角色呼吸辅助函数
	/// </summary>
	private void BreathAudioControl()
    {
		// 如果体力值较低则进行急促呼吸
		if (hp < maxHP / 2 && !playerDeath)
		{
			breathAudio.Play();
		}
		else
		{
			breathAudio.Stop();
		}
	}




	/// <summary>
	///  爬行辅助函数
	/// </summary>
	private void ClimpHelp()
    {
		if(Input.GetKey(KeyCode.W))
        {
			m_Transform.position += new Vector3(0, 0.1f, 0);
        }
		else if (Input.GetKey(KeyCode.S))
		{
			m_Transform.position -= new Vector3(0, 0.1f, 0);
		}
	}

}
