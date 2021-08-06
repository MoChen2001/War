using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CraftingTabController : MonoBehaviour 
{

	private Transform m_Transform;
	// 对应的图标
	private Image itemIcon;
	private GameObject itemBG;
	private Button itemButton;
	private int index;




	void Awake ()
	{
		m_Transform = gameObject.GetComponent<Transform>();
		itemButton = gameObject.GetComponent<Button>();

		itemBG = m_Transform.Find("CraftingTagButtonBG").gameObject;
		itemIcon = m_Transform.Find("CraftingTagTagIcon").GetComponent<Image>();


		itemButton.onClick.AddListener(ButtonOnClick);
	}



	/// <summary>
	/// 初始化 tab 函数
	/// </summary>
	/// <param name="iconName">icon 的名称</param>
	/// <param name="index">tab 的索引</param>
	public void InitTab(Sprite iconSprite,int index)
    {
		this.index = index;
		itemIcon.sprite = iconSprite;

	}




	/// <summary>
	/// 按钮点击方法
	/// </summary>
	private void ButtonOnClick()
    {
		SendMessageUpwards("ResetTab", index);
    }



	/// <summary>
	/// tab 的被选中时的状态
	/// </summary>
	public void ButtonBGClick()
	{
		if (itemBG.activeSelf == true)
		{
			itemBG.SetActive(false);
			itemButton.enabled = false;
		}
	}


	/// <summary>
	/// tab 的普通状态
	/// </summary>
	public void ButtonBGNormal()
	{
		itemBG.SetActive(true);
		itemButton.enabled = true;
	}

}
