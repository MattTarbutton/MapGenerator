﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace MapGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum MapType : int
        {
            Custom = 0,
            Maze = 1,
        }

        public IList<MapType> MapTypes
        {
            get { return Enum.GetValues(typeof(MapType)).Cast<MapType>().ToList<MapType>(); }
        }

        Level displayLevel;
        public MapType SelectedMapType { get; set; }
        List<MapNodeControl> mapNodes;
        List<ConnectionControl> connections;
        bool regeneratingLevel;
        bool needToRegenerateLevel;

        private UserControl _selectedControl;
        public UserControl SelectedControl
        {
            set
            {
                if (_selectedControl != null)
                {
                    if (_selectedControl.GetType() == typeof(MapNodeControl))
                    {
                        ((MapNodeControl)_selectedControl).IsSelected = false;
                    }
                    else if (_selectedControl.GetType() == typeof(ConnectionControl))
                    {
                        ((ConnectionControl)_selectedControl).IsSelected = false;
                    }
                }
                if (value == null)
                {
                    PropertyGrid1.SelectedObject = null;
                    _selectedControl = value;
                }
                else if (value.GetType() == typeof(MapNodeControl))
                {
                    PropertyGrid1.SelectedObject = value.DataContext;
                    _selectedControl = value;
                    ((MapNodeControl)_selectedControl).IsSelected = true;
                }
                else if (value.GetType() == typeof(ConnectionControl))
                {
                    PropertyGrid1.SelectedObject = value.DataContext;
                    _selectedControl = value;
                    ((ConnectionControl)_selectedControl).IsSelected = true;
                }
            }
        }
        private Line _connectionLine;
        private bool _addingConnection;

        public delegate void MapCanvasRatioChangedEventHandler(double newRatio);
        public event MapCanvasRatioChangedEventHandler MapCanvasRatioChanged;

        public MainWindow()
        {
            InitializeComponent();

            mapNodes = new List<MapNodeControl>();
            connections = new List<ConnectionControl>();

            SeedTextBox.Text = Properties.Settings.Default.rngSeed.ToString();
            
            MapTypeComboBox.SelectedValue = (MapType)Properties.Settings.Default.selectedMapType;
            MazeRowsTextBox.Text = Properties.Settings.Default.mazeRows.ToString();
            MazeColumnsTextBox.Text = Properties.Settings.Default.mazeColumns.ToString();
            BirthLimitTextBox.Text = Properties.Settings.Default.birthLimit.ToString();
            DeathLimitTextBox.Text = Properties.Settings.Default.deathLimit.ToString();
            ChanceToStartAliveTextBox.Text = Properties.Settings.Default.chanceToStartAlive.ToString();
            SimStepsTextBox.Text = Properties.Settings.Default.numberOfSimSteps.ToString();
            PathWidthTextBox.Text = Properties.Settings.Default.pathWidth.ToString();
            MapWidthTextBox.Text = Properties.Settings.Default.mapWidth.ToString();
            MapHeightTextBox.Text = Properties.Settings.Default.mapHeight.ToString();
            LineSizeTextBox.Text = Properties.Settings.Default.lineSize.ToString();
            SizeMultiplierTextBox.Text = Properties.Settings.Default.sizeMultiplier.ToString();
            ImageHeightTextBox.Text = (Properties.Settings.Default.sizeMultiplier * Properties.Settings.Default.mapHeight).ToString();
            ImageWidthTextBox.Text = (Properties.Settings.Default.sizeMultiplier * Properties.Settings.Default.mapWidth).ToString();
            DrawSmoothCheckBox.IsChecked = Properties.Settings.Default.drawSmooth;
            DrawGridLinesCheckBox.IsChecked = Properties.Settings.Default.drawGrid;
            GridCellWidthTextBox.Text = Properties.Settings.Default.gridCellWidth.ToString();
            GridCellHeightTextBox.Text = Properties.Settings.Default.gridCellHeight.ToString();
            string rngSeedString = Properties.Settings.Default.rngSeed.ToString();
            int rngSeed = 0;
            foreach (char c in SeedTextBox.Text)
            {
                rngSeed += char.ConvertToUtf32(c.ToString(), 0);
            }
            displayLevel = new Level()
            {
                RngSeed = rngSeed,
                MazeRows = Properties.Settings.Default.mazeRows,
                MazeColumns = Properties.Settings.Default.mazeColumns,
                DeathLimit = Properties.Settings.Default.deathLimit,
                BirthLimit = Properties.Settings.Default.birthLimit,
                ChanceToStartAlive = Properties.Settings.Default.chanceToStartAlive,
                PathWidth = Properties.Settings.Default.pathWidth,
                MapHeight = Properties.Settings.Default.mapHeight,
                MapWidth = Properties.Settings.Default.mapWidth,
                NumberOfSteps = Properties.Settings.Default.numberOfSimSteps,
                LineSize = Properties.Settings.Default.lineSize,
                PixelsPerMapUnit = Properties.Settings.Default.sizeMultiplier,
                DrawSmooth = Properties.Settings.Default.drawSmooth,
                DrawGridLines = Properties.Settings.Default.drawGrid,
                GridLineHeight = Properties.Settings.Default.gridCellWidth,
                GridLineWidth = Properties.Settings.Default.gridCellHeight
            };
            //displayLevel.RegenerateLevel();
            displayLevel.OnLevelRegenerated += DisplayLevel_OnLevelRegenerated;
            //ShowImage(displayLevel);

            MapNode newNode = new MapNode(150, 150, 1);
            MapNodeControl newControl = new MapNodeControl(newNode, 1, this)
            {
                Width = 20,
                Height = 20
            };
            Canvas.SetLeft(newControl, 150);
            Canvas.SetTop(newControl, 150);
            MapCanvas.Children.Add(newControl);
            newControl.MapNode.PropertyChanged += OnPropertyChanged;
            mapNodes.Add(newControl);
            PropertyGrid1.SelectedObject = newNode;
        }

        private void RegenerateLevel()
        {
            if (regeneratingLevel)
            {
                needToRegenerateLevel = true;
                return;
            }
            else
            {
                regeneratingLevel = true;
                if (SelectedMapType == MapType.Custom)
                {
                    RegenerateFromNodes();
                }
                else if (SelectedMapType == MapType.Maze)
                {
                    displayLevel.RegenerateLevel();
                }
            }
        }

        private void RegenerateFromNodes()
        {
            MapNode[] nodes = new MapNode[mapNodes.Count];
            for (int i = 0; i < mapNodes.Count; i++)
            {
                nodes[i] = mapNodes[i].MapNode;
            }
            Connection[] conns = new Connection[connections.Count];
            for (int i = 0; i < connections.Count; i++)
            {
                conns[i] = connections[i].Connection;
            }
            displayLevel.RegenerateLevel(nodes, conns);
        }

        private bool DisplayLevel_OnLevelRegenerated()
        {
            regeneratingLevel = false;
            Dispatcher.Invoke(() => { ShowImage(displayLevel); });

            if (needToRegenerateLevel)
            {
                needToRegenerateLevel = false;
                RegenerateLevel();
            }
            
            return true;
        }
        
        private void ShowImage(Level levelToShow)
        {
            if (levelToShow.tex != null)
            {
                try
                {
                    using (MemoryStream memory = new MemoryStream())
                    {
                        levelToShow.tex.Save(memory, ImageFormat.Png);
                        memory.Position = 0;
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memory;
                        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                        bitmapImage.EndInit();
                        MapImage.Source = bitmapImage;
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private void RestoreDefaultsButton_Click(object sender, RoutedEventArgs e)
        {
            SeedTextBox.Text = "42";
            MazeRowsTextBox.Text = "5";
            MazeColumnsTextBox.Text = "5";
            BirthLimitTextBox.Text = "3";
            DeathLimitTextBox.Text = "2";
            ChanceToStartAliveTextBox.Text = "45";
            SimStepsTextBox.Text = "3";
            PathWidthTextBox.Text = "6";
            MapWidthTextBox.Text = "300";
            MapHeightTextBox.Text = "300";
            ImageHeightTextBox.Text = "600";
            ImageWidthTextBox.Text = "600";
        }

        private void SeedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int newValue = 0;
            foreach (char c in SeedTextBox.Text)
            {
                newValue += char.ConvertToUtf32(c.ToString(), 0);
            }
            if (displayLevel != null)
            {
                displayLevel.RngSeed = newValue;
                RegenerateLevel();
            }
        }

        private void SeedTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SeedTextBox.SelectAll();
        }

        private void MapTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedMapType == MapType.Custom)
            {
                // Show all user controls
                foreach (MapNodeControl control in mapNodes)
                {
                    if (!MapCanvas.Children.Contains(control))
                    {
                        MapCanvas.Children.Add(control);
                        control.MapNode.PropertyChanged += OnPropertyChanged;
                    }
                }
                foreach (ConnectionControl control in connections)
                {
                    if (!MapCanvas.Children.Contains(control))
                    {
                        MapCanvas.Children.Add(control);
                        control.Connection.PropertyChanged += OnPropertyChanged;
                    }
                }
                
                DoubleAnimation anim = new DoubleAnimation(GridCellHeightTextBlock.ActualHeight, 0, new Duration(TimeSpan.FromSeconds(0.25)));
                MazeRowsTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                MazeRowsTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                MazeRowsTextBox.IsTabStop = false;
                MazeColumnsTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                MazeColumnsTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                MazeColumnsTextBox.IsTabStop = false;
                PathWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                PathWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                PathWidthTextBox.IsTabStop = false;
                if (PropertyWindowColumn.MaxWidth < 190)
                {
                    DoubleAnimation propertyWindowAnim = new DoubleAnimation(0, 200, new Duration(TimeSpan.FromSeconds(0.25)));
                    PropertyWindowColumn.BeginAnimation(ColumnDefinition.MaxWidthProperty, propertyWindowAnim);
                }
            }
            else if (SelectedMapType == MapType.Maze)
            {
                // Hide all user controls
                foreach (MapNodeControl control in mapNodes)
                {
                    if (MapCanvas.Children.Contains(control))
                    {
                        MapCanvas.Children.Remove(control);
                        control.MapNode.PropertyChanged -= OnPropertyChanged;
                    }
                }
                foreach (ConnectionControl control in connections)
                {
                    if (MapCanvas.Children.Contains(control))
                    {
                        MapCanvas.Children.Remove(control);
                        control.Connection.PropertyChanged -= OnPropertyChanged;
                    }
                }

                DoubleAnimation anim = new DoubleAnimation(0, 50, new Duration(TimeSpan.FromSeconds(0.5)));
                MazeRowsTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                MazeRowsTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                MazeRowsTextBox.IsTabStop = true;
                MazeColumnsTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                MazeColumnsTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                MazeColumnsTextBox.IsTabStop = true;
                PathWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                PathWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                PathWidthTextBox.IsTabStop = true;
                DoubleAnimation propertyWindowAnim = new DoubleAnimation(PropertyWindowColumn.ActualWidth, 0, new Duration(TimeSpan.FromSeconds(0.25)));
                PropertyWindowColumn.BeginAnimation(ColumnDefinition.MaxWidthProperty, propertyWindowAnim);
            }
            RegenerateLevel();
        }

        private void MazeRowsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MazeRowsTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.MazeRows = newValue;
                RegenerateLevel();
            }
        }

        private void MazeRowsTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MazeRowsTextBox.SelectAll();
        }

        private void MazeColumnsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MazeColumnsTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.MazeColumns = newValue;
                RegenerateLevel();
            }
        }

        private void MazeColumnsTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MazeColumnsTextBox.SelectAll();
        }

        private void BirthLimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(BirthLimitTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.BirthLimit = newValue;
                RegenerateLevel();
            }
        }

        private void BirthLimitTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            BirthLimitTextBox.SelectAll();
        }

        private void DeathLimitTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(DeathLimitTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.DeathLimit = newValue;
                RegenerateLevel();
            }
        }

        private void DeathLimitTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            DeathLimitTextBox.SelectAll();
        }

        private void ChanceToStartAliveTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(ChanceToStartAliveTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.ChanceToStartAlive = newValue;
                RegenerateLevel();
            }
        }

        private void ChanceToStartAliveTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            ChanceToStartAliveTextBox.SelectAll();
        }

        private void SimStepsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(SimStepsTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.NumberOfSteps = newValue;
                RegenerateLevel();
            }
        }

        private void SimStepsTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SimStepsTextBox.SelectAll();
        }

        private void PathWidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(PathWidthTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.PathWidth = newValue;
                RegenerateLevel();
            }
        }

        private void PathWidthTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            PathWidthTextBox.SelectAll();
        }

        private void MapWidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MapWidthTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.MapWidth = newValue;
                if (int.TryParse(SizeMultiplierTextBox.Text, out int sizeMultiplier))
                {
                    ImageWidthTextBox.Text = (newValue * sizeMultiplier).ToString();
                }
                else
                {
                    ImageWidthTextBox.Text = "????";
                }
                RegenerateLevel();
            }
            else
            {
                ImageWidthTextBox.Text = "????";
            }
        }

        private void MapWidthTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MapWidthTextBox.SelectAll();
        }

        private void MapHeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MapHeightTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.MapHeight = newValue;
                if (int.TryParse(SizeMultiplierTextBox.Text, out int sizeMultiplier))
                {
                    ImageHeightTextBox.Text = (newValue * sizeMultiplier).ToString();
                }
                else
                {
                    ImageHeightTextBox.Text = "????";
                }
                RegenerateLevel();
            }
            else
            {
                ImageHeightTextBox.Text = "????";
            }
        }

        private void MapHeightTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            MapHeightTextBox.SelectAll();
        }

        private void SizeMultiplierTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(SizeMultiplierTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.PixelsPerMapUnit = newValue;
                if (int.TryParse(MapWidthTextBox.Text, out int mapWidth))
                {
                    ImageWidthTextBox.Text = (newValue * mapWidth).ToString();
                }
                else
                {
                    ImageWidthTextBox.Text = "????";
                }
                if (int.TryParse(MapHeightTextBox.Text, out int mapHeight))
                {
                    ImageHeightTextBox.Text = (newValue * mapWidth).ToString();
                }
                else
                {
                    ImageHeightTextBox.Text = "????";
                }

                RegenerateLevel();
            }
        }

        private void SizeMultiplierTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SizeMultiplierTextBox.SelectAll();
        }
        
        private void LineSizeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(LineSizeTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.LineSize = newValue;
                RegenerateLevel();
            }
        }

        private void LineSizeTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            LineSizeTextBox.SelectAll();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                DefaultExt = ".png",
                Filter = "Images|*.png;*.bmp;*.jpg"
            };
            ImageFormat format = ImageFormat.Png;
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                string ext = System.IO.Path.GetExtension(dialog.FileName);
                switch (ext)
                {
                    case ".jpg":
                        format = ImageFormat.Jpeg;
                        break;
                    case ".bmp":
                        format = ImageFormat.Bmp;
                        break;
                }
                displayLevel.tex.Save(dialog.FileName, format);
            }
        }

        private void ZoomToFitCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ZoomToFitCheckBox.IsChecked == true)
            {
                MapScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Hidden;
                MapScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Hidden;
                UpdateMapCanvas();
            }
            else
            {
                Binding widthBinding = new Binding()
                {
                    Source = MapImage,
                    Path = new PropertyPath("ActualWidth")
                };
                BindingOperations.SetBinding(MapCanvas, Canvas.WidthProperty, widthBinding);
                Binding heightBinding = new Binding()
                {
                    Source = MapImage,
                    Path = new PropertyPath("ActualHeight")
                };
                BindingOperations.SetBinding(MapCanvas, Canvas.HeightProperty, heightBinding);
                MapScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                MapScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
        }
        
        private void DrawSmoothCheckBox_Click(object sender, RoutedEventArgs e)
        {
            displayLevel.DrawSmooth = DrawSmoothCheckBox.IsChecked == true;
            RegenerateLevel();
        }

        private void DrawGridLinesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            displayLevel.DrawGridLines = DrawGridLinesCheckBox.IsChecked == true;

            if (DrawGridLinesCheckBox.IsChecked == true)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 50, new Duration(TimeSpan.FromSeconds(0.5)));
                GridCellHeightTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellHeightTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellHeightTextBox.IsTabStop = true;
                GridCellWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellWidthTextBox.IsTabStop = true;
            }
            else
            {
                DoubleAnimation anim = new DoubleAnimation(GridCellHeightTextBlock.ActualHeight, 0, new Duration(TimeSpan.FromSeconds(0.25)));
                GridCellHeightTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellHeightTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellHeightTextBox.IsTabStop = false;
                GridCellWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellWidthTextBox.IsTabStop = false;
            }

            RegenerateLevel();
        }

        private void GridCellWidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(GridCellWidthTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.GridLineWidth = newValue;
                RegenerateLevel();
            }
        }

        private void GridCellWidthTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            GridCellWidthTextBox.SelectAll();
        }

        private void GridCellHeightTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (int.TryParse(GridCellHeightTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.GridLineHeight = newValue;
                RegenerateLevel();
            }
        }

        private void GridCellHeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GridCellHeightTextBox.SelectAll();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //TODO: add saving settings
        }

        private void RandomizeButton_Click(object sender, RoutedEventArgs e)
        {
            string newSeed = "";
            Random rng = new Random();
            for (int i = 0; i < 8; i++)
            {
                newSeed += char.ConvertFromUtf32(rng.Next(33, 127));
            }

            SeedTextBox.Text = newSeed;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ZoomToFitCheckBox.IsChecked == true)
            {
                UpdateMapCanvas();
            }
        }

        private void MapImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateMapCanvas();
        }

        private void UpdateMapCanvas()
        {
            if (MapImage.ActualHeight < 0.1 && MapImage.ActualWidth < 0.1)
                return;

            double maxHeight = Math.Min(MapScrollViewer.ActualHeight, MapImage.ActualHeight);
            double maxWidth = Math.Min(MapScrollViewer.ActualWidth, MapImage.ActualWidth);

            double heightDifference = maxHeight - MapImage.ActualHeight;
            double widthDifference = maxWidth - MapImage.ActualWidth;

            if (heightDifference < widthDifference)
            {
                MapCanvas.Height = maxHeight;
                MapCanvas.Width = maxHeight * MapImage.ActualWidth / MapImage.ActualHeight;
            }
            else
            {
                MapCanvas.Width = maxWidth;
                MapCanvas.Height = maxWidth * MapImage.ActualHeight / MapImage.ActualWidth;
            }
        }

        private void MapCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double hDelta = e.NewSize.Width / e.PreviousSize.Width;
            double vDelta = e.NewSize.Height / e.PreviousSize.Height;

            if (double.IsInfinity(hDelta) || double.IsInfinity(vDelta)) return;

            foreach (UIElement element in MapCanvas.Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                if (double.IsNaN(left))
                    left = 0;
                if (double.IsNaN(top))
                    top = 0;

                Canvas.SetLeft(element, left * hDelta);
                Canvas.SetTop(element, top * vDelta);
            }
            
            MapCanvasRatioChanged?.Invoke(GetMapCanvasRatio());
        }

        private double GetMapCanvasRatio()
        {
            return displayLevel.PixelsPerMapUnit * (MapCanvas.ActualHeight / MapImage.ActualHeight);
        }

        private void AddNodeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Point location = AddNodeContextMenu.TranslatePoint(new System.Windows.Point(), MapCanvas);
            MapNode newNode = new MapNode((int)location.X, (int)location.Y, GetMapCanvasRatio());
            MapNodeControl newControl = new MapNodeControl(newNode, GetMapCanvasRatio(), this)
            {
                Width = 20,
                Height = 20
            };
            Canvas.SetLeft(newControl, location.X - 10);
            Canvas.SetTop(newControl, location.Y - 10);
            MapCanvas.Children.Add(newControl);
            newControl.MapNode.PropertyChanged += OnPropertyChanged;
            mapNodes.Add(newControl);
            PropertyGrid1.SelectedObject = newNode;
            RegenerateLevel();
        }

        private void MapCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_addingConnection && _selectedControl != null && _connectionLine != null)
            {
                //_connectionLine = new Line()
                //{
                //    Stroke = System.Windows.Media.Brushes.Green,
                //    IsHitTestVisible = false
                //};
                //MapCanvas.Children.Add(_connectionLine);
                //Canvas.SetZIndex(_connectionLine, -1);

                _connectionLine.X1 = Canvas.GetLeft(_selectedControl) + _selectedControl.ActualWidth / 2;
                _connectionLine.Y1 = Canvas.GetTop(_selectedControl) + _selectedControl.ActualHeight / 2;
                _connectionLine.X2 = e.GetPosition(MapCanvas).X;
                _connectionLine.Y2 = e.GetPosition(MapCanvas).Y;
            }
        }

        public void AddConnectionStart(UserControl startControl)
        {
            _addingConnection = true;
            _connectionLine = new Line()
            {
                Stroke = System.Windows.Media.Brushes.Green,
                IsHitTestVisible = false
            };

            MapCanvas.Children.Add(_connectionLine);
            Canvas.SetZIndex(_connectionLine, -1);
        }

        public void RemoveControl(MapNodeControl controlToRemove)
        {
            MapCanvas.Children.Remove(controlToRemove);
            controlToRemove.MapNode.PropertyChanged -= OnPropertyChanged;
            mapNodes.Remove(controlToRemove);
            SelectedControl = null;
            RegenerateLevel();
        }

        public void RemoveControl(ConnectionControl controlToRemove)
        {
            MapCanvas.Children.Remove(controlToRemove);
            //controlToRemove.Connection.PropertyChanged -= OnPropertyChanged;
            connections.Remove(controlToRemove);
            SelectedControl = null;
            RegenerateLevel();
        }

        private void MapCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(MapCanvas);
            if (_addingConnection && e.LeftButton == MouseButtonState.Pressed && e.Source.GetType() == typeof(MapNodeControl) && e.Source != _selectedControl)
            {
                Connection newCon = new Connection((MapNode)_selectedControl.DataContext, (MapNode)((UserControl)e.Source).DataContext)
                {
                    PathWidth = 10,
                    PerturbAmount = 10
                };
                ConnectionControl newConnection = new ConnectionControl((MapNodeControl)_selectedControl, (MapNodeControl)e.Source, newCon, GetMapCanvasRatio(), this);
                ((MapNodeControl)_selectedControl).AddConnectionControl(newConnection);
                ((MapNodeControl)e.Source).AddConnectionControl(newConnection);

                MapCanvas.Children.Add(newConnection);
                newCon.PropertyChanged += OnPropertyChanged;
                connections.Add(newConnection);
                Canvas.SetZIndex(newConnection, -1);

                MapCanvas.Children.Remove(_connectionLine);
                _connectionLine = null;
                _addingConnection = false;
                RegenerateLevel();
            }
            else if (_addingConnection && e.RightButton == MouseButtonState.Pressed)
            {
                MapCanvas.Children.Remove(_connectionLine);
                _connectionLine = null;
                _addingConnection = false;
            }
            // Select the control if right or left mouse button is pressed and we are not adding a connection
            else if (!_addingConnection && (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed))
            {
                if (e.Source.GetType() == typeof(MapNodeControl) || e.Source.GetType() == typeof(ConnectionControl))
                {
                    SelectedControl = (UserControl)e.Source;
                }
            }
        }
        
        private void MapCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Deselect if we click on nothing
            if (!_addingConnection && !(e.Source.GetType() == typeof(MapNodeControl) || e.Source.GetType() == typeof(ConnectionControl)))
            {
                SelectedControl = null;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RegenerateLevel();
        }

        private void MapCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (_selectedControl.GetType() == typeof(MapNodeControl))
                    ((MapNodeControl)_selectedControl).Dispose();
                else if (_selectedControl.GetType() == typeof(ConnectionControl))
                    ((ConnectionControl)_selectedControl).Dispose();
            }
        }

        
    }
}
