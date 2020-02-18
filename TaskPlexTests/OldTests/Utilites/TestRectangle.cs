using System;
using System.Drawing;

namespace Aptacode.TaskPlex.Tests.OldTests.Utilites
{
    public class TestRectangle
    {
        private Color _backgroundColor;

        private int _height;
        private string _name;

        private double _opacity;

        private int _width;

        public TestRectangle()
        {
            Width = 10;
            Height = 20;
            Opacity = 0;
            BackgroundColor = Color.White;
        }

        public int Width
        {
            get => _width;
            set
            {
                var oldValue = _width;
                _width = value;
                OnWidthChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, _width));
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                var oldValue = _height;
                _height = value;
                OnHeightChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, _height));
            }
        }

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

        public event EventHandler<ValueUpdateArgs<int>> OnWidthChange;
        public event EventHandler<ValueUpdateArgs<int>> OnHeightChange;
        public event EventHandler<ValueUpdateArgs<double>> OnOpacityChanged;
        public event EventHandler<ValueUpdateArgs<string>> OnNameChanged;
        public event EventHandler<ValueUpdateArgs<Color>> OnBackgroundChanged;
    }
}