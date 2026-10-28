
Shader "Custom/ColorblindScreen"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mode ("Mode", Float) = 0
        _Correct ("Correct", Float) = 0
        _Flip ("Flip Y", Float) = 0
    }
    SubShader
    {
        Pass
        {
            ZTest Always Cull Off ZWrite Off
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _Mode, _Correct, _Flip;

            static const float3x3 Protan = float3x3(
                 0.152286, 1.052583, -0.204868,
                 0.114503, 0.786281,  0.099216,
                -0.003882, -0.048116, 1.051998);
            static const float3x3 Deutan = float3x3(
                 0.367322, 0.860646, -0.227968,
                 0.280085, 0.672501,  0.047413,
                -0.011820, 0.042940,  0.968881);
            static const float3x3 Tritan = float3x3(
                 1.255528, -0.076749, -0.178779,
                -0.078411,  0.930809,  0.147602,
                 0.004733,  0.691367,  0.303900);

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;
                if (_Flip > 0.5) uv.y = 1 - uv.y;

                float3 c = tex2D(_MainTex, uv).rgb;
                int m = (int)round(_Mode);
                if (m == 0) return fixed4(c, 1);

                float3x3 M = (m == 1) ? Protan : (m == 2) ? Deutan : Tritan;
                float3 sim = saturate(mul(M, c));
                if (_Correct < 0.5) return fixed4(sim, 1);

                float3 e = c - sim;
                float3 shift = (m == 3)
                    ? float3(0.7 * e.b + e.r, 0.7 * e.b + e.g, 0)
                    : float3(0, 0.7 * e.r + e.g, 0.7 * e.r + e.b);
                return fixed4(saturate(c + shift), 1);
            }
            ENDCG
        }
    }
}