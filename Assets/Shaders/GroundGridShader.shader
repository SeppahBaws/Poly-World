Shader "Poly World/Ground Grid"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _IsGridActive("Use Grid", Range(0, 1)) = 1
        _FocusPos("Focus Position", Vector) = (0, 0, 0)
        _FocusRadius("Focus Radius", Float) = 10.0
        _GridColor("Grid Color", Vector) = (0, 0, 0)
        _GridThickness("Gird Thickness", Range(0, 0.5)) = 0.05
        _FalloffDistance("FalloffDistance", Float) = 3
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        // Access the shaderlab properties
        uniform sampler2D _MainTex;
        uniform float _IsGridActive;
        uniform float3 _FocusPos;
        uniform float _FocusRadius;
        uniform float3 _GridColor;
        uniform float _GridThickness;
        uniform float _FalloffDistance;

        void surf(Input IN, inout SurfaceOutput o)
        {
            bool isOnGridLine = false;

            // Check x position
            float f = frac(IN.worldPos.x + (_GridThickness / 2)) - _GridThickness;
            if (f < 0)
            {
                isOnGridLine = true;
            }

            // Check y position
            f = frac(IN.worldPos.z + (_GridThickness / 2)) - _GridThickness;
            if (f < 0)
            {
                isOnGridLine = true;
            }

            // Check if we are close enough
            if (distance(IN.worldPos.xyz, _FocusPos.xyz) > _FocusRadius)
            {
                isOnGridLine = false;
            }

            float3 texColor = tex2D(_MainTex, IN.uv_MainTex).rgb;

            // Final Color calculation
            float3 finalColor = texColor;
            if (isOnGridLine && _IsGridActive)
            {
                float falloffMultiplier = 1;

                // Distance from point to center
                float dist = distance(IN.worldPos.xyz, _FocusPos.xyz);
                // Check if we're in the falloff zone
                if (dist > (_FocusRadius - _FalloffDistance))
                {
                    // Get how far we are in the falloff zone
                    float zoneDistance = dist - (_FocusRadius - _FalloffDistance);
                    falloffMultiplier = 1 - (zoneDistance / _FalloffDistance);
                    //finalColor = float3(1, 0, 1);
                }

                finalColor += _GridColor * falloffMultiplier;
                //finalColor += _GridColor;
            }

            o.Albedo = finalColor;
        }
        ENDCG
    }
    Fallback "Diffuse"
}