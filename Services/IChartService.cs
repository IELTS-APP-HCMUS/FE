using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ vẽ biểu đồ.
	/// </summary>
	public interface IChartService
    {
		/// <summary>
		/// Vẽ biểu đồ tròn (Pie Chart) trên một đối tượng Canvas.
		/// </summary>
		/// <param name="canvas">Đối tượng Canvas để vẽ biểu đồ.</param>
		/// <param name="centerX">Tọa độ X của tâm biểu đồ.</param>
		/// <param name="centerY">Tọa độ Y của tâm biểu đồ.</param>
		/// <param name="radius">Bán kính của biểu đồ tròn.</param>
		/// <param name="correctPercentage">Tỷ lệ phần trăm câu trả lời đúng.</param>
		/// <param name="wrongPercentage">Tỷ lệ phần trăm câu trả lời sai.</param>
		/// <param name="unansweredPercentage">Tỷ lệ phần trăm câu trả lời chưa được trả lời.</param>
		void DrawPieChart(Canvas canvas, double centerX, double centerY, double radius,
                         double correctPercentage, double wrongPercentage, double unansweredPercentage);

       // void DrawFullCircle(Canvas canvas, double centerX, double centerY, double radius, Color color);
    }

}
