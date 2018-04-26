Shader "MyShader/OutLightShader" 
{
    Properties   
    {  
        _MainTex ("Texture", 2D) = "white" {}  
        _BumpMap ("Bumpmap", 2D) = "bump" {}  
        _RimColor ("Rim Color", Color) = (0.06,0.05,0.04,0.0)  
        _RimPower ("Rim Power", Range(0.5,8.0)) = 0.6  
    }

	
    SubShader   
    {  
        Tags {"RenderType" = "Transparent"}  
		LOD 200
        CGPROGRAM

		#pragma surface surf Lambert
			
		sampler2D _MainTex;  
        sampler2D _BumpMap;  
        float4 _RimColor;  
        float _RimPower; 

        struct Input {  
            float2 uv_MainTex;  
            float2 uv_BumpMap;  
            float3 viewDir;  
        };  
        
        void surf (Input IN, inout SurfaceOutput o) {  

			half4 c = tex2D (_MainTex, IN.uv_MainTex);  
            o.Albedo = c.rgb;  
            o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));  
            o.Emission = _RimColor.rgb * pow (rim, _RimPower);
        }  
        ENDCG  
    }
    Fallback "Diffuse"  
}  