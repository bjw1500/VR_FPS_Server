Shader "TY/SnowBuildup"
{
    Properties
    {
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _PaintMap("PaintMap", 2D) = "white"{}
        _SnowHeight("SnowHeight",range(0,10)) = 0 //range���� �ڽ��� �� �����Ͽ� ���� �����ϰ� �����սô�.
        _SnowAngle("SnowAngle",vector) = (0,1,0,0)
        _SnowSize("SnowSize",range(-1.1,3)) = 0 //������ -1.1 ~ 1.1�� ������ �ƿ� ���� ������ �������� ~ ������ ���� �ڵ��λ��¸� ����� �����Դϴ�.
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _PaintMap;
            float _SnowHeight;
            float _SnowSize;
            float4 _SnowAngle;

            void vert(inout appdata_full v)
            {
                float4 snowA = mul(normalize(_SnowAngle), unity_ObjectToWorld); //���� ���͸� ����������� ������ݴϴ�.

                //���� ���̴� ������ ũ�⸦ �������ݴϴ�.
                if (dot(v.normal, snowA.xyz) >= _SnowSize)
                {
                    v.vertex.xyz += v.normal.xyz * _SnowHeight;
                }
            }

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_PaintMap;
            };

            void surf(Input IN, inout SurfaceOutputStandard o)
            {
                fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
                float4 p = tex2D(_PaintMap, IN.uv_PaintMap);
                o.Albedo = c.rgb;
                o.Alpha = c.a;

                //�� ���� ���� ����� ĥ�մϴ�.
                float4 snowA = mul(normalize(_SnowAngle), unity_ObjectToWorld);
                if (dot(mul(o.Normal,unity_ObjectToWorld) , snowA.xyz) >= _SnowSize - 0.1)
                {
                    o.Albedo = 1;//�� �κ��� '��'�� �� �κ��Դϴ�. �� �κ��� �����ϸ� ���θ��� ���ο� ���� ���� �� �ֽ��ϴ�!
                }

            }
            ENDCG
        }
            FallBack "Diffuse"
}


