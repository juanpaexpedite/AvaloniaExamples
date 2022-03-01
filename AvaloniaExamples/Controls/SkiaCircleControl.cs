using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaCircleControl : SkiaControl
    {
        #region X
        public static readonly DirectProperty<SkiaCircleControl, double> XProperty =
            AvaloniaProperty.RegisterDirect<SkiaCircleControl, double>(nameof(X), o => o.X, (o, v) => o.X = v);

        private double x = 32;
        public double X
        {
            get { return x; }
            set
            {
                if (SetAndRaise(XProperty, ref x, Math.Round(value)))
                {
                    Update();
                }
            }
        }
        #endregion

        #region Y
        public static readonly DirectProperty<SkiaCircleControl, double> YProperty =
            AvaloniaProperty.RegisterDirect<SkiaCircleControl, double>(nameof(Y), o => o.Y, (o, v) => o.Y = v);

        private double y = 32;
        public double Y
        {
            get { return y; }
            set
            {
                if (SetAndRaise(YProperty, ref y, Math.Round(value)))
                {
                    Update();
                }
            }
        }
        #endregion

        #region Radius
        public static readonly DirectProperty<SkiaCircleControl, double> RadiusProperty =
            AvaloniaProperty.RegisterDirect<SkiaCircleControl, double>(nameof(Radius), o => o.Radius, (o, v) => o.Radius = v);

        private double radius = 32;
        public double Radius
        {
            get { return radius; }
            set
            {
                if (SetAndRaise(RadiusProperty, ref radius, Math.Round(value)))
                {
                    Update();
                }
            }
        }
        #endregion

        public override void DrawOnCanvasOperation(SKCanvas canvas)
        {
            SKPaint circlepaint = new SKPaint() { Color = SKColors.Blue };
            SKPoint circlepos = new SKPoint((float)x, (float)y);
            canvas.DrawCircle(circlepos, (float)radius, circlepaint);
        }
    }
}
