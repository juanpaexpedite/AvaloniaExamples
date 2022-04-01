using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;

namespace AvaloniaExamples.Windows
{
    public partial class BorderlessWindow : Window
    {
        #region Resize

        //This is used to avoid the border but allow to resize.

        public static readonly DirectProperty<BorderlessWindow, bool> ResizeProperty =
            AvaloniaProperty.RegisterDirect<BorderlessWindow, bool>(nameof(Resize), o => o.Resize, (o, v) => o.Resize = v);

        private bool resize = true;
        public bool Resize
        {
            get { return resize; }
            set
            {
                SetAndRaise(CanResizeProperty, ref resize, value);
            }
        }

        private double resizedistance = 6;

        private bool resizingX = false;
        private bool resizingY = false;
        #endregion

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
            var pos = e.GetCurrentPoint(this);

            if (lastPos.HasValue)
            {
                if (resizingX)
                {
                    double rx = pos.Position.X  - this.Width;
                    this.Width += rx;
                    
                }
                if(resizingY)
                {
                    double ry = pos.Position.Y - this.Height;
                    this.Height += ry;
                }
                
                if(!resizingX && !resizingY)
                {
                    double rx = this.Width - pos.Position.X;
                    double ry = this.Height - pos.Position.Y;

                    if (rx < resizedistance)
                    {
                        resizingX = true;
                    }
                    if(ry < resizedistance)
                    {
                        resizingY = true;
                    }

                    if(!resizingX && !resizingY)
                    {
                        delta = pos.Position - lastPos.Value;
                        this.Position = new PixelPoint(this.Position.X + (int)delta.X, this.Position.Y + (int)delta.Y);
                    }
                }
            }
            else
            {
                CheckCursor(pos.Position);
            }
        }

        private void CheckCursor(Point pos)
        {
            double drx = this.Width - pos.X;
            double dby = this.Height - pos.Y;

            if (drx < resizedistance && dby < resizedistance)
            {
                this.Cursor = new Cursor(StandardCursorType.BottomRightCorner);
            }
            else if(drx < resizedistance)
            {
                this.Cursor = new Cursor(StandardCursorType.RightSide);
            }
            else if(dby < resizedistance)
            {
                this.Cursor = new Cursor(StandardCursorType.BottomSide);
            }
            else
            {
                this.Cursor = new Cursor(StandardCursorType.Arrow);
            }
        }

        protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
        {
            resizingX = false;
            resizingY = false;
            lastPos = null;
        }

        protected override void OnPointerReleased(PointerReleasedEventArgs e)
        {
            resizingX = false;
            resizingY = false;
            lastPos = null;
        }
        #endregion
    }
}
