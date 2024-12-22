using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public class TextHighlightService
    {
        private readonly Dictionary<string, List<HighlightInfo>> _highlightedTexts = new();

        public class HighlightInfo
        {
            public string Text { get; set; }
            public int StartIndex { get; set; }
            public int Length { get; set; }
            public string Color { get; set; }
        }

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

        public List<HighlightInfo> GetOverlappingHighlights(string documentId, int startIndex, int length)
        {
            if (!_highlightedTexts.ContainsKey(documentId))
                return new List<HighlightInfo>();

            return _highlightedTexts[documentId]
                .Where(h => !(startIndex + length <= h.StartIndex || startIndex >= h.StartIndex + h.Length))
                .ToList();
        }

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

        public List<HighlightInfo> GetHighlights(string documentId)
        {
            return _highlightedTexts.TryGetValue(documentId, out var highlights)
                ? highlights
                : new List<HighlightInfo>();
        }
    }
}
