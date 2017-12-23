using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapGenerator
{
    /// <summary>
    /// Interaction logic for MapNodeControl.xaml
    /// </summary>
    public partial class MapNodeControl : UserControl
    {
        protected double _canvasSizeRatio;

        protected List<ConnectionControl> _connections;

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                SelectionRectangle.GetBindingExpression(Rectangle.VisibilityProperty).UpdateTarget();
            }
        }

        public Visibility ShowSelectionRectangle
        {
            get { return IsSelected ? Visibility.Visible : Visibility.Hidden; }
        }
        
        private MapNode _mapNode;
        public MapNode MapNode
        {
            get { return _mapNode; }
            set
            {
                _mapNode = value;
                base.DataContext = value;
                _mapNode.PropertyChanged += MapNode_PropertyChanged;
            }
        }

        public double RoomSize
        {
            get { return _mapNode.RoomSize * _canvasSizeRatio * 2; }
        }

        public double PathWidth
        {
            get { return (_mapNode.RoomSize + _mapNode.PathWidth) * _canvasSizeRatio * 2; }
        }

        public double MaxPerturb
        {
            get { return (_mapNode.RoomSize + _mapNode.PathWidth + _mapNode.MaxPerturb) * _canvasSizeRatio * 2; }
        }
        
        public double SelectionRectangleSize
        {
            get { return Math.Max(MaxPerturb + 2, 22); }
        }
        
        // Offsets the room ellipse in the control by half the room size
        public Thickness RoomOffset
        {   
            get { return new Thickness(-_mapNode.RoomSize * _canvasSizeRatio + NodeEllipse.Width / 2, -_mapNode.RoomSize * _canvasSizeRatio + NodeEllipse.Height / 2, 0, 0); }
        }

        public Thickness PathWidthOffset
        {
            get { return new Thickness(-(_mapNode.RoomSize + _mapNode.PathWidth) * _canvasSizeRatio + NodeEllipse.Width / 2, -(_mapNode.RoomSize + _mapNode.PathWidth) * _canvasSizeRatio + NodeEllipse.Height / 2, 0, 0); }
        }

        public Thickness PerturbOffset
        {
            get { return new Thickness(-(_mapNode.RoomSize + _mapNode.PathWidth + _mapNode.MaxPerturb) * _canvasSizeRatio + NodeEllipse.Width / 2, -(_mapNode.RoomSize + _mapNode.PathWidth + _mapNode.MaxPerturb) * _canvasSizeRatio + NodeEllipse.Height / 2, 0, 0); }
        }

        public Thickness SelectionRectangleOffset
        {
            get { return new Thickness(Math.Min(-(_mapNode.RoomSize + _mapNode.PathWidth + _mapNode.MaxPerturb) * _canvasSizeRatio + NodeEllipse.Width / 2 - 1, -1), Math.Min(-(_mapNode.RoomSize + _mapNode.PathWidth + _mapNode.MaxPerturb) * _canvasSizeRatio + NodeEllipse.Height / 2 - 1, -1), 0, 0); }
        }

        private bool _isDragging;
        private Point _clickPosition;
        
        //public MapNodeControl()
        //{
        //    InitializeComponent();

        //    _canvasSizeRatio = 1;
        //}

        //public MapNodeControl(MapNodeControl control)
        //{
        //    InitializeComponent();

        //    this.Width = control.Width;
        //    this.Height = control.Height;
        //    _canvasSizeRatio = control._canvasSizeRatio;
        //}

        //public MapNodeControl(MapNode node)
        //{
        //    InitializeComponent();

        //    MapNode = node;
        //    _canvasSizeRatio = 1;
        //}

        public MapNodeControl(MapNode node, double canvasRatio, MainWindow window)
        {
            InitializeComponent();

            _connections = new List<ConnectionControl>();
            MapNode = node;
            _canvasSizeRatio = canvasRatio;
            window.MapCanvasRatioChanged += CanvasRatioChanged;
        }

        private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _isDragging = true;
            UserControl draggableControl = sender as UserControl;
            _clickPosition = e.GetPosition(this);
            draggableControl.CaptureMouse();
        }

        private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _isDragging = false;
            UserControl draggable = sender as UserControl;
            draggable.ReleaseMouseCapture();
        }

        private void UserControl_MouseMove(object sender, MouseEventArgs e)
        {
            UserControl draggableControl = sender as UserControl;

            if (_isDragging && draggableControl != null)
            {
                Point currentPosition = e.GetPosition(base.Parent as UIElement);
                
                FrameworkElement parent = base.Parent as FrameworkElement;
                if (double.IsNaN(currentPosition.X))
                    currentPosition.X = 0;
                if (double.IsNaN(currentPosition.Y))
                    currentPosition.Y = 0;

                Point newPosition = new Point(currentPosition.X - _clickPosition.X, currentPosition.Y - _clickPosition.Y);

                if (newPosition.X < 0)
                    newPosition.X = 0;
                if (newPosition.Y < 0)
                    newPosition.Y = 0;
                if (newPosition.X > parent.ActualWidth - base.ActualWidth)
                    newPosition.X = parent.ActualWidth - base.ActualWidth;
                if (newPosition.Y > parent.ActualHeight - base.ActualHeight)
                    newPosition.Y = parent.ActualHeight - base.ActualHeight;

                Canvas.SetLeft(this, newPosition.X);
                Canvas.SetTop(this, newPosition.Y);

                MapNode.XPosition = (int)Math.Round(newPosition.X);
                MapNode.YPosition = (int)Math.Round(newPosition.Y);
                MapNode.TruePosition = new System.Drawing.Point((int)Math.Round(newPosition.X / _canvasSizeRatio), (int)Math.Round(newPosition.Y / _canvasSizeRatio));
            }
        }

        private void AddConnectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.AddConnectionStart(this);
        }
        
        private void MapNode_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "RoomSize" || e.PropertyName == "MaxPerturb" || e.PropertyName == "PathWidth")
            {
                UpdateBindings();
            }
        }
        
        private void CanvasRatioChanged(double newRatio)
        {
            MapNode.XPosition = (int)Math.Round(Canvas.GetLeft(this));
            MapNode.YPosition = (int)Math.Round(Canvas.GetTop(this));
            _canvasSizeRatio = newRatio;
            MapNode.TruePosition = new System.Drawing.Point((int)Math.Round(MapNode.XPosition / _canvasSizeRatio), (int)Math.Round(MapNode.YPosition / _canvasSizeRatio));
            UpdateBindings();
        }

        public void UpdateBindings()
        {
            RoomSizeEllipse.GetBindingExpression(Ellipse.MarginProperty).UpdateTarget();
            RoomSizeEllipse.GetBindingExpression(Ellipse.HeightProperty).UpdateTarget();
            RoomSizeEllipse.GetBindingExpression(Ellipse.WidthProperty).UpdateTarget();
            PathWidthEllipse.GetBindingExpression(Ellipse.MarginProperty).UpdateTarget();
            PathWidthEllipse.GetBindingExpression(Ellipse.HeightProperty).UpdateTarget();
            PathWidthEllipse.GetBindingExpression(Ellipse.WidthProperty).UpdateTarget();
            MaxPerturbEllipse.GetBindingExpression(Ellipse.MarginProperty).UpdateTarget();
            MaxPerturbEllipse.GetBindingExpression(Ellipse.HeightProperty).UpdateTarget();
            MaxPerturbEllipse.GetBindingExpression(Ellipse.WidthProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.MarginProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.HeightProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.WidthProperty).UpdateTarget();
        }

        private void RemoveNodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
        }

        public void AddConnectionControl(ConnectionControl controlToAdd)
        {
            _connections.Add(controlToAdd);
        }

        public void RemoveConnectionControl(ConnectionControl controlToRemove)
        {
            _connections.Remove(controlToRemove);
        }

        public void Dispose()
        {
            while (_connections.Count > 0)
            {
                _connections[0].Dispose();
            }

            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.MapCanvasRatioChanged -= CanvasRatioChanged;
            window.RemoveControl(this);
        }
    }
}
