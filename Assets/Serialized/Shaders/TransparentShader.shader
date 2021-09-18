Shader "RacingGame/TransparentShader" {
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
        _Color("Color (RGBA)", Color) = (1, 1, 1, 1) // add _Color property
            _Curvature("Curvature", Float) = 0.0001
    }

        SubShader
        {
            Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            Cull front
            LOD 100

            Pass
            {
                CGPROGRAM

                #pragma vertex vert alpha
                #pragma fragment frag alpha
                #pragma multi_compile_fog

                #include "UnityCG.cginc"

                struct appdata_t
                {
                    float4 vertex   : POSITION;
                    float2 texcoord : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex  : SV_POSITION;
                    half2 texcoord : TEXCOORD0;
                    UNITY_FOG_COORDS(1)
                };

                sampler2D _MainTex;
                float _Curvature;
                float4 _MainTex_ST;
                float4 _Color;

                v2f vert(appdata_t v)
                {
                    v2f o;

                    float4 worldSpace = mul(unity_ObjectToWorld, v.vertex);
                    worldSpace.xyz -= _WorldSpaceCameraPos.xyz;
                    worldSpace = float4(0.0f, (worldSpace.z * worldSpace.z) * -_Curvature, 0.0f, 0.0f);

                    v.vertex += mul(unity_WorldToObject, worldSpace);

                    o.vertex = UnityObjectToClipPos(v.vertex);
                    v.texcoord.x = 1 - v.texcoord.x;
                    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                    UNITY_TRANSFER_FOG(o, o.vertex);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.texcoord) * _Color;
                    UNITY_APPLY_FOG(i.fogCoord, col);
                   // UNITY_OPAQUE_ALPHA(col.a);



                    return col;
                }

                ENDCG
            }
        }
            Fallback "Mobile/Diffuse"
}
