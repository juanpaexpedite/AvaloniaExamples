using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaBitmapCColorControl : SkiaBitmapControl
    {
        public override void PreProcessDraw()
        {
            //Create a new Render Target or process the existing depending on your needs.
            //E.G. Make the width dynamic and height fixed
            RenderTarget = new SKBitmap(
                (int)Math.Max(1, Bounds.Width),
                320);
        }

        public override void Draw(SKCanvas canvas)
        {
            //Work with the canvas of the Render Target
            int width = RenderTarget.Width;
            int height = RenderTarget.Height;
            canvas.DrawRect(0,0,width, height, new SKPaint() { Color = SKColors.DarkGray });
            canvas.DrawCircle(new SKPoint(width / 4, width / 4), width/4, new SKPaint() { Color = SKColors.Orange});
        }

        public override void PostProcessDraw()
        {
            //Post processing of the Render Target
            unsafe
            {
               
                var width = RenderTarget.Width;
                var height = RenderTarget.Height;
                var sptr = RenderTargetPtr;

                SKColor scolor = SKColors.Orange;

                float blue = 1;
                uint color = (uint)new SKColor(0, 0, (byte)blue);
                
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        int p = x + y * width;
                        if (*(sptr + p) == (uint)scolor)
                        {
                            *(sptr + p) = color;
                        }
                    }
                    blue += 0.5f;
                    color = (uint)new SKColor(0, 0, (byte)blue);
                }
            }
        }
    }
}
