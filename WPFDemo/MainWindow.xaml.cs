using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Aptacode.TaskPlex;
using Aptacode.TaskPlex.Enums;
using Aptacode.TaskPlex.Interfaces;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.WPF.Tasks.Transformation;
using Microsoft.Extensions.Logging.Abstractions;

namespace WPFDemo
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ITaskCoordinator _taskCoordinator;

        private bool _isPlaying;

        //References to each item in the canvas
        private CanvasItem _myRectangle1, _myRectangle2, _myRectangle3, _myRectangle4;

        //The currently selected task to be applied
        private BaseTask _selectedTransformation;

        public MainWindow()
        {
            InitializeComponent();
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory());
            CanvasItems = new ObservableCollection<CanvasItem>();
            DataContext = this;
            AddCanvasItems();
        }

        //A collection of all the items to be displayed on the canvas
        public ObservableCollection<CanvasItem> CanvasItems { get; set; }

        private void AddCanvasItems()
        {
            _myRectangle1 =
                new CanvasItem(
                    new Rectangle {Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100))},
                    new Point(0, 0));

            _myRectangle2 =
                new CanvasItem(
                    new Rectangle {Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(150, 100, 100))},
                    new Point(100, 50));

            _myRectangle3 =
                new CanvasItem(
                    new Rectangle {Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 150, 100))},
                    new Point(100, 110));

            _myRectangle4 =
                new CanvasItem(
                    new Rectangle {Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 100, 150))},
                    new Point(100, 170));

            CanvasItems.Add(_myRectangle1);
            CanvasItems.Add(_myRectangle2);
            CanvasItems.Add(_myRectangle3);
            CanvasItems.Add(_myRectangle4);
        }

        private void PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!_isPlaying)
            {
                _taskCoordinator.Resume();
            }
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            _isPlaying = false;
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
                    _selectedTransformation = GetParallelTransformation(true);
                    break;
            }

            _taskCoordinator.Reset();

            _isPlaying = true;
            _taskCoordinator.Apply(_selectedTransformation);
        }

        private BaseTask GetSequentialTransformation()
        {
            var transitions = new List<BaseTask>();
            for (var i = 5; i > 1; i--)
            {
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(0, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(450, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(450, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(0, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
            }

            for (var i = 2; i < 5; i++)
            {
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(0, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(450, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(450, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(_myRectangle1, "Position", new Point(0, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
            }

            return TaskPlexFactory.Sequential(transitions.ToArray());
        }

        private BaseTask GetParallelTransformation(bool isReversed = false)
        {
            var destinationX = isReversed ? 100 : 400;

            var transform1 = WPFTransformationFactory.Create(_myRectangle2, "Position", new Point(destinationX, 50),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            var transform2 = WPFTransformationFactory.Create(_myRectangle3, "Position", new Point(destinationX, 110),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            var transform3 = WPFTransformationFactory.Create(_myRectangle4, "Position", new Point(destinationX, 170),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            return TaskPlexFactory.Parallel(transform1, transform2, transform3);
        }
    }
}