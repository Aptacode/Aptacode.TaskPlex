using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Aptacode.TaskPlex;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Interpolators.Easers;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.WPF.Tasks.Transformation;
using Microsoft.Extensions.Logging.Abstractions;
using Color = System.Drawing.Color;

namespace WPFDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITaskCoordinator _taskCoordinator;

        //The currently selected task to be applied
        private BaseTask _selectedTransformation;

        public MainWindow()
        {
            InitializeComponent();
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory(), RefreshRate.High);
            Rectangles = new ObservableCollection<Rectangle>();
            DataContext = this;
            AddCanvasItems();
        }

        //A collection of all the items to be displayed on the canvas
        public ObservableCollection<Rectangle> Rectangles { get; set; }

        private void AddCanvasItems()
        {
            var colors = GetColors();
            for (var i = 0; i < 10; i++)
            {
                Rectangles.Add(
                    new Rectangle
                    {
                        Width = 35, Height = 35, Fill = colors.ElementAt(i), Margin = new Thickness(40, i * 40, 0, 0)
                    });
            }
        }

        private static IEnumerable<SolidColorBrush> GetColors()
        {
            var interval = 30;

            var colors = new List<Color>();
            for (var red = 100; red < 200; red += interval)
            {
                for (var green = 110; green < 210; green += interval)
                {
                    for (var blue = 120; blue < 220; blue += interval)
                    {
                        if ((red > 150) | (blue > 150) | (green > 150)) //to make sure color is not too dark
                        {
                            colors.Add(Color.FromArgb(255, (byte) red, (byte) green, (byte) blue));
                        }
                    }
                }
            }

            return colors.OrderBy(c => c.GetHue())
                .ThenBy(c => c.GetSaturation())
                .ThenBy(c => c.GetBrightness())
                .Select(c => new SolidColorBrush(System.Windows.Media.Color.FromRgb(c.R, c.G, c.B)));
        }

        private void PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_selectedTransformation.State == TaskState.Paused)
            {
                _taskCoordinator.Resume();
            }
            else
            {
                _taskCoordinator.Apply(_selectedTransformation);
            }
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            _taskCoordinator.Pause();
        }

        private void OnDemoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    _selectedTransformation = GetSequentialTransformation();
                    break;

                case 1:
                    _selectedTransformation = GetParallelTransformation();
                    break;
                case 2:
                    _selectedTransformation = GetSweep();
                    break;
            }

            _taskCoordinator.Reset();

            _taskCoordinator.Apply(_selectedTransformation);
        }

        private BaseTask GetSequentialTransformation()
        {
            var transformations = new List<BaseTask>();

            foreach (var rectangle in Rectangles)
            {
                transformations.Add(GetTransformation(rectangle, 600, rectangle.Margin.Top, 100));
                transformations.Add(GetTransformation(rectangle, 40, rectangle.Margin.Top, 100));
            }

            return TaskPlexFactory.Sequential(transformations.ToArray());
        }

        private BaseTask GetParallelTransformation()
        {
            var transformations = new List<BaseTask>();

            var counter = 0;
            foreach (var rectangle in Rectangles)
            {
                var sequentialTransformation = TaskPlexFactory.Sequential(
                    TaskPlexFactory.Wait(TimeSpan.FromMilliseconds(counter++ * 40)),
                    GetTransformation(rectangle, 600, rectangle.Margin.Top, 300),
                    GetTransformation(rectangle, 40, rectangle.Margin.Top, 300));
                transformations.Add(sequentialTransformation);
            }

            return TaskPlexFactory.Parallel(transformations.ToArray());
        }

        private BaseTask GetSweep(bool isReversed = false)
        {
            var destinationX = isReversed ? 40 : 600;
            return TaskPlexFactory.Parallel(
                Rectangles
                    .Select(r => GetTransformation(r, destinationX, r.Margin.Top, 300))
                    .ToArray());
        }

        private BaseTask GetTransformation(Rectangle target, double destinationX, double destinationY, int duration)
        {
            var transformation = WPFTransformationFactory.Create(target, "Margin",
                new Thickness(destinationX, destinationY, 0, 0),
                TimeSpan.FromMilliseconds(duration), Easers.EaseInOutCubic);

            transformation.SynchronizationContext = SynchronizationContext.Current;
            return transformation;
        }
    }
}