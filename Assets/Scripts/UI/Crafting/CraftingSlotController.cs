using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSlotController : MonoBehaviour {

	private Transform m_Transform;
    private Image itemUI;
    private int itemId;

    public int ItemId
    {
        get { return itemId; }
        set { itemId = value; }
    }


	private void Awake()
    {
        m_Transform = gameObject.GetComponent<Transform>();
        itemUI = m_Transform.Find("CraftingSlotItem").GetComponent<Image>();
        itemUI.gameObject.SetActive(false);
        itemId = -1;
    }

    /// <summary>
    ///  设置合成槽的 UI
    /// </summary>
    /// <param name="sprite"></param>
    public void SetSlotItemUI(int id ,Sprite sprite)
    {
        itemId = id;
        itemUI.gameObject.SetActive(true);
        itemUI.sprite = sprite;

    }

    /// <summary>
    /// 重置合成槽
    /// </summary>
    public void ResetSlot()
    {
        itemId = -1;
        itemUI.gameObject.SetActive(false);
    }
}
