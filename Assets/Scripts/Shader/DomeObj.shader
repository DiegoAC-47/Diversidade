Shader "Custom/DomeObj" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_NormalMap("Normal Map", 2D) = "bump" {}

		_MainTexBeta("Texture Under Dome", 2D) = "white" {}
		_Radius("Raio", float) = 10
		_Center("Centro", Vector) = (0,0,0,0)

		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Outline("Outline width", Range(.002, 0.03)) = .005
		_Ramp("Toon Ramp (RGB)", 2D) = "gray" {}
	}
		SubShader
		{
			Tags { "RenderType"="Opaque" }
			LOD 200

			CGPROGRAM

			sampler2D _MainTex;
			sampler2D _MainTexBeta;
			sampler2D _NormalMap;
			float _Radius;
			fixed4 _Center;

			fixed4 _Color;
			sampler2D _Ramp;

			struct Input {
				float2 uv_MainTex;
				float2 uv_NormalMap;
				float3 worldPos;
			};


			#pragma surface surf ToonRamp
			#pragma lighting ToonRamp exclude_path:prepass


			UNITY_INSTANCING_CBUFFER_START(Props)
			UNITY_INSTANCING_CBUFFER_END

			inline half4 LightingToonRamp(SurfaceOutput s, half3 lightDir, half atten){
			
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

				fixed4 c;

				float dist = distance(float2(_Center.x, _Center.z)
					, float2(IN.worldPos.x, IN.worldPos.z));

				if (dist < _Radius)
				{
					c = tex2D(_MainTexBeta, IN.uv_MainTex) * _Color;
				}
				else
				{
					c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				}
				o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));			
				o.Albedo = c.rgb;
			}

			ENDCG
		}
		
		FallBack "Diffuse"

}