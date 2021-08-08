using System.Collections;
using System.Collections.Generic;
using UnityEngine;




/// <summary>
///  拾取的材料脚本
/// </summary>
public class MaterialHelp : MonoBehaviour 
{


	private int id;
	private string itemName;
	private Transform m_Transform;



	public int ID { get { return id; } set { id = value; } }
	public string ItemName { set { itemName = value; } get { return itemName; } }


	void Start () 
	{
		m_Transform = gameObject.GetComponent<Transform>();

	}
	



	/// <summary>
	///  碰撞检测方法
	/// </summary>
	private void OnCollisionEnter(Collision coll)
    {
		if(coll.collider.tag == "Player")
        {
			InventoryPanelController.Instance.AddItemWithID(id, itemName);
			GameObject.Destroy(gameObject);
        }
    }



}
