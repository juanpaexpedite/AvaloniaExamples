using Avalonia;
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
	public class SkiaGLControl : OpenGlControlBase
	{
		public SkiaGLControl() : this(33)
		{

		}

		public SkiaGLControl(uint debouncingmilliseconds)
		{
			ClipToBounds = true;

			InitializeDebounce(debouncingmilliseconds);
		}

        /// <summary>
        /// In case you have to call Update when SetAndRaise
        /// </summary>
        public void SetAndRaiseUpdate<T>(AvaloniaProperty<T> property, ref T field, T value)
        {
            if (SetAndRaise(property, ref field, value))
            {
                InvalidateStable();
            }
        }

        #region Debouncing
        /// <summary>
        /// In case you have to call Update with debouning
        /// </summary>
        public void SetAndRaiseUpdateDebounce<T>(AvaloniaProperty<T> property, ref T field, T value)
        {
            if (SetAndRaise(property, ref field, value))
            {
                Invalidate();
            }
        }

        /// <summary>
        /// It is public in case you update the fields in code behind and want to 'animate' stable it.
        /// </summary>
        public void Invalidate()
        {
            debouncedog.Stop();
            debouncedog.Start();
        }

        private System.Timers.Timer debouncedog;

        private void InitializeDebounce(uint debouncingmiliseconds)
        {
            debouncedog = new System.Timers.Timer();
            debouncedog.Interval = debouncingmiliseconds;
            debouncedog.Elapsed += Debouncedog_Elapsed;
        }

        private void Debouncedog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            InvalidateStable();
        }

        #endregion

        /// <summary>
        /// It is public in case you update the fields in code behind and want to 'animate' it.
        /// </summary>
        public void InvalidateStable()
        {
            InvalidateVisual();
        }


#nullable disable
        private GRGlInterface grGlInterface;
		private GRContext grContext;
		private SKSurface surface;
		private SKCanvas canvas;
        private GRBackendRenderTarget renderTarget;
        private uint _fb;
#nullable enable

        private void InitCanvas()
        {
            renderTarget = new GRBackendRenderTarget((int)Width, (int)Height, 0, 8, new GRGlFramebufferInfo(_fb, SKColorType.Rgba8888.ToGlSizedFormat()));
            surface = SKSurface.Create(grContext, renderTarget, GRSurfaceOrigin.TopLeft, SKColorType.Rgba8888);
            canvas = surface.Canvas;
        }

        protected override void OnOpenGlInit(GlInterface gl, int fb)
		{
            _fb = (uint)fb;
            grGlInterface = GRGlInterface.Create(gl.GetProcAddress);
			grGlInterface.Validate();
			grContext = GRContext.CreateGl(grGlInterface);
            InitCanvas();
			
		}

        protected override void OnMeasureInvalidated()
        {
            //On some situations can be called OnMeasureInvalidated Before OnOpenGlInit so this solves that.
            if (grGlInterface.Handle != IntPtr.Zero)
            {
                InitCanvas();
            }
            base.OnMeasureInvalidated();
        }



        protected override void OnOpenGlRender(GlInterface gl, int fb)
		{
			grContext.ResetContext();
            canvas.Clear();
			Draw(canvas);
			canvas.Flush();
		}

		public virtual void Draw(SKCanvas canvas)
		{
			canvas.Clear(SKColors.Black);
		}

		protected override void OnOpenGlDeinit(GlInterface gl, int fb)
		{
			canvas.Dispose();
			surface.Dispose();
			grContext.Dispose();
			grGlInterface.Dispose();
		}
	}
}
