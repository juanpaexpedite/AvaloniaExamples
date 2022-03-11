using Avalonia.Animation;
using Avalonia.OpenGL;
using Avalonia.OpenGL.Controls;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaGLLoopControl : OpenGlControlBase
	{

#nullable disable
		private GRGlInterface grGlInterface;
		private GRContext grContext;
		private SKSurface surface;
		private SKCanvas canvas;
#nullable enable

		protected override void OnOpenGlInit(GlInterface gl, int fb)
		{
			grGlInterface = GRGlInterface.Create(gl.GetProcAddress);
			grGlInterface.Validate();
			grContext = GRContext.CreateGl(grGlInterface);
			var renderTarget = new GRBackendRenderTarget((int)Width, (int)Height, 0, 8, new GRGlFramebufferInfo((uint)fb, SKColorType.Rgba8888.ToGlSizedFormat()));
			surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);
			canvas = surface.Canvas;

			if (clock == null)
			{
				clock = new Clock();
				clock.Subscribe(UpdateTime);
			}
			else
			{
				clock.PlayState = PlayState.Run;
			}
		}

		float x = 0;
		float y = 64;

		private void Update()
        {
			x += 0.5f;
			InvalidateVisual();

		}

		private Clock clock;
		private double cspan = 0;
		private double deltaspan = 0;
		private double lastspan = 0;
		private double millistimespan = 24;

		private void UpdateTime(TimeSpan span)
		{
			if (lastspan == cspan)
			{
				InvalidateVisual();
			}

			cspan = span.TotalMilliseconds;
			deltaspan = cspan - lastspan;

			if (deltaspan > millistimespan)
			{
				lastspan = cspan;
				Update();
			}
		}

		//protected override void OnOpenGlRender(GlInterface gl, int fb)
		//{
		//	grContext.ResetContext();
		//	canvas.Clear(SKColors.Cyan);
		//	using var red = new SKPaint { Color = SKColors.Red };
		//	canvas.DrawCircle(x,y,32, red);
		//	canvas.Flush();
		//	//Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		//}

		private const string PathToImages = @"D:\AVALONIA\AvaloniaExamples\AvaloniaExamples\Assets\";

		protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
			grContext.ResetContext();
			Draw(canvas);
			canvas.Flush();
		}

		protected  void OldOnOpenGlRender(GlInterface gl, int fb)
		{
			grContext.ResetContext();
			canvas.Clear(SKColors.Cyan);

			using var blueShirt = SKImage.FromEncodedData(Path.Combine(PathToImages, "colorwheel.png"));
			using var textureShader = blueShirt.ToShader();

			var src = @"
	uniform shader image; 
	half4 main(float2 coord) {
	coord.x += sin(coord.y / 3) * 4;
	//return image.eval(coord);
	return sample(image, coord);
	}";

			//uniform shader color_map;
			//uniform float scale;
			//uniform half exp;
			//uniform float3 in_colors0;
			//half4 main(float2 p)
			//{
			//	half4 texColor = sample(color_map, p);
			//	if (length(abs(in_colors0 - pow(texColor.rgb, half3(exp)))) < scale)
			//		discard;
			//	return texColor;

			using var effect = SKRuntimeEffect.Create(src, out var errorText);


			var uniformSize = effect.UniformSize;

			var uniforms = new SKRuntimeEffectUniforms(effect)
			{
			};

            //var uniforms = new SKRuntimeEffectUniforms(effect)
            //{

            //	//["scale"] = 1.0f,
            //	//["exp"] = 0.5f,
            //	//["in_colors0"] = new[] { 1f, 1f, 1f },
            //};

            var children = new SKRuntimeEffectChildren(effect)
            {
                ["image"] = textureShader
            };

            using var shader = effect.ToShader(true, uniforms ,children);
			using var paint = new SKPaint { Shader = shader };
			canvas.DrawRect(SKRect.Create(400, 400), paint);

			canvas.Flush();
			//Dispatcher.UIThread.Post(InvalidateVisual, DispatcherPriority.Render);
		}

		public virtual void Draw(SKCanvas canvas)
		{
			canvas.Clear(SKColors.Black);
		}


		protected override void OnOpenGlDeinit(GlInterface gl, int fb)
		{
			clock.PlayState = PlayState.Stop;
			canvas.Dispose();
			surface.Dispose();
			grContext.Dispose();
			grGlInterface.Dispose();
		}
	}


}
