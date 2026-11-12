Shader "Custom/VolumetricFog"
{
    Properties
    {
        _FogColor("Fog Color", Color) = (1, 1, 1, 1)
        _MaxDistance("Max distance", float) = 100
        _StepSize("Step size", Range(0.01, 20)) = 1
        _DensityMultiplier("Density multiplier", Range(0, 1)) = 1
        _NoiseOffset("Noise offset", float) = 0
        
        _FogDirection("Fog direction", vector) = (1, 1, 0, 0)
        _FogSpeed("Fog speed", Range(0, 10)) = 1
        
        _FogNoise("Fog noise", 3D) = "white" {}
        _NoiseTiling("Noise tiling", float) = 1
        _DensityThreshold("Density threshold", Range(0, 1)) = 0.1
        
        [HDR]_LightContribution("Light contribution", Color) = (1, 1, 1, 1)
        _LightScattering("Light Scattering", Range(0, 1)) = 0.2
        
        _YCutOff("Y Cut-off", float) = 0
        _YSmoothing("Y Smoothing", float) = .2
        _XCutOff("X Cut-Off", float) = 1.75
        _ZCutOff("Z Cut-off", float) = 1
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
            
            float4 _FogColor;
            
            float _MaxDistance;
            float _StepSize;
            float _DensityMultiplier;
            float _NoiseOffset;
            
            float3 _FogDirection;
            float _FogSpeed;
            
            Texture3D _FogNoise;
            float _NoiseTiling;
            float _DensityThreshold;
            
            float4 _LightContribution;
            float _LightScattering;
            
            float _YCutOff;
            float _YSmoothing;
            float _XCutOff;
            float _ZCutOff;
            
            float heyney_greenstein(float angle, float scattering)
            {
                return (1.0 - angle * angle) / (4.0 * PI * pow(1.0 + scattering * scattering - (2.0 * scattering) * angle, 1.5f));
            }
            
            float get_density(float3 worldPos)
            {
                if (worldPos.y > _YCutOff)
                    return 0;
                if (abs(worldPos.x) > _XCutOff)
                    return 0;
                if (abs(worldPos.z) > _ZCutOff)
                    return 0;
                float4 noise = _FogNoise.SampleLevel(sampler_TrilinearRepeat, (worldPos + _FogDirection*_Time.y*_FogSpeed) * 0.01 * _NoiseTiling, 0);
                float density = dot(noise, noise);
                density = saturate(density - _DensityThreshold) * _DensityMultiplier;

                if (worldPos.y > _YCutOff - _YSmoothing)
                {
                    float margin = (_YCutOff - worldPos.y)/_YSmoothing;
                    density *= margin;
                }
                
                return density;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float4 col = SAMPLE_TEXTURE2D(_BlitTexture, sampler_LinearClamp, IN.texcoord);
                float depth = SampleSceneDepth(IN.texcoord);
                float3 worldPos = ComputeWorldSpacePosition(IN.texcoord, depth, UNITY_MATRIX_I_VP);
                
                float3 entryPoint = _WorldSpaceCameraPos;
                float3 viewDir = worldPos - _WorldSpaceCameraPos;
                float viewLength = length(viewDir);
                float3 rayDir = normalize(viewDir);
                float2 pixelCoords = IN.texcoord * _BlitTexture_TexelSize.zw;
                
                float4 fogCol = _FogColor;
                
                float distLimit = min(viewLength, _MaxDistance);
                float distTravelled = InterleavedGradientNoise(pixelCoords, (int)(_Time.y / max(HALF_EPS, unity_DeltaTime.x))) * _NoiseOffset;
                float transmittance = 1;
                
                while (distTravelled < distLimit)
                {
                    float3 rayPos = entryPoint + rayDir*distTravelled;
                    float density = get_density(rayPos);
                    if (density > 0)
                    {
                        Light mainLight = GetMainLight(TransformWorldToShadowCoord(rayPos));
                        fogCol.rgb += mainLight.color.rgb * _LightContribution.rgb * heyney_greenstein(dot(rayDir, mainLight.direction), _LightScattering) * density * mainLight.shadowAttenuation * _StepSize;
                        transmittance *= exp(-density * _StepSize);
                    }
                    distTravelled += _StepSize;
                }
                
                return lerp(col, fogCol, 1.0 - saturate(transmittance));
            }
            ENDHLSL
        }
    }
}
