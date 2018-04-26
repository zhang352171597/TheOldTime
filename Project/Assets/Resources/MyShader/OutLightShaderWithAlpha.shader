Shader "MyShader/OutLightShaderWithAlpha"
{
	Properties   
    {  
        _MainTex ("Texture", 2D) = "white" {}  
        _BumpMap ("Bumpmap", 2D) = "bump" {}  
        _RimColor ("Rim Color", Color) = (0.06,0.05,0.04,0.0)  
		_FinalAlpha ("Final Alpha", Range(0,1)) = 1 
        _RimPower ("Rim Power", Range(0.5,8.0)) = 0.6  
    }
	
    SubShader   
    {  
        Tags {"RenderType" = "Transparent"}  


		//Extra pass that renders to depth buffer only
		Pass {
			ZWrite On
			ColorMask 0
		}

		LOD 200
		ZWrite Off
		//Blend SrcAlpha OneMinusSrcAlpha
		//Blend One One

        CGPROGRAM

		///在#pragma声明中添加一个新的参数：alpha参数。 否则无法改变透明
		///注意  设置透明通道之后 引擎默认绘制顺序是从远到进
		#pragma surface surf Lambert alpha
			
		sampler2D _MainTex;  
        sampler2D _BumpMap;  
        float4 _RimColor;  
		float _FinalAlpha;
        float _RimPower; 

        struct Input {  
            float2 uv_MainTex;  
            float2 uv_BumpMap;  
            float3 viewDir;  
        };  
        
        void surf (Input IN, inout SurfaceOutput o) {  

			half4 c = tex2D (_MainTex, IN.uv_MainTex);  
            o.Albedo = c.rgb;  
            o.Alpha = _FinalAlpha; 
            //o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));  
            o.Emission = _RimColor.rgb * pow (rim, _RimPower) * _FinalAlpha;
        }  
        ENDCG
    }
    Fallback "Diffuse"
} 

