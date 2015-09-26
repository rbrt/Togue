Shader "Custom/Wireframe" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
}

SubShader {
	Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
	LOD 100

	Alphatest Greater 0
	ZWrite Off
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask RGBA
	//Cull Off

	Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				half2 texcoord  : TEXCOORD0;
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				gIn OUT;
 				UNITY_INITIALIZE_OUTPUT(IN, OUT);
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f IN) : COLOR
			{
				return fixed4(IN.texcoord.xy, 0, 1);
			}

		ENDCG
		}
	}
}
