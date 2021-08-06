using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HotGunViewBase : GunViewBase
{

    #region 字段

    private Transform shell_Effects;    // 弹壳特效的父物体
	private Transform fire_Effects;     // 枪火特效的父物体
	private Transform shellPos;         // 子弹壳的弹出位置

	private GameObject prefab_Bullet;   // 子弹预制体
	private GameObject prefab_Shell;    // 弹壳预制体

	private ObjectPool[] pools;         // 对象池

	#endregion

	#region 属性

	/// <summary>
	/// 弹壳特效的父物体
	/// </summary>
	public Transform Shell_Effects { get { return shell_Effects; } }
	/// <summary>
	/// 枪火特效的父物体
	/// </summary>
	public Transform Fire_Effects { get { return fire_Effects; } }
	/// <summary>
	/// 弹壳弹出的位置
	/// </summary>
	public Transform ShellPos { get { return shellPos; } set { shellPos = value; } }
	public GameObject Prefab_Bullet { get { return prefab_Bullet; } set { prefab_Bullet = value; } }
	public GameObject Prefab_Shell { get { return prefab_Shell; } set { prefab_Shell = value; } }

	/// <summary>
	/// 索引为零存储枪火
	/// 索引为一存储弹壳
	/// </summary>
	public ObjectPool[] Pools { get { return pools; } }

	#endregion


	/// <summary>
	///  初始化对象池
	/// </summary>
	private void InitObjPool()
	{
		pools = gameObject.GetComponents<ObjectPool>();
		if (pools.Length == 0)
		{
			pools = new ObjectPool[2];
			pools[0] = gameObject.AddComponent<ObjectPool>();
			pools[1] = gameObject.AddComponent<ObjectPool>();
		}
		else if (pools.Length == 1)
		{
			ObjectPool temp = pools[0];
			pools = new ObjectPool[2];
			pools[0] = temp;
			pools[1] = gameObject.AddComponent<ObjectPool>();
		}
	}

	/// <summary>
	/// 子类要初始化其他的特有对象时，应该要重载的同时调用父类的方法
	/// </summary>
	protected override void InitOther()
    {
		InitPrefab();
		InitShellPos();
		InitObjPool();
		shell_Effects = GameObject.Find("TempObject/Shell_Effects").GetComponent<Transform>();
		fire_Effects = GameObject.Find("TempObject/Fire_Effects").GetComponent<Transform>();
	}


	#region 虚方法
	/// <summary>
	/// 初始化预制体的方法
	/// </summary>
	protected abstract void InitPrefab();
	/// <summary>
	/// 初始化弹壳弹出位置的方法
	/// </summary>
	protected abstract void InitShellPos();



    #endregion

}
