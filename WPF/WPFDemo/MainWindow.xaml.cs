using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Aptacode.TaskPlex.Engine;
using Aptacode.TaskPlex.Engine.Enums;
using Aptacode.TaskPlex.Engine.Interfaces;
using Aptacode.TaskPlex.Engine.Tasks;
using Aptacode.TaskPlex.Interpolation.Easers;
using Aptacode.TaskPlex.WPF.Tasks.Transformation;
using Color = System.Drawing.Color;

namespace WPFDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IPlexEngine _plexEngine;

        //The currently selected task to be applied
        private BaseTask _selectedTransformation;

        public MainWindow()
        {
            InitializeComponent();
            _plexEngine = new PlexEngine(new NullLoggerFactory(), new SystemTimerUpdater(RefreshRate.High));
            _plexEngine.Start();
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
            for (var i = 0; i < 20; i++)
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
            if (_plexEngine.State == TaskState.Paused)
            {
                _plexEngine.Resume();
            }
            else
            {
                _plexEngine.Apply(_selectedTransformation);
            }
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            _plexEngine.Pause();
        }

        private BaseTask SingleTransformation()
        {
            var transformation = WPFTransformationFactory.Create(Rectangles[0], "Margin",
                TimeSpan.FromMilliseconds(600), Easers.EaseInOutCubic, false,
                new Thickness(100, 0, 0, 0),
                new Thickness(50, 300, 0, 0),
                new Thickness(300, 50, 0, 0),
                new Thickness(400, 100, 0, 0));

            transformation.SynchronizationContext = SynchronizationContext.Current;
            return transformation;
        }

        private BaseTask Transformation_Linear()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.Linear),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.Linear));
        }

        private BaseTask Transformation_EaseIn()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseInQuad));
        }

        private BaseTask Transformation_EaseOut()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseOutQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseOutQuad));
        }

        private BaseTask Transformation_EaseInOut()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutQuad),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.EaseInOutQuad));
        }

        private BaseTask Transformation_Elastic()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.Elastic),
                GetTransformation(Rectangles[0], 40, Rectangles[0].Margin.Top, 300, Easers.Elastic));
        }

        private BaseTask SequentialTransformations()
        {
            return PlexFactory.Sequential(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic),
                GetTransformation(Rectangles[1], 600, Rectangles[1].Margin.Top, 300, Easers.EaseInOutCubic));
        }

        private BaseTask ParallelTransformations()
        {
            return PlexFactory.Parallel(
                GetTransformation(Rectangles[0], 600, Rectangles[0].Margin.Top, 300, Easers.EaseInOutCubic),
                GetTransformation(Rectangles[1], 600, Rectangles[1].Margin.Top, 300, Easers.EaseInOutCubic));
        }

        private BaseTask RepeatTransformations()
        {
            return PlexFactory.Repeat(
                PlexFactory.Sequential(
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

            _plexEngine.Reset();
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

            return PlexFactory.Sequential(transformations.ToArray());
        }

        private BaseTask ParallelTransformation2()
        {
            var transformations = new List<BaseTask>();

            var counter = 0;

            foreach (var rectangle in Rectangles)
            {
                var sequentialTransformation = PlexFactory.Sequential(
                    PlexFactory.Wait(TimeSpan.FromMilliseconds(counter++ * 30)),
                    GetTransformation(rectangle, 600, rectangle.Margin.Top, 250, Easers.EaseInOutCubic),
                    GetTransformation(rectangle, 40, rectangle.Margin.Top, 250, Easers.EaseInOutCubic));
                transformations.Add(sequentialTransformation);
            }

            return PlexFactory.Repeat(PlexFactory.Parallel(transformations.ToArray()), 5);
        }

        private BaseTask ParallelTransformation3(bool isReversed = false)
        {
            var destinationX = isReversed ? 40 : 600;
            return PlexFactory.Parallel(
                Rectangles
                    .Select(r => GetTransformation(r, destinationX, r.Margin.Top, 300, Easers.EaseInOutCubic))
                    .ToArray());
        }

        private BaseTask GetTransformation(Rectangle target, double destinationX, double destinationY, int duration,
            EaserFunction func)
        {
            var transformation = WPFTransformationFactory.Create(target, "Margin",
                TimeSpan.FromMilliseconds(duration), func, true, new Thickness(destinationX, destinationY, 0, 0));

            transformation.SynchronizationContext = SynchronizationContext.Current;
            return transformation;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            AddCanvasItems();
            _plexEngine.Reset();
        }
    }
}