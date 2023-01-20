Shader "TY/Shad_SnowFootPrint"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_PaintMap("PaintMap",2D) = "white"{}
		_PaintColor("PaintColor",Color) = (0.8,0.8,0.8,1)
		_Thickness("Thickness",float) = 0
		_BumpMap("NormalMap",2D) = "bump"{}  //눈의 노말맵		
		[Space(20)]
		_Disp("Displacement Texture",2D) = "black"{} //디스플레이스먼트 맵
		_DisHeight("Displacement Height",Range(0,1)) = 0 //디스플레이스먼트 사이즈
		[Space(20)]
		_NoiseTex("NoiseTexture",2D) = "white"{} //반짝이가 무작위로 반짝이게 만들기 위한 노이즈 텍스쳐
		_Sparkle("SparkleTexture",2D) = "black"{} //반짝이 텍스쳐
		_Spower("SparklePower",Range(0,10)) = 1.5 //반짝이가 빛나는 정도 (포스트프로세스의 블룸 등을 넣어야 티가 납니다)
		[Space(20)]
		_RimColor("RimColor",Color) = (1,1,1,1) //림 라이트 컬러
		_RimPower("RimPower",float) = 0		//림라이트 영역
		[Space(20)]
		_SnowColor("SnowColor",Color) = (0,0,0,0) //눈의 베이스 색깔
		_ShadowColor("ShadowColor",Color) = (0,0,0,0) //그림자의 색깔
		[Space(20)]
		_Tess("Tessellation",Range(1,32)) = 4
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf SnowShader fullforwardshadows tessellate:tessFixed vertex:vert addshadow

			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			float _Tess;
			float _Thickness;
			sampler2D _MainTex;
			sampler2D _PaintMap;
			float4 _PaintColor;

			sampler2D _Disp, _Sparkle, _BumpMap, _NoiseTex;
			float _DisHeight, _Spower, _RimPower;
			float4 _RimColor, _ShadowColor, _SnowColor;

			float4 tessFixed()
			{
				return _Tess;
			}

			struct Input
			{
				float2 uv_MainTex;
				float2 uv_PaintMap;
				float2 uv_Sparkle, uv_BumpMap;
				float4 screenPos;
			};

			void vert(inout appdata_full v)
			{
				float p = tex2Dlod(_PaintMap, v.texcoord).r;
				float h = tex2Dlod(_Disp, v.texcoord).r;
				float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
				worldPos.y += _Thickness * p.r;
				v.vertex.xyz = mul(unity_WorldToObject, worldPos.xyz);
				v.vertex.xyz += v.normal.xyz * h * _DisHeight;
			}

			void surf(Input IN, inout SurfaceOutput o)
			{
				float4 c = tex2D(_MainTex, IN.uv_MainTex);
				float4 p = tex2D(_PaintMap, IN.uv_PaintMap);

				float3 screenUV = IN.screenPos.rgb / IN.screenPos.a; //노이즈텍스쳐를 넣기위한 스크린 uv를 불러옵니다.
				float noise = tex2D(_NoiseTex, screenUV.xy).r; //스크린uv를 기반으로 노이즈텍스쳐를 만듭니다.

				float3 sparklemap = tex2D(_Sparkle, IN.uv_Sparkle).rgb; //반짝이 텍스쳐를 불러옵니다.

				float3 normalTex = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * 2 - 1;
				normalTex = normalTex * p.r;
				normalTex = normalTex * 0.5 + 0.5;

				o.Normal = normalTex; //노말맵을 적용합니다.


				o.Specular = noise;
				o.Gloss = sparklemap.r;
				// 노이즈텍스쳐와 반짝이텍스쳐를 커스텀 쉐이더에서 쓰고싶어서 스펙큘러와 글로스맵에 집어넣었습니다.
				// 이 두 맵은 노말맵과 달리 여기다가 적어도 바로 적용이 안되거든요.

				o.Alpha = 1;

				o.Albedo = c.rgb * (1 - (1 - p.rgb) * (1 - _PaintColor));
				o.Alpha = c.a;
			}

			float4 LightingSnowShader(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
			{
				//기본 half lambert계산
				float ndotl = saturate(dot(s.Normal, lightDir));
				float halfLambert = ndotl * 0.7 + 0.3;

				//림라이트 계산
				float rim = saturate(dot(s.Normal, viewDir));
				float3 rimColor = pow((1 - rim), _RimPower) * _RimColor.rgb;


				//반짝이 계산 
				float sparkle = (s.Gloss * _Spower * ndotl * s.Specular);


				float4 final;
				final.rgb = (s.Albedo * _SnowColor * halfLambert * _LightColor0 * atten) + ((1 - halfLambert) * _ShadowColor) + rimColor + sparkle;
				/*
				(_SnowColor* halfLambert*_LightColor0) 하프램버트를 이용해서 베이크럴러를 입힙니다
				((1 - halfLambert)*_ShadowColor) 하프램버트를 뒤집어서 쉐도우컬러를 지정해줍니다.
				rimColor 림라이트입니다.
				sparkle 반짝이입니다.
				이것들을 다더해주면 베이스컬러 와 쉐도우컬러를 직접 지정할 수 있고, 림라이트가 있으며, 반짝이가 있는 결과물이 나오는것이죠.
				*/
				final.a = s.Alpha;

				return final;
			}


			ENDCG
		}
			FallBack "Diffuse"
}