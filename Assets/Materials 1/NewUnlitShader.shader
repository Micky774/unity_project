// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
            #define M_PI 3.1415926535897932384626433832795

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float4 vertex : SV_POSITION; // clip space position
            };

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                // (multiply with model*view*projection matrix)
                o.vertex = UnityObjectToClipPos(v.vertex);
                // just pass the texture coordinate
                o.uv = v.uv;
                return o;
            }
            
            // texture we will sample
            sampler2D _MainTex;

            float2 scale(float2 uv, float2 X_bounds, float2 Y_bounds){
                float nu_x = (X_bounds.y-X_bounds.x)*uv.x+X_bounds.x;
                float nu_y = (Y_bounds.y-Y_bounds.x)*uv.y+Y_bounds.x;
                return float2(nu_x, nu_y);
            }

            float point_dist(float2 uv, float2 trap_point){
                return length(uv-trap_point);
            }
            float line_dist(float2 uv, float2 trap_point){
                return abs(uv.x-trap_point.x);
            }
            float circle_dist(float2 uv, float2 trap_point, float radius){
                return abs(length(uv-trap_point)-radius);
            }
            // pixel shader; returns low precision ("fixed4" type)
            // color ("SV_Target" semantic)
            fixed4 frag (v2f fragCoord) : SV_Target
            {
                // Normalized pixel coordinates (from 0 to 1)
                float2 uv = fragCoord.uv;
                
                float2 X_BOUNDS = float2(-2.5, 1.5);
                float2 Y_BOUNDS = float2(-1.5, 1.5);

                // Center coordinate
                uv = scale(uv, X_BOUNDS, Y_BOUNDS);

                // Rescale for aspect-ratio
                // uv.x *= iResolution.x/iResolution.y;

                float trap_radius = .25;
                float scaled_time = _Time.w;
                float trap_path_radius = 2.*cos(scaled_time)*sin(3.*scaled_time);
                float2 trap = trap_path_radius*float2(cos(scaled_time), sin(scaled_time));
                // float2 trap = scale(float2(0.5, 0.5), X_BOUNDS, Y_BOUNDS);
                // float2 trap = scale(iMouse.xy/iResolution.xy, X_BOUNDS, Y_BOUNDS);
                
                float d=1000.;
                float xx, yy, t_x, t_y;
                float a = uv.x;
                float b = uv.y;
                
                int max_iter = 50;
                float final_score = 0.;
                for(int i = 0; i < max_iter; i++){
                    d = min(d, line_dist(uv,trap));
                    xx = uv.x*uv.x;
                    yy = uv.y*uv.y;
                    t_x = xx - yy + a;
                    t_y = (uv.x+uv.x)*uv.y + b;
                    uv.x = t_x;
                    uv.y = t_y;
                    /*
                    if(uv.x*uv.x+uv.y*uv.y>4.){
                        final_score = float(i)+1.;
                    }
                    */
                    final_score = d;
                }
                
                float c;
                // if(d <r) c = 1.; else c = 1.-1./(1+exp());
                // c = float(final_score)/float(max_iter+1);
                c = 1./(1. + 1.75*pow(final_score, 1./2.));
                // c = float(15.*log(1.+0.1/(1.+final_score)));
                
                fixed4 col = 0;
                col.rgb = c;

                // Output to screen
                return fixed4(col.rgb, 1.0);
            }
            ENDCG
        }
    }
}
