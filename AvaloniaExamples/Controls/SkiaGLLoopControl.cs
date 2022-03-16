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
				cspan = 0; lastspan = 0;
				clock = new Clock();
				clock.Subscribe(UpdateTime);
			}
			
		}

		public virtual void Update()
        {
			
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

		protected override void OnOpenGlRender(GlInterface gl, int fb)
        {
			grContext.ResetContext();
			Draw(canvas);
			canvas.Flush();
		}

		public virtual void Draw(SKCanvas canvas)
		{
			canvas.Clear(SKColors.Black);
		}


		protected override void OnOpenGlDeinit(GlInterface gl, int fb)
		{
			clock.PlayState = PlayState.Stop;
			clock = null;
			canvas.Dispose();
			surface.Dispose();
			grContext.Dispose();
			grGlInterface.Dispose();
		}
	}


}
