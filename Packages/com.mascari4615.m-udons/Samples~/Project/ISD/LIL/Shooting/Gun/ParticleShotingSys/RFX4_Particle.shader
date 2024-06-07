Shader "Particles/Particles RFX4" {
	Properties 
	{
		[HDR]_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Particle Texture", 2D) = "white" {}
		[Enum(OFF, 0, ON, 1, ON ALPHA, 2)]CM("Cutout mode", Int) = 0
	 	_Cutout ("Cutout", Float) = 0.2
        [Space]
        [Toggle(_)]SOFTP("Soft Particles", int) = 0
        [Toggle(_)]LA("Light Attention", int) = 0
        _InvFade("Soft Particles Factor", Float) = 1.0
        [Header(Blending)]
        [Space]
		[Enum(UnityEngine.Rendering.BlendMode)] _SourceBlend ("Source Blend", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _DestinationBlend ("Destination Blend", Float) = 10
        [Header(Additive One One)]
        [Header(AlphaBlended SrcAlpha OneMinusSrcAlpha)]
        [Space]
        _("", int) = 0
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend [_SourceBlend] [_DestinationBlend]
		Cull Off 
		ZWrite Off
		Pass {
			Tags { "LightMode"="ForwardBase" }
			CGPROGRAM
			#pragma target 4.0
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile SOFTPARTICLES_OFF SOFTPARTICLES_ON
			#include "UnityCG.cginc"
			#include "AutoLight.cginc"
			#include "Lighting.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 wPos : TEXCOORD1;
				#ifdef SOFTPARTICLES_ON
					float4 projPos : TEXCOORD2;
				#endif
			};

			uniform sampler2D _MainTex, _CameraDepthTexture;
			uniform float4 _MainTex_ST, _TintColor;
			uniform float _Cutout, _InvFade;
			uniform int CM, SOFTP, LA, _SourceBlend, _DestinationBlend;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				#ifdef SOFTPARTICLES_ON
					o.projPos = ComputeScreenPos(o.vertex);
					COMPUTE_EYEDEPTH(o.projPos.z);
				#endif
				o.wPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.color = v.color;
				o.texcoord = v.texcoord;
				return o;
			}

			half4 frag (v2f i) : SV_Target
			{
				float fade = 1.0;
				#ifdef SOFTPARTICLES_ON
					fade = saturate(_InvFade * (LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - i.projPos.z));
				#endif
				UNITY_BRANCH if (SOFTP)
				{
					i.color.a *= fade;
				}
				
				half4 tex = tex2D(_MainTex, TRANSFORM_TEX(i.texcoord, _MainTex));
				half4 res = float4(2 * tex.rgb * _TintColor, tex.a * _TintColor.a);

				UNITY_BRANCH if (CM == 1)
				{
					res.a = step(_Cutout, tex.a) * res.a;
				}

				UNITY_BRANCH if (CM == 2)
				{
					res.a = step(1.0 - i.color.a + _Cutout, tex.a);
				}

				UNITY_BRANCH if (_SourceBlend == 5 && _DestinationBlend == 10)
				{
					res *= i.color;
				}
				
				else
				{
					res.rgb *= i.color.a;
					res.rgb *= i.color.rgb;
				}

				UNITY_LIGHT_ATTENUATION(Attenuation, i, i.wPos);

				float3 lightning = saturate(_LightColor0.xyz * Attenuation + ShadeSH9(half4(0.0, 0.0, 0.0, 1.0)).xyz);

				UNITY_BRANCH if (LA != 0)
				{
					res.rgb *= lightning;
				}

				res.a = saturate(res.a);
				return res;
			}
			ENDCG 
		}
	}	
}