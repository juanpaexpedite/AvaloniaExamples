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
    public class SkiaGLBitmapControl : SkiaGLControl
    {
        #region X
        public static readonly DirectProperty<SkiaGLCircleControl, double> XProperty =
            AvaloniaProperty.RegisterDirect<SkiaGLCircleControl, double>(nameof(X), o => o.X, (o, v) => o.X = v);

        private double x = 4;
        public double X
        {
            get { return x; }
            set
            {
                SetAndRaiseUpdate(XProperty, ref x, Math.Round(value));
            }
        }
        #endregion

        #region Y
        public static readonly DirectProperty<SkiaGLCircleControl, double> YProperty =
            AvaloniaProperty.RegisterDirect<SkiaGLCircleControl, double>(nameof(Y), o => o.Y, (o, v) => o.Y = v);

        private double y = 3;
        public double Y
        {
            get { return y; }
            set
            {
                SetAndRaiseUpdate(YProperty, ref y, Math.Round(value));
            }
        }
        #endregion

        public override void Draw(SKCanvas canvas)
        {
            var apppath = System.AppDomain.CurrentDomain.BaseDirectory;
            using var colorwheel = SKImage.FromEncodedData(Path.Combine(apppath, "Assets\\colorwheel.png"));
            using var textureShader = colorwheel.ToShader();

            var src = @"
                uniform float dx;
                uniform float dy;
	            uniform shader image; 
	            half4 main(float2 coord) {
	            coord.x += sin(coord.y / dy) * dx;
	            return sample(image, coord);
	            }";

            using var effect = SKRuntimeEffect.Create(src, out var errorText);
            var uniformSize = effect.UniformSize;

            var uniforms = new SKRuntimeEffectUniforms(effect) { ["dx"] = (float)x, ["dy"] = (float)y };
            var children = new SKRuntimeEffectChildren(effect) { ["image"] = textureShader };

            using var shader = effect.ToShader(true, uniforms, children);
            using var paint = new SKPaint { Shader = shader };
            canvas.DrawRect(SKRect.Create(colorwheel.Width, colorwheel.Height), paint);
        }
    }
}
