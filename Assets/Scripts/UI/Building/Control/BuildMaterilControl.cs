using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class BuildMaterilControl : MonoBehaviour 
{
	private Transform m_Transform;
	private Image m_BGImage;                // 背景图片


	private string m_ModelName;
	private GameObject m_Model;
	

	public GameObject M_Model { get { return m_Model; } }
	public string Name 
	{ 
		get 
		{ 
			return m_ModelName; 
		}
		set 
		{ 
			// 设置名称时如果模型为空就加载模型
			m_ModelName = value;
			if(m_Model == null)
            {
				m_Model = Resources.Load<GameObject>("Building/Prefabs/" + value);
			}
		} 
	}


	private void Awake()
    {
		m_Transform = gameObject.GetComponent<Transform>();
		m_BGImage = gameObject.GetComponent<Image>();
		m_BGImage.color = Color.white;

	}


	/// <summary>
	///  初始化二级材质
	/// </summary>
	public void Init(int offset,Sprite sprite)
    {
		m_Transform.rotation = Quaternion.Euler(new Vector3(0, 0, offset));

		m_Transform.Find("ItemChildIcon").GetComponent<Transform>().rotation = Quaternion.Euler(Vector3.zero);
		m_Transform.Find("ItemChildIcon").GetComponent<Image>().sprite = sprite;

		// 默认隐藏
		gameObject.SetActive(false);
	}



	/// <summary>
	///  选中时的高光状态
	/// </summary>
	public void HighLight()
    {
		m_BGImage.color = Color.red;

	}

	/// <summary>
	///  没选中时的普通状态
	/// </summary>
	public void Normal()
    {
		m_BGImage.color = Color.white;
	}

	/// <summary>
	///  显示
	/// </summary>
	public void Show()
    {
		gameObject.SetActive(true);
    }
	/// <summary>
	///  隐藏
	/// </summary>
	public void Hide()
    {
		gameObject.SetActive(false);
    }
}
