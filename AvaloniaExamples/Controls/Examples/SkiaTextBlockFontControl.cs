using Avalonia;
using Avalonia.Media;
using Avalonia.Visuals.Media.Imaging;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaExamples.Controls
{
    public class SkiaTextBlockFontControl : SkiaControl
    {
        #region FontFamily
        public static readonly DirectProperty<SkiaTextBlockFontControl, String> FontFamilyProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockFontControl, String>(nameof(FontFamily), o => o.FontFamily, (o, v) => o.FontFamily = v);

        private String fontfamily = "Arial";
        public string FontFamily
        {
            get => fontfamily;
            set => SetAndRaiseUpdate(FontFamilyProperty, ref fontfamily, value);
        }
        #endregion

        #region Text
        public static readonly DirectProperty<SkiaTextBlockFontControl, String> TextProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockFontControl, String>(nameof(Text), o => o.Text, (o, v) => o.Text = v);

        private String text = "hello";
        public string Text
        {
            get => text;
            set => SetAndRaiseUpdate(TextProperty, ref text, value);
        }
        #endregion

        #region Antialias
        public static readonly DirectProperty<SkiaTextBlockFontControl, bool> AntialiasProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockFontControl, bool>(nameof(Antialias), o => o.Antialias, (o, v) => o.Antialias = v);

        private bool antialias = true;
        public bool Antialias
        {
            get => antialias;
            set => SetAndRaiseUpdate(AntialiasProperty, ref antialias, value);
        }
        #endregion

        #region Blur
        public static readonly DirectProperty<SkiaTextBlockFontControl, bool> BlurProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockFontControl, bool>(nameof(Blur), o => o.Blur, (o, v) => o.Blur = v);

        private bool blur = true;
        public bool Blur
        {
            get =>  blur;
            set => SetAndRaiseUpdate(BlurProperty, ref blur, value);
        }
        #endregion

        public override void Draw(SKCanvas canvas)
        {
            SKPaint textpaint = new SKPaint() { Color = SKColors.White, StrokeWidth=0, SubpixelText = true, IsAntialias = antialias, TextSize=64};
            SKFont textfont = new SKFont(SKTypeface.FromFamilyName(FontFamily), size: 64);
            textfont.Edging = antialias ? SKFontEdging.Antialias : SKFontEdging.Alias;
            textfont.Subpixel = true;
            if (blur)
            {
                textpaint.ImageFilter = SKImageFilter.CreateBlur(2, 2);
            }

            canvas.DrawText(text, 4, 64, textfont, textpaint);

        }
    }
}
