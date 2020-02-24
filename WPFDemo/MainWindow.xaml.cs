using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Aptacode.TaskPlex;
using Aptacode.TaskPlex.Tasks;
using Aptacode.TaskPlex.Tasks.Transformation;
using Aptacode.TaskPlex.WPF.Tasks.Transformation;
using Microsoft.Extensions.Logging.Abstractions;

namespace WPFDemo
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion INotifyPropertyChanged
    }

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
                //   Console.WriteLine((int)value.X + " " + (int));
            }
        }

        public Rectangle Content { get; set; }
    }

    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly TaskCoordinator _taskCoordinator;
        private BaseTask selectedTransformation;
        public ObservableCollection<CanvasItem> CanvasItems { get; set; }
        private readonly CanvasItem myRectangle1, myRectangle2, myRectangle3, myRectangle4;

        private bool isPlaying = false;

        public MainWindow()
        {
            InitializeComponent();
            _taskCoordinator = new TaskCoordinator(new NullLoggerFactory());
            CanvasItems = new ObservableCollection<CanvasItem>();
            DataContext = this;
            myRectangle1 =
                new CanvasItem(
                    new Rectangle {Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 100, 100))},
                    new Point(0, 0));
            
            myRectangle2 =
                new CanvasItem(
                    new Rectangle { Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(150, 100, 100)) },
                    new Point(100, 50));

            myRectangle3 =
                new CanvasItem(
                    new Rectangle { Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 150, 100)) },
                    new Point(100, 110));

            myRectangle4 =
                new CanvasItem(
                    new Rectangle { Width = 50, Height = 50, Fill = new SolidColorBrush(Color.FromRgb(100, 100, 150)) },
                    new Point(100, 170));

            CanvasItems.Add(myRectangle1);
            CanvasItems.Add(myRectangle2);
            CanvasItems.Add(myRectangle3);
            CanvasItems.Add(myRectangle4);
        }

        private void PlayButtonClicked(object sender, RoutedEventArgs e)
        {
            if (!isPlaying)
            {
                _taskCoordinator.Resume();
            }
        }

        private void PauseButtonClicked(object sender, RoutedEventArgs e)
        {
            isPlaying = false;
            _taskCoordinator.Pause();
        }

        private void OnDemoSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    selectedTransformation = GetSequentialTransformation();
                    break;

                case 1:
                    selectedTransformation = GetParallelTransformation();
                    break;
                case 2:
                    selectedTransformation = GetParallelTransformation(true);
                    break;
            }

            _taskCoordinator.Reset();

            isPlaying = true;
            _taskCoordinator.Apply(selectedTransformation);
        }

        private BaseTask GetSequentialTransformation()
        {
            var transitions = new List<BaseTask>();
            for (var i = 5; i > 1; i--)
            {
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(0, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(450, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(450, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(0, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
            }

            for (var i = 2; i < 5; i++)
            {
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(0, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(450, 0),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(450, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
                transitions.Add(WPFTransformationFactory.Create(myRectangle1, "Position", new Point(0, 450),
                    TimeSpan.FromMilliseconds(i * 20), RefreshRate.High));
            }

            return TaskPlexFactory.Sequential(transitions.ToArray());
        }

        private BaseTask GetParallelTransformation(bool isReveresed = false)
        {

            int destinationX = isReveresed ? 100 : 400;

            var transform1 = WPFTransformationFactory.Create(myRectangle2, "Position", new Point(destinationX, 50),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            var transform2 = WPFTransformationFactory.Create(myRectangle3, "Position", new Point(destinationX, 110),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            var transform3 = WPFTransformationFactory.Create(myRectangle4, "Position", new Point(destinationX, 170),
                TimeSpan.FromMilliseconds(100), RefreshRate.High);

            return TaskPlexFactory.Parallel(transform1, transform2, transform3);
        }
    }
}