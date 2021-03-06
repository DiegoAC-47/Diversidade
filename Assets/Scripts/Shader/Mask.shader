﻿Shader "Custom/Mask" {
	Properties {
		_MainTex ("TextureBlackMirror", 2D) = "white" {}
		_MainTexFace ("TextureFace", 2D) = "white" {}
		_OffSet_X ("OffSet_X", float) = 0
		_OffSet_Y ("OffSet_Y", float) = 0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		

		struct Input {
			float2 uv_MainTex;
		};
		
		sampler2D _MainTex;
		sampler2D _MainTexFace;
		
		float _OffSet_X;
		float _OffSet_Y; 

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
		
			fixed4 colF = tex2D(_MainTexFace, IN.uv_MainTex);
			fixed4 col = tex2D(_MainTex, IN.uv_MainTex - float2 (_OffSet_X ,_OffSet_Y ));
			
			if(colF.a > 0.6f)
			{				
				
				col = colF;
				
			}		
		
			o.Albedo = col.rgb;
			o.Alpha = col.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
