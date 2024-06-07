// v 1.1
//Full Reworked
//Optimisation
Shader "Doppels shaders/Distortion Wave 3D v 1.1"
{
	Properties
	{
		_dspow("Distortion power", range(0,1)) = 0.2
		_wamt("Waves Amount", range(1,100)) = 25
		_wpow("Waves Power", range(0,1)) = 0.3
		_wms("Waves Moving Speed", range(0,1)) = 0.5
	}
	CustomEditor "DWGUI"
	SubShader
	{
		Tags { "RenderType"="Overlay" "Queue"="Overlay" }
		ZTest Always ZWrite Off Cull Front Blend SrcAlpha OneMinusSrcAlpha
		GrabPass {"_bt"}
		Pass
		{
			CGPROGRAM
			#pragma vertex DoppelgangerScreenShaderVert
			#pragma fragment DoppelgangerScreenShaderFrag
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex 		: POSITION;
				float3 normal 		: NORMAL;
				float4 vertexColor 	: COLOR;
				float3 Cen 			: TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex 		: SV_POSITION;
				float3 wNorm 		: NORMAL;
				float vca 			: COLOR;
				float4 grabuv 		: TEXCOORD0;
				float3 wDir 		: TEXCOORD1;
				float3 Cen 			: TEXCOORD2;
				float4 grabcen 		: TEXCOORD3;
			};

			uniform sampler2D _bt;
			uniform float _dspow, _wamt, _wpow, _wms;

			inline bool IsInMirror()
			{
				return unity_CameraProjection[2][0] != 0.f || unity_CameraProjection[2][1] != 0.f;
			}

			static bool mirror = IsInMirror();

			v2f DoppelgangerScreenShaderVert (appdata v)
			{
				v2f o;
				o.vca = v.vertexColor.a;
				o.Cen = v.Cen;
				o.wNorm = v.normal;
				o.wDir = WorldSpaceViewDir(v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.grabuv = ComputeGrabScreenPos(o.vertex);
				o.grabcen = ComputeGrabScreenPos(UnityObjectToClipPos(float4(o.Cen, 1.0)));
				return o;
			}
			
			fixed4 DoppelgangerScreenShaderFrag (v2f i) : SV_Target
			{
				float vz = dot(-UnityObjectToWorldNormal(i.wNorm), normalize(i.wDir)), vca = i.vca;
				float2 grabuv = i.grabuv.xy / i.grabuv.w, vcen = i.grabcen.xy / i.grabcen.w;
				float ds = saturate(pow((1.0 - vz) * vz, 3.0) * 50.0);
				float3 col = tex2D(_bt, lerp(grabuv, vcen, ds * vca + _wpow * pow(vz, 3.0) * vca * sin(vz * _wamt + _wms * 20.0 * _Time.y)));
				return fixed4(col, 1.0);
			}
			ENDCG
		}
	}
}