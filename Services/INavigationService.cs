using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(Type pageType);
        // Thêm các phương thức navigation khác nếu cần
        Task NavigateToAsync(Type pageType, object parameter);
    }
}
