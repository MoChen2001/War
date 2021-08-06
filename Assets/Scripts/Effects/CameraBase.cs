using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraBase : MonoBehaviour 
{
    protected void Start()
    {
        CheckResources();
    }

    /// <summary>
    ///  总的支持判断函数
    /// </summary>
    protected void CheckResources()
    {
        bool isSupported = CheckSupport();
        if(isSupported == false)
        {
            NotSupport();
        }
    }

    /// <summary>
    ///  是否支持后处理的判断
    /// </summary>
    protected bool CheckSupport()
    {
        if(SystemInfo.supportsImageEffects == false || SystemInfo.supportsRenderTextures == false)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    ///  不支持则直接禁用
    /// </summary>
    protected void NotSupport()
    {
        this.enabled = false;
    }


    /// <summary>
    ///  子类会用到的东西
    /// </summary>
    protected Material CheckShaderAndCreateMaterial(Shader shader,Material material)
    {
        if(shader == null)
        {
            return null;
        }
        if(shader.isSupported && material != null && material.shader == shader)
        {
            return material;
        }
        if (!shader.isSupported)
        {
            return null;
        }
        else
        {
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            if(material != null)
            {
                return material;
            }
            else
            {
                return null;
            }
        }
    }


}
