using System;
using System.Drawing;

namespace Aptacode_TaskCoordinator.Tests.Utilites
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
        public event EventHandler<ValueUpdateArgs<int>> OnHeigtChange;
        public event EventHandler<ValueUpdateArgs<double>> OnOpacityChanged;
        public event EventHandler<ValueUpdateArgs<string>> OnNameChanged;
        public event EventHandler<ValueUpdateArgs<Color>> OnBackgroundChanged;

        private int width;

        public int Width
        {
            get { return width; }
            set
            {
                int oldValue = width;
                width = value;
                OnWidthChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, width));
            }
        }

        private int height;

        public int Height
        {
            get { return height; }
            set
            {
                int oldValue = height;
                height = value;
                OnHeigtChange?.Invoke(this, new ValueUpdateArgs<int>(oldValue, height));
            }
        }

        private double opacity;

        public double Opacity
        {
            get { return opacity; }
            set
            {
                double oldValue = opacity;
                opacity = value;
                OnOpacityChanged?.Invoke(this, new ValueUpdateArgs<double>(oldValue, opacity));
            }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set
            {
                string oldValue = name;
                name = value;
                OnNameChanged?.Invoke(this, new ValueUpdateArgs<string>(oldValue, name));
            }
        }

        private Color backgroundColor;

        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set
            {
                Color oldValue = backgroundColor;
                backgroundColor = value;
                OnBackgroundChanged?.Invoke(this, new ValueUpdateArgs<Color>(oldValue, backgroundColor));
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
