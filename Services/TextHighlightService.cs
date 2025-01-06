using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    /// <summary>
    // Service quản lý việc đánh dấu văn bản.
    // Cung cấp các chức năng thêm, lấy, và xóa các đoạn văn bản được đánh dấu.
    // </summary>
    public class TextHighlightService
    {

        /// <summary>
        /// Dictionary lưu trữ các đoạn văn bản được đánh dấu theo documentId.
        /// </summary>
        private readonly Dictionary<string, List<HighlightInfo>> _highlightedTexts = new();
        /// <summary>
        /// Thông tin về đoạn văn bản được đánh dấu.
        /// </summary>
        public class HighlightInfo
        {
            public string Text { get; set; }
            public int StartIndex { get; set; }
            public int Length { get; set; }
            public string Color { get; set; }
        }
        /// <summary>
        /// Thêm một đoạn văn bản được đánh dấu vào document.
        /// </summary>
        /// <param name="documentId">ID của document</param>
        /// <param name="text">Văn bản được đánh dấu</param>
        /// <param name="startIndex">Vị trí bắt đầu</param>
        /// <param name="length">Độ dài của đoạn văn bản</param>
        /// <param name="color">Màu sắc của đoạn văn bản (mặc định là vàng)</param>
        public void AddHighlight(string documentId, string text, int startIndex, int length, string color = "#FFFF00")
        {
            if (!_highlightedTexts.ContainsKey(documentId))
            {
                _highlightedTexts[documentId] = new List<HighlightInfo>();
            }

            _highlightedTexts[documentId].Add(new HighlightInfo
            {
                Text = text,
                StartIndex = startIndex,
                Length = length,
                Color = color
            });
        }
        /// <summary>
        /// Lấy danh sách các đoạn văn bản bị chồng lấn với đoạn văn bản được đánh dấu.
        /// </summary>
        /// <param name="documentId">ID của document</param>
        /// <param name="startIndex">Vị trí bắt đầu</param>
        /// <param name="length">Độ dài của đoạn văn bản</param>
        /// <returns>Danh sách các đoạn văn bản bị chồng lấn</returns>
        public List<HighlightInfo> GetOverlappingHighlights(string documentId, int startIndex, int length)
        {
            if (!_highlightedTexts.ContainsKey(documentId))
                return new List<HighlightInfo>();

            return _highlightedTexts[documentId]
                .Where(h => !(startIndex + length <= h.StartIndex || startIndex >= h.StartIndex + h.Length))
                .ToList();
        }
        /// <summary>
        /// Xóa một đoạn văn bản được đánh dấu khỏi document.
        /// </summary>
        /// <param name="documentId">ID của document</param>
        /// <param name="startIndex">Vị trí bắt đầu</param>
        /// <param name="length">Độ dài của đoạn văn bản</param>
        public void RemoveHighlight(string documentId, int startIndex, int length)
        {
            if (_highlightedTexts.ContainsKey(documentId))
            {
                var highlightsToRemove = _highlightedTexts[documentId]
                    .Where(h => h.StartIndex == startIndex && h.Length == length)
                    .ToList();

                foreach (var highlight in highlightsToRemove)
                {
                    _highlightedTexts[documentId].Remove(highlight);
                }
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả các đoạn văn bản được đánh dấu trong document.
        /// </summary>
        /// <param name="documentId">ID của document</param>
        /// <returns>Danh sách các đoạn văn bản được đánh dấu</returns>
        public List<HighlightInfo> GetHighlights(string documentId)
        {
            return _highlightedTexts.TryGetValue(documentId, out var highlights)
                ? highlights
                : new List<HighlightInfo>();
        }
    }
}
