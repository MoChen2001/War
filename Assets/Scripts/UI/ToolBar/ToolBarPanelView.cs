using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolBarPanelView : MonoBehaviour {

	private Transform m_Transform;          // 当前物体的 Transform


	private Transform grid_Transform;       // 工具槽的父物体

	private GameObject toolBarSlot;         // 工具槽的预制体



	// 上面的字段的只读属性
	public Transform M_Transform { get { return m_Transform; } }
	public Transform Grid_Transform { get { return grid_Transform; } }
	public GameObject Prefab_ToolBarSlot { get { return toolBarSlot; } }




	private void Start()
    {
		m_Transform = gameObject.GetComponent<Transform>();


		grid_Transform = m_Transform.Find("Grid").GetComponent<Transform>();

		toolBarSlot = Resources.Load<GameObject>("UI/Prefabs/ToolBarSlot");
    }



}
