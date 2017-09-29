Shader "Custom/Dome"
{
	Properties
	{
		_MainTex ("Texture Alpha", 2D) = "white" {}
		_MainTexBeta("Texture Beta", 2D) = "white" {}
		_Radius("Raio", float) = 10
		_Center("Centro", Vector) = (0,0,0,0)
	}

	SubShader
	{
		Tags { "RenderType"="Opaque" } 
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"


			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _MainTexBeta;
			float _Radius;	
			Vector _Center;
			
			struct Input {
				float2 uv_BumpMap : TEXCOORD0;
			};
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 pos : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};
						
			
			v2f vert (appdata v)
			{
				v2f o;
				
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				
				o.pos = mul(unity_ObjectToWorld,v.vertex);
				 
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;					
				
				float dist = distance(_Center.xyz, i.pos); 
				
				
				if (dist > _Radius)
				{				
					col = tex2D(_MainTex, i.uv);					
				}
				else
				{
					col = tex2D(_MainTexBeta, i.uv);
				}
				
				return col;
			}
				
			ENDCG
		}
	}
}
