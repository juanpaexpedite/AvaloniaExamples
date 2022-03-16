using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using AvaloniaExamples.Controls;

namespace AvaloniaExamples.Windows
{
    public partial class FullScreenBorderlessWindow : Window
    {
        public FullScreenBorderlessWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            var openglcontrol = this.FindControl<SkiaGLExampleLoopControl>("SkiaGLExampleLoop");
            if (openglcontrol != null)
            {
                openglcontrol.Width = Screens.Primary.Bounds.Width;
                openglcontrol.Height = Screens.Primary.Bounds.Height;
            }
        }

        #region Input
        Point? lastPos = null;
        Point delta;

        protected override void OnPointerPressed(PointerPressedEventArgs e)
        {
            if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            {
                lastPos = e.GetCurrentPoint(this).Position;
            }
            else
            {
                this.Close();
            }
        }

        protected override void OnPointerMoved(PointerEventArgs e)
        {
            if (lastPos.HasValue)
            {
                delta = e.GetPosition(this) - lastPos.Value;
                this.Position = new PixelPoint(this.Position.X + (int)delta.X, this.Position.Y + (int)delta.Y);
            }
        }

        protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
        {
            lastPos = null;
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            lastPos = null;
        }
        #endregion
    }
}
