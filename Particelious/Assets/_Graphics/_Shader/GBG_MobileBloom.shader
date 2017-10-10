Shader "Hidden/GBG_MobileBloom"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Threshold("Threshold", Float) = 0.5
		_ReturnValue("Predefined black vector for optimization", Vector) = (0,0,0,0)
		_OriginalScene("Texture", 2D) = "white" {}
	}
	SubShader
	{
		// No culling or depth
		Cull Back ZWrite Off ZTest Always

		Pass
		{
			Name "HorizontalBlurring"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform fixed4 _ReturnValue;

			uniform half _SingleStepOffset[2];

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

			struct v2f
            {
			    float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				half2 bc1 : TEXCOORD1;
				half2 bc2 : TEXCOORD2;
				half2 bc3 : TEXCOORD3;
				half2 bc4 : TEXCOORD4;
            };

			v2f vert (appdata v)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
                output.uv = v.uv;
				output.bc1 = v.uv + half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 0.53805;
				output.bc2 = v.uv - half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 0.53805;
				output.bc3 = v.uv + half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 2.06278;
				output.bc4 = v.uv - half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 2.06278;
                return output;
            }

			fixed4 frag (v2f fragData) : SV_Target
			{
				_ReturnValue += tex2D(_MainTex, fragData.bc1) * 0.44908;
				_ReturnValue += tex2D(_MainTex, fragData.bc2) * 0.44908;
				_ReturnValue += tex2D(_MainTex, fragData.bc3) * 0.05092;
				_ReturnValue += tex2D(_MainTex, fragData.bc4) * 0.05092;

				return _ReturnValue;
			}
			ENDCG
		}

		Pass
		{
			Name "VerticalBlurringAndFinalProduct"

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _OriginalScene;
			uniform half _SingleStepOffset[2];

			struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

			struct v2f
            {
			    float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				half2 bc1 : TEXCOORD1;
				half2 bc2 : TEXCOORD2;
				half2 bc3 : TEXCOORD3;
				half2 bc4 : TEXCOORD4;
            };

			v2f vert (appdata v)
            {
                v2f output;
                output.vertex = UnityObjectToClipPos(v.vertex);
                output.uv = v.uv;
				output.bc1 = v.uv + half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 0.53805;
				output.bc2 = v.uv - half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 0.53805;
				output.bc3 = v.uv + half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 2.06278;
				output.bc4 = v.uv - half2(_SingleStepOffset[0], _SingleStepOffset[1]) * 2.06278;
                return output;
            }

			fixed4 frag (v2f fragData) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, fragData.bc1) * 0.44908;
				col += tex2D(_MainTex, fragData.bc2) * 0.44908;
				col += tex2D(_MainTex, fragData.bc3) * 0.05092;
				col += tex2D(_MainTex, fragData.bc4) * 0.05092;

				col += tex2D(_OriginalScene, fragData.uv);
				return col;
			}
			ENDCG
		}

		Pass
		{
			Name "Thresholding"

			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform half _Threshold;

			fixed4 frag (v2f_img imgData) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, imgData.uv);
				float brightness = dot(col.rgb, half3(0.2126, 0.7152, 0.0722));

				if(brightness < _Threshold){
					col = fixed4(0,0,0,0);
				}

				return col;
			}

			ENDCG
		}
	}
}
