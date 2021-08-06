using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  枪械数据实体类
/// </summary>
public class GunWeaponData 
{
    private int id;
    private string name;
    private int damage;
    private int durage;


    public int Id { get { return id; } set { id = value; } }
    public string Name { get { return name; } set { name = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public int Durage { get { return durage; } set { durage = value; } }

    public GunWeaponData() { }

    public GunWeaponData(int id,string name,int damge,int durage)
    {
        this.Id =  id;
        this.Name = name;
        this.Damage = damage;
        this.Durage = durage;
    }


    public override string ToString()
    {
        return string.Format(Id +"--->" + name + "--->" + damage + "--->" + durage + "    ");
    }
}
