
//Texture where the scene was rendered
texture sceneTexture;
sampler sceneSampler = sampler_state
{
	texture = <sceneTexture>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
}; 

//Texture where the gui is rendered.
texture guiTexture;
sampler guiSampler = sampler_state
{
	texture = <guiTexture>;
	magfilter = POINT;
	minfilter = POINT;
	mipfilter = POINT;
};

//Structures used by the shaders
struct PPVertexToPixel
{
	float4 Position : POSITION;
	float2 TexCoord	: TEXCOORD0;
};
/*struct PPPixelToFrame
{
    float4 Color 	: COLOR0;
};*/


//Vertex shader, pretty much just pass the vertex to the pixel shader.
//Used by the rest of the post process pixel shaders.
PPVertexToPixel PassThroughVertexShader(float4 inPos: POSITION0, float2 inTexCoord: TEXCOORD0)
{
	PPVertexToPixel Output = (PPVertexToPixel)0;
	Output.Position = inPos;
	Output.TexCoord = inTexCoord;
	return Output;
}

//--------------------------- Noise Post Process

// A timer to animate our shader
float fTimer;

// the amount of distortion
float fNoiseAmount;

// just a random starting number
int iSeed;

// Noise
float4 NoisePS(float2 Tex: TEXCOORD0) : COLOR0
{
	// Distortion factor
	float NoiseX = iSeed * fTimer * sin(Tex.x * Tex.y+fTimer);
	NoiseX=fmod(NoiseX,8) * fmod(NoiseX,4);	

	// Use our distortion factor to compute how much it will affect each
	// texture coordinate
	float DistortX = fmod(NoiseX,fNoiseAmount);
	float DistortY = fmod(NoiseX,fNoiseAmount+0.002);
	
	// Create our new texture coordinate based on our distortion factor
	float2 DistortTex = float2(DistortX,DistortY);
	
	// Use our new texture coordinate to look-up a pixel in ColorMapSampler.
	float4 Color=tex2D(sceneSampler, Tex+DistortTex);
	
	// Keep our alphachannel at 1.
	Color.a = 1.0f;

    return Color;
}

technique Noise
{
	pass P0
	{		
		VertexShader = compile vs_2_0 PassThroughVertexShader();
		PixelShader = compile ps_2_0 NoisePS();
	}
}

//--------------------------- Grayscale Post Process

float4 GrayscalePS(float2 Tex: TEXCOORD0) : COLOR0
{
	//Sample the texture
	float4 Color = tex2D(sceneSampler, Tex); 
	
	//Modulate using the good predefined factors
	Color.rgb = dot(Color.rgb, float3(0.3, 0.59, 0.11));
	
	// Keep our alphachannel at 1.
	Color.a = 1.0f;
  
	//Voila!
    return Color;
}

technique Grayscale
{
 pass P0
 {
 		VertexShader = compile vs_2_0 PassThroughVertexShader();
		PixelShader = compile ps_2_0 GrayscalePS();
 }
} 