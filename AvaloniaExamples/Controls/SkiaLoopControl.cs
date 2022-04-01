using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
   
    public class SkiaLoopControl : Control
    {
        private double millistimespan = 0;
        private DrawOperation operation;

        public SkiaLoopControl() : this(10)
        {
            operation = new DrawOperation(Bounds, DrawOnCanvasOperation);
            ClipToBounds = true;
        }

        public SkiaLoopControl(double millistimespan)
        {
            operation = new DrawOperation(Bounds, DrawOnCanvasOperation);
            ClipToBounds = true;

            this.millistimespan = millistimespan;
        }

        #region Clock
        private Clock clock;
       

        private double cspan = 0;
        private double deltaspan = 0;
        private double lastspan = 0;
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            if(clock == null)
            {
                clock = new Clock();
                clock.Subscribe(UpdateTime);
            }
            else
            {
                clock.PlayState = PlayState.Run;
            }
            base.OnAttachedToVisualTree(e);
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            clock.PlayState = PlayState.Stop;
            base.OnDetachedFromVisualTree(e);
        }
        private void UpdateTime(TimeSpan span)
        {
            if (lastspan == cspan)
            {
                Draw();
            }

            cspan = span.TotalMilliseconds;
            deltaspan = cspan - lastspan;
            
            if(deltaspan > millistimespan)
            {
                lastspan = cspan;
                Update();
            }
        }
        #endregion

        public virtual void Update()
        {

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
            //Do nothing because can cover what was painted if called base.Draw
        }


        public override void Render(DrawingContext context)
        {
            context.Custom(operation);
        }
    }
}
