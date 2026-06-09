Shader "Custom/SphereFill3D"
{
    Properties
    {
        _BaseColor      ("Base Color",              Color)          = (1,1,1,1)
        _FillColor      ("Fill Color",              Color)          = (1,0,0,1)
        _LineColor      ("Line Color",              Color)          = (1,1,1,1)
        _FillValue      ("Fill Value (0~1)",        Range(0,1))     = 0.5
        _AxisMin        ("Axis Min",                Float)          = -0.5
        _AxisMax        ("Axis Max",                Float)          =  0.5
        _Axis           ("Axis (0=X 1=Y 2=Z)",     Float)          = 1
        _Direction      ("Direction (1 or -1)",     Float)          = 1
        _LineThickness  ("Line Thickness",          Range(0, 0.05)) = 0.01
        _DashScale      ("Dash Scale",              Range(1, 20))   = 5
        _DashRatio      ("Dash Ratio (0~1)",        Range(0, 1))    = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 normalWS    : TEXCOORD0;
                float3 localPos    : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                half4 _FillColor;
                half4 _LineColor;
                float _FillValue;
                float _AxisMin;
                float _AxisMax;
                float _Axis;
                float _Direction;
                float _LineThickness;
                float _DashScale;
                float _DashRatio;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normalWS    = TransformObjectToWorldNormal(IN.normalOS);
                OUT.localPos    = IN.positionOS.xyz;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 축 선택 + 각도 기반 점선
                float axisVal;
                float dashVal;
                if (_Axis < 0.5)
                {
                    axisVal = IN.localPos.x;
                    dashVal = atan2(IN.localPos.z, IN.localPos.y);
                }
                else if (_Axis < 1.5)
                {
                    axisVal = IN.localPos.y;
                    dashVal = atan2(IN.localPos.z, IN.localPos.x);
                }
                else
                {
                    axisVal = IN.localPos.z;
                    dashVal = atan2(IN.localPos.y, IN.localPos.x);
                }

                // 방향 반전
                if (_Direction < 0) axisVal = -axisVal;

                // 정규화
                float minVal     = (_Direction < 0) ? -_AxisMax : _AxisMin;
                float maxVal     = (_Direction < 0) ? -_AxisMin : _AxisMax;
                float normalized = (axisVal - minVal) / (maxVal - minVal);

                // 각도 기반 점선 패턴 (-PI~PI 를 0~1로)
                float dashNorm = (dashVal + 3.14159265) / (2.0 * 3.14159265);
                float dash     = frac(dashNorm * _DashScale);
                bool  isDash   = dash < _DashRatio;

                // 경계선
                bool  isLine   = abs(normalized - _FillValue) < _LineThickness;

                half4 color;
                if (isLine && isDash)
                    color = _LineColor;
                else
                    color = normalized < _FillValue ? _FillColor : _BaseColor;

                // Diffuse 라이팅
                float3 normal   = normalize(IN.normalWS);
                Light mainLight = GetMainLight();
                float diffuse   = max(0, dot(normal, mainLight.direction));
                float3 lighting = mainLight.color * diffuse + float3(0.2, 0.2, 0.2);

                return half4(color.rgb * lighting, 1);
            }
            ENDHLSL
        }
    }
}