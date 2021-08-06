using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingContentItemController : MonoBehaviour 
{


	private Transform m_Transform;
	private Text item_Text;
	private GameObject item_BG;
	private Button item_Button;

	private int item_ID;
	public int ItemID
    {
        get { return item_ID; }
    }


	void Awake () 
	{
		m_Transform = gameObject.GetComponent<Transform>();
		item_Text = m_Transform.Find("CraftingContentText").GetComponent<Text>();
		item_Button = gameObject.GetComponent<Button>();
		item_BG = m_Transform.Find("CraftingContentBG").gameObject;
		item_Button.onClick.AddListener(ButtonOnClick);
	}
	

	public void InitContentItem(CraftingContentItem item)
    {
		item_Text.text = item.ItemName;
		item_ID = item.ItemID;

	}


	public void ItemNormalState()
    {
		item_BG.SetActive(false);
    }

	public void ItemClickState()
    {
		item_BG.SetActive(true);
	}

	/// <summary>
	/// 点击按钮时更改选项卡的状态
	/// </summary>
	private void ButtonOnClick()
    {
		SendMessageUpwards("ResetContentItem", this);
    }
}
