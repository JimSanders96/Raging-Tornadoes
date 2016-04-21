﻿Shader "Custom/HandShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_Alpha ("Alpha", Range(0,1)) = 0.5
					
      	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
      	
      	
      	_RimInvColor ("Inv Rim Color", Color) = (0.26,0.19,0.16,0.0)
      	_RimInvPower ("Inv Rim Power", Range(0.5,8.0)) = 3.0
	}
	SubShader {
 		Tags { "Queue" = "Transparent+1000" "IgnoreProjector" = "true" "RenderType"="Transparent" "PerformanceChecks"="False" }
		LOD 300
		
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite On
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
          	float3 viewDir;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

      	float4 _RimColor;
      	float _RimPower;
      	
      	float4 _RimInvColor;
      	float _RimInvPower;
      	
      	float _Alpha;
      	
		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = _Alpha;
			
			
	        half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
    	    o.Emission = _RimColor.rgb * pow (rim, _RimPower) + _RimInvColor.rgb  * pow (1.0 -rim, _RimInvPower);	
		}
		ENDCG
	} 
	FallBack "Diffuse"
}