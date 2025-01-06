using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ điều hướng giữa các trang.
	/// </summary>
	public interface INavigationService
    {
		/// <summary>
		/// Điều hướng đến một trang cụ thể được chỉ định bởi kiểu dữ liệu của trang.
		/// </summary>
		/// <param name="pageType">Kiểu của trang cần điều hướng đến.</param>
		/// <returns>Trả về một tác vụ bất đồng bộ (Task).</returns>
		Task NavigateToAsync(Type pageType);


		/// <summary>
		/// Điều hướng đến một trang cụ thể với tham số bổ sung.
		/// </summary>
		/// <param name="pageType">Kiểu của trang cần điều hướng đến.</param>
		/// <param name="parameter">Tham số được truyền đến trang.</param>
		/// <returns>Trả về một tác vụ bất đồng bộ (Task).</returns>
		Task NavigateToAsync(Type pageType, object parameter);
    }
}
