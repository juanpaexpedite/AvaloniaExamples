using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaGLCircleControl : SkiaGLControl
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
            //TODO: Check if I can avoid creating a previous bitmap and apply the shader over the canvas.

            int margin = 16;
            int hmargin = 8;
            int size = 128;
            int hsize = 64;
            var bmp = new SKBitmap(size + margin, size + margin);
            var bmpcanvas = new SKCanvas(bmp);
            bmpcanvas.DrawCircle(hsize + hmargin, hsize + hmargin, hsize, new SKPaint() { Color = SKColors.Orange });

            using var textureShader = bmp.ToShader();

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
            using var paint = new SKPaint { Shader = shader};
            canvas.DrawRect(SKRect.Create(bmp.Width, bmp.Height), paint);
        }

        
    }
}
