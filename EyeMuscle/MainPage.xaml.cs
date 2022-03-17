using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;

using System.Diagnostics;
using Windows.UI.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace EyeMuscle
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterReceived;
        }

        private double MaxSliderValue = 0d;
        private double MinSliderValue = 1000d;

        private void UpdateMinMax(double value)
        {
            {
                if (value > MaxSliderValue)
                {
                    MaxSliderValue = value;
                }
                if (value < MinSliderValue)
                {
                    MinSliderValue = value;
                }
            }
            //Debug.WriteLine(value + "=>" + MinSliderValue + ";" + MaxSliderValue);
        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            double v = e.NewValue;
            double w = v;

            if (false == isZooming)
            {
                UpdateMinMax(w);
            }
            MainImage.Width = (int)w;
        }
        /// <summary>
        /// 
        /// </summary>
        private bool isZooming = false;

        /// <summary>
        /// スライダ移動速度
        /// </summary>
        private double speed = 1d;

        private void ToggleZooming()
        {
            if (isZooming)
            {
                isZooming = false;
                StartButton.Content = "press space key to start!";
                timer.Stop();
            }
            else
            {
                isZooming = true;
                StartButton.Content = "press space key to stop!";
                timer.Start();
            }
        }

        private void StartButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ToggleZooming();
        }

        DispatcherTimer timer;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(0.01)
            };
            timer.Tick += Timer_Tick;

            Slider.ValueChanged += Slider_ValueChanged;
        }

        private void Timer_Tick(object sender, object e)
        {
            double v = Slider.Value;
            if ((v > MaxSliderValue - 1) || (v < MinSliderValue + 1))
            {
                speed *= -1d;
            }
            //Debug.WriteLine("speed=" + speed);
            v += speed;

            Slider.Value = (int)v;
        }

        private void Grid_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pp = e.GetCurrentPoint(this);
            int delta = pp.Properties.MouseWheelDelta;
            //Debug.WriteLine("pointer wheel=" + delta);
            if (delta > 0)
            {
                Slider.Value += 5d;
            }
            else
            {
                Slider.Value += -5d;
            }
        }

        /// <summary>
        /// スペースキーでZoomingをon/off
        /// 1,2,3キーでズーム速度切り替え
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void CoreWindow_CharacterReceived(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.CharacterReceivedEventArgs args)
        {
            switch (args.KeyCode)
            {
                case (uint)Windows.System.VirtualKey.Space:
                    ToggleZooming();
                    break;
                case (uint)Windows.System.VirtualKey.Number1:
                    speed = 1d;
                    break;
                case (uint)Windows.System.VirtualKey.Number2:
                    speed = 2d;
                    break;
                case (uint)Windows.System.VirtualKey.Number3:
                    speed = 3d;
                    break;
                case (uint)Windows.System.VirtualKey.A:

                    MainImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/image2.jpg"));
                    break;
                case (uint)Windows.System.VirtualKey.B:
                    MainImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Images/image1.jpg"));
                    break;
            }
            if (args.KeyCode == 27) //Escape
            {
                // your code here fore Escape key
            }
            if (args.KeyCode == 13) //Enter
            {
                // your code here fore Enter key
            }
        }
    }
}
