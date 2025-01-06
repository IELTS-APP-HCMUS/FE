using login_full.Models;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ xuất dữ liệu bài kiểm tra đọc sang PDF.
	/// </summary>
	public interface IPdfExportService
    {
		/// <summary>
		/// Xuất chi tiết bài kiểm tra đọc sang tệp PDF.
		/// </summary>
		/// <param name="testDetail">Chi tiết bài kiểm tra đọc cần xuất.</param>
		/// <param name="outputPath">Đường dẫn tệp đầu ra để lưu file PDF.</param>
		/// <returns>Trả về một tác vụ bất đồng bộ.</returns>
		Task ExportReadingTestToPdfAsync(ReadingTestDetail testDetail, string outputPath);
    }
} 