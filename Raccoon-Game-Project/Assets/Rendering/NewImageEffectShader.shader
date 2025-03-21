Shader "Hidden/NewImageEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 colorArray[42] = 
            {
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255),
                float4(33, 24, 27, 255)
            };

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                int closestColor = 0;
                for(int i = 0; i < 42; i++)
                {
                    if(distance(col - colorArray[i], colorArray[closestColor]) > 1.0)
                    {
                        closestColor = i;
                    }
                }
                col = colorArray[closestColor];
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }
            ENDCG
        }
    }
}
