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
    public class SkiaTextBlockBlurFontControl : SkiaControl
    {
        #region FontFamily
        public static readonly DirectProperty<SkiaTextBlockBlurFontControl, String> FontFamilyProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockBlurFontControl, String>(nameof(FontFamily), o => o.FontFamily, (o, v) => o.FontFamily = v);

        private String fontfamily = "Arial";
        public string FontFamily
        {
            get => fontfamily;
            set => SetAndRaiseUpdate(FontFamilyProperty, ref fontfamily, value);
        }
        #endregion

        #region Text
        public static readonly DirectProperty<SkiaTextBlockBlurFontControl, String> TextProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockBlurFontControl, String>(nameof(Text), o => o.Text, (o, v) => o.Text = v);

        private String text = "hello";
        public string Text
        {
            get => text;
            set => SetAndRaiseUpdate(TextProperty, ref text, value);
        }
        #endregion

        #region Antialias
        public static readonly DirectProperty<SkiaTextBlockBlurFontControl, bool> AntialiasProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockBlurFontControl, bool>(nameof(Antialias), o => o.Antialias, (o, v) => o.Antialias = v);

        private bool antialias = true;
        public bool Antialias
        {
            get => antialias;
            set => SetAndRaiseUpdate(AntialiasProperty, ref antialias, value);
        }
        #endregion

        #region Blurness
        public static readonly DirectProperty<SkiaTextBlockBlurFontControl, uint> BlurnessProperty =
            AvaloniaProperty.RegisterDirect<SkiaTextBlockBlurFontControl, uint>(nameof(Blurness), o => o.Blurness, (o, v) => o.Blurness = v);

        private uint blurness = 0;
        public uint Blurness
        {
            get => blurness;
            set => SetAndRaiseUpdateDebounce(BlurnessProperty, ref blurness, value);
        }
        #endregion

        public override void Draw(SKCanvas canvas)
        {
            SKPaint textpaint = new SKPaint() { Color = SKColors.White, StrokeWidth = 0, SubpixelText = true, IsAntialias = antialias, TextSize = 64 };
            SKFont textfont = new SKFont(SKTypeface.FromFamilyName(FontFamily), size: 64);
            textfont.Edging = antialias ? SKFontEdging.Antialias : SKFontEdging.Alias;
            textfont.Subpixel = true;
            if (blurness > 0)
            {
                textpaint.ImageFilter = SKImageFilter.CreateBlur(blurness, blurness);
            }

            canvas.DrawText(text, 4, 64, textfont, textpaint);

        }
    }
}
