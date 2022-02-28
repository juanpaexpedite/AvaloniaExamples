using Avalonia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaTextBlockControl : SkiaControl
    {
        #region Text
        public static readonly DirectProperty<SkiaTextBlockControl, String> TextProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockControl, String>(nameof(Text), o => o.Text, (o, v) => o.Text = v);

        private String text = "hello";
        public string Text
        {
            get { return text; }
            set
            {
                if(SetAndRaise(TextProperty, ref text, value))
                {
                    Update();
                }
                
            }
        }
        #endregion

        Random rnd = new Random();
        public override void DrawOnCanvasOperation(SKCanvas canvas)
        {
            canvas.Clear(SKColors.Black);

            SKPaint textpaint = new SKPaint() { Color = SKColors.White, TextSize = 32.0f };
            SKPoint textpos = new SKPoint(12, 64);
            for (byte i = 0; i < (byte)text.Length; i++)
            {
                textpaint.Color = SKColor.FromHsl(rnd.Next(0, 360), 100, 50);
                canvas.DrawText($"{text[i]}", textpos, textpaint);
                textpos.X += textpaint.MeasureText($"{text[i]}"); ;
            }
        }
    }
}
