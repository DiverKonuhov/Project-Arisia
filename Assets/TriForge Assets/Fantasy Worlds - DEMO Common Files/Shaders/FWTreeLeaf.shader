// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "TriForge/Fantasy Worlds/FWTreeLeaf"
{
	Properties
	{
		_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_BaseColor("Base Color", 2D) = "white" {}
		_BendingMaskStrength1("Bending Mask Strength", Range( 0.05 , 2)) = 1.236128
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_NormalMap("Normal Map", 2D) = "bump" {}
		_NormalScale("Normal Scale", Float) = 1
		_LeafFlutterStrength("Leaf Flutter Strength", Range( 0 , 2)) = 0.3
		_WindOverallStrength("Wind Overall Strength", Range( 0 , 1)) = 1
		_ParentWindStrength("Parent Wind Strength", Range( 0 , 2)) = 0.5
		_ParentWindMapScale("Parent Wind Map Scale", Range( 0 , 5)) = 1
		_AOIntensity("AO Intensity", Range( 0 , 1)) = 1
		_MaskClip("Mask Clip", Range( 0 , 1)) = 0.5588235
		_Color("Color", Color) = (1,1,1,0)
		_VertexAOIntensity("Vertex AO Intensity", Range( 0 , 1)) = 1
		_BaseColorSaturation("Base Color Saturation", Range( 0 , 1)) = 1
		_ChildWindStrength("Child Wind Strength", Range( 0 , 2)) = 0.5
		_ChildWindMapScale("Child Wind Map Scale", Range( 0 , 5)) = 0
		[Toggle(_DISTANCEBASEDMASKCLIP_ON)] _DistanceBasedMaskClip("Distance Based Mask Clip", Float) = 1
		_MainWindStrength("Main Wind Strength", Range( 0 , 2)) = 0.5
		_MainWindScale("Main Wind Scale", Range( 0 , 1)) = 1
		_MainBendMaskStrength("Main Bend Mask Strength", Range( 0 , 5)) = 0
		_Undercolor("Undercolor", Color) = (1,1,1,0)
		_UndercolorAmount("Undercolor Amount", Range( 0 , 1)) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
		[Header(Forward Rendering Options)]
		[ToggleOff] _SpecularHighlights("Specular Highlights", Float) = 1.0
		[ToggleOff] _GlossyReflections("Reflections", Float) = 1.0
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" }
		Cull Off
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#include "UnityPBSLighting.cginc"
		#pragma target 3.0
		#pragma multi_compile_instancing
		#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
		#pragma shader_feature _GLOSSYREFLECTIONS_OFF
		#pragma shader_feature_local _DISTANCEBASEDMASKCLIP_ON
		#pragma surface surf StandardCustom keepalpha addshadow fullforwardshadows exclude_path:deferred nolightmap  dithercrossfade vertex:vertexDataFunc 

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
			float3 worldPos;
			float2 uv_texcoord;
			float3 vertexToFrag194;
			float4 vertexColor : COLOR;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Translucency;
		};

		uniform float3 TF_WIND_DIRECTION;
		uniform float _ChildWindMapScale;
		uniform float _ChildWindStrength;
		uniform float _BendingMaskStrength1;
		uniform float _ParentWindMapScale;
		uniform float _ParentWindStrength;
		uniform float _MainBendMaskStrength;
		uniform float _MainWindScale;
		uniform float _MainWindStrength;
		uniform float _WindOverallStrength;
		uniform float TF_WIND_STRENGTH;
		uniform float _LeafFlutterStrength;
		uniform sampler2D _NormalMap;
		uniform float4 _NormalMap_ST;
		uniform float _NormalScale;
		uniform float4 _Undercolor;
		uniform sampler2D _BaseColor;
		uniform float4 _BaseColor_ST;
		uniform float _BaseColorSaturation;
		uniform float4 _Color;
		uniform float _UndercolorAmount;
		uniform float _Smoothness;
		uniform float _VertexAOIntensity;
		uniform float _AOIntensity;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;
		uniform float _MaskClip;


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
			float3 appendResult18_g2 = (float3(( -1.0 * v.texcoord2.xy.y ) , ( -1.0 * v.texcoord3.xy.y ) , v.texcoord3.xy.x));
			float3 temp_output_20_0_g2 = ( 0.001 * appendResult18_g2 );
			float dotResult16_g2 = dot( temp_output_20_0_g2 , temp_output_20_0_g2 );
			float ifLocalVar182_g2 = 0;
			if( dotResult16_g2 > 0.0001 )
				ifLocalVar182_g2 = 1.0;
			else if( dotResult16_g2 < 0.0001 )
				ifLocalVar182_g2 = 0.0;
			float ChildMask26_g2 = saturate( ( ifLocalVar182_g2 * 100.0 ) );
			float SelfBendMask34_g2 = ( 1.0 - v.ase_texcoord4.xy.y );
			float ifLocalVar220 = 0;
			if( TF_WIND_DIRECTION.x == 0.0 )
				ifLocalVar220 = 0.0;
			else
				ifLocalVar220 = 1.0;
			float ifLocalVar221 = 0;
			if( TF_WIND_DIRECTION.z == 0.0 )
				ifLocalVar221 = 0.0;
			else
				ifLocalVar221 = 1.0;
			float3 lerpResult227 = lerp( float3(0,0,1) , TF_WIND_DIRECTION , ( ifLocalVar220 + ifLocalVar221 ));
			float3 worldToObjDir226 = normalize( mul( unity_WorldToObject, float4( lerpResult227, 0 ) ).xyz );
			float3 WindDir225 = worldToObjDir226;
			float3 WindVector226_g2 = WindDir225;
			float3 appendResult11_g2 = (float3(( -1.0 * v.texcoord1.xy.x ) , v.texcoord2.xy.x , ( -1.0 * v.texcoord1.xy.y )));
			float3 temp_output_19_0_g2 = ( 0.001 * appendResult11_g2 );
			float3 SelfPivot28_g2 = temp_output_19_0_g2;
			float3 objToWorld54_g2 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float2 temp_cast_0 = ((( ( SelfPivot28_g2 * 1.0 ) + ( objToWorld54_g2 / -2.0 ) )).z).xx;
			float2 panner48_g2 = ( 1.0 * _Time.y * float2( 0,0.85 ) + temp_cast_0);
			float simplePerlin2D53_g2 = snoise( panner48_g2*_ChildWindMapScale );
			simplePerlin2D53_g2 = simplePerlin2D53_g2*0.5 + 0.5;
			float ChildRotation43_g2 = radians( ( simplePerlin2D53_g2 * 12.0 * _ChildWindStrength ) );
			float3 ase_vertex3Pos = v.vertex.xyz;
			float3 rotatedValue81_g2 = RotateAroundAxis( SelfPivot28_g2, ase_vertex3Pos, WindVector226_g2, ChildRotation43_g2 );
			float3 ChildRotationResult119_g2 = ( ( ChildMask26_g2 * SelfBendMask34_g2 ) * ( rotatedValue81_g2 - ase_vertex3Pos ) );
			float temp_output_113_0_g2 = saturate( ( 4.0 * pow( SelfBendMask34_g2 , _BendingMaskStrength1 ) ) );
			float dotResult9_g2 = dot( temp_output_19_0_g2 , temp_output_19_0_g2 );
			float ifLocalVar189_g2 = 0;
			if( dotResult9_g2 > 0.0001 )
				ifLocalVar189_g2 = 1.0;
			else if( dotResult9_g2 < 0.0001 )
				ifLocalVar189_g2 = 0.0;
			float TrunkMask29_g2 = saturate( ( ifLocalVar189_g2 * 1000.0 ) );
			float3 ParentPivot27_g2 = temp_output_20_0_g2;
			float3 lerpResult51_g2 = lerp( SelfPivot28_g2 , ParentPivot27_g2 , ChildMask26_g2);
			float2 temp_cast_1 = ((lerpResult51_g2).z).xx;
			float2 panner61_g2 = ( 1.0 * _Time.y * float2( 0,0.45 ) + temp_cast_1);
			float simplePerlin2D60_g2 = snoise( panner61_g2*_ParentWindMapScale );
			simplePerlin2D60_g2 = simplePerlin2D60_g2*0.5 + 0.5;
			float saferPower185_g2 = abs( simplePerlin2D60_g2 );
			float ParentRotation63_g2 = radians( ( pow( saferPower185_g2 , 3.0 ) * 25.0 * _ParentWindStrength ) );
			float3 lerpResult98_g2 = lerp( SelfPivot28_g2 , ParentPivot27_g2 , ChildMask26_g2);
			float3 rotatedValue96_g2 = RotateAroundAxis( lerpResult98_g2, ( ChildRotationResult119_g2 + ase_vertex3Pos ), WindVector226_g2, ParentRotation63_g2 );
			float saferPower160_g2 = abs( v.ase_texcoord4.xy.x );
			float MainBendMask35_g2 = saturate( pow( saferPower160_g2 , _MainBendMaskStrength ) );
			float3 ParentRotationResult131_g2 = ( ChildRotationResult119_g2 + ( ( ( ( ( temp_output_113_0_g2 * ( 1.0 - ChildMask26_g2 ) ) + ChildMask26_g2 ) * TrunkMask29_g2 ) * ( rotatedValue96_g2 - ase_vertex3Pos ) ) * MainBendMask35_g2 ) );
			float3 objToWorld47_g2 = mul( unity_ObjectToWorld, float4( float3( 0,0,0 ), 1 ) ).xyz;
			float3 saferPower172_g2 = abs( ( objToWorld47_g2 / ( -15.0 * _MainWindScale ) ) );
			float2 panner71_g2 = ( 1.0 * _Time.y * float2( 0,0.07 ) + (pow( saferPower172_g2 , 2.0 )).xz);
			float simplePerlin2D70_g2 = snoise( panner71_g2*2.0 );
			simplePerlin2D70_g2 = simplePerlin2D70_g2*0.5 + 0.5;
			float MainRotation67_g2 = radians( ( simplePerlin2D70_g2 * 25.0 * _MainWindStrength ) );
			float3 temp_output_125_0_g2 = ( ParentRotationResult131_g2 + ase_vertex3Pos );
			float3 rotatedValue121_g2 = RotateAroundAxis( float3(0,0,0), temp_output_125_0_g2, WindVector226_g2, MainRotation67_g2 );
			float temp_output_148_0_g2 = pow( MainBendMask35_g2 , 5.0 );
			float3 ase_worldPos = mul( unity_ObjectToWorld, v.vertex );
			float2 panner86 = ( 1.0 * _Time.y * float2( -0.2,0.4 ) + (( ase_worldPos / -8.0 )).xz);
			float simplePerlin2D85 = snoise( panner86*10.0 );
			simplePerlin2D85 = simplePerlin2D85*0.5 + 0.5;
			v.vertex.xyz += ( ( ( ParentRotationResult131_g2 + ( ( rotatedValue121_g2 - temp_output_125_0_g2 ) * temp_output_148_0_g2 ) ) * _WindOverallStrength * TF_WIND_STRENGTH ) + ( _LeafFlutterStrength * ( v.texcoord.xy.y * simplePerlin2D85 ) * TF_WIND_STRENGTH * 0.6 ) );
			v.vertex.w = 1;
			float3 ase_vertexNormal = v.normal.xyz;
			o.vertexToFrag194 = ase_vertexNormal;
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !defined(DIRECTIONAL)
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_NormalMap = i.uv_texcoord * _NormalMap_ST.xy + _NormalMap_ST.zw;
			float3 tex2DNode11 = UnpackScaleNormal( tex2D( _NormalMap, uv_NormalMap ), _NormalScale );
			float3 NormalMap46 = tex2DNode11;
			o.Normal = NormalMap46;
			float2 uv_BaseColor = i.uv_texcoord * _BaseColor_ST.xy + _BaseColor_ST.zw;
			float4 tex2DNode1 = tex2D( _BaseColor, uv_BaseColor );
			float4 BaseColor33 = tex2DNode1;
			float3 desaturateInitialColor183 = BaseColor33.rgb;
			float desaturateDot183 = dot( desaturateInitialColor183, float3( 0.299, 0.587, 0.114 ));
			float3 desaturateVar183 = lerp( desaturateInitialColor183, desaturateDot183.xxx, ( 1.0 - _BaseColorSaturation ) );
			float4 lerpResult186 = lerp( ( _Undercolor * float4( desaturateVar183 , 0.0 ) ) , ( _Color * float4( desaturateVar183 , 0.0 ) ) , saturate( ( i.vertexToFrag194.y - ( _UndercolorAmount * -2.0 ) ) ));
			float4 FinalColor37 = lerpResult186;
			o.Albedo = FinalColor37.rgb;
			o.Smoothness = _Smoothness;
			o.Occlusion = saturate( ( saturate( ( ( 1.0 - _VertexAOIntensity ) + i.vertexColor.r ) ) + ( 1.0 - _AOIntensity ) ) );
			float3 temp_cast_4 = (1.0).xxx;
			o.Translucency = temp_cast_4;
			o.Alpha = 1;
			float Opacity34 = tex2DNode1.a;
			float saferPower150 = abs( Opacity34 );
			float3 ase_worldPos = i.worldPos;
			float lerpResult167 = lerp( _MaskClip , ( _MaskClip * 0.4 ) , ( distance( ase_worldPos , _WorldSpaceCameraPos ) / 150.0 ));
			#ifdef _DISTANCEBASEDMASKCLIP_ON
				float staticSwitch196 = lerpResult167;
			#else
				float staticSwitch196 = _MaskClip;
			#endif
			float OpacityFinal157 = pow( saferPower150 , staticSwitch196 );
			clip( OpacityFinal157 - _MaskClip );
		}

		ENDCG
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
