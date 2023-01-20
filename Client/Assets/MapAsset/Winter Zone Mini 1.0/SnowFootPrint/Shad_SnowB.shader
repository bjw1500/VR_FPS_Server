Shader "Custom/Shad_SnowB"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Thickness("Thickness", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows vertex:vert addshadow

        #pragma target 3.0

        sampler2D _MainTex;
        float _Thickness;

        struct Input
        {
            float2 uv_MainTex;
        };

        void vert(inout adddata_full v)
        {
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
            worldPos y += _Thickness;
            v.vertex.xyz = mul(unity_ObjectToWorld, worldPos.xyz);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
