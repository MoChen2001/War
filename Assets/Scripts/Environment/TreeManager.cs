using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : EnvManagerBase
{

	// 初始化
	protected override void Init()
	{

		GameObject temp = Resources.Load<GameObject>("Env/Conifer");
		Prefab_Dic.Add(0, temp);
		temp = Resources.Load<GameObject>("Env/Broadleaf");
		Prefab_Dic.Add(1, temp);
		temp = Resources.Load<GameObject>("Env/Palm");
		Prefab_Dic.Add(2, temp);
	}


	/// <summary>
	///  根据初始的埋藏点实例化石头
	/// </summary>
	protected override void InitPrefab()
	{
		for (int i = 1; i < Points.Length; i++)
		{
			Points[i].GetComponent<MeshRenderer>().enabled = false;
			Points[i].name = string.Format("Point_" + i);

			int num = Random.Range(0, 12) % 3;

			GameObject tree = GameObject.Instantiate(Prefab_Dic[num], Points[i].position,
						Quaternion.identity, Parents_Transform);
			tree.name = Prefab_Dic[num].name;

			Transform temp = tree.GetComponent<Transform>();
			float height = Random.Range(0.5f, 1.0f);
			temp.localScale *= height;

		}
	}
}