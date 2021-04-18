Shader "XSOverlay/UI/DefaultUI"
{
     Properties
    {
        //Add Custom Properties here

        //---

        //Built In Shader Parameters
        _ClockColor("Clock Color", Color) = (1,1,1,1)
        _CursorPositionVelocity("Cursor Pos Vel", Vector) = (0,0,0,0)
        _HMDPosition("HMD Pos", Vector) = (0,0,0,0)
        _HMDRotation("HMD Rot", Vector) = (0,0,0,0)
        _OverlayPosition("Overlay Pos", Vector) = (0,0,0,0)
        _OverlayRotation("Overlay Rot", Vector) = (0,0,0,0)
        _OverlayVelocity("Overlay Vel", Vector) = (0,0,0,0)
        _OverlayAngleToHMD("Overlay Angle HMD", Float) = 0
        _XSOAccentColor("XSO Accent Color", Color) = (0,0,0,1)
        //---

        //Keep these, they're important for rendering on UI correctly.
        [HideInInspector][PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector]_Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector]_Stencil ("Stencil ID", Float) = 0
        [HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector]_ColorMask ("Color Mask", Float) = 15
        [HideInInspector][Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
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
        Blend SrcAlpha OneMinusSrcAlpha, OneMinusDstAlpha One
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
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv  : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;

            float4 _ClockColor;
            float4 _CursorPositionVelocity;
            float4 _HMDPosition;
            float4 _HMDRotation;
            float4 _OverlayPosition;
            float4 _OverlayRotation;
            float4 _OverlayVelocity;
            float _OverlayAngleToHMD;
            float4 _XSOAccentColor;

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

                OUT.vertex = UnityObjectToClipPos(v.vertex);
                OUT.uv = TRANSFORM_TEX(v.uv, _MainTex);
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                //Keep all this stuff
                    half4 color = (tex2D(_MainTex, IN.uv) + _TextureSampleAdd) * IN.color;
                    #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                    #endif

                    #ifdef UNITY_UI_ALPHACLIP
                    clip (color.a - 0.001);
                    #endif
                //---

                //You can return anything you want, but the above is important for UI Rendering.

                return color;
            }
        ENDCG
        }
    }
}
