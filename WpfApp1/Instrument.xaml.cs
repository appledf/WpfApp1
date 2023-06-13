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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Instrument.xaml 的交互逻辑
    /// </summary>
    public partial class Instrument : UserControl
    {
        public double Value
        {
            get { return (double)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty =
                DependencyProperty.Register("Value", typeof(double), typeof(Instrument),
                new PropertyMetadata(double.NaN, new PropertyChangedCallback(OnPropertyChanged)));
        public static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as Instrument).Refresh();
        }
        public Instrument()
        {
            InitializeComponent();
            this.SizeChanged += UCCycleProcessBar_SizeChanged;
        }
        private void UCCycleProcessBar_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double minSize = Math.Min(this.RenderSize.Width, this.RenderSize.Height);
            this.backEllipse.Width = minSize;
            this.backEllipse.Height = minSize;
            //this.frontEllipse.Width = minSize - 50;
            //this.frontEllipse.Height = minSize - 50;
        }
        private void Refresh()
        {
            //半径
            double raduis = this.backEllipse.Width / 2;
            this.mainCanvas.Children.Clear();
            double min = 0, max = 100;
            double scalAreaCount = 10;
            double step = 270.0 / (max - min);
            for (int i = 0; i < max - min; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = raduis - (raduis - 13) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = raduis - (raduis - 13) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = raduis - (raduis - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = raduis - (raduis - 8) * Math.Sin((i * step - 45) * Math.PI / 180);

                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 2;
                this.mainCanvas.Children.Add(lineScale);
            }
            step = 270.0 / scalAreaCount;
            int scaleText = (int)min;
            for (int i = 0; i <= scalAreaCount; i++)
            {
                Line lineScale = new Line();
                lineScale.X1 = raduis - (raduis - 20) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y1 = raduis - (raduis - 20) * Math.Sin((i * step - 45) * Math.PI / 180);
                lineScale.X2 = raduis - (raduis - 8) * Math.Cos((i * step - 45) * Math.PI / 180);
                lineScale.Y2 = raduis - (raduis - 8) * Math.Sin((i * step - 45) * Math.PI / 180);

                lineScale.Stroke = Brushes.White;
                lineScale.StrokeThickness = 2;
                this.mainCanvas.Children.Add(lineScale);

                TextBlock textScale = new TextBlock();
                textScale.Text = (scaleText + (max - min) / scalAreaCount * i).ToString();
                textScale.Width = 34;
                textScale.TextAlignment = TextAlignment.Center;
                textScale.FontSize = 14;
                textScale.Foreground = Brushes.White;
                Canvas.SetLeft(textScale, raduis - (raduis - 36) * Math.Cos((i * step - 45) * Math.PI / 180) - 17);
                Canvas.SetTop(textScale, raduis - (raduis - 36) * Math.Sin((i * step - 45) * Math.PI / 180) - 10);
                this.mainCanvas.Children.Add(textScale);
            }
            //圆心装饰
            string sData = "M{0} {1} A{0} {0} 0 1 1 {1} {2}";
            sData = string.Format(sData, raduis / 2, raduis, raduis * 1.5);
            var converter = TypeDescriptor.GetConverter(typeof(Geometry));
            this.circle.Data = (Geometry)converter.ConvertFrom(sData);

            //指针
            step = 270.0 / (max - min);
            //this.rtPointer.Angle = this.Value * step - 45;

            DoubleAnimation da = new DoubleAnimation(this.Value * step - 45, new Duration(TimeSpan.FromMilliseconds(200)));
            //this.rtPointer.BeginAnimation(ValueProperty, da);
            //DoubleAnimation da = new DoubleAnimation((int)((this.Value - this.Minimum) * step) - 45, new Duration(TimeSpan.FromMilliseconds(200)));
            this.rtPointer.BeginAnimation(RotateTransform.AngleProperty, da);

            sData = "M{0} {1},{1} {2},{1} {3}";
            sData = string.Format(sData, raduis * 0.3, raduis, raduis - 5, raduis + 5);
            this.pointer.Data = (Geometry)converter.ConvertFrom(sData);
        }
    }
}
