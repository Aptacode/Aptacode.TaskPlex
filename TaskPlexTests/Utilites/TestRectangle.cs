using System;
using System.Drawing;

namespace Aptacode.TaskPlex.Tests.Utilites
{
    public class ValueUpdateArgs<T> : EventArgs
    {
        public T OldValue { get; set; }
        public T NewValue { get; set; }
        public ValueUpdateArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
    public class TestRectangle
    {
        public event EventHandler<ValueUpdateArgs<int>> OnWidthChange;
        public event EventHandler<ValueUpdateArgs<int>> OnHeightChange;
        public event EventHandler<ValueUpdateArgs<double>> OnOpacityChanged;
        public event EventHandler<ValueUpdateArgs<string>> OnNameChanged;
        public event EventHandler<ValueUpdateArgs<Color>> OnBackgroundChanged;

        private int _width;

        public int Width
        {
            get => _width;
            set
            {
                int oldValue = _width;
                _width = value;
                OnWidthChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, _width));
            }
        }

        private int _height;

        public int Height
        {
            get => _height;
            set
            {
                int oldValue = _height;
                _height = value;
                OnHeightChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, _height));
            }
        }

        private double _opacity;

        public double Opacity
        {
            get => _opacity;
            set
            {
                var oldValue = _opacity;
                _opacity = value;
                OnOpacityChanged?.Invoke(this, new ValueUpdateArgs<double>(oldValue, _opacity));
            }
        }
        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                var oldValue = _name;
                _name = value;
                OnNameChanged?.Invoke(this, new ValueUpdateArgs<string>(oldValue, _name));
            }
        }

        private Color _backgroundColor;

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set
            {
                var oldValue = _backgroundColor;
                _backgroundColor = value;
                OnBackgroundChanged?.Invoke(this, new ValueUpdateArgs<Color>(oldValue, _backgroundColor));
            }
        }

        public TestRectangle()
        {
            Width = 10;
            Height = 20;
            Opacity = 0;
            BackgroundColor = Color.White;
        }

    }
}
