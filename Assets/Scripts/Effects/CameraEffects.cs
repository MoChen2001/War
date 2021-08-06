using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : CameraBase
{
    [SerializeField]
    [Range(0.0f,1.0f)]
    private float bloodTextureAlpha;
    public float BloodTextureAlpha { set { bloodTextureAlpha = value; } get { return bloodTextureAlpha; } }

    [SerializeField]
    private Texture bloodTexture;


    [SerializeField]
    private Shader m_Shader;


    [SerializeField]
    private Material m_Material = null;
    public Material M_Material
    {
        get 
        {
            if(m_Material == null)
            {
                m_Material = CheckShaderAndCreateMaterial(m_Shader, m_Material);
            }
            return m_Material;
        }
    }




    private void OnRenderImage(RenderTexture src,RenderTexture dest)
    {
        if (M_Material != null)
        {
            M_Material.SetFloat("_TexAlpha", bloodTextureAlpha);
            M_Material.SetTexture("_BloodTex", bloodTexture);

            Graphics.Blit(src, dest, M_Material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

}
