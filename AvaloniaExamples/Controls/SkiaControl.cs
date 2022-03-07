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
    public class SkiaControl : Control
    {
        private DrawOperation operation;

        public SkiaControl() : this(33)
        {

        }

        public SkiaControl(uint debouncingmilliseconds)
        {
            operation = new DrawOperation(Bounds, DrawOnCanvasOperation);
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
                Draw();
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
                debouncedog.Stop();
                debouncedog.Start();
            }
        }

        private System.Timers.Timer debouncedog;

        private void InitializeDebounce(uint debouncingmiliseconds)
        {
            debouncedog= new System.Timers.Timer();
            debouncedog.Interval = debouncingmiliseconds;
            debouncedog.Elapsed += Debouncedog_Elapsed;
        }
        #endregion

        private void Debouncedog_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Draw();
        }

        public void Draw()
        {
            //This is the rectangle that will be redrawn!
            if (TransformedBounds != null)
            {
                operation.Bounds = TransformedBounds.Value.Bounds;
            }

            InvalidateVisual();
        }

        public virtual void DrawOnCanvasOperation(SKCanvas canvas)
        {
            canvas.Clear(SKColors.Black);
        }

        public override void Render(DrawingContext context)
        {
            context.Custom(operation);
        }
    }


    /// <summary>
    /// No need to touch
    /// </summary>
    class DrawOperation : ICustomDrawOperation
    {

        public DrawOperation(Rect bounds, Action<SKCanvas> DrawOnCanvasOperation)
        {
            this.Bounds = bounds;
            this.drawoncanvasoperation = DrawOnCanvasOperation;
        }

        #region ICustomDrawOperation
        public Rect Bounds { get; set; }
        public void Dispose() { }
        public bool HitTest(Point p) => false;
        public bool Equals(ICustomDrawOperation other) => false;
        #endregion

        private Action<SKCanvas> drawoncanvasoperation;

        public void Render(IDrawingContextImpl context)
        {
            drawoncanvasoperation((context as ISkiaDrawingContextImpl)?.SkCanvas);
        }
    }
}
