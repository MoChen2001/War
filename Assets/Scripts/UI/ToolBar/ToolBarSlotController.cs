using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolBarSlotController : MonoBehaviour 
{

	private Transform m_Transform;          // 当前物体的 transform

	private int index;
	private Text num_Text;					// 当前槽的数字按键的数值
	private GameObject select_Bg;			// 选中时呈现出的背景状态
	private bool selected;					// 是否被选中的字段

	public bool Selected
    {
        get { return selected; }
        set { selected = value; select_Bg.SetActive(value); }
	}					// 是否被选中的属性，同时也会设置背景框

	private void Awake()
    {
		InitOthers();
	}


	/// <summary>
	/// 初始化乱七八糟的东西的函数
	/// </summary>
	private void InitOthers()
    {
		m_Transform = gameObject.GetComponent<Transform>();


		num_Text = m_Transform.Find("ClickNum").GetComponent<Text>();
		select_Bg = m_Transform.Find("Selected").gameObject;


		NormalState();
	}




	/// <summary>
	/// 初始化工具槽的函数，外部访问调用
	/// </summary>
	public void InitSlot(string text)
    {
		num_Text.text = text;
		index = int.Parse(text);
	}

	/// <summary>
	/// 选中时的状态
	/// </summary>
	private void SelectState()
    {
		Selected = true;
    }

	/// <summary>
	/// 普通情况下的状态
	/// </summary>
	private void NormalState()
    {
		Selected = false;
	}
	
}
