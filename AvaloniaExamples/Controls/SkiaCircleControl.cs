using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaCircleControl : SkiaLoopControl
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
                SetAndRaise(XProperty, ref x, Math.Round(value));
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
                SetAndRaise(YProperty, ref y, Math.Round(value));
            }
        }
        #endregion

        #region Velocity
        public static readonly DirectProperty<SkiaCircleControl, double> VelocityProperty =
            AvaloniaProperty.RegisterDirect<SkiaCircleControl, double>(nameof(Velocity), o => o.Velocity, (o, v) => o.Velocity = v);

        private double velocity = 2;
        public double Velocity
        {
            get { return velocity; }
            set
            {
                SetAndRaise(VelocityProperty, ref velocity, Math.Round(value));
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
                SetAndRaise(RadiusProperty, ref radius, Math.Round(value));
            }
        }
        #endregion

        private double xvsign=1;
        private double yvsign=1;
        public override void Update()
        {
            X+= xvsign * velocity;
            Y+= yvsign * velocity;

            if(X < 0 || X > 320) { xvsign = -xvsign; }

            if (Y < 0 || Y > 320) { yvsign = -yvsign; }
        }


        SKPaint circlepaint = new SKPaint() { Color = SKColors.Blue };
        SKPoint circlepos = new SKPoint();
        public override void DrawOnCanvasOperation(SKCanvas canvas)
        {
            circlepos.X = (float)x;
            circlepos.Y = (float)y;
            canvas.DrawCircle(circlepos, (float)radius, circlepaint);

        }
    }
}
