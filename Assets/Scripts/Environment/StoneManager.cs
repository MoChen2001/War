using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneManager : EnvManagerBase
{
	// 初始化
	protected override void Init()
	{
		GameObject temp = Resources.Load<GameObject>("Env/Rock_Normal");
		Prefab_Dic.Add(0, temp);
		temp = Resources.Load<GameObject>("Env/Rock_Metal");
		Prefab_Dic.Add(1, temp);

	}


	/// <summary>
	///  根据初始的埋藏点实例化石头
	/// </summary>
	protected override void InitPrefab()
	{
		for(int i = 1; i < Points.Length; i++)
        {
			// 隐藏网格
			Points[i].GetComponent<MeshRenderer>().enabled = false;
			Points[i].name = string.Format("Point_" + i);

			// 决定实例化石头的类型
			int temp = Random.Range(0, 10) % 2;
			GameObject stone = GameObject.Instantiate(Prefab_Dic[temp], Points[i].position,
								Quaternion.identity, Parents_Transform);


			// 初始化石头
			stone.name = string.Format("Stone_" + i);
			Transform tempTransform = stone.GetComponent<Transform>();
			tempTransform.localScale *= Random.Range(0.5f, 2.5f);

			Vector3 eulerAngle = new Vector3(Random.Range(20, 360), Random.Range(20, 360), Random.Range(20, 360));
			tempTransform.localRotation = Quaternion.Euler( eulerAngle);

		}
    }
}
