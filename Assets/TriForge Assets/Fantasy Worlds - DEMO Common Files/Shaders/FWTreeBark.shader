// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriForge/Fantasy Worlds/FWTreeBark"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Color("Color", Color) = (1,1,1,0)
		_BaseColor("Base Color", 2D) = "white" {}
		_BendingMaskStrength1("Bending Mask Strength", Range( 0.05 , 2)) = 1.236128
		_NormalMap("Normal Map", 2D) = "bump" {}
		_MaskMapMAOS("Mask Map (M, AO, S)", 2D) = "white" {}
		_WindOverallStrength("Wind Overall Strength", Range( 0 , 1)) = 1
		_ChildWindStrength("Child Wind Strength", Range( 0 , 2)) = 0.5
		_ParentWindStrength("Parent Wind Strength", Range( 0 , 2)) = 0.5
		_ParentWindMapScale("Parent Wind Map Scale", Range( 0 , 5)) = 1
		_MainWindStrength("Main Wind Strength", Range( 0 , 2)) = 0.5
		_MainWindScale("Main Wind Scale", Range( 0 , 1)) = 1
		_MainBendMaskStrength("Main Bend Mask Strength", Range( 0 , 5)) = 0
		_AOIntensity("AO Intensity", Range( 0 , 1)) = 1
		_VertexAOIntensity("Vertex AO Intensity", Range( 0 , 1)) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "DisableBatching" = "True" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma surface surf Standard keepalpha addshadow fullforwardshadows nolightmap  dithercrossfade vertex:vertexDataFunc 

		struct appdata_full_custom
		{
			float4 vertex : POSITION;
			float4 tangent : TANGENT;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 texcoord3 : TEXCOORD3;
			float4 color : COLOR;
			UNITY_VERTEX_INPUT_INSTANCE_ID
			float4 ase_texcoord4 : TEXCOORD4;
		};
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float3 TF_WIND_DIRECTION;
		uniform float _ChildWindStrength;
		uniform float _BendingMaskStrength1;
		uniform float _ParentWindMapScale;
		uniform float _ParentWindStrength;
		uniform float _MainBendMaskStrength;
		uniform float _MainWindScale;
		uniform float _MainWindStrength;
		uniform float _WindOverallStrength;
		uniform float TF_WIND_STRENGTH;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform sampler2D _BaseColor;
		uniform float4 _BaseColor_ST;
		uniform float4 _Color;
		uniform sampler2D _MaskMapMAOS;
		uniform float4 _MaskMapMAOS_ST;
		uniform float _Smoothness;
		uniform float _AOIntensity;
		uniform float _VertexAOIntensity;


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


		void vertexDataFunc( inout appdata_full_custom v, out Input o )
		{
			UNITY_INITIALIZE_OUTPUT( Input, o );
			float3 appendResult18_g1 = (float3(( -1.0 * v.texcoord2.xy.y ) , ( -1.0 * v.texcoord3.xy.y ) , v.texcoord3.xy.x));
			float3 temp_output_20_0_g1 = ( 0.001 * appendResult18_g1 );
			float dotResult16_g1 = dot( temp_output_20_0_g1 , temp_output_20_0_g1 );
			float ifLocalVar182_g1 = 0;
			if( dotResult16_g1 > 0.0001 )
				ifLocalVar182_g1 = 1.0;
			else if( dotResult16_g1 < 0.0001 )
				ifLocalVar182_g1 = 0.0;
			float ChildMask26_g1 = saturate( ( ifLocalVar182_g1 * 100.0 ) );
			float SelfBendMask34_g1 = ( 1.0 - v.ase_texcoord4.xy.y );
			float ifLocalVar368 = 0;
			if( TF_WIND_DIRECTION.x == 0.0 )
				ifLocalVar368 = 0.0;
			else
				ifLocalVar368 = 1.0;
			float ifLocalVar369 = 0;
			if( TF_WIND_DIRECTION.z == 0.0 )
				ifLocalVar369 = 0.0;
			else
				ifLocalVar369 = 1.0;
			float3 lerpResult355 = lerp( float3(0,0,1) , TF_WIND_DIRECTION , ( ifLocalVar368 + ifLocalVar369 ));
			float3 worldToObjDir361 = normalize( mul( unity_WorldToObject, float4( lerpResult355, 0 ) ).xyz );
			float3 WindDir362 = worldToObjDir361;
			float3 WindVector226_g1 = WindDir362;
			float3 appendResult11_g1 = (float3(( -1.0 * v.texcoord1.xy.x ) , v.texcoord2.xy.x , ( -1.0 * v.texcoord1.xy.y )));
			float3 temp_output_19_0_g1 = ( 0.001 * appendResult11_g1 );
			float3 SelfPivot28_g1 = temp_output_19_0_g1;
			float3 objToWorld54_g1 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float2 temp_cast_0 = ((( ( SelfPivot28_g1 * 1.0 ) + ( objToWorld54_g1 / -2.0 ) )).z).xx;
			float2 panner48_g1 = ( 1.0 * _Time.y * float2( 0,0.85 ) + temp_cast_0);
			float simplePerlin2D53_g1 = snoise( panner48_g1*2.0 );
			simplePerlin2D53_g1 = simplePerlin2D53_g1*0.5 + 0.5;
			float ChildRotation43_g1 = radians( ( simplePerlin2D53_g1 * 12.0 * _ChildWindStrength ) );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 rotatedValue81_g1 = RotateAroundAxis( SelfPivot28_g1, ase_vertex3Pos, WindVector226_g1, ChildRotation43_g1 );
			float3 ChildRotationResult119_g1 = ( ( ChildMask26_g1 * SelfBendMask34_g1 ) * ( rotatedValue81_g1 - ase_vertex3Pos ) );
			float temp_output_113_0_g1 = saturate( ( 4.0 * pow( SelfBendMask34_g1 , _BendingMaskStrength1 ) ) );
			float dotResult9_g1 = dot( temp_output_19_0_g1 , temp_output_19_0_g1 );
			float ifLocalVar189_g1 = 0;
			if( dotResult9_g1 > 0.0001 )
				ifLocalVar189_g1 = 1.0;
			else if( dotResult9_g1 < 0.0001 )
				ifLocalVar189_g1 = 0.0;
			float TrunkMask29_g1 = saturate( ( ifLocalVar189_g1 * 1000.0 ) );
			float3 ParentPivot27_g1 = temp_output_20_0_g1;
			float3 lerpResult51_g1 = lerp( SelfPivot28_g1 , ParentPivot27_g1 , ChildMask26_g1);
			float2 temp_cast_1 = ((lerpResult51_g1).z).xx;
			float2 panner61_g1 = ( 1.0 * _Time.y * float2( 0,0.45 ) + temp_cast_1);
			float simplePerlin2D60_g1 = snoise( panner61_g1*_ParentWindMapScale );
			simplePerlin2D60_g1 = simplePerlin2D60_g1*0.5 + 0.5;
			float saferPower185_g1 = abs( simplePerlin2D60_g1 );
			float ParentRotation63_g1 = radians( ( pow( saferPower185_g1 , 3.0 ) * 25.0 * _ParentWindStrength ) );
			float3 lerpResult98_g1 = lerp( SelfPivot28_g1 , ParentPivot27_g1 , ChildMask26_g1);
			float3 rotatedValue96_g1 = RotateAroundAxis( lerpResult98_g1, ( ChildRotationResult119_g1 + ase_vertex3Pos ), WindVector226_g1, ParentRotation63_g1 );
			float saferPower160_g1 = abs( v.ase_texcoord4.xy.x );
			float MainBendMask35_g1 = saturate( pow( saferPower160_g1 , _MainBendMaskStrength ) );
			float3 ParentRotationResult131_g1 = ( ChildRotationResult119_g1 + ( ( ( ( ( temp_output_113_0_g1 * ( 1.0 - ChildMask26_g1 ) ) + ChildMask26_g1 ) * TrunkMask29_g1 ) * ( rotatedValue96_g1 - ase_vertex3Pos ) ) * MainBendMask35_g1 ) );
			float3 objToWorld47_g1 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 saferPower172_g1 = abs( ( objToWorld47_g1 / ( -15.0 * _MainWindScale ) ) );
			float2 panner71_g1 = ( 1.0 * _Time.y * float2( 0,0.07 ) + (pow( saferPower172_g1 , 2.0 )).xz);
			float simplePerlin2D70_g1 = snoise( panner71_g1*2.0 );
			simplePerlin2D70_g1 = simplePerlin2D70_g1*0.5 + 0.5;
			float MainRotation67_g1 = radians( ( simplePerlin2D70_g1 * 25.0 * _MainWindStrength ) );
			float3 temp_output_125_0_g1 = ( ParentRotationResult131_g1 + ase_vertex3Pos );
			float3 rotatedValue121_g1 = RotateAroundAxis( float3(0,0,0), temp_output_125_0_g1, WindVector226_g1, MainRotation67_g1 );
			float temp_output_148_0_g1 = pow( MainBendMask35_g1 , 5.0 );
			v.vertex.xyz += ( ( ParentRotationResult131_g1 + ( ( rotatedValue121_g1 - temp_output_125_0_g1 ) * temp_output_148_0_g1 ) ) * _WindOverallStrength * TF_WIND_STRENGTH );
			v.vertex.w = 1;
		}

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			o.Normal = UnpackNormal( tex2D( _NormalMap, uv_NormalMap ) );
			float2 uv_BaseColor = i.uv_texcoord * _BaseColor_ST.xy + _BaseColor_ST.zw;
			o.Albedo = ( tex2D( _BaseColor, uv_BaseColor ) * _Color ).rgb;
			float2 uv_MaskMapMAOS = i.uv_texcoord * _MaskMapMAOS_ST.xy + _MaskMapMAOS_ST.zw;
			float4 tex2DNode235 = tex2D( _MaskMapMAOS, uv_MaskMapMAOS );
			o.Smoothness = ( tex2DNode235.a * _Smoothness );
			o.Occlusion = saturate( ( saturate( ( ( 1.0 - _AOIntensity ) + tex2DNode235.g ) ) - ( 1.0 - saturate( ( ( 1.0 - _VertexAOIntensity ) + i.vertexColor.r ) ) ) ) );
			o.Alpha = 1;
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}