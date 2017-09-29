Shader "Custom/DomeObjMulti" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Shadow ("Shadow", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex_Shadow ("Shadow Texture", 2D) = "white" {}		
		_NormalMap ("Normal Map", 2D) = "bump" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		
		
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _MainTex_Shadow;
		sampler2D _NormalMap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex_Shadow;
			float2 uv_NormalMap;			
			float3 worldPos;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _Shadow;
		float3 _Targets[10];
		float  _Radius[10];
		float _Size;
		
		UNITY_INSTANCING_CBUFFER_START(Props)
		UNITY_INSTANCING_CBUFFER_END
		
		fixed4 calDist(float2 worldPos, float2 center, float dist, float4 mytexture, fixed4 shadow)
		{		
			if (distance(worldPos,center) < dist)
			{	
				return shadow;
			}
			else
			{			
				return mytexture;			
			}
		}
		
		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			fixed4 color;
						
			float2 posi = float2(IN.worldPos.x, IN.worldPos.z);
			fixed4 mytexture = tex2D (_MainTex, IN.uv_MainTex) ;
			fixed4 shadow =  tex2D (_MainTex_Shadow, IN.uv_MainTex_Shadow);
			
			color = mytexture;

			float distance_now;
			float dist = 1000000;
			
			for (int i = 0; i < _Size; i++)
			{		
				distance_now = distance(posi, float2(_Targets[i].x, _Targets[i].z));
				
				if(distance_now < _Radius[i])
				{
					dist = distance_now;
					color = calDist(posi,float2(_Targets[i].x, _Targets[i].z), _Radius[i], mytexture, shadow);
				}
			}
			//color = shadow;
			//color.r = color.g = color.b = _Targets[0].z;
			o.Normal = UnpackNormal (tex2D (_NormalMap, IN.uv_NormalMap));
			o.Albedo = color.rgb; 
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
