using System.Net.Http;
using System.Threading.Tasks;
using static HttpContentExtractor;
using UglyToad.PdfPig;
using UglyToad.PdfPig.Content;
using UglyToad.PdfPig.DocumentLayoutAnalysis.WordExtractor;
using UglyToad.PdfPig.DocumentLayoutAnalysis.TextExtractor;

namespace alxnbl.OneNoteMdExporter.Helpers
{
    public static class PdfContentExtractor
    {
        public static string GetPdfContent(string pdfUrl)
        {
            byte[] pdfContent = FetchPdfContentAsync(pdfUrl).Result;
            return ExtractTextFromPdf(pdfContent);
        }

        // takes a string which represents the contents of a pdf file and
        // returns a string which is the text extracted from the pdf file
        public static string ExtractTextFromPdf(byte[] pdfContent)
        {
            using (var pdfDocument = PdfDocument.Open(pdfContent))
            {
                var text = new System.Text.StringBuilder();

                foreach (Page page in pdfDocument.GetPages())
                {
                    var wordExtractor = NearestNeighbourWordExtractor.Instance;
                   

                    var words = wordExtractor.GetWords(page.Letters);
                    foreach (var word in words)
                    {
                        text.Append(word.Text + " ");
                    }
                    text.AppendLine();
                }

                return text.ToString();
            }
        }

        // takes a URL pointing to a PDF and fetches it over HTTP
        // and returns the PDF content as an array of bytes
        public static async Task<byte[]> FetchPdfContentAsync(string pdfUrl)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(pdfUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
        }
    }
}

