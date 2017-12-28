using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ConnectionControl.xaml
    /// </summary>
    public partial class ConnectionControl : UserControl
    {
        private double _canvasSizeRatio;

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
        
        private Connection _connection;
        public Connection Connection
        {
            get { return _connection; }
            set
            {
                if (_connection != null)
                {
                    _connection.PositionChanged -= OnPositionChanged;
                    _connection.PropertyChanged -= Connection_PropertyChanged;
                }
                _connection = value;
                this.DataContext = _connection;
                if (value != null)
                {
                    _connection.PositionChanged += OnPositionChanged;
                    _connection.PropertyChanged += Connection_PropertyChanged;
                }
            }
        }

        private MapNodeControl[] _nodes;

        public Visibility ShowSelectionRectangle
        {
            get { return IsSelected ? Visibility.Visible : Visibility.Hidden; }
        }

        public double SelectionRectangleWidth
        {
            get
            {
                Point point1 = new Point(ConnectionLine.X1, ConnectionLine.Y1);
                Point point2 = new Point(ConnectionLine.X2, ConnectionLine.Y2);

                return Point.Subtract(point1, point2).Length;
            }
        }

        public double SelectionRectangleHeight
        {
            get { return PathLineThickness; }
        }

        public Thickness SelectionRectangleOffset
        {
            get { return new Thickness(0, 0, 0, 0); }
        }

        public double SelectionRectangleRotation
        {
            get
            {
                double xDif = ConnectionLine.X2 - ConnectionLine.X1;
                double yDif = ConnectionLine.Y2 - ConnectionLine.Y1;

                return Math.Atan2(yDif, xDif) * 180.0 / Math.PI;
            }
        }

        public double SelectionLineThickness
        {
            get { return (_connection.PathWidth + 2) * _canvasSizeRatio * 2; }
        }

        public double PathLineThickness
        {
            get { return _connection.PathWidth * _canvasSizeRatio * 2; }
        }

        public double PerturbLineThickness
        {
            get { return (_connection.PathWidth + _connection.PerturbAmount) * _canvasSizeRatio * 2; }
        }
        
        public ConnectionControl(MapNodeControl node1, MapNodeControl node2, Connection connection, double canvasRatio, MainWindow window)
        {
            InitializeComponent();

            Connection = connection;
            _nodes = new MapNodeControl[2] { node1, node2 };
            _canvasSizeRatio = canvasRatio;
            window.MapCanvasRatioChanged += CanvasRatioChanged;
            _connection.PositionChanged += OnPositionChanged;
            RefreshNodeLocations();
            SelectionRectangleTransform.Angle = SelectionRectangleRotation;
        }

        private void RefreshNodeLocations()
        {
            //Add 10 to x and y coordinates to account for node ellipse
            System.Drawing.Point nodeLocation = _connection.GetNodeLocation(0);
            nodeLocation.X += 10;
            nodeLocation.Y += 10;
            System.Drawing.Point node2Location = _connection.GetNodeLocation(1);
            node2Location.X += 10;
            node2Location.Y += 10;
            SetLinePoints(nodeLocation.X, nodeLocation.Y, node2Location.X, node2Location.Y);
        }

        public void SetLinePoints(double X1, double Y1, double X2, double Y2)
        {
            ConnectionLine.X1 = X1;
            ConnectionLine.Y1 = Y1;
            ConnectionLine.X2 = X2;
            ConnectionLine.Y2 = Y2;
        }

        private void OnPositionChanged(object sender, PropertyChangedEventArgs args)
        {
            RefreshNodeLocations();
            UpdateBindings();
        }

        private void Connection_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "PathWidth" || e.PropertyName == "PerturbAmount")
            {
                UpdateBindings();
            }
        }

        private void CanvasRatioChanged(double newRatio)
        {
            _canvasSizeRatio = newRatio;
            UpdateBindings();
        }

        public void UpdateBindings()
        {
            PathLine.GetBindingExpression(Line.StrokeThicknessProperty).UpdateTarget();
            //PerturbLine.GetBindingExpression(Line.StrokeThicknessProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.WidthProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.HeightProperty).UpdateTarget();
            SelectionRectangle.GetBindingExpression(Rectangle.MarginProperty).UpdateTarget();
            SelectionRectangleTransform.Angle = SelectionRectangleRotation;
        }

        public void Dispose()
        {
            _connection.Dispose();
            _connection = null;
            foreach(MapNodeControl node in _nodes)
            {
                node.RemoveConnectionControl(this);
            }
            _nodes = null;
            MainWindow window = (MainWindow)Window.GetWindow(this);
            window.MapCanvasRatioChanged -= CanvasRatioChanged;
            window.RemoveControl(this);
        }

        private void RemoveConnectionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Dispose();
        }
    }
}
