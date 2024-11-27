using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface IChartService
    {
        void DrawPieChart(Canvas canvas, double centerX, double centerY, double radius,
                         double correctPercentage, double wrongPercentage, double unansweredPercentage);

       // void DrawFullCircle(Canvas canvas, double centerX, double centerY, double radius, Color color);
    }

}
