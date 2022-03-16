using Avalonia;
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
        #region Number
        public static readonly DirectProperty<SkiaGLExampleLoopControl, int> NumberProperty =
            AvaloniaProperty.RegisterDirect<SkiaGLExampleLoopControl, int>(nameof(Number), o => o.Number, (o, v) => o.Number = v);

        private int number = 1;
        public int Number
        {
            get { return number; }
            set
            {
                if(SetAndRaise(NumberProperty, ref number, value))
                {
                    CreateExample();
                }
            }
        }

        private void CreateExample()
        {
            switch (number)
            {
                case 1:
                    CreateExampleOne();
                    break;
                case 2:
                    CreateExampleTwo();
                    break;
                case 3:
                    CreateExampleThree();
                    break;
                default:
                    CreateExampleOne();
                    break;
            }
        }
        #endregion

        float time = 0;
        public override void Update()
        {
            if (uniforms == null)
            {
                CreateExample();
                return;
            }
                

            time += 0.01f;
            

            if(number == 1)
            {
                UpdateExampleOne();
            }
            else if(number == 2)
            {
                UpdateExampleTwo();
            }
            else if(number == 3)
            {
                UpdateExampleThree();
            }

            base.Update();
        }


        public override void Draw(SKCanvas canvas)
        {
            canvas.Clear();
            canvas.DrawRect(SKRect.Create((int)this.Bounds.Width, (int)this.Bounds.Height), paint);
        }

        SKRuntimeEffectUniforms uniforms;
        SKPaint paint = new SKPaint() {  };
        SKShader shader;
        SKRuntimeEffect effect;
        SKRuntimeEffectChildren children;
        private void CreateExampleOne()
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

            effect = SKRuntimeEffect.Create(src, out var errorText);
            var uniformSize = effect.UniformSize;

            uniforms = new SKRuntimeEffectUniforms(effect) { ["iTime"] = (float)time, ["iResolution"] = (float)this.Bounds.Height };

            children = new SKRuntimeEffectChildren(effect) { };

            paint.Shader = effect.ToShader(false, uniforms, children);
        }

        private void UpdateExampleOne()
        {
            uniforms["iTime"] = (float)time;
            uniforms["iResolution"] = (float)this.Bounds.Height;
            paint.Shader = effect.ToShader(false, uniforms, children);
        }

        private void CreateExampleTwo()
        {
            //Example source: https://twitter.com/nasana_x/status/1254091446333759488

            var src = @"
uniform float2 r;  
uniform float  t;       // Shader playback time (s)

vec4 main(vec2 fragcoord) {

vec2 p=(fragcoord.xy*2.-r)/min(r.y,r.x);

for(float i=1.;i<7.;i++) {
 p.x+=i*cos(p.y*i*.1+t);
 p.y+=i*sin(p.x*i*.1+t);
};

 return vec4(.5/length(p),.1/length(p),1./length(p),1.);
}
";

            effect = SKRuntimeEffect.Create(src, out var errorText);
            var uniformSize = effect.UniformSize;

            uniforms = new SKRuntimeEffectUniforms(effect) { ["t"] = (float)time, ["r"] = new float[2] { 32, 32 } };

            children = new SKRuntimeEffectChildren(effect) { };

            paint.Shader = effect.ToShader(true, uniforms, children);
        }

        private void UpdateExampleTwo()
        {
            uniforms["t"] = (float)time;
            uniforms["r"] = new float[2] { 32, 32 };
            paint.Shader = effect.ToShader(true, uniforms, children);
        }

        private void CreateExampleThree()
        {
            //Example source: https://www.shadertoy.com/view/wt2GRt

            var bmp = new SKBitmap(128,128);
            var bmpcanvas = new SKCanvas(bmp);
            bmpcanvas.DrawCircle(32,32,32, new SKPaint() { Color = SKColors.Orange });

            var textureShader = bmp.ToShader();

            var src = @"
uniform float time;       // Shader playback time (s)
uniform vec2 resolution;  // Viewport resolution (pixels)
uniform shader tex; 

half4 main(vec2 fragcoord) { 
    
vec2 uv = fragcoord.xy / resolution.x;
vec4 texture_color = vec4(0.192156862745098, 0.6627450980392157, 0.9333333333333333, 1.0);
    
     vec4 k = vec4(time)*0.8;
	k.xy = uv * 7.0;
    float val1 = length(0.5-fract(k.xyw*=mat3(vec3(-2.0,-1.0,0.0), vec3(3.0,-1.0,1.0), vec3(1.0,-1.0,-1.0))*0.5));
    float val2 = length(0.5-fract(k.xyw*=mat3(vec3(-2.0,-1.0,0.0), vec3(3.0,-1.0,1.0), vec3(1.0,-1.0,-1.0))*0.2));
    float val3 = length(0.5-fract(k.xyw*=mat3(vec3(-2.0,-1.0,0.0), vec3(3.0,-1.0,1.0), vec3(1.0,-1.0,-1.0))*0.5));
    vec4 color = vec4 ( pow(min(min(val1,val2),val3), 7.0) * 3.0)+texture_color;
   return color;

}
";

            effect = SKRuntimeEffect.Create(src, out var errorText);
            var uniformSize = effect.UniformSize;

            uniforms = new SKRuntimeEffectUniforms(effect) { 
                ["time"] = (float)time, 
                ["resolution"] = new float[2] { (float)this.Bounds.Width, (float)this.Bounds.Height }
            };

            children = new SKRuntimeEffectChildren(effect) { ["tex"] = textureShader };

            paint.Shader = effect.ToShader(true, uniforms, children);

        }

        private void UpdateExampleThree()
        {
            uniforms["time"] = (float)time;
            uniforms["resolution"] = new float[2] { (float)this.Bounds.Width, (float)this.Bounds.Height };
            paint.Shader = effect.ToShader(true, uniforms, children);
        }
    }
}
