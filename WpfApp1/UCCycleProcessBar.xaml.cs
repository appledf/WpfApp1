using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    /// UCCycleProcessBar.xaml 的交互逻辑
    /// </summary>
    public partial class UCCycleProcessBar : UserControl
    {
        private int minValue = 0;
        private int maxValue = 100;
        public UCCycleProcessBar()
        {
            InitializeComponent();
        }
        private double _value;
        private double _angle;
        public double Value
        {
            get { return _value; }
            set {
                _value = value;
                if (_value >= maxValue)
                {
                    _value = maxValue;
                }
                if (_value <= minValue)
                {
                    _value = minValue;
                }
                SetValue(_value); }
        }
        public double Step
        { get; set; }
        public double StartAngle
        {
            get { return _angle; }
            set { 
                _angle = value;
                if (_angle > 0.0)
                {
                    RotateTransform rt = new RotateTransform();
                    rt.Angle = StartAngle;
                    mainCanvas.RenderTransformOrigin = new Point(0.5, 0.5);
                    mainCanvas.RenderTransform = rt;
                }
            } 
        }

        private void btnReduce_Click(object sender, RoutedEventArgs e)
        {
            this.Value = this.Value - Step;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            this.Value = this.Value + Step;
        }

        /// <summary>
        /// 设置百分百，输入小数，自动乘100
        /// </summary>
        /// <param name="percentValue"></param>
        private void SetValue(double Value)
        {
            /*****************************************
              方形矩阵边长为34，半长为17
              环形半径为14，所以距离边框3个像素
              环形描边3个像素
            ******************************************/
            double percentValue = Value / Convert.ToDouble(maxValue);
            double angel = percentValue * 360; //角度

            double radius = 140; //环形半径

            //起始点
            double leftStart = 170;
            double topStart = 30;
            //结束点
            double endLeft = 0;
            double endTop = 0;

            //数字显示
            lbValue.Content = (percentValue * 100).ToString("0") + "%";

            /***********************************************
            * 整个环形进度条使用Path来绘制，采用三角函数来计算
            * 环形根据角度来分别绘制，以90度划分，方便计算比例
            ***********************************************/

            bool isLagreCircle = false; //是否优势弧，即大于180度的弧形

            //小于90度
            if (angel <= 90)
            {
                double ra = (90 - angel) * Math.PI / 180; //弧度
                endLeft = leftStart + Math.Cos(ra) * radius; //余弦横坐标
                endTop = topStart + radius - Math.Sin(ra) * radius; //正弦纵坐标

            }
            else if (angel <= 180)
            {
                double ra = (angel - 90) * Math.PI / 180; //弧度
                endLeft = leftStart + Math.Cos(ra) * radius; //余弦横坐标
                endTop = topStart + radius + Math.Sin(ra) * radius;//正弦纵坐标
            }

            else if (angel <= 270)
            {
                isLagreCircle = true; //优势弧
                double ra = (angel - 180) * Math.PI / 180;
                endLeft = leftStart - Math.Sin(ra) * radius;
                endTop = topStart + radius + Math.Cos(ra) * radius;
            }

            else if (angel < 360)
            {
                isLagreCircle = true; //优势弧
                double ra = (angel - 270) * Math.PI / 180;
                endLeft = leftStart - Math.Cos(ra) * radius;
                endTop = topStart + radius - Math.Sin(ra) * radius;
            }
            else
            {
                isLagreCircle = true; //优势弧
                endLeft = leftStart - 0.001; //不与起点在同一点，避免重叠绘制出非环形
                endTop = topStart;
            }
            //结尾圆
            Canvas.SetLeft(Pointer, endLeft-20);
            Canvas.SetTop(Pointer, endTop-20);

            Point arcEndPt = new Point(endLeft, endTop); //结束点
            Size arcSize = new Size(radius, radius);
            SweepDirection direction = SweepDirection.Clockwise; //顺时针弧形
            //弧形
            ArcSegment arcsegment = new ArcSegment(arcEndPt, arcSize, 0, isLagreCircle, direction, true);

            //形状集合
            PathSegmentCollection pathsegmentCollection = new PathSegmentCollection();
            pathsegmentCollection.Add(arcsegment);

            //路径描述
            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(leftStart, topStart); //起始地址
            pathFigure.Segments = pathsegmentCollection;

            //路径描述集合
            PathFigureCollection pathFigureCollection = new PathFigureCollection();
            pathFigureCollection.Add(pathFigure);

            //复杂形状
            PathGeometry pathGeometry = new PathGeometry();
            pathGeometry.Figures = pathFigureCollection;

            //Data赋值
            //myCycleProcessBar.Data = pathGeometry;
            myCycleProcessBar1.Data = pathGeometry;
            //达到100%则闭合整个
            if (angel == 360)
                myCycleProcessBar1.Data = Geometry.Parse(myCycleProcessBar1.Data.ToString() + " z");
        }
    }
}
