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
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory(), new SystemTimerUpdater(RefreshRate.High));
            _taskCoordinator.Start();
            Rectangles = new ObservableCollection<Rectangle>();
            DataContext = this;
            AddCanvasItems();
        }

        //A collection of all the items to be displayed on the canvas
        public ObservableCollection<Rectangle> Rectangles { get; set; }

        private void AddCanvasItems()
        {
            Rectangles.Clear();
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
            if (_taskCoordinator.State == TaskState.Paused)
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

        private BaseTask SingleTransformation()
        {
            return GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.Linear);
        }

        private BaseTask Transformation_Linear()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.Linear),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.Linear));
        }

        private BaseTask Transformation_EaseIn()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseInQuad));
        }

        private BaseTask Transformation_EaseOut()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseOutQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseOutQuad));
        }

        private BaseTask Transformation_EaseInOut()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseInOutQuad));
        }

        private BaseTask Transformation_Elastic()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.Elastic),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.Elastic));
        }

        private BaseTask SequentialTransformations()
        {
            return TaskPlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic),
                GetTransformation(Rectangles[1], 600, Rectangles[1].Margin.Top, 300, Easers.EaseInOutCubic));
        }

        private BaseTask ParallelTransformations()
        {
            return TaskPlexFactory.Parallel(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic),
                GetTransformation(Rectangles[1], 600, Rectangles[1].Margin.Top, 300, Easers.EaseInOutCubic));
        }

        private BaseTask RepeatTransformations()
        {
            return TaskPlexFactory.Repeat(
                TaskPlexFactory.Sequential(
                    GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic),
                    GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic)), 2);
        }

        private void OnDemoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem) ComboBox.SelectedValue).Content)
            {
                case "SingleTransformation":
                    _selectedTransformation = SingleTransformation();
                    break;
                case "Transformation_Linear":
                    _selectedTransformation = Transformation_Linear();
                    break;
                case "Transformation_EaseIn":
                    _selectedTransformation = Transformation_EaseIn();
                    break;
                case "Transformation_EaseOut":
                    _selectedTransformation = Transformation_EaseOut();
                    break;
                case "Transformation_EaseInOut":
                    _selectedTransformation = Transformation_EaseInOut();
                    break;
                case "Transformation_Elastic":
                    _selectedTransformation = Transformation_Elastic();
                    break;
                case "SequentialTransformations":
                    _selectedTransformation = SequentialTransformations();
                    break;
                case "ParallelTransformations":
                    _selectedTransformation = ParallelTransformations();
                    break;
                case "RepeatTransformations":
                    _selectedTransformation = RepeatTransformations();
                    break;
                case "SequentialTransformation2":
                    _selectedTransformation = SequentialTransformation2();
                    break;

                case "ParallelTransformation2":
                    _selectedTransformation = ParallelTransformation2();
                    break;
                case "ParallelTransformation3":
                    _selectedTransformation = ParallelTransformation3();
                    break;
            }

            _taskCoordinator.Reset();
        }

        private BaseTask SequentialTransformation2()
        {
            var transformations = new List<BaseTask>();

            foreach (var rectangle in Rectangles)
            {
                transformations.Add(GetTransformation(rectangle, 600, rectangle.Margin.Top, 100,
                    Easers.EaseInOutCubic));
                transformations.Add(GetTransformation(rectangle, 40, rectangle.Margin.Top, 100, Easers.EaseInOutCubic));
            }

            return TaskPlexFactory.Sequential(transformations.ToArray());
        }

        private BaseTask ParallelTransformation2()
        {
            var transformations = new List<BaseTask>();
            var counter = 0;

            foreach (var rectangle in Rectangles)
            {
                var sequentialTransformation = TaskPlexFactory.Sequential(
                    TaskPlexFactory.Wait(TimeSpan.FromMilliseconds(counter++ * 40)),
                    GetTransformation(rectangle, 600, rectangle.Margin.Top, 300, Easers.EaseInOutCubic),
                    GetTransformation(rectangle, 40, rectangle.Margin.Top, 300, Easers.EaseInOutCubic));
                transformations.Add(sequentialTransformation);
            }

            return TaskPlexFactory.Parallel(transformations.ToArray());
        }

        private BaseTask ParallelTransformation3(bool isReversed = false)
        {
            var destinationX = isReversed ? 40 : 600;
            return TaskPlexFactory.Parallel(
                Rectangles
                    .Select(r => GetTransformation(r, destinationX, r.Margin.Top, 300, Easers.EaseInOutCubic))
                    .ToArray());
        }

        private BaseTask GetTransformation(Rectangle target, double destinationX, double destinationY, int duration,
            EaserFunction func)
        {
            var transformation = WPFTransformationFactory.Create(target, "Margin",
                new Thickness(destinationX, destinationY, 0, 0),
                TimeSpan.FromMilliseconds(duration), func);

            transformation.SynchronizationContext = SynchronizationContext.Current;
            return transformation;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            AddCanvasItems();
            _taskCoordinator.Reset();
        }
    }
}