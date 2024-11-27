using Microsoft.UI.Xaml.Controls;
using Windows.Foundation;
using System.Collections.Generic;
using Microsoft.UI.Text;

namespace login_full.Extensions
{
    public static class RichEditBoxExtensions
    {
        public static TextRange GetSelectedText(this RichEditBox richEditBox)
        {
            TextRange textRange = new TextRange();

            // Lấy selection từ RichEditBox
            ITextSelection selection = richEditBox.Document.Selection;

            if (selection != null)
            {
                // Lấy text được chọn và thông tin vị trí
                selection.GetText(TextGetOptions.None, out string selectedText);
                textRange.Text = selectedText;
                textRange.StartPosition = selection.StartPosition;
                textRange.Length = selection.Length;
            }

            return textRange;
        }

        public static List<Rect> GetSelectionRects(this RichEditBox richEditBox)
        {
            List<Rect> rects = new List<Rect>();

            ITextSelection selection = richEditBox.Document.Selection;

            if (selection != null)
            {
                // Lấy vị trí của selection
                ITextRange range = selection;
                if (range != null)
                {
                    range.GetPoint(HorizontalCharacterAlignment.Left, VerticalCharacterAlignment.Top, PointOptions.None, out Point startPoint);
                    range.GetPoint(HorizontalCharacterAlignment.Right, VerticalCharacterAlignment.Bottom, PointOptions.None, out Point endPoint);
                    var rect = new Rect(startPoint, endPoint);
                    rects.Add(rect);
                }
            }

            return rects;
        }
    }

    public class TextRange
    {
        public string Text { get; set; } = string.Empty;
        public int StartPosition { get; set; }
        public int Length { get; set; }
    }
}
