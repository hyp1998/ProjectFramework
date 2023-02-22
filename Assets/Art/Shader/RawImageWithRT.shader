// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

Shader "UI/RawImageDefault"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _AlphaFix("Alpha Fix", Range(0, 1)) = 0.05
        _HeadBox("Head Box", Vector) = (0,0,1,1)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255

        _ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 texcoord  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            half4 _MainTex_TexelSize;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            half _AlphaFix;
            half4 _HeadBox;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
				

                #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
				
                half2 uv = IN.texcoord;


                half a1 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(-1,-1)).a;
                half a2 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(-1,0)).a;
                half a3 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(-1,1)).a;
                half a4 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(0,-1)).a;
                half a5 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(0,1)).a;
                half a6 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(1,-1)).a;
                half a7 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(1,0)).a;
                half a8 = tex2D(_MainTex, uv + _MainTex_TexelSize.xy * half2(1,1)).a;
                half avgAlpha = min((a1 + a2 + a3 + a4 + a5 + a6 + a7 + a8 + color.a) * 0.1111111 , 1.0);

                half maxVal = max(max(color.r, color.g), color.b);
                half rtAlpha = avgAlpha > _AlphaFix ? 1 : avgAlpha;
                //half rtAlpha = avgAlpha;

                return half4(color.rgb, rtAlpha);
            }
        ENDCG
        }

        
    }
}
