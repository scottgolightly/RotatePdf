using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RotatePdf
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                PrintUsage();
                return;
            }

            if (!Directory.Exists(args[0]))
            {
                PrintUsage();
                return;
            }
            var sourceFiles = Directory.GetFiles(args[0], "*.pdf");
            if (sourceFiles.Length == 0)
            {
                Console.WriteLine($"No PDF files in {args[0]}");
                return;
            }

            int degrees;
            if (!int.TryParse(args[1], out degrees))
            {
                PrintUsage();
                return;
            }

            foreach (var sourceFile in sourceFiles)
            {
                var pdf = new FileInfo(sourceFile);
                using (PdfReader pdfReader = new PdfReader(sourceFile))
                {
                    Console.Write($"{sourceFile} ");
                    var rotatedPdfFile = pdf.DirectoryName + "\\" + pdf.Name.Substring(0, pdf.Name.LastIndexOf('.')) + "_rotated" + pdf.Extension;
                    using(PdfWriter pdfWriter = new PdfWriter(rotatedPdfFile))
                    {
                        PdfDocument pdfDocument = new PdfDocument(pdfReader);
                        PdfDocument pdfRotatedDocument = new PdfDocument(pdfWriter);
                        for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
                        {
                            // PDF pages are numbered starting with 1
                            var currentPage = pdfDocument.GetPage(i);
                            var copiedPage = currentPage.CopyTo(pdfRotatedDocument);
                            copiedPage.SetRotation(degrees);
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
            }
        }

        /*
    using (Document doc = new Document()) {
      using (PdfCopy copy = new PdfCopy(doc, Response.OutputStream)) {
        doc.Open();
        foreach (PdfReader reader in readers) {
          int n = reader.NumberOfPages;
          for (int page = 0; page < n;) {
            ++page;
            float width = reader.GetPageSize(page).Width;
            float height = reader.GetPageSize(page).Height;
            if (width > height) {
              PdfDictionary pageDict = reader.GetPageN(page);
              pageDict.Put(PdfName.ROTATE, new PdfNumber(90));
            }
            copy.AddPage(copy.GetImportedPage(reader, page));
          }
        }        
      }
    }
  }
         */
        static void PrintUsage()
        {
            Console.WriteLine("Example: RotatePdf <directorypath> <degrees>");
            Console.WriteLine("\tdirectorypath is fully qualified (e.g. \"c:\\files\\\")");
            Console.WriteLine("\tdegrees is an integer value (e.g. 180)");
        }
    }
}
