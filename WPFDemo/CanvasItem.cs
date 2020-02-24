using System.Windows;
using System.Windows.Shapes;

namespace WPFDemo
{
    public class CanvasItem : BaseViewModel
    {
        private Point _position;

        public CanvasItem(Rectangle content, Point position)
        {
            Content = content;
            Position = position;

            ;
        }

        public Point Position
        {
            get => _position;
            set
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    Content.Margin = new Thickness(value.X, value.Y, 0, 0);
                });
                _position = value;
            }
        }

        public Rectangle Content { get; set; }
    }
}