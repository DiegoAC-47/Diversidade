Shader "Custom/DomeColor"
{
	Properties
	{
		_Top ("Topo", float) = 10
		_Bot ("Fundo", float) = 1
	}
	SubShader
	{
		Tags {"Queue"="Transparent"  "RenderType"="Transparent"}
		LOD 200
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Back
		
		Pass
		{
			CGPROGRAM
			#pragma vertex vert 
			#pragma fragment frag alpha	
			#include "UnityCG.cginc"
			
			
			float _Top;
			float _Bot;
			
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 pos : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION; 
			};

			
			
			float convPosi(float p)
			{
				float t2 = 0.7f;
				float b2 = 0.2f;
				return ((t2-b2) / (_Top - _Bot)) * (p - _Bot);
			
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos.y = convPosi(mul(unity_ObjectToWorld,v.vertex).y);
				o.vertex = UnityObjectToClipPos(v.vertex);
				 
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col;
				float posi = 1 - i.pos.y; 
				
				
				
				col.r = posi;
				col.g = posi;
				col.b = 0;
				col.a = posi;
				return col;
			}
			ENDCG
		}
	}
}
