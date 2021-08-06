using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnvManagerBase : MonoBehaviour 
{
	private Transform m_Transform;                                         // 总的 Transform 组件
	private Transform[] points;                                            // 埋藏点的 Transform 组件
	private Transform parents_Transform;                                   // 树木的父物体的 Transform 组件
	private Dictionary<int, GameObject> prefab_Dic;                        // 树木预制体数组



	// 访问使用的字段
	protected Transform M_Transform { get { return m_Transform; } }
	protected Transform[] Points { get { return points; } }
	protected Transform Parents_Transform { get { return parents_Transform; } }
	protected Dictionary<int, GameObject> Prefab_Dic { get { return prefab_Dic; } set { prefab_Dic = value; } }


	/// <summary>
	///  初始化函数，设置为 private
	/// </summary>
	private void Start()
    {
		m_Transform = gameObject.GetComponent<Transform>();
		prefab_Dic = new Dictionary<int, GameObject>();
		points = m_Transform.Find("Points").GetComponentsInChildren<Transform>();
		parents_Transform = m_Transform.Find("Parents").GetComponent<Transform>();
		Init();
		InitPrefab();
	}


	// 子类重写的函数
	protected abstract void Init();
	protected abstract void InitPrefab();

}
