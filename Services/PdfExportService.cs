using login_full.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Threading.Tasks;
using Windows.UI;

namespace login_full.Services
{
    public class PdfExportService : IPdfExportService
    {
        public async Task ExportReadingTestToPdfAsync(ReadingTestDetail testDetail, string outputPath)
        {
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = false;

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.DefaultTextStyle(x => x.FontSize(11));

                    page.Header().ShowOnce().Element(ComposeHeader);
                    page.Content().Element(x => ComposeContent(x, testDetail));
                    page.Footer().Element(ComposeFooter);
                });
            })
            .GeneratePdf(outputPath);

            await Task.CompletedTask;
        }

        private void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text("MePass - Reading Practice")
                        .FontSize(20)
                        .SemiBold();
                        
                    column.Item().AlignRight().Text(DateTime.Now.ToString("dd/MM/yyyy HH:mm"))
                        .FontSize(8)
                        .Italic();
                });
            });
        }

        private void ComposeContent(IContainer container, ReadingTestDetail testDetail)
        {
            container.Column(column =>
            {
                // Title
                column.Item().AlignCenter().Text(testDetail.Title ?? "Reading Test")
                    .FontSize(16)
                    .SemiBold();




                //column.Item().Text(testDetail.Content);
                // Content with better formatting
                column.Item().PaddingVertical(10).Text(text =>
                {
                    text.Span(testDetail.Content)
                        .FontSize(11)
                        .LineHeight(1.5f);  // Điều chỉnh khoảng cách dòng
                });


                column.Item().Height(20);

             

                int questionIndex = 1;
                foreach (var question in testDetail.Questions)
                {
                    column.Item().Text($"Question {questionIndex}:")
                        .SemiBold();
                    column.Item().Text(question.QuestionText);
                    column.Item().Height(5);

                    // Options
                    if (question.Options != null)
                    {
                        char optionLetter = 'A';
                        foreach (var option in question.Options)
                        {
                            column.Item().Text($"{optionLetter}. {option}");
                            optionLetter++;

                        }
                    }

                    column.Item().Height(10);
                    questionIndex++;
                }
            });
        }

        private void ComposeFooter(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Text(x =>
                {
                    x.Span("Page ").FontSize(8);
                    x.CurrentPageNumber().FontSize(8);
                    x.Span(" of ").FontSize(8);
                    x.TotalPages().FontSize(8);

                });

                row.RelativeItem().AlignRight().Text("Generated by MePass")
                         .FontSize(8)
                        .Italic();
            });
        }
    }
} 