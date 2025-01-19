#ifndef LIGHTING_CEL_SHADED_INCLUDED
#define LIGHTING_CEL_SHADED_INCLUDED

#ifndef SHADERGRAPH_PREVIEW
struct EdgeConstants {

   float specular;
   float rim;
   float distanceAttenuation;

};

struct SurfaceVariables {

   float smoothness;
   float shininess;
   
   float rimStrength;
   float rimAmount;
   float rimThreshold;
   
   float3 normal;
   float3 view;

   EdgeConstants ec;

};



float3 CalculateCelShading(Light l, SurfaceVariables s, float ShadowSteps, float ShadowType) {
      
   //shadow occlusion from other objects
   float attenuation = 
   smoothstep(0.0f, 1, l.distanceAttenuation) * 
   smoothstep(0.0f, 1, l.shadowAttenuation);
   
   float lightPower = (1 + dot(s.normal, l.direction)) / 2; //[0,1] how much light this point gets
   
   float lightOut = 0;
   float step = 1.0/ShadowSteps;
   for(int i = 1; i<=ShadowSteps ; i++)
   {
      if(lightPower > step * i)
      {
         lightOut += step;
      }
      else 
      {
         break;
      }
   }   

   lightOut = pow(saturate(lightOut), ShadowType);

   //RIM light at the corner of the shape
   float rim = 1 - dot(s.view, s.normal);
   rim *= pow(lightOut, s.rimThreshold);
   rim = s.rimStrength * smoothstep(
      s.rimAmount - 0.5f * s.ec.rim, 
      s.rimAmount + 0.5f * s.ec.rim, 
      rim
   );
   
   //specular light reflected to the camera
   float3 h = SafeNormalize(l.direction + s.view);
   float specular = saturate(dot(s.normal, h));
   specular = pow(specular, s.shininess);
   specular *= lightPower;
   specular = s.smoothness * smoothstep(0.005f, 
      0.005f + s.ec.specular * s.smoothness, specular);

   //OLD RIM Implementation

   //return  l.color *  max(max(specular, rim) * 2 * lightOut, lightOut) * attenuation;
   
   //black rim  
   if(max(specular, rim) == 0)
   {
      return l.color * lightOut * attenuation;
   }
   if( specular > rim )
   {
      return l.color * specular * attenuation;
   }
   return float3(0.0,0.0,0.0);
}

float3 CalculateNotMainLightShading(Light l, SurfaceVariables s) {
   
   float diffuse = saturate(dot(s.normal, l.direction));

   float3 h = SafeNormalize(l.direction + s.view);
   float specular = saturate(dot(s.normal, h));
   specular = pow(specular, s.shininess);
   specular *= diffuse;

   float rim = 1 - dot(s.view, s.normal);
   rim *= pow(diffuse, s.rimThreshold);

   diffuse = smoothstep(0.0f, 1, 1);
   specular = s.smoothness * smoothstep(0.005f, 
      0.005f + s.ec.specular * s.smoothness, specular);
   rim = s.rimStrength * smoothstep(
      s.rimAmount - 0.5f * s.ec.rim, 
      s.rimAmount + 0.5f * s.ec.rim, 
      rim
   );
   
   return l.color * (diffuse + max(specular, rim) * 0.1);
}
#endif


void LightingCelShaded_float(float Smoothness, 
      float RimStrength, float RimAmount, float RimThreshold, 
      float3 Position, float3 Normal, float3 View,
      float EdgeSpecular, float EdgeDistanceAttenuation, 
      float EdgeRim, float ShadowSteps, float ShadowType, out float3 Color) {

#if defined(SHADERGRAPH_PREVIEW)
   Color = half3(0.5f, 0.5f, 0.5f);
#else
   SurfaceVariables s;
   s.smoothness = Smoothness;
   s.shininess = exp2(10 * Smoothness + 1);
   s.rimStrength = RimStrength;
   s.rimAmount = RimAmount;
   s.rimThreshold = RimThreshold;
   s.normal = normalize(Normal);
   s.view = SafeNormalize(View);
   s.ec.specular = EdgeSpecular;
   s.ec.distanceAttenuation = EdgeDistanceAttenuation;
   s.ec.rim = EdgeRim;

#if SHADOWS_SCREEN
   float4 clipPos = TransformWorldToHClip(Position);
   float4 shadowCoord = ComputeScreenPos(clipPos);
#else
   float4 shadowCoord = TransformWorldToShadowCoord(Position);
#endif
     
   Light light = GetMainLight(shadowCoord);
   
   Color = CalculateCelShading(light, s, ShadowSteps, ShadowType);

   int pixelLightCount = GetAdditionalLightsCount();
   for (int i = 0; i < pixelLightCount; i++) 
   {
      light = GetAdditionalLight(i, Position, 1);
      //Color += CalculateNotMainLightShading(light, s);
      Color += CalculateCelShading(light, s, ShadowSteps, ShadowType);
   }
   
#endif
}

#endif