using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaGLExampleLoopControl : SkiaGLLoopControl
    {
        float time = 0;
        public override void Update()
        {
            time += 0.01f;
            base.Update();
        }

        public override void Draw(SKCanvas canvas)
        {
            //Example source: https://shaders.skia.org/ from https://twitter.com/notargs/status/1250468645030858753

            var src = @"
uniform float iResolution;  // Viewport resolution (pixels)
uniform float  iTime;       // Shader playback time (s)

float f(vec3 p) {
    p.z -= iTime * 10.;
    float a = p.z * .1;
    p.xy *= mat2(cos(a), sin(a), -sin(a), cos(a));
    return .1 - length(cos(p.xy) + sin(p.yz));
}

half4 main(vec2 fragcoord) { 
    vec3 d = .5 - fragcoord.xy1 / iResolution;
    vec3 p=vec3(0);
    for (int i = 0; i < 32; i++) {
      p += f(p) * d;
    }
    return ((sin(p) + vec3(2, 5, 9)) / length(p)).xyz1;
}
";

            using var effect = SKRuntimeEffect.Create(src, out var errorText);
            var uniformSize = effect.UniformSize;

            var uniforms = new SKRuntimeEffectUniforms(effect) { ["iTime"] =(float)time, ["iResolution"] = (float)this.Bounds.Height};

            var children = new SKRuntimeEffectChildren(effect) {  };

            using var shader = effect.ToShader(true, uniforms, children);
            using var paint = new SKPaint { Shader = shader };
            
            
            canvas.DrawRect(SKRect.Create((int)this.Bounds.Width, (int)this.Bounds.Height), paint);
        }
    }
}
