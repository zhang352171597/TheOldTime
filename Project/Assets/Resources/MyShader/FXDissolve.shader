// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:2,rntp:3,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33627,y:32877,varname:node_4013,prsc:2|emission-2457-OUT,clip-9282-OUT;n:type:ShaderForge.SFN_Color,id:1304,x:32374,y:32585,ptovrint:False,ptlb:tietuyanse,ptin:_tietuyanse,varname:node_1304,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:1,c3:1,c4:1;n:type:ShaderForge.SFN_Tex2d,id:6413,x:32411,y:32779,ptovrint:False,ptlb:tietu,ptin:_tietu,varname:node_6413,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:19edf01955a40fb43898b58f35cb34ea,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1437,x:32790,y:32612,varname:node_1437,prsc:2|A-1304-RGB,B-6413-RGB,C-1304-A;n:type:ShaderForge.SFN_Tex2d,id:9488,x:32085,y:33162,ptovrint:False,ptlb:niuqutu,ptin:_niuqutu,varname:node_9488,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f892732d83f4a6c40a2bccef0d4ce1f4,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1707,x:32347,y:33130,varname:node_1707,prsc:2|A-9488-R,B-1428-OUT;n:type:ShaderForge.SFN_Slider,id:1428,x:31906,y:33404,ptovrint:False,ptlb:rongjie,ptin:_rongjie,varname:node_1428,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Add,id:7705,x:32701,y:33172,varname:node_7705,prsc:2|A-1707-OUT,B-1428-OUT;n:type:ShaderForge.SFN_Power,id:2426,x:32954,y:33172,varname:node_2426,prsc:2|VAL-7705-OUT,EXP-5068-OUT;n:type:ShaderForge.SFN_Vector1,id:5068,x:32715,y:33350,varname:node_5068,prsc:2,v1:3;n:type:ShaderForge.SFN_TexCoord,id:205,x:32248,y:33545,varname:node_205,prsc:2,uv:0;n:type:ShaderForge.SFN_Multiply,id:4903,x:32702,y:33504,varname:node_4903,prsc:2|A-1428-OUT,B-4224-OUT;n:type:ShaderForge.SFN_Add,id:3816,x:32881,y:33440,varname:node_3816,prsc:2|A-1428-OUT,B-4903-OUT;n:type:ShaderForge.SFN_Power,id:9418,x:33156,y:33451,varname:node_9418,prsc:2|VAL-3816-OUT,EXP-5068-OUT;n:type:ShaderForge.SFN_Multiply,id:9282,x:33371,y:33321,varname:node_9282,prsc:2|A-2426-OUT,B-9418-OUT;n:type:ShaderForge.SFN_OneMinus,id:4224,x:32507,y:33572,varname:node_4224,prsc:2|IN-205-U;n:type:ShaderForge.SFN_If,id:8999,x:33211,y:32969,varname:node_8999,prsc:2|A-4787-OUT,B-9282-OUT,GT-6018-OUT,EQ-3935-OUT,LT-3935-OUT;n:type:ShaderForge.SFN_Slider,id:4787,x:32824,y:32856,ptovrint:False,ptlb:rongjiebianyuan,ptin:_rongjiebianyuan,varname:node_4787,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Vector1,id:6018,x:32937,y:32949,varname:node_6018,prsc:2,v1:1;n:type:ShaderForge.SFN_Vector1,id:3935,x:32937,y:33022,varname:node_3935,prsc:2,v1:0;n:type:ShaderForge.SFN_Add,id:2457,x:33434,y:32918,varname:node_2457,prsc:2|A-1437-OUT,B-7215-OUT;n:type:ShaderForge.SFN_Multiply,id:7215,x:33366,y:33060,varname:node_7215,prsc:2|A-8999-OUT,B-9984-RGB;n:type:ShaderForge.SFN_Color,id:9984,x:33184,y:33132,ptovrint:False,ptlb:bianyuanyanse,ptin:_bianyuanyanse,varname:node_9984,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;proporder:1304-6413-9488-1428-4787-9984;pass:END;sub:END;*/

Shader "MyShader/FXDissolve" {
    Properties {
        _tietuyanse ("tietuyanse", Color) = (1,1,1,1)
        _tietu ("tietu", 2D) = "white" {}
        _niuqutu ("niuqutu", 2D) = "white" {}
        _rongjie ("rongjie", Range(0, 1)) = 1
        _rongjiebianyuan ("rongjiebianyuan", Range(0, 1)) = 0
        _bianyuanyanse ("bianyuanyanse", Color) = (0.5,0.5,0.5,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "Queue"="AlphaTest"
            "RenderType"="TransparentCutout"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Cull Off
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _tietuyanse;
            uniform sampler2D _tietu; uniform float4 _tietu_ST;
            uniform sampler2D _niuqutu; uniform float4 _niuqutu_ST;
            uniform float _rongjie;
            uniform float _rongjiebianyuan;
            uniform float4 _bianyuanyanse;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _niuqutu_var = tex2D(_niuqutu,TRANSFORM_TEX(i.uv0, _niuqutu));
                float node_5068 = 3.0;
                float node_9282 = (pow(((_niuqutu_var.r*_rongjie)+_rongjie),node_5068)*pow((_rongjie+(_rongjie*(1.0 - i.uv0.r))),node_5068));
                clip(node_9282 - 0.5);
////// Lighting:
////// Emissive:
                float4 _tietu_var = tex2D(_tietu,TRANSFORM_TEX(i.uv0, _tietu));
                float node_8999_if_leA = step(_rongjiebianyuan,node_9282);
                float node_8999_if_leB = step(node_9282,_rongjiebianyuan);
                float node_3935 = 0.0;
                float3 emissive = ((_tietuyanse.rgb*_tietu_var.rgb*_tietuyanse.a)+(lerp((node_8999_if_leA*node_3935)+(node_8999_if_leB*1.0),node_3935,node_8999_if_leA*node_8999_if_leB)*_bianyuanyanse.rgb));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_SHADOWCASTER
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _niuqutu; uniform float4 _niuqutu_ST;
            uniform float _rongjie;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                float2 uv0 : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float4 _niuqutu_var = tex2D(_niuqutu,TRANSFORM_TEX(i.uv0, _niuqutu));
                float node_5068 = 3.0;
                float node_9282 = (pow(((_niuqutu_var.r*_rongjie)+_rongjie),node_5068)*pow((_rongjie+(_rongjie*(1.0 - i.uv0.r))),node_5068));
                clip(node_9282 - 0.5);
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
