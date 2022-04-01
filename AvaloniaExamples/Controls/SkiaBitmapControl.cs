using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using Avalonia.Threading;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{

    /// <summary>
    /// In this case the control renders in a SKBitmap instead of the control itself.
    /// After operate on rendertarget and allows postprocess, it draws on itself.
    /// </summary>
    public class SkiaBitmapControl : Control
    {
        /// <summary>
        /// Call this inside the Update Method
        /// </summary>
        protected SKBitmap RenderTarget = new SKBitmap(1, 1);

        /// <summary>
        /// Call this inside the Update Method
        /// </summary>
        unsafe
        protected uint* RenderTargetPtr { get => (uint*)RenderTarget.GetPixels(); }

        public SkiaBitmapControl() : this(33)
        {
           
        }

        /// <summary>
        /// Because the control is not rendered but into a bitmap we have to add when the size changes.
        /// </summary>
        static SkiaBitmapControl()
        {
            AffectsMeasure<SkiaBitmapCColorControl>(TransformedBoundsProperty);
        }


        public SkiaBitmapControl(uint debouncingmilliseconds)
        {
            operation = new DrawOperation(Bounds, DrawRenderTarget);
            ClipToBounds = true;

            InitializeDebounce(debouncingmilliseconds);
        }

        /// <summary>
        /// In case you have to call Update when SetAndRaise
        /// </summary>
        public bool SetAndRaiseUpdate<T>(AvaloniaProperty<T> property, ref T field, T value)
        {
            if (SetAndRaise(property, ref field, value))
            {
                InvalidateStable();
                return true;
            }
            return false;
        }

        #region Debouncing
        /// <summary>
        /// In case you have to call Update with debouning
        /// </summary>
        public bool SetAndRaiseUpdateDebounce<T>(AvaloniaProperty<T> property, ref T field, T value)
        {
            if (SetAndRaise(property, ref field, value))
            {
                Invalidate();
                return true;
            }
            return false;
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

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            Invalidate();
            base.OnAttachedToVisualTree(e);
        }

        //TODO: Add in the other controls
        private void Debouncedog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            InvalidateStable();
        }

        /// <summary>
        /// Call this in the constructor AffectsMeasure<SkiaBitmapCColorControl>(BoundsProperty);
        /// </summary>
        protected override void OnMeasureInvalidated()
        {
            Invalidate();
        }
        #endregion

        /// <summary>
        /// It is public in case you update the fields in code behind and want to 'animate' it.
        /// </summary>
        public void InvalidateStable()
        {
            debouncedog.Stop();

            //This is the rectangle that will be redrawn!
            if (TransformedBounds != null)
            {
                operation.Bounds = TransformedBounds.Value.Bounds;
            }

            PreProcessDraw();

            using (var canvas = new SKCanvas(RenderTarget))
            {
                Draw(canvas);
            }

            PostProcessDraw();

            InvalidateVisual();
        }

        #region Draw RenderTarget in Control
        private DrawOperation operation;
        private SKPoint skzero = new SKPoint(0, 0);
        private void DrawRenderTarget(SKCanvas canvas)
        {
            canvas.DrawBitmap(RenderTarget, skzero);
        }
        #endregion


        public override void Render(DrawingContext context)
        {
            context.Custom(operation);
        }

        /// <summary>
        /// Update here the rendertarget bitmap size for instance
        /// </summary>
        public virtual void PreProcessDraw()
        {

        }

        public virtual void Draw(SKCanvas canvas)
        {

        }

        /// <summary>
        /// Update here the rendertarget bitmap
        /// </summary>
        public virtual void PostProcessDraw()
        {
           
        }
       
      
    }
}
