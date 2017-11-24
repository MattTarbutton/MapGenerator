using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        Level displayLevel;
        bool regenerateLevel;

        public MainWindow()
        {
            InitializeComponent();

            SeedTextBox.Text = Properties.Settings.Default.rngSeed.ToString();
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
                rngSeed += (int)char.GetNumericValue(c);
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
            displayLevel.RegenerateLevel();
            displayLevel.OnLevelRegenerated += DisplayLevel_OnLevelRegenerated;
            ShowImage(displayLevel);
        }

        private bool DisplayLevel_OnLevelRegenerated()
        {
            if (regenerateLevel)
            {
                regenerateLevel = false;
                displayLevel.RegenerateLevel();
            }
            else
            {
                Dispatcher.Invoke(() => { ShowImage(displayLevel); });
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
                newValue += (int)char.GetNumericValue(c);
            }
            if (displayLevel != null)
            {
                displayLevel.RngSeed = newValue;
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
            }
        }

        private void SeedTextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SeedTextBox.SelectAll();
        }

        private void MazeRowsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(MazeRowsTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.MazeRows = newValue;
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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

                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                MapCanvas.Width = double.NaN;
                MapCanvas.Height = double.NaN;
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
            }
        }
        
        private void DrawSmoothCheckBox_Click(object sender, RoutedEventArgs e)
        {
            displayLevel.DrawSmooth = DrawSmoothCheckBox.IsChecked == true;
            displayLevel.RegenerateLevel();
            ShowImage(displayLevel);
        }

        private void DrawGridLinesCheckBox_Click(object sender, RoutedEventArgs e)
        {
            displayLevel.DrawGridLines = DrawGridLinesCheckBox.IsChecked == true;

            if (DrawGridLinesCheckBox.IsChecked == true)
            {
                DoubleAnimation anim = new DoubleAnimation(0, 50, new Duration(TimeSpan.FromSeconds(0.5)));
                GridCellHeightTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellHeightTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
            }
            else
            {
                DoubleAnimation anim = new DoubleAnimation(GridCellHeightTextBlock.ActualHeight, 0, new Duration(TimeSpan.FromSeconds(0.25)));
                GridCellHeightTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellHeightTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
                GridCellWidthTextBlock.BeginAnimation(TextBlock.MaxHeightProperty, anim);
                GridCellWidthTextBox.BeginAnimation(TextBox.MaxHeightProperty, anim);
            }

            displayLevel.RegenerateLevel();
            ShowImage(displayLevel);
        }

        private void GridCellWidthTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(GridCellWidthTextBox.Text, out int newValue) && displayLevel != null)
            {
                displayLevel.GridLineWidth = newValue;
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
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
                displayLevel.RegenerateLevel();
                ShowImage(displayLevel);
            }
        }

        private void GridCellHeightTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            GridCellHeightTextBox.SelectAll();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
