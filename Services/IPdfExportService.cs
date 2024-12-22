using login_full.Models;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface IPdfExportService
    {
        Task ExportReadingTestToPdfAsync(ReadingTestDetail testDetail, string outputPath);
    }
} 