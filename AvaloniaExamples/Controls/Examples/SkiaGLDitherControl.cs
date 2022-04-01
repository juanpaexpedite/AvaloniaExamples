using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaGLDitherControl : SkiaGLControl
    {
        SKRuntimeEffectUniforms uniforms;
        SKPaint paint = new SKPaint() { };
        SKRuntimeEffect effect;
        SKRuntimeEffectChildren children;

        public override void Draw(SKCanvas canvas)
        {
            //TODO: Check if I can avoid creating a previous bitmap and apply the shader over the canvas.

            var apppath = System.AppDomain.CurrentDomain.BaseDirectory;
            using var bmp = SKImage.FromEncodedData(Path.Combine(apppath, "Assets\\gradient.png"));
            using var textureShader = bmp.ToShader();

            var osrc = @"

uniform shader image;

float mod(float x, float y) {
return x-y*floor(x/y);
}

float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}

float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

float getbayerdither(vec2 position, float brightness)
{    
    int dither[2][2];

    dither[0][0] = 0.25;
    dither[0][1] = 0.50;
    dither[1][0] = 0.75;
    dither[1][1] = 1.0;


    //int pixelIndex4 = (position.x % 2) + (position.y % 2) * 2;
    int x = int(mod(position.x, 2.0));
    int y = int(mod(position.y, 2.0));
    int pixelindex4  = x + y * 2;
    //return brightness > (float(bayerMatrix4[pixelIndex4]) + 0.5) / 4.0 ? 1.0 : 0.0;
    
}

vec3 dither2x2(vec2 position, vec3 color) {
  return color * getbayerdither(position, luma(color));
}

vec4 dither2x2(vec2 position, vec4 color) {
  return vec4(color.rgb * getbayerdither(position, luma(color)), 1.0);
}

half4 main(vec2 fragcoord) { 

    vec4 color = sample(image, fragcoord);
    return dither2x2(fragcoord.xy, color);
}
";

            var src2x2 = @"
uniform shader image; 

float mod(float x, float y) {
return x-y*floor(x/y);
}

float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}

float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

float dither2x2(vec2 position, float brightness) {

  int x = int(mod(position.x, 2.0));
  int y = int(mod(position.y, 2.0));
  int index = x + y * 2;
  float limit = 0.0;

    if (index == 0) limit = 0.25;
    if (index == 1) limit = 0.50;
    if (index == 2) limit = 0.75;
    if (index == 3) limit = 1.00;

  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither2x2(vec2 position, vec3 color) {
  return color * dither2x2(position, luma(color));
}

vec4 dither2x2(vec2 position, vec4 color) {
  return vec4(color.rgb * dither2x2(position, luma(color)), 1.0);
}

half4 main(vec2 fragcoord) { 

    vec4 color = sample(image, fragcoord);
    return dither2x2(fragcoord.xy, color);
}
";

            var src4x4 = @"
uniform shader image; 

float mod(float x, float y) {
return x-y*floor(x/y);
}

float luma(vec3 color) {
  return dot(color, vec3(0.299, 0.587, 0.114));
}

float luma(vec4 color) {
  return dot(color.rgb, vec3(0.299, 0.587, 0.114));
}

float dither4x4(vec2 position, float brightness) {

  int x = int(mod(position.x, 4.0));
  int y = int(mod(position.y, 4.0));
  int index = x + y * 4;
  float limit = 0.0;

   if (x < 8) {
    if (index == 0) limit = 0.0625;
    if (index == 1) limit = 0.5625;
    if (index == 2) limit = 0.1875;
    if (index == 3) limit = 0.6875;
    if (index == 4) limit = 0.8125;
    if (index == 5) limit = 0.3125;
    if (index == 6) limit = 0.9375;
    if (index == 7) limit = 0.4375;
    if (index == 8) limit = 0.25;
    if (index == 9) limit = 0.75;
    if (index == 10) limit = 0.125;
    if (index == 11) limit = 0.625;
    if (index == 12) limit = 1.0;
    if (index == 13) limit = 0.5;
    if (index == 14) limit = 0.875;
    if (index == 15) limit = 0.375;
  }


  return brightness < limit ? 0.0 : 1.0;
}

vec3 dither4x4(vec2 position, vec3 color) {
  return color * dither4x4(position, luma(color));
}

vec4 dither4x4(vec2 position, vec4 color) {
  return vec4(color.rgb * dither4x4(position, luma(color)), 1.0);
}

half4 main(vec2 fragcoord) { 

    vec4 color = sample(image, fragcoord);
    return dither4x4(fragcoord.xy, color);
}
";

            effect = SKRuntimeEffect.Create(src4x4, out var errorText);

            uniforms = new SKRuntimeEffectUniforms(effect) { };

            var children = new SKRuntimeEffectChildren(effect) { ["image"] = textureShader };

            paint.Shader = effect.ToShader(true, uniforms, children);
            //canvas.Save();
            //canvas.Scale(4.0f);
            canvas.DrawRect(SKRect.Create(bmp.Width, bmp.Height), paint);
            //canvas.Restore();
        }


        private void CreateExampleFour()
        {
            //Example source: https://shaders.skia.org/ from https://twitter.com/notargs/status/1250468645030858753

          
        }



    }
}
