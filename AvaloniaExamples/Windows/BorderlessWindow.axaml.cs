using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AvaloniaExamples.Windows
{
    public partial class BorderlessWindow : Window
    {
        public BorderlessWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

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
