using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Rendering.SceneGraph;
using Avalonia.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaControl : Control
    {
        private DrawOperation operation;

        public SkiaControl()
        {
            operation = new DrawOperation(Bounds, DrawOnCanvasOperation);
            ClipToBounds = true;
        }

        public void Update()
        {
            operation.Bounds = this.Bounds;

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
