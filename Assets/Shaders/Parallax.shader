//
// Parallax Shader
// A simple Shader which uses a texture to recreate a parallax effect 
// Based on the Unity's build-in parallax bump mapping shader.
//

Shader "Custom/Parallax" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Parallax ("Parallax", Range (0, 1)) = 0.25
	}
	
	SubShader {
		Tags { "RenderType" = "Opaque" }
		LOD 500		
	
		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _MainTex;
		fixed4 _Color;
		float _Parallax;

		struct Input
		{
			float2 uv_MainTex;
			float3 viewDir;
		};

		// Calculates UV offset for parallax
		inline float2 _ParallaxOffset(half height, half3 viewDir)
		{
			float2 h = height - height/4.0;
			float3 v = normalize(viewDir);
			v.z += 0.42;
			return h * (v.xy / v.z);
		}

		void surf (Input IN, inout SurfaceOutput o)
		{		
			float2 parallaxOffset = _ParallaxOffset(_Parallax, IN.viewDir);
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex + parallaxOffset) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
	
		ENDCG
	}

	FallBack "Legacy Shaders/Diffuse"
}
