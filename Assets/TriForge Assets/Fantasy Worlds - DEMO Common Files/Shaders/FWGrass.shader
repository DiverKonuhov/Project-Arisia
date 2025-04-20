// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriForge/Fantasy Worlds/FWGrass"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 1
		_BaseColorSaturation("Base Color Saturation", Range( 0 , 1)) = 1
		_NormalScale("Normal Scale", Float) = 1
		_RootColor("Root Color", Color) = (0.2235294,0.2431373,0.1058824,1)
		_RootMaskStrength("Root Mask Strength", Float) = 2
		_WindStrength("Wind Strength", Range( 0 , 1)) = 1
		_WindRootMaskStrength("Wind Root Mask Strength", Range( 0 , 1)) = 1
		_SecondaryBendingStrength("Secondary Bending Strength", Range( 0 , 1)) = 0
		_WindDirectionMap("Wind Direction Map", 2D) = "white" {}
		_WindMap("Wind Map", 2D) = "white" {}
		_RotationMapInfluence("Rotation Map Influence", Range( 0 , 1)) = 0
		_WindRotationMapSpeed("Wind Rotation Map Speed", Range( 0 , 1)) = 1
		_HFRotationMapInfluence("HF Rotation Map Influence", Range( 0 , 1)) = 0.35
		_MaskMap("Mask Map", 2D) = "white" {}
		_MainBendingStrength("Main Bending Strength", Range( 0 , 5)) = 1
		_NormalMap("Normal Map", 2D) = "bump" {}
		[Toggle(_TFW_FLIPNORMALS)] _FlipBackNormals("Flip Back Normals", Float) = 0
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		_BaseColor("Base Color", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,0)
		[Toggle(_TF_ENABLE_WIND_ON)] _TF_ENABLE_WIND("TF_ENABLE_WIND", Float) = 1
		_FadeFalloff("Fade Falloff", Range( 1 , 5)) = 2
		_FadeDistance("Fade Distance", Float) = 30
		[HideInInspector] _texcoord3( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "DisableBatching" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma shader_feature _TF_ENABLE_WIND_ON
		#pragma shader_feature _TFW_FLIPNORMALS
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows nolightmap  dithercrossfade vertex:vertexDataFunc 
		struct Input
		{
			float3 worldPos;
			float2 uv_texcoord;
			half ASEIsFrontFacing : VFACE;
			float2 uv3_texcoord3;
		};

		uniform float _WindRootMaskStrength;
		uniform float3 TF_WIND_DIRECTION;
		uniform sampler2D _WindDirectionMap;
		uniform float _WindRotationMapSpeed;
		uniform float _HFRotationMapInfluence;
		uniform float _RotationMapInfluence;
		uniform float TF_ROTATION_MAP_INFLUENCE;
		uniform sampler2D _WindMap;
		uniform float _MainBendingStrength;
		uniform float _SecondaryBendingStrength;
		uniform float _WindStrength;
		uniform float TF_WIND_STRENGTH;
		uniform float TF_GRASS_WIND_STRENGTH;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _NormalScale;
		uniform float4 _RootColor;
		uniform float4 _Color;
		uniform sampler2D _BaseColor;
		uniform float4 _BaseColor_ST;
		uniform float _BaseColorSaturation;
		uniform float _RootMaskStrength;
		uniform sampler2D _MaskMap;
		uniform float4 _MaskMap_ST;
		uniform float _Smoothness;
		uniform float _FadeDistance;
		uniform float _FadeFalloff;
		uniform float _Cutoff = 0.5;


		float3 mod2D289( float3 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float2 mod2D289( float2 x ) { return x - floor( x * ( 1.0 / 289.0 ) ) * 289.0; }

		float3 permute( float3 x ) { return mod2D289( ( ( x * 34.0 ) + 1.0 ) * x ); }

		float snoise( float2 v )
		{
			const float4 C = float4( 0.211324865405187, 0.366025403784439, -0.577350269189626, 0.024390243902439 );
			float2 i = floor( v + dot( v, C.yy ) );
			float2 x0 = v - i + dot( i, C.xx );
			float2 i1;
			i1 = ( x0.x > x0.y ) ? float2( 1.0, 0.0 ) : float2( 0.0, 1.0 );
			float4 x12 = x0.xyxy + C.xxzz;
			x12.xy -= i1;
			i = mod2D289( i );
			float3 p = permute( permute( i.y + float3( 0.0, i1.y, 1.0 ) ) + i.x + float3( 0.0, i1.x, 1.0 ) );
			float3 m = max( 0.5 - float3( dot( x0, x0 ), dot( x12.xy, x12.xy ), dot( x12.zw, x12.zw ) ), 0.0 );
			m = m * m;
			m = m * m;
			float3 x = 2.0 * frac( p * C.www ) - 1.0;
			float3 h = abs( x ) - 0.5;
			float3 ox = floor( x + 0.5 );
			float3 a0 = x - ox;
			m *= 1.79284291400159 - 0.85373472095314 * ( a0 * a0 + h * h );
			float3 g;
			g.x = a0.x * x0.x + h.x * x0.y;
			g.yz = a0.yz * x12.xz + h.yz * x12.yw;
			return 130.0 * dot( m, g );
		}


		float3 RotateAroundAxis( float3 center, float3 original, float3 u, float angle )
		{
			original -= center;
			float C = cos( angle );
			float S = sin( angle );
			float t = 1 - C;
			float m00 = t * u.x * u.x + C;
			float m01 = t * u.x * u.y - S * u.z;
			float m02 = t * u.x * u.z + S * u.y;
			float m10 = t * u.x * u.y + S * u.z;
			float m11 = t * u.y * u.y + C;
			float m12 = t * u.y * u.z - S * u.x;
			float m20 = t * u.x * u.z - S * u.y;
			float m21 = t * u.y * u.z + S * u.x;
			float m22 = t * u.z * u.z + C;
			float3x3 finalMatrix = float3x3( m00, m01, m02, m10, m11, m12, m20, m21, m22 );
			return mul( finalMatrix, original ) + center;
		}


		void vertexDataFunc( inout appdata_full v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float temp_output_86_0 = ( 1.0 - v.texcoord2.xy.y );
			float saferPower141 = abs( ( temp_output_86_0 * 1.0 ) );
			float ifLocalVar273 = 0;
			if( TF_WIND_DIRECTION.x == 0.0 )
				ifLocalVar273 = 0.0;
			else
				ifLocalVar273 = 1.0;
			float ifLocalVar274 = 0;
			if( TF_WIND_DIRECTION.z == 0.0 )
				ifLocalVar274 = 0.0;
			else
				ifLocalVar274 = 1.0;
			float3 lerpResult278 = lerp( float3(0,0,1) , TF_WIND_DIRECTION , ( ifLocalVar273 + ifLocalVar274 ));
			float3 WindVector251 = lerpResult278;
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 temp_output_182_0 = (( ase_worldPos / -200.0 )).xz;
			float2 panner176 = ( 1.0 * _Time.y * float2( 0.04,0 ) + temp_output_182_0);
			float2 panner220 = ( 1.0 * _Time.y * float2( 0,0 ) + temp_output_182_0);
			float4 lerpResult223 = lerp( tex2Dlod( _WindDirectionMap, float4( panner176, 0, 0.0) ) , tex2Dlod( _WindDirectionMap, float4( panner220, 0, 0.0) ) , _WindRotationMapSpeed);
			float2 RotationMapLF162 = (lerpResult223).rg;
			float2 break180 = ( (float2( -0.5,-0.5 ) + (RotationMapLF162 - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * 2.0 );
			float3 appendResult181 = (float3(( break180.x * -1.0 ) , 0.0 , break180.y));
			float2 RotationMapHF187 = (lerpResult223).ba;
			float2 break190 = ( (float2( -0.5,-0.5 ) + (RotationMapHF187 - float2( 0,0 )) * (float2( 0.5,0.5 ) - float2( -0.5,-0.5 )) / (float2( 1,1 ) - float2( 0,0 ))) * 1.0 );
			float3 appendResult191 = (float3(( break190.x * -1.0 ) , 0.0 , break190.y));
			float3 lerpResult230 = lerp( appendResult181 , appendResult191 , _HFRotationMapInfluence);
			float3 lerpResult209 = lerp( WindVector251 , lerpResult230 , ( _RotationMapInfluence * TF_ROTATION_MAP_INFLUENCE ));
			float3 worldToObjDir207 = mul( unity_WorldToObject, float4( lerpResult209, 0 ) ).xyz;
			float3 WindDirection212 = worldToObjDir207;
			float2 panner90 = ( 1.0 * _Time.y * float2( 0.02,0 ) + (( ase_worldPos / -200.0 )).xyz.xy);
			float3 objToWorld234 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float2 panner78 = ( 1.0 * _Time.y * float2( 1,0 ) + ( ( ( v.texcoord1.xy + float2( 2,2 ) ) * 50.0 ) + ( (objToWorld234).xz * 1.0 ) ));
			float simplePerlin2D76 = snoise( panner78 );
			float3 appendResult45 = (float3(( -1.0 * v.texcoord1.xy.x ) , 0.0 , ( -1.0 * v.texcoord1.xy.y )));
			float3 PivotCoords138 = ( 0.01 * appendResult45 );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 rotatedValue53 = RotateAroundAxis( PivotCoords138, ase_vertex3Pos, normalize( WindDirection212 ), ( ( radians( ( ( (0.3 + ((tex2Dlod( _WindMap, float4( panner90, 0, 0.0) )).r - 0.0) * (1.0 - 0.3) / (1.0 - 0.0)) * 100.0 * _MainBendingStrength ) + ( simplePerlin2D76 * _SecondaryBendingStrength * 30.0 ) ) ) * _WindStrength * TF_WIND_STRENGTH * TF_GRASS_WIND_STRENGTH ) * v.texcoord3.xy.x ) );
			#ifdef _TF_ENABLE_WIND_ON
				float3 staticSwitch269 = ( saturate( ( pow( saferPower141 , _WindRootMaskStrength ) * 1.0 ) ) * ( rotatedValue53 - ase_vertex3Pos ) );
			#else
				float3 staticSwitch269 = float3( 0,0,0 );
			#endif
			v.vertex.xyz += staticSwitch269;
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode11 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ), _NormalScale );
			float3 break4 = tex2DNode11;
			float switchResult9 = (((i.ASEIsFrontFacing>0)?(break4.z):(-break4.z)));
			float3 appendResult6 = (float3(break4.x , break4.y , switchResult9));
			#ifdef _TFW_FLIPNORMALS
				float3 staticSwitch7 = appendResult6;
			#else
				float3 staticSwitch7 = tex2DNode11;
			#endif
			float3 FinalNormal8 = staticSwitch7;
			o.Normal = FinalNormal8;
			float2 uv_BaseColor = i.uv_texcoord * _BaseColor_ST.xy + _BaseColor_ST.zw;
			float4 tex2DNode1 = tex2D( _BaseColor, uv_BaseColor );
			float3 desaturateInitialColor40 = tex2DNode1.rgb;
			float desaturateDot40 = dot( desaturateInitialColor40, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar40 = lerp( desaturateInitialColor40, desaturateDot40.xxx, ( 1.0 - _BaseColorSaturation ) );
			float4 BaseColor34 = ( _Color * float4( desaturateVar40 , 0.0 ) );
			float temp_output_86_0 = ( 1.0 - i.uv3_texcoord3.y );
			float saferPower127 = abs( saturate( ( _RootMaskStrength + temp_output_86_0 ) ) );
			float RootMask261 = pow( saferPower127 , 1.0 );
			float4 lerpResult125 = lerp( _RootColor , BaseColor34 , RootMask261);
			o.Albedo = lerpResult125.rgb;
			float2 uv_MaskMap = i.uv_texcoord * _MaskMap_ST.xy + _MaskMap_ST.zw;
			o.Smoothness = ( tex2D( _MaskMap, uv_MaskMap ).a * _Smoothness );
			o.Alpha = 1;
			float OpacityMap35 = tex2DNode1.a;
			float3 ase_worldPos = i.worldPos;
			float Distance_Mask290 = ( 1.0 - saturate( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / _FadeDistance ) , _FadeFalloff ) ) );
			float lerpResult297 = lerp( 0.0 , OpacityMap35 , Distance_Mask290);
			float Opacity300 = lerpResult297;
			clip( Opacity300 - _Cutoff );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
