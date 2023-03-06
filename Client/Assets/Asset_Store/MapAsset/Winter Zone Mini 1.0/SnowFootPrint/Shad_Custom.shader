Shader "Custom/Shad_Custom"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _PaintMap("PaintMap",2D) = "white"{}
        _Thickness("Thickness", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        
        #pragma surface surf Standard fullforwardshadows

        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _PaintMap;
        float _Thickness;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_PaintMap;
        };

        void vert(inout appdata_full v)
        {
            float3 worldPos = mul(unity_ObjectToWorld, v.vertex.xyz);
            worldPos.y += _Thickness;
            v.vertex.xyz = mul(unity_WorldToObject, worldPos.xyz);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 c = tex2D(_MainTex, In.uv_MainTex);
            float4 p = tex2D(_PaintMap, In.uv_PaintMap);
            o.Albedo = c.rgb * p.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
