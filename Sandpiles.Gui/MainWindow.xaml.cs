using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sandpiles.Gui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Core.Sandpiles _piles;
        private TextBlock[,] _textBlocks;
        private DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();
            btnNext.Click += BtnNext_Click;
            this.Loaded += MainWindow_Loaded;
            var valRange = 55;
            int[] probDecay = new int[valRange];
            int[] probGrowth = new int[valRange];

            for (var i = 4; i < valRange; i++)
            {
                var prob = (50 + (i - 4)) * 200_000;
                probDecay[i] = prob;
            }
            for (var i = 3; i < valRange - 1; i++)
            {
                var prob = (int)(Math.Pow(0.762718, i) * 2253927.46861);
                probGrowth[i] = prob;
            }
            _piles = new Core.Sandpiles(25, 25, probDecay, probGrowth);
            _piles.Init((i, j) =>
            {
                if (i != 12 || j != 12) return 0;
                return 25;
            });

            _timer = new DispatcherTimer {
                Interval = TimeSpan.FromMilliseconds(250)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            _piles.ComputeRound();
            UpdateUi();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _textBlocks = new TextBlock[_piles.Width, _piles.Height];
            valueGrid.ColumnDefinitions.Clear();
            valueGrid.RowDefinitions.Clear();
            for (var i = 0; i < _piles.Width; i++)
            {
                valueGrid.ColumnDefinitions.Add(new ColumnDefinition
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });
            }
            for (var j = 0; j < _piles.Height; j++)
            {
                valueGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = new GridLength(1, GridUnitType.Star)
                });
            }
            CreateUi();
            UpdateUi();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_timer.IsEnabled) _timer.Stop();
            else _timer.Start();
        }

        private void CreateUi()
        {
            valueGrid.Children.Clear();
            for (var i = 0; i < _piles.Width; i++)
            {
                for (var j = 0; j < _piles.Height; j++)
                {
                    var myText = new TextBlock
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        TextAlignment = TextAlignment.Center
                    };
                    Grid.SetColumn(myText, i);
                    Grid.SetRow(myText, j);
                    valueGrid.Children.Add(myText);
                    _textBlocks[i, j] = myText;
                }
            }
        }

        private void UpdateUi()
        {
            for (var i = 0; i < _piles.Width; i++)
            {
                for (var j = 0; j < _piles.Height; j++)
                {
                    var color = (byte) (255 - Math.Min(_piles.Piles[i, j], 54) * 4.722);
                    
                    _textBlocks[i, j].Text = _piles.Piles[i, j].ToString();
                    _textBlocks[i, j].Background = new SolidColorBrush(new Color
                    {
                        A = 255,
                        G = (byte) (255 - color),
                        R = 255,
                        B = color
                    });
                }
            }
        }
    }
}
