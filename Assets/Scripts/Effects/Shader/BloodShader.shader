Shader "MyShader/BloodShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BloodTex("Blood Texture",2D) = "white"{}
		_TexAlpha("Blood Textrue Alpha",Range(0,1)) = 0 
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;		
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _BloodTex_ST;
			sampler2D _BloodTex;
			float _TexAlpha;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _BloodTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 bloodCol = tex2D(_BloodTex,i.uv);
				if(bloodCol.a > 0.9)
				{
					bloodCol.a = _TexAlpha * 2.0f;
				}
				else if(bloodCol.a > 0.7)
				{
					bloodCol.a = _TexAlpha;		
				}
				else
				{
					bloodCol.a = 0.0f;
				}
				return bloodCol;
			}
			ENDCG
		}

		Pass
		{

			Blend SrcColor  DstColor 
			Blend SrcAlpha DstAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			

			#include "UnityCG.cginc"

			struct a2v
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;		
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_ST;
			sampler2D _MainTex;
			float _TexAlpha;
			
			v2f vert (a2v v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv= TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex,i.uv);
				return col;
			}
			ENDCG
		}
	}
}
