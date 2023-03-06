Shader "TY/Shad_SnowFootPrint"
{
	Properties
	{
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_PaintMap("PaintMap",2D) = "white"{}
		_PaintColor("PaintColor",Color) = (0.8,0.8,0.8,1)
		_Thickness("Thickness",float) = 0
		_BumpMap("NormalMap",2D) = "bump"{}  //���� �븻��		
		[Space(20)]
		_Disp("Displacement Texture",2D) = "black"{} //���÷��̽���Ʈ ��
		_DisHeight("Displacement Height",Range(0,1)) = 0 //���÷��̽���Ʈ ������
		[Space(20)]
		_NoiseTex("NoiseTexture",2D) = "white"{} //��¦�̰� �������� ��¦�̰� ����� ���� ������ �ؽ���
		_Sparkle("SparkleTexture",2D) = "black"{} //��¦�� �ؽ���
		_Spower("SparklePower",Range(0,10)) = 1.5 //��¦�̰� ������ ���� (����Ʈ���μ����� ��� ���� �־�� Ƽ�� ���ϴ�)
		[Space(20)]
		_RimColor("RimColor",Color) = (1,1,1,1) //�� ����Ʈ �÷�
		_RimPower("RimPower",float) = 0		//������Ʈ ����
		[Space(20)]
		_SnowColor("SnowColor",Color) = (0,0,0,0) //���� ���̽� ����
		_ShadowColor("ShadowColor",Color) = (0,0,0,0) //�׸����� ����
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

				float3 screenUV = IN.screenPos.rgb / IN.screenPos.a; //�������ؽ��ĸ� �ֱ����� ��ũ�� uv�� �ҷ��ɴϴ�.
				float noise = tex2D(_NoiseTex, screenUV.xy).r; //��ũ��uv�� ������� �������ؽ��ĸ� ����ϴ�.

				float3 sparklemap = tex2D(_Sparkle, IN.uv_Sparkle).rgb; //��¦�� �ؽ��ĸ� �ҷ��ɴϴ�.

				float3 normalTex = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap)) * 2 - 1;
				normalTex = normalTex * p.r;
				normalTex = normalTex * 0.5 + 0.5;

				o.Normal = normalTex; //�븻���� �����մϴ�.


				o.Specular = noise;
				o.Gloss = sparklemap.r;
				// �������ؽ��Ŀ� ��¦���ؽ��ĸ� Ŀ���� ���̴����� ����; ����ŧ���� �۷ν��ʿ� ����־����ϴ�.
				// �� �� ���� �븻�ʰ� �޸� ����ٰ� ��� �ٷ� ������ �ȵǰŵ��.

				o.Alpha = 1;

				o.Albedo = c.rgb * (1 - (1 - p.rgb) * (1 - _PaintColor));
				o.Alpha = c.a;
			}

			float4 LightingSnowShader(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
			{
				//�⺻ half lambert���
				float ndotl = saturate(dot(s.Normal, lightDir));
				float halfLambert = ndotl * 0.7 + 0.3;

				//������Ʈ ���
				float rim = saturate(dot(s.Normal, viewDir));
				float3 rimColor = pow((1 - rim), _RimPower) * _RimColor.rgb;


				//��¦�� ��� 
				float sparkle = (s.Gloss * _Spower * ndotl * s.Specular);


				float4 final;
				final.rgb = (s.Albedo * _SnowColor * halfLambert * _LightColor0 * atten) + ((1 - halfLambert) * _ShadowColor) + rimColor + sparkle;
				/*
				(_SnowColor* halfLambert*_LightColor0) ��������Ʈ�� �̿��ؼ� ����ũ������ �����ϴ�
				((1 - halfLambert)*_ShadowColor) ��������Ʈ�� ����� �������÷��� �������ݴϴ�.
				rimColor ������Ʈ�Դϴ�.
				sparkle ��¦���Դϴ�.
				�̰͵��� �ٴ����ָ� ���̽��÷� �� �������÷��� ���� ������ �� �ְ�, ������Ʈ�� ������, ��¦�̰� �ִ� ������� �����°�����.
				*/
				final.a = s.Alpha;

				return final;
			}


			ENDCG
		}
			FallBack "Diffuse"
}