Shader "Custom/Transformation" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}
		
		_Alpha("Alpha", Range(0, 1)) = 0.5
		
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}
		SubShader
		{
			Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
			LOD 200					
			Cull Back  
			
			// extra pass that renders to depth buffer only
			Pass {
				ZWrite On
				ColorMask 0
			}
			
			
			CGPROGRAM

			sampler2D _MainTex;
			sampler2D _NormalMap;

			float _Alpha;
			float _OffSet_X;
			float _OffSet_Y;
			
			fixed4 _Color;
			sampler2D _Ramp;

			struct Input {
				float2 uv_MainTex;
				float2 uv__MainTexFace;
				float2 uv_NormalMap;
				float3 worldPos;
			};


			#pragma surface surf Lambert alpha
			#pragma lighting ToonRamp exclude_path:prepass


			UNITY_INSTANCING_CBUFFER_START(Props)
			UNITY_INSTANCING_CBUFFER_END

			inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten)
		{
			#ifndef USING_DIRECTIONAL_LIGHT
			lightDir = normalize(lightDir);
			#endif

			half d = dot(s.Normal, lightDir)*0.5 + 0.5;
			half3 ramp = tex2D(_Ramp, float2(d,d)).rgb;

			half4 c;
			c.rgb = s.Albedo * _LightColor0.rgb * ramp * (atten * 2);
			c.a = 0;
			return c;
		}

		void surf(Input IN, inout SurfaceOutput o) 
		{
			fixed4 color = tex2D(_MainTex, IN.uv_MainTex - float2 (_OffSet_X ,_OffSet_Y )) * _Color;		

			if (color.a < 0.7f)
			{
				color.r = 1;
				color.g = 1;
				color.b = 1;
			}

			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
			o.Albedo = color.rgb;			
			o.Alpha = _Alpha;
		}
		ENDCG
		
		
	}
		FallBack "Transparent/VertexLit"


}