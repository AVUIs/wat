 Shader "Vertex Color Lit" {
     Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
     }
     SubShader {
        Pass {
            Lighting On
            ColorMaterial AmbientAndDiffuse
            SetTexture [_MainTex] {
               combine texture * primary DOUBLE
            }
        }
     }
 }


/*
Shader "Unlit Transparent Vertex Colored" 
{
	Properties 
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
	}
	
	Category 
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		ZWrite Off
		//Alphatest Greater 0
		Blend SrcAlpha OneMinusSrcAlpha 
		Fog { Color(0,0,0,0) }
		Lighting On
		Cull Off //we can turn backface culling off because we know nothing will be facing backwards

		BindChannels 
		{
			Bind "Vertex", vertex
			Bind "texcoord", texcoord 
			Bind "Color", color 
		}

		SubShader   
		{
			Pass 
			{
				SetTexture [_MainTex] 
				{
					Combine texture * primary
				}
			}
		} 
	}
}
*/