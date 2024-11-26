using login_full.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Microsoft.UI.Xaml.Shapes;

namespace login_full.Services
{
    public class ChartService : IChartService
    {
        public void DrawPieChart(Canvas canvas, double centerX, double centerY, double radius,
                               double correctPercentage, double wrongPercentage, double unansweredPercentage)
        {
            canvas.Children.Clear();
            double startAngle = 0;
            // Kiểm tra tổng số câu hỏi đã làm
            if (correctPercentage == 100 || wrongPercentage == 100 || unansweredPercentage == 100)
            {
                // Xác định màu dựa vào loại câu trả lời chiếm 100%
                Color color = Colors.WhiteSmoke; // Mặc định là màu cho câu chưa trả lời
                if (correctPercentage == 100)
                {
                    color = Colors.Green;
                }
                else if (wrongPercentage == 100)
                {
                    color = Colors.Red;
                }

                // Vẽ hình tròn đầy đủ với màu tương ứng
                DrawFullCircle(canvas, centerX, centerY, radius, color);
                return;
            }
            // Vẽ từng phần của pie chart
            if (correctPercentage > 0)
            {
                DrawPieSlice(canvas, centerX, centerY, radius, startAngle,
                            correctPercentage / 100 * 360, Colors.Green);
                startAngle += correctPercentage / 100 * 360;
            }

            if (wrongPercentage > 0)
            {
                DrawPieSlice(canvas, centerX, centerY, radius, startAngle,
                            wrongPercentage / 100 * 360, Colors.Red);
                startAngle += wrongPercentage / 100 * 360;
            }

            if (unansweredPercentage > 0)
            {
                DrawPieSlice(canvas, centerX, centerY, radius, startAngle,
                            unansweredPercentage / 100 * 360, Colors.WhiteSmoke);
            }
        }

        private void DrawPieSlice(Canvas canvas, double centerX, double centerY,
                                double radius, double startAngle, double sweepAngle, Color color)
        {
            double startRadians = startAngle * Math.PI / 180;
            double sweepRadians = sweepAngle * Math.PI / 180;
            double endRadians = startRadians + sweepRadians;

            bool isLargeArc = sweepAngle > 180;

            Point startPoint = new Point(
                centerX + radius * Math.Cos(startRadians),
                centerY + radius * Math.Sin(startRadians));

            Point endPoint = new Point(
                centerX + radius * Math.Cos(endRadians),
                centerY + radius * Math.Sin(endRadians));

            var figure = new PathFigure
            {
                StartPoint = startPoint,
                IsClosed = true
            };

            figure.Segments.Add(new ArcSegment
            {
                Point = endPoint,
                Size = new Size(radius, radius),
                IsLargeArc = isLargeArc,
                SweepDirection = SweepDirection.Clockwise
            });
            figure.Segments.Add(new LineSegment { Point = new Point(centerX, centerY) });

            var geometry = new PathGeometry();
            geometry.Figures.Add(figure);

            // Sử dụng Microsoft.UI.Xaml.Shapes.Path thay vì Path
            var path = new Microsoft.UI.Xaml.Shapes.Path
            {
                Fill = new SolidColorBrush(color),
                Data = geometry
            };

            canvas.Children.Add(path);
        }

        // Thêm phương thức mới để vẽ hình tròn đầy đủ
        private void DrawFullCircle(Canvas canvas, double centerX, double centerY, double radius, Color color)
        {
            var ellipse = new Microsoft.UI.Xaml.Shapes.Ellipse
            {
                Width = radius * 2,
                Height = radius * 2,
                Fill = new SolidColorBrush(color)
            };

            Canvas.SetLeft(ellipse, centerX - radius);
            Canvas.SetTop(ellipse, centerY - radius);
            canvas.Children.Add(ellipse);
        }
    }
}
